/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: InterruptableConnector.cs,v 1.2 2011/10/27 23:21:57 kzmi Exp $
 */
using System;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.Net.Sockets;


namespace Poderosa.Protocols {

    /// <summary>
    /// �\�P�b�g���J���A�ڑ����m������菇�����^�C���A�E�g��r���Œ��~���邱�Ƃ��ł��邽�߂̋@�\
    /// </summary>
    internal abstract class InterruptableConnector : IInterruptable {
        private IPAddressList _addressSet;
        protected IInterruptableConnectorClient _client;
        protected ITCPParameter _tcpDestination;
        protected IPAddress _connectedAddress;
        protected Socket _socket;
        protected string _host;
        protected int _port;
        protected Socks _socks;

        protected bool _succeeded;
        protected bool _interrupted;
        protected bool _overridingErrorMessage;
        protected bool _tcpConnected;

        protected string _errorMessage;

        public void AsyncConnect(IInterruptableConnectorClient client, ITCPParameter param) {
            _client = client;
            _tcpDestination = param;
            _host = param.Destination;
            _port = param.Port;

            //AgentForward���̃`�F�b�N������
            foreach (IConnectionResultEventHandler ch in ProtocolsPlugin.Instance.ConnectionResultEventHandler.GetExtensions())
                ch.BeforeAsyncConnect((ITerminalParameter)param.GetAdapter(typeof(ITerminalParameter)));

            Thread th = new Thread(new ThreadStart(this.Run));
            th.Start();
        }


        private void NotifyAsyncClient() {
            if (_succeeded)
                _client.SuccessfullyExit(this.Result);
            else
                _client.ConnectionFailed(_errorMessage);
        }

        public void Interrupt() {
            _interrupted = true;
            //�ڑ��X���b�h���u���b�N���Ă�����ʐM���ł����Ă��A�\�P�b�g����Ă��܂��΂�����O�ɂȂ�͂��ł���A
            //����Run()��catch��finally�u���b�N�����s�����B
            if (_socket != null)
                _socket.Close();
        }

        //Start..End�ԂŔ�������Exception�ɂ��ẮA�G���[���b�Z�[�W���㏑������B
        //�ς�SocketException�̃G���[���b�Z�[�W���g�������Ȃ��Ƃ��p
        protected void StartOverridingErrorMessage(string message) {
            _errorMessage = message;
            _overridingErrorMessage = true;
        }
        protected void EndOverridingErrorMessage() {
            _overridingErrorMessage = false;
        }

        private void Run() {
            _tcpConnected = false;
            _succeeded = false;
            _socket = null;

            try {
                _addressSet = null;
                _errorMessage = null;
                MakeConnection();
                Debug.Assert(_socket != null);

                _errorMessage = null;
                Negotiate();

                _succeeded = true;
                ProtocolUtil.FireConnectionSucceeded(_tcpDestination);
            }
            catch (Exception ex) {
                if (!_interrupted) {
                    RuntimeUtil.DebuggerReportException(ex);
                    if (!_overridingErrorMessage) {
                        _errorMessage = ex.Message;
                    }
                    ProtocolUtil.FireConnectionFailure(_tcpDestination, _errorMessage);
                }
            }
            finally {
                if (!_interrupted) {
                    if (!_succeeded && _socket != null && _socket.Connected) {
                        try {
                            _socket.Shutdown(SocketShutdown.Both); //Close()���Ɣ񓯊���M���Ă�ꏊ�ő�Exception�ɂȂ��Ă��܂�
                        }
                        catch (Exception ex) { //�����ł����ƕ��邱�Ƃ��o���Ȃ��ꍇ��������
                            RuntimeUtil.SilentReportException(ex);
                        }
                    }
                    //�����őҋ@���Ă����X���b�h�������o���̂ŁA���̑O��Socket��Disconnect�͂�����Ă����B�����ɂ����������\�P�b�g�̓��삪���ɂȂ����P�[�X����B
                    NotifyAsyncClient();
                }
            }
        }

        protected virtual void MakeConnection() {
            //�܂�SOCKS���g���ׂ����ǂ����𔻒肷��
            IProtocolOptions opt = ProtocolsPlugin.Instance.ProtocolOptions;
            if (opt.UseSocks && SocksApplicapable(opt.SocksNANetworks, IPAddressList.SilentGetAddress(_host))) {
                _socks = new Socks();
                _socks.Account = opt.SocksAccount;
                _socks.Password = opt.SocksPassword;
                _socks.DestName = _host;
                _socks.DestPort = (short)_port;
                _socks.ServerName = opt.SocksServer;
                _socks.ServerPort = (short)opt.SocksPort;
            }

            string dest = _socks == null ? _host : _socks.ServerName;
            int port = _socks == null ? _port : _socks.ServerPort;

            IPAddress address = null;
            if (IPAddress.TryParse(dest, out address)) {
                _addressSet = new IPAddressList(address); //�ŏ�����IP�A�h���X�`���̂Ƃ��͎�ŕϊ��B�����łȂ���DNS�̋t���������ă^�C���A�E�g�A�Ƃ���₱�������Ƃ��N����
            }
            else { //�z�X�g���`��
                StartOverridingErrorMessage(String.Format(PEnv.Strings.GetString("Message.AddressNotResolved"), dest));
                _addressSet = new IPAddressList(dest);
                EndOverridingErrorMessage();
            }

            StartOverridingErrorMessage(String.Format(PEnv.Strings.GetString("Message.FailedToConnectPort"), dest, port));
            _socket = NetUtil.ConnectTCPSocket(_addressSet, port);
            EndOverridingErrorMessage();
            _connectedAddress = ((IPEndPoint)_socket.RemoteEndPoint).Address;

            if (_socks != null) {
                _errorMessage = "An error occurred in SOCKS negotiation.";
                _socks.Connect(_socket);
                //�ڑ�����������_host,_port�����ɖ߂�
                _host = _socks.DestName;
                _port = _socks.DestPort;
            }

            _tcpConnected = true;
        }


        //������I�[�o�[���C�h����TCP�ڑ���̓��������
        protected abstract void Negotiate();

        //���������炱����������Č��ʂ�Ԃ�
        internal abstract TerminalConnection Result {
            get;
        }


        public bool Succeeded {
            get {
                return _succeeded;
            }
        }
        public bool Interrupted {
            get {
                return _interrupted;
            }
        }

        public string ErrorMessage {
            get {
                return _errorMessage;
            }
        }
        public IPAddress IPAddress {
            get {
                return _connectedAddress;
            }
        }

        internal IPAddressList IPAddressSet {
            get {
                return _addressSet;
            }
        }

        public Socket RawSocket {
            get {
                return _socket;
            }
        }

        private static bool SocksApplicapable(string nss, IPAddressList address) {
            foreach (string netaddress in nss.Split(';')) {
                if (netaddress.Length == 0)
                    continue;

                if (!NetAddressUtil.IsNetworkAddress(netaddress)) {
                    throw new FormatException(String.Format("{0} is not suitable as a network address.", netaddress));
                }
                if (address.AvailableAddresses.Length > 0 && NetAddressUtil.NetAddressIncludesIPAddress(netaddress, address.AvailableAddresses[0])) //�P�����Ŕ��f�A��₳�ڂ�
                    return false;
            }
            return true;
        }
    }

}
