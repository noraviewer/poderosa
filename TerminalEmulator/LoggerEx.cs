/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: LoggerEx.cs,v 1.2 2011/12/19 17:14:35 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.Text;

using Poderosa.Document;
using Poderosa.Protocols;

namespace Poderosa.Terminal {
    /// <summary>
    /// <ja>
    /// ���O�̊��C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Base interface of the log.
    /// </en>
    /// </summary>
    public interface ILoggerBase {
        /// <summary>
        /// <ja>���O����܂��B</ja>
        /// <en>Close log</en>
        /// </summary>
        void Close();
        /// <summary>
        /// <ja>���O���t���b�V�����܂��B</ja>
        /// <en>Flush log</en>
        /// </summary>
        void Flush();
        /// <summary>
        /// <ja>�����t���b�V���̏������s���܂��B</ja>
        /// <en>Do the auto flush.</en>
        /// </summary>
        void AutoFlush();
    }

    /// <summary>
    /// <ja>
    /// �o�C�i���̃��K�[�������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that show the logger of binary.
    /// </en>
    /// </summary>
    public interface IBinaryLogger : ILoggerBase {
        /// <summary>
        /// <ja>�o�C�i�����O���������݂܂��B</ja><en>Write a binary log</en>
        /// </summary>
        /// <param name="data"><ja>�������܂�悤�Ƃ��Ă���f�[�^�ł��B</ja><en>Data to write.</en></param>
        /// <remarks>
        /// <ja>�o�C�i�����K�[�̎����҂́A<paramref name="data"/>�ɓn���ꂽ�f�[�^���������ނ悤�Ɏ������܂��B</ja><en>Those who implements about binary logger implement like writing the data passed to <paramref name="data"/>. </en>
        /// </remarks>
        void Write(ByteDataFragment data);
    }

    /// <summary>
    /// <ja>
    /// �e�L�X�g�̃��K�[�������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that show the logger of text.
    /// </en>
    /// </summary>
    public interface ITextLogger : ILoggerBase {
        /// <summary>
        /// <ja>
        /// �e�L�X�g���O���������݂܂��B
        /// </ja>
        /// <en>Write a text log</en>
        /// </summary>
        /// <param name="line"><ja>�������܂�悤�Ƃ��Ă���f�[�^�ł��B</ja><en>Data to write.</en></param>
        /// <remarks>
        /// <ja>�e�L�X�g���K�[�̎����҂́A<paramref name="line"/>�ɓn���ꂽ�f�[�^���������ނ悤�Ɏ������܂��B</ja><en>Those who implements about text logger implement like writing the data passed to <paramref name="line"/>. </en>
        /// </remarks>
        void WriteLine(GLine line); //�e�L�X�g�x�[�X��Line�P��
        /// <summary>
        /// <ja>�R�����g���������݂܂��B</ja>
        /// <en>Write a comment</en>
        /// </summary>
        /// <param name="comment"><ja>�������܂�悤�Ƃ��Ă���R�����g�ł��B</ja><en>Comment to write.</en></param>
        /// <remarks>
        /// <ja>�e�L�X�g���K�[�̎����҂́A<paramref name="comment"/>�ɓn���ꂽ�f�[�^���������ނ悤�Ɏ������܂��B</ja><en>Those who implements about text logger implement like writing the data passed to <paramref name="comment"/>. </en>
        /// </remarks>
        void Comment(string comment);
    }

    /// <summary>
    /// <ja>
    /// XML�̃��K�[�������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that show the logger of XML.
    /// </en>
    /// </summary>
    public interface IXmlLogger : ILoggerBase {
        /// <summary>
        /// <ja>
        /// XML���O���������݂܂��B
        /// </ja>
        /// <en>Write a XML log</en>
        /// </summary>
        /// <param name="ch"><ja>�������܂�悤�Ƃ��Ă���f�[�^�ł��B</ja><en>Data to write.</en></param>
        /// <remarks>
        /// <ja>XML���K�[�̎����҂́A<paramref name="char"/>�ɓn���ꂽ�f�[�^���������ނ悤�Ɏ������܂��B</ja><en>Those who implements about XML logger implement like writing the data passed to <paramref name="char"/>. </en>
        /// </remarks>
        void Write(char ch);
        /// <summary>
        /// <ja>
        /// XML���O���G�X�P�[�v���ď������݂܂��B
        /// </ja>
        /// <en>
        /// writes log escaping in the XML.
        /// </en>
        /// </summary>
        /// <param name="body"><ja>�������܂�悤�Ƃ��Ă���f�[�^�ł��B</ja><en>Data to write.</en></param>
        /// <remarks>
        /// <ja>XML���K�[�̎����҂́A<paramref name="body"/>�ɓn���ꂽ�f�[�^���G�X�P�[�v���ď������ނ悤�Ɏ������܂��B</ja>
        /// <en>Those who implements about XML logger implement like writing the data passed to <paramref name="body"/> with escaping. </en>
        /// </remarks>
        void EscapeSequence(char[] body);
        /// <summary>
        /// <ja>
        /// �R�����g���������݂܂��B
        /// </ja>
        /// <en>Write a comment</en>
        /// </summary>
        /// <param name="comment"><ja>�������܂�悤�Ƃ��Ă���R�����g�ł��B</ja><en>Comment to write.</en></param>
        /// <remarks>
        /// <ja>XML���K�[�̎����҂́A<paramref name="comment"/>�ɓn���ꂽ�f�[�^���������ނ悤�Ɏ������܂��B</ja><en>Those who implements about XML logger implement like writing the data passed to <paramref name="comment"/>. </en>
        /// </remarks>
        void Comment(string comment);
    }

    /// <summary>
    /// <ja>
    /// ���O�T�[�r�X�ɃA�N�Z�X����C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface accessed log service.
    /// </en>
    /// </summary>
    public interface ILogService {
        /// <summary>
        /// <ja>
        /// �o�C�i���̃��K�[��o�^���܂��B
        /// </ja>
        /// <en>
        /// Regist the logger of binary
        /// </en>
        /// </summary>
        /// <param name="logger"><ja>�o�^���郍�K�[</ja><en>Logger to regist.</en></param>
        void AddBinaryLogger(IBinaryLogger logger);
        /// <summary>
        /// <ja>
        /// �o�C�i���̃��K�[���������܂��B
        /// </ja>
        /// <en>
        /// Remove the logger of binary
        /// </en>
        /// </summary>
        /// <param name="logger"><ja>�������郍�K�[</ja><en>Logger to remove.</en></param>
        void RemoveBinaryLogger(IBinaryLogger logger);
        /// <summary>
        /// <ja>
        /// �e�L�X�g�̃��K�[��o�^���܂��B
        /// </ja>
        /// <en>
        /// Regist the logger of text
        /// </en>
        /// </summary>
        /// <param name="logger"><ja>�o�^���郍�K�[</ja><en>Logger to regist.</en></param>
        void AddTextLogger(ITextLogger logger);
        /// <summary>
        /// <ja>
        /// �e�L�X�g�̃��K�[���������܂��B
        /// </ja>
        /// <en>
        /// Remove the logger of text
        /// </en>
        /// </summary>
        /// <param name="logger"><ja>�������郍�K�[</ja><en>Logger to remove.</en></param>
        void RemoveTextLogger(ITextLogger logger);
        /// <summary>
        /// <ja>
        /// XML�̃��K�[��o�^���܂��B
        /// </ja>
        /// <en>
        /// Regist the logger of XML
        /// </en>
        /// </summary>
        /// <param name="logger"><ja>�o�^���郍�K�[</ja><en>Logger to regist.</en></param>
        void AddXmlLogger(IXmlLogger logger);
        /// <summary>
        /// <ja>
        /// XML�̃��K�[���������܂��B
        /// </ja>
        /// <en>
        /// Remove the logger of XML
        /// </en>
        /// </summary>
        /// <param name="logger"><ja>�������郍�K�[</ja><en>Logger to remove.</en></param>
        void RemoveXmlLogger(IXmlLogger logger);

        /// <summary>
        /// <ja>
        /// ���O�ݒ�𔽉f�����܂��B
        /// </ja>
        /// <en>
        /// Apply the log setting.
        /// </en>
        /// </summary>
        /// <param name="settings"><ja>���O�̐ݒ�</ja><en>Set of log.</en></param>
        /// <param name="clear_previous"><ja>�ݒ�O�ɃN���A���邩�ǂ����̃t���O</ja><en>Flag whether clear before it sets it</en></param>
        /// <exclude/>
        void ApplyLogSettings(ILogSettings settings, bool clear_previous);
        /// <summary>
        /// <ja>
        /// ���O�̃R�����g��ݒ肵�܂��B
        /// </ja>
        /// <en>
        /// Set the comment on the log.
        /// </en>
        /// </summary>
        /// <param name="comment"><ja>�ݒ肷��R�����g</ja><en>Comment to set.</en></param>
        /// <exclude/>
        void Comment(string comment);
    }

}
