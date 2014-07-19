/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: TerminalConnectionT.cs,v 1.1 2010/11/19 15:41:03 kzmi Exp $
 */
#if UNITTEST
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Net.Sockets;
using System.IO;

using Granados;

using Poderosa.Boot;
using Poderosa.Plugins;

using NUnit.Framework;

namespace Poderosa.Protocols
{
    //TerminalConnection�ɂ��Ẵe�X�g
    //�@�]���āA�ڑ����m������܂ł̓N���A���Ă���Ƃ����O��ł���B

    [TestFixture]
    public class TerminalConnectionTests {
        private static IPoderosaApplication _poderosaApplication;
        private static IPoderosaWorld _poderosaWorld;
        private static IProtocolService _protocolService;

        private static string CreatePluginManifest() {
            return String.Format("Root {{\r\n  {0} {{\r\n  plugin=Poderosa.Preferences.PreferencePlugin\r\n  plugin=Poderosa.Protocols.ProtocolsPlugin\r\n}}\r\n}}", "Poderosa.Monolithic.dll");
        }
        internal class TestReceiver : IByteAsyncInputStream {
            private int _normalTerminateCount;
            private int _abnormalTerminateCount;
            private string _message;
            private Thread _mainThread;

            public TestReceiver() {
                _mainThread = Thread.CurrentThread;
            }

            //�������Ă��邱�Ƃ��m�F���AClose����
            public void AssertNormalTerminate() {
                Assert.AreEqual(1, _normalTerminateCount);
                Assert.AreEqual(0, _abnormalTerminateCount);
            }
            //���s���Ă��邱�Ƃ̊m�F
            public void AssertAbnormalTerminate() {
                Assert.AreEqual(0, _normalTerminateCount);
                Assert.AreEqual(1, _abnormalTerminateCount);
                Assert.IsNotNull(_message);
                Debug.WriteLine(_message);
            }


            //�������͔񓯊��ł��邱�Ƃ��m�F
            public void OnReception(ByteDataFragment data) {
                Assert.AreNotEqual(Thread.CurrentThread, _mainThread);
            }

            public void OnNormalTermination() {
                Assert.AreNotEqual(Thread.CurrentThread, _mainThread);
                _normalTerminateCount++;
            }

            public void OnAbnormalTermination(string message) {
                Assert.AreNotEqual(Thread.CurrentThread, _mainThread);
                _message = message;
                _abnormalTerminateCount++;
            }
        }


        [TestFixtureSetUp]
        public void Init() {
            try {
                _poderosaApplication = PoderosaStartup.CreatePoderosaApplication(CreatePluginManifest(), new StructuredText("Poderosa"));
                _poderosaWorld = _poderosaApplication.Start();
                _protocolService = ProtocolsPlugin.Instance;
            }
            catch(Exception ex) {
                Debug.WriteLine(ex.StackTrace);
            }
        }

        [TestFixtureTearDown]
        public void Terminate() {
            _poderosaApplication.Shutdown();
        }

        //�ȉ���CreateXXXConnection�n�ŃZ�b�g�����
        private Socket _rawsocket;
        private TestReceiver _testReceiver;

        [Test]
        public void T00_TelnetClose() {
            ITerminalConnection con = CreateTelnetConnection();
            Thread.Sleep(100); //Telnet�̓l�S�V�G�[�V����������̂ŁA�����҂��ēr���܂Ői�܂���

            con.Close();
            Assert.IsTrue(con.IsClosed);
            Thread.Sleep(100); //�񓯊��Ȃ̂ł�����Ƒ҂�
            _testReceiver.AssertNormalTerminate();
            Assert.IsFalse(_rawsocket.Connected); //�؂ꂽ���Ɗm�F
        }

        [Test]
        public void T01_TelnetCrash() {
            ITerminalConnection con = CreateTelnetConnection();
            Thread.Sleep(100); 

            _rawsocket.Close(); //�l�b�g���[�N�G���[��z��
            Thread.Sleep(100); //�񓯊��Ȃ̂ł�����Ƒ҂�
            _testReceiver.AssertAbnormalTerminate();
            Assert.IsTrue(con.IsClosed);
        }

        [Test]
        public void T02_SSH2Close() {
            ITerminalConnection con = CreateSSHConnection(SSHProtocol.SSH2);

            con.Close();
            Assert.IsTrue(con.IsClosed);
            Thread.Sleep(100); //�񓯊��Ȃ̂ł�����Ƒ҂�
            _testReceiver.AssertNormalTerminate();
            //Assert.IsFalse(_rawsocket.Connected); //�؂ꂽ���Ɗm�F //�����ʂ��ĂȂ��B������ƌ��
        }

        [Test]
        public void T03_SSH2Crash() {
            ITerminalConnection con = CreateSSHConnection(SSHProtocol.SSH2);

            _rawsocket.Close();
            Thread.Sleep(100); //�񓯊��Ȃ̂ł�����Ƒ҂�
            Assert.IsTrue(con.IsClosed);
            _testReceiver.AssertAbnormalTerminate();
        }

        [Test]
        public void T04_SSH1Close() {
            ITerminalConnection con = CreateSSHConnection(SSHProtocol.SSH1);

            con.Close();
            Assert.IsTrue(con.IsClosed);
            Thread.Sleep(100); //�񓯊��Ȃ̂ł�����Ƒ҂�
            _testReceiver.AssertNormalTerminate();
            Assert.IsFalse(_rawsocket.Connected); //�؂ꂽ���Ɗm�F
        }

        [Test]
        public void T05_SSH1Crash() {
            ITerminalConnection con = CreateSSHConnection(SSHProtocol.SSH1);

            _rawsocket.Close();
            Thread.Sleep(100); //�񓯊��Ȃ̂ł�����Ƒ҂�
            Assert.IsTrue(con.IsClosed);
            _testReceiver.AssertAbnormalTerminate();
        }

        //TODO �V���A���p�e�X�g�P�[�X�͒ǉ��K�v�BCygwin��Telnet�Ɠ����Ȃ̂ł܂��������낤

        //TODO ITerminalOutput�̃e�X�g�B���������M���ꂽ�����m�F����͓̂�����������邪

        //TODO Reproduce�T�|�[�g�̌�ASSH2��1Connection-����Channel���J���A�ʂɊJ���Ă݂�

        private ITerminalConnection CreateTelnetConnection() {
            ITCPParameter tcp = _protocolService.CreateDefaultTelnetParameter();
            tcp.Destination = UnitTestUtil.GetUnitTestConfig("protocols.telnet_connectable");
            Debug.Assert(tcp.Port==23);
            
            ISynchronizedConnector sc = _protocolService.CreateFormBasedSynchronozedConnector(null);
            IInterruptable t = _protocolService.AsyncTelnetConnect(sc.InterruptableConnectorClient, tcp);
            ITerminalConnection con =  sc.WaitConnection(t, 5000);

            //Assert.ReferenceEquals(con.Destination, tcp); //�Ȃ����������Ȃ�
            Debug.Assert(con.Destination==tcp);
            _rawsocket = ((InterruptableConnector)t).RawSocket;
            _testReceiver = new TestReceiver();
            con.Socket.RepeatAsyncRead(_testReceiver);
            return con;
        }

        private ITerminalConnection CreateSSHConnection(SSHProtocol sshprotocol) {
            ISSHLoginParameter ssh = _protocolService.CreateDefaultSSHParameter();
            ssh.Method = sshprotocol;
            ssh.Account = UnitTestUtil.GetUnitTestConfig("protocols.ssh_account");
            ssh.PasswordOrPassphrase = UnitTestUtil.GetUnitTestConfig("protocols.ssh_password");
            ITCPParameter tcp = (ITCPParameter)ssh.GetAdapter(typeof(ITCPParameter));
            tcp.Destination = UnitTestUtil.GetUnitTestConfig("protocols.ssh_connectable");
            Debug.Assert(tcp.Port==22);

            ISynchronizedConnector sc = _protocolService.CreateFormBasedSynchronozedConnector(null);
            IInterruptable t = _protocolService.AsyncSSHConnect(sc.InterruptableConnectorClient, ssh);
            ITerminalConnection con =  sc.WaitConnection(t, 5000);

            Debug.Assert(con.Destination==ssh);
            _rawsocket = ((InterruptableConnector)t).RawSocket;
            _testReceiver = new TestReceiver();
            con.Socket.RepeatAsyncRead(_testReceiver);
            return con;
        }
    }
}
#endif