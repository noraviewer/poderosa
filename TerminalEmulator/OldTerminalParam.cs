/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: OldTerminalParam.cs,v 1.12 2012/03/18 14:15:24 kzmi Exp $
 */
using System;
using System.Diagnostics;
using System.Collections;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

using Poderosa.Document;
using Poderosa.Terminal;
using Poderosa.Util;
using Poderosa.View;
using Poderosa.Protocols;

namespace Poderosa.ConnectionParam {

    /*
     * TerminalParam�̓}�N��������t���ɃA�N�Z�X�\�ɂ��邽��public�ɂ���
     * ���J����K�v�̂Ȃ����\�b�h��internal�ɂ���
     */

    //Granados����AuthenticationType�Ɠ��ꂾ���A�N���̍������̂��ߎg��Ȃ�

    /// <summary>
    /// <ja>SSH�ł̔F�ؕ��@�������܂��B</ja>
    /// <en>Specifies the authemtication method of SSH.</en>
    /// </summary>
    /// <exclude/>
    public enum AuthType {
        /// <summary>
        /// <ja>�p�X���[�h�F��</ja>
        /// <en>Authentication using password.</en>
        /// </summary>
        [EnumValue(Description = "Enum.AuthType.Password")]
        Password,

        /// <summary>
        /// <ja>�茳�̔閧���ƃ����[�g�z�X�g�ɓo�^�������J�����g�����F��</ja>
        /// <en>Authentication using the local private key and the remote public key.</en>
        /// </summary>
        [EnumValue(Description = "Enum.AuthType.PublicKey")]
        PublicKey,

        /// <summary>
        /// <ja>�R���\�[����Ńp�X���[�h����͂���F��</ja>
        /// <en>Authentication by sending the password through the console.</en>
        /// </summary>
        [EnumValue(Description = "Enum.AuthType.KeyboardInteractive")]
        KeyboardInteractive
    }

    /// <summary>
    /// <ja>�ڑ��̎�ނ������܂��B</ja>
    /// <en>Specifies the type of the connection.</en>
    /// </summary>
    /// <exclude/>
    public enum ConnectionMethod {
        /// <summary>
        /// Telnet
        /// </summary>
        Telnet,
        /// <summary>
        /// SSH1
        /// </summary>
        SSH1,
        /// <summary>
        /// SSH2
        /// </summary>
        SSH2
    }

    /// <summary>
    /// <ja>�G���R�[�f�B���O�������܂��B</ja>
    /// <en>Specifies the encoding of the connection.</en>
    /// <!--
    /// <seealso cref="Poderosa.ConnectionParam.TerminalParam.Encoding"/>
    /// -->
    /// </summary>
    /// <exclude/>
    public enum EncodingType {

        // For supporting the third-party plugin, integer value of each member shouldn't be changed.
        // However, the item order according to the integer value may not be suitable for the UI (like combo box).
        // So we specify integer values explicitly, and place members in order that we want to show.

        /// <summary>
        /// <ja>ISO 8859-1</ja>
        /// <en>ISO 8859-1</en>
        /// </summary>
        [EnumValue(Description = "Enum.EncodingType.ISO8859_1")]
        ISO8859_1 = 0,
        /// <summary>
        /// <ja>UTF-8 (CJK�e�L�X�g�\���p)</ja>
        /// <en>UTF-8 (for displaying CJK text)</en>
        /// </summary>
        /// <remarks>
        /// <ja>CJK�L�����N�^�Z�b�g�Ɋ܂܂��L���A�r���A�����������́ACJK�t�H���g�őS�p�\������܂��B</ja>
        /// <en>Characters like symbols, box-drawing characters or european characters that are contained in CJK character sets are displayed in zenkaku using CJK font.</en>
        /// </remarks>
        [EnumValue(Description = "Enum.EncodingType.UTF8")]
        UTF8 = 1,
        /// <summary>
        /// <ja>UTF-8 (�����\���p)</ja>
        /// <en>UTF-8 (for displaying american or european text)</en>
        /// </summary>
        /// <remarks>
        /// <ja>�L���A�r���A�����������́A���C���t�H���g�Ŕ��p�\������܂��B
        /// �������̓��A�W�A�̕�����CJK�t�H���g�őS�p�\������܂��B</ja>
        /// <en>Characters like symbols, box-drawing characters or european characters are displayed in Hankaku using main font.
        /// East asian characters like Kanji are displayed in Zenkaku using CJK font.</en>
        /// </remarks>
        [EnumValue(Description = "Enum.EncodingType.UTF8_Latin")]
        UTF8_Latin = 8,
        /// <summary>
        /// <ja>EUC JP (��ɓ��{��̕����Ŏg�p)</ja>
        /// <en>EUC JP (This encoding is primarily used with Japanese characters.)</en>
        /// </summary>
        [EnumValue(Description = "Enum.EncodingType.EUC_JP")]
        EUC_JP = 2,
        /// <summary>
        /// <ja>Shift JIS (��ɓ��{��̕����Ŏg�p)</ja>
        /// <en>Shift JIS (This encoding is primarily used with Japanese characters.)</en>
        /// </summary>
        [EnumValue(Description = "Enum.EncodingType.SHIFT_JIS")]
        SHIFT_JIS = 3,
        /// <summary>
        /// <ja>GB2312 (��Ɋȑ̎��Ŏg�p)</ja>
        /// <en>GB2312 (This encoding is primarily used with simplified Chinese characters.)</en>
        /// </summary>
        [EnumValue(Description = "Enum.EncodingType.GB2312")]
        GB2312 = 4,
        /// <summary>
        /// <ja>Big5 (��ɔɑ̎��Ŏg�p)</ja>
        /// <en>Big5 (This encoding is primarily used with traditional Chinese characters.)</en>
        /// </summary>
        [EnumValue(Description = "Enum.EncodingType.BIG5")]
        BIG5 = 5,
        /// <summary>
        /// <ja>EUC CN (��Ɋȑ̎��Ŏg�p)</ja>
        /// <en>EUC CN (This encoding is primarily used with simplified Chinese characters.)</en>
        /// </summary>
        [EnumValue(Description = "Enum.EncodingType.EUC_CN")]
        EUC_CN = 6,
        /// <summary>
        /// <ja>EUC KR (��Ɋ؍��ꕶ���Ŏg�p)</ja>
        /// <en>EUC KR (This encoding is primarily used with Korean characters.)</en>
        /// </summary>
        [EnumValue(Description = "Enum.EncodingType.EUC_KR")]
        EUC_KR = 7,
        /// <summary>
        /// <ja>OEM 850</ja>
        /// <en>OEM 850</en>
        /// </summary>
        [EnumValue(Description = "Enum.EncodingType.OEM850")]
        OEM850 = 9,
    }

    /// <summary>
    /// <ja>���O�̎�ނ������܂��B</ja>
    /// <en>Specifies the log type.</en>
    /// </summary>
    /// <exclude/>
    public enum LogType {
        /// <summary>
        /// <ja>���O�͂Ƃ�܂���B</ja>
        /// <en>The log is not recorded.</en>
        /// </summary>
        [EnumValue(Description = "Enum.LogType.None")]
        None,
        /// <summary>
        /// <ja>�e�L�X�g���[�h�̃��O�ł��B���ꂪ�W���ł��B</ja>
        /// <en>The log is a plain text file. This is standard.</en>
        /// </summary>
        [EnumValue(Description = "Enum.LogType.Default")]
        Default,
        /// <summary>
        /// <ja>�e�L�X�g���[�h�̃��O�A�^�C���X�^���v�t�B</ja>
        /// <en>Plain text file, logged with timestamp.</en>
        /// </summary>
        [EnumValue(Description = "Enum.LogType.PlainTextWithTimestamp")]
        PlainTextWithTimestamp,
        /// <summary>
        /// <ja>�o�C�i�����[�h�̃��O�ł��B</ja>
        /// <en>The log is a binary file.</en>
        /// </summary>
        [EnumValue(Description = "Enum.LogType.Binary")]
        Binary,
        /// <summary>
        /// <ja>XML�ŕۑ����܂��B�܂������I�ȃo�O�ǐՂɂ����Ă��̃��[�h�ł̃��O�̎�����肢���邱�Ƃ�����܂��B</ja>
        /// <en>The log is an XML file. We may ask you to record the log in this type for debugging.</en>
        /// </summary>
        [EnumValue(Description = "Enum.LogType.Xml")]
        Xml
    }

    /// <summary>
    /// <ja>���M���̉��s�̎�ނ������܂��B</ja>
    /// <en>Specifies the new-line characters for transmission.</en>
    /// </summary>
    /// <exclude/>
    public enum NewLine {
        /// <summary>
        /// CR
        /// </summary>
        [EnumValue(Description = "Enum.NewLine.CR")]
        CR,
        /// <summary>
        /// LF
        /// </summary>
        [EnumValue(Description = "Enum.NewLine.LF")]
        LF,
        /// <summary>
        /// CR+LF
        /// </summary>
        [EnumValue(Description = "Enum.NewLine.CRLF")]
        CRLF
    }

    /// <summary>
    /// <ja>�^�[�~�i���̎�ʂ������܂��B</ja>
    /// <en>Specifies the type of the terminal.</en>
    /// </summary>
    /// <remarks>
    /// <ja>XTerm�ɂ�VT100�ɂ͂Ȃ��������̃G�X�P�[�v�V�[�P���X���܂܂�Ă��܂��B</ja>
    /// <en>XTerm supports several escape sequences in addition to VT100.</en>
    /// <ja>KTerm�͒��g��XTerm�ƈꏏ�ł����ASSH��Telnet�̐ڑ��I�v�V�����ɂ����ă^�[�~�i���̎�ނ�����������Ƃ���"kterm"���Z�b�g����܂��B</ja>
    /// <en>Though the functionality of KTerm is identical to XTerm, the string "kterm" is used for specifying the type of the terminal in the connection of Telnet or SSH.</en>
    /// <ja>���̐ݒ�́A�����̏ꍇTERM���ϐ��̒l�ɉe�����܂��B</ja>
    /// <en>In most cases, this setting affects the TERM environment variable.</en>
    /// </remarks>
    /// <exclude/>
    public enum TerminalType {
        /// <summary>
        /// vt100
        /// </summary>
        [EnumValue(Description = "Enum.TerminalType.VT100")]
        VT100,
        /// <summary>
        /// xterm
        /// </summary>
        [EnumValue(Description = "Enum.TerminalType.XTerm")]
        XTerm,
        /// <summary>
        /// kterm
        /// </summary>
        [EnumValue(Description = "Enum.TerminalType.KTerm")]
        KTerm
    }

    /// <summary>
    /// <ja>��M���������ɑ΂�����s���@�������܂��B</ja>
    /// <en>Specifies line breaking style.</en>
    /// </summary>
    /// <exclude/>
    public enum LineFeedRule {
        /// <summary>
        /// <ja>�W��</ja>
        /// <en>Standard</en>
        /// </summary>
        [EnumValue(Description = "Enum.LineFeedRule.Normal")]
        Normal,
        /// <summary>
        /// <ja>LF�ŉ��s��CR�𖳎�</ja>
        /// <en>LF:Line Break, CR:Ignore</en>
        /// </summary>
        [EnumValue(Description = "Enum.LineFeedRule.LFOnly")]
        LFOnly,
        /// <summary>
        /// <ja>CR�ŉ��s��LF�𖳎�</ja>
        /// <en>CR:Line Break, LF:Ignore</en>
        /// </summary>
        [EnumValue(Description = "Enum.LineFeedRule.CROnly")]
        CROnly
    }

#if !MACRODOC
    /// <summary>
    /// <ja>�t���[�R���g���[���̐ݒ�</ja>
    /// <en>Specifies the flow control.</en>
    /// </summary>
    /// <exclude/>
    public enum FlowControl {
        /// <summary>
        /// <ja>�Ȃ�</ja>
        /// <en>None</en>
        /// </summary>
        [EnumValue(Description = "Enum.FlowControl.None")]
        None,
        /// <summary>
        /// X ON / X OFf
        /// </summary>
        [EnumValue(Description = "Enum.FlowControl.Xon_Xoff")]
        Xon_Xoff,
        /// <summary>
        /// <ja>�n�[�h�E�F�A</ja>
        /// <en>Hardware</en>
        /// </summary>
        [EnumValue(Description = "Enum.FlowControl.Hardware")]
        Hardware
    }

    /// <summary>
    /// <ja>�p���e�B�̐ݒ�</ja>
    /// <en>Specifies the parity.</en>
    /// </summary>
    /// <exclude/>
    public enum Parity {
        /// <summary>
        /// <ja>�Ȃ�</ja>
        /// <en>None</en>
        /// </summary>
        [EnumValue(Description = "Enum.Parity.NOPARITY")]
        NOPARITY = 0,
        /// <summary>
        /// <ja>�</ja>
        /// <en>Odd</en>
        /// </summary>
        [EnumValue(Description = "Enum.Parity.ODDPARITY")]
        ODDPARITY = 1,
        /// <summary>
        /// <ja>����</ja>
        /// <en>Even</en>
        /// </summary>
        [EnumValue(Description = "Enum.Parity.EVENPARITY")]
        EVENPARITY = 2
        //MARKPARITY  =        3,
        //SPACEPARITY =        4
    }

    /// <summary>
    /// <ja>�X�g�b�v�r�b�g�̐ݒ�</ja>
    /// <en>Specifies the stop bits.</en>
    /// </summary>
    /// <exclude/>
    public enum StopBits {
        /// <summary>
        /// <ja>1�r�b�g</ja>
        /// <en>1 bit</en>
        /// </summary>
        [EnumValue(Description = "Enum.StopBits.ONESTOPBIT")]
        ONESTOPBIT = 0,
        /// <summary>
        /// <ja>1.5�r�b�g</ja>
        /// <en>1.5 bits</en>
        /// </summary>
        [EnumValue(Description = "Enum.StopBits.ONE5STOPBITS")]
        ONE5STOPBITS = 1,
        /// <summary>
        /// <ja>2�r�b�g</ja>
        /// <en>2 bits</en>
        /// </summary>
        [EnumValue(Description = "Enum.StopBits.TWOSTOPBITS")]
        TWOSTOPBITS = 2
    }
#endif

}