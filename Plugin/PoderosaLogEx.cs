/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: PoderosaLogEx.cs,v 1.2 2011/10/27 23:21:56 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace Poderosa {
    /// <summary>
    /// <ja>
    /// ���O�̃J�e�S���������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface of category of log.
    /// </en>
    /// </summary>
    public interface IPoderosaLogCategory : IAdaptable {
        /// <summary>
        /// <ja>
        /// �J�e�S�����ł��B
        /// </ja>
        /// <en>
        /// Name of category.
        /// </en>
        /// </summary>
        string Name {
            get;
        }
    }
    /// <summary>
    /// <ja>
    /// ���O�̃A�C�e���������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface of log items
    /// </en>
    /// </summary>
    public interface IPoderosaLogItem : IAdaptable {
        /// <summary>
        /// <ja>
        /// ���O�̃J�e�S���ł��B
        /// </ja>
        /// <en>
        /// Category of log.
        /// </en>
        /// </summary>
        IPoderosaLogCategory Category {
            get;
        }
        /// <summary>
        /// <ja>
        /// ���O�̃e�L�X�g�ł��B
        /// </ja>
        /// <en>
        /// Text of log.
        /// </en>
        /// </summary>
        string Text {
            get;
        }

        /// <summary>
        /// <ja>
        /// ���O�A�C�e���̃C���f�b�N�X�ԍ��ł��B
        /// </ja>
        /// <en>
        /// Index number of log item.
        /// </en>
        /// </summary>
        int Index {
            get;
        }
        //Time������������Ă�������
    }

    /// <summary>
    /// <ja>
    /// ���O�@�\��񋟂��܂��B
    /// </ja>
    /// <en>
    /// Offered log function.
    /// </en>
    /// </summary>
    public interface IPoderosaLog : IAdaptable, IEnumerable<IPoderosaLogItem> {
        /// <summary>
        /// <ja>
        /// ���O�̗e�ʂ��擾�^�ݒ肵�܂��B
        /// </ja>
        /// <en>
        /// Get / set the capacity of log.
        /// </en>
        /// </summary>
        int Capacity {
            get;
            set;
        }
        /// <summary>
        /// <ja>
        /// ���O�ɏ������݂܂��B
        /// </ja>
        /// <en>
        /// Write log.
        /// </en>
        /// </summary>
        /// <param name="category">
        /// <ja>���O�̃J�e�S���ł��B</ja>
        /// <en>Category of log.</en>
        /// </param>
        /// <param name="text">
        /// <ja>�������ރe�L�X�g�ł��B</ja>
        /// <en>Text to write.</en>
        /// </param>
        void AddItem(IPoderosaLogCategory category, string text);
        /// <summary>
        /// <ja>
        /// ���O�̃A�C�e���̌��������܂��B
        /// </ja>
        /// <en>
        /// Count of log item.
        /// </en>
        /// </summary>
        int Count {
            get;
        }

        /// <summary>
        /// <ja>
        /// ���O�̃��X�i��o�^���܂��B
        /// </ja>
        /// <en>
        /// Add the log listener
        /// </en>
        /// </summary>
        /// <param name="listener">
        /// <ja>�o�^���郊�X�i</ja>
        /// <en>Listener to regist.</en>
        /// </param>
        void AddChangeListener(IPoderosaLogListener listener);

        /// <summary>
        /// <ja>
        /// ���O�̃��X�i���������܂��B
        /// </ja>
        /// <en>
        /// Remove the log listener
        /// </en>
        /// </summary>
        /// <param name="listener">
        /// <ja>�������郊�X�i</ja>
        /// <en>Listener to release.</en>
        /// </param>
        void RemoveChangeListener(IPoderosaLogListener listener);

        /// <summary>
        /// <ja>
        /// �W���̃��O�J�e�S���������܂��B
        /// </ja>
        /// <en>
        /// Get the generic category of log.
        /// </en>
        /// </summary>
        IPoderosaLogCategory GenericCategory {
            get;
        }

    }

    /// <summary>
    /// <ja>
    /// ���O���X�i�������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface of loglistener.
    /// </en>
    /// </summary>
    public interface IPoderosaLogListener {
        /// <summary>
        /// <ja>
        /// �V�������O�A�C�e�����ǉ������Ƃ��ɌĂяo����܂��B
        /// </ja>
        /// <en>
        /// Called when added the new log item.
        /// </en>
        /// </summary>
        /// <param name="item">
        /// <ja>�ǉ������A�C�e���ł��B</ja>
        /// <en>Item to add.</en>
        /// </param>
        void OnNewItem(IPoderosaLogItem item);
    }

    /// <summary>
    /// <ja>
    /// ���O�J�e�S������������w���p�N���X�ł��B
    /// </ja>
    /// <en>
    /// Helperclasss for implementation of category of log.
    /// </en>
    /// </summary>
    public class PoderosaLogCategoryImpl : IPoderosaLogCategory {
        private string _name;
        /// <summary>
        /// <ja>
        /// ���O�J�e�S�����쐬���܂��B
        /// </ja>
        /// <en>
        /// Create the category of log.
        /// </en>
        /// </summary>
        /// <param name="name">
        /// <ja>���O�J�e�S���̖��O�ł��B</ja>
        /// <en>Name of category of log.</en>
        /// </param>
        public PoderosaLogCategoryImpl(string name) {
            _name = name;
        }
        /// <summary>
        /// <ja>
        /// ���O�J�e�S������Ԃ��܂��B
        /// </ja>
        /// <en>
        /// Get the category of log.
        /// </en>
        /// </summary>
        public string Name {
            get {
                return _name;
            }
        }

        public IAdaptable GetAdapter(Type adapter) {
            return PoderosaLog.Instance.PoderosaWorld.AdapterManager.GetAdapter(this, adapter);
        }
    }

    /// <summary>
    /// <ja>
    /// ���O�A�C�e�����\������w���p�N���X�ł��B
    /// </ja>
    /// <en>
    /// Helper class to compose the log item.
    /// </en>
    /// </summary>
    public class PoderosaLogItemImpl : IPoderosaLogItem {
        private IPoderosaLogCategory _category;
        private string _text;
        private int _index;

        /// <summary>
        /// <ja>
        /// ���O�A�C�e�����쐬���܂��B
        /// </ja>
        /// <en>
        /// Create the log item.
        /// </en>
        /// </summary>
        /// <param name="category"><ja>���O�A�C�e���̃J�e�S���ł��B</ja>
        /// <en>Category of log item.</en>
        /// </param>
        /// <param name="text"><ja>���O�̃e�L�X�g�ł��B</ja>
        /// <en>Text of log.</en>
        /// </param>
        /// <param name="index"><ja>���O�̃C���f�b�N�X�ʒu�ł��B</ja>
        /// <en>Index position of log.</en></param>
        public PoderosaLogItemImpl(IPoderosaLogCategory category, string text, int index) {
            _category = category;
            _text = text;
            _index = index;
        }

        /// <summary>
        /// <ja>
        /// ���O�J�e�S���������܂��B
        /// </ja>
        /// <en>
        /// Category of log.
        /// </en>
        /// </summary>
        public IPoderosaLogCategory Category {
            get {
                return _category;
            }
        }

        /// <summary>
        /// <ja>
        /// ���O�̃e�L�X�g�������܂��B
        /// </ja>
        /// <en>
        /// Text of log.
        /// </en>
        /// </summary>
        public string Text {
            get {
                return _text;
            }
        }

        /// <summary>
        /// <ja>
        /// ���O�̃C���f�b�N�X�ʒu�������܂��B
        /// </ja>
        /// <en>
        /// Index of log.
        /// </en>
        /// </summary>
        public int Index {
            get {
                return _index;
            }
        }

        public IAdaptable GetAdapter(Type adapter) {
            return PoderosaLog.Instance.PoderosaWorld.AdapterManager.GetAdapter(this, adapter);
        }
    }
}
