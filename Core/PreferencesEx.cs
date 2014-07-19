/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: PreferencesEx.cs,v 1.3 2012/05/20 09:10:30 kzmi Exp $
 */
using System;
using System.IO;
using System.Collections;

/*
 * StructuredText�̏�ɁA�^���A�g�����U�N�V�����Ȃǂ̋@�\���ڂ���Preference�Ƃ��Ďg����悤�ɂ���
 */

namespace Poderosa.Preferences {

    // Exported Part
    /// <summary>
    /// <ja>
    /// ���[�U�[�ݒ�l�̌��،��ʂ������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows verification result of user setting value.
    /// </en>
    /// </summary>
    public interface IPreferenceValidationResult {
        /// <summary>
        /// <ja>
        /// ���؂������������ۂ��������܂��B
        /// </ja>
        /// <en>
        /// It is shown whether the verification succeeded. 
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// true�̂Ƃ������Afalse�̂Ƃ����s���Ӗ����܂��B
        /// </ja>
        /// <en>
        /// It succeeds at true, and the failure is meant at false. 
        /// </en>
        /// </remarks>
        bool Validated {
            get;
        }
        /// <summary>
        /// <ja>
        /// ���؎��̃G���[���b�Z�[�W�������܂��B
        /// </ja>
        /// <en>
        /// The error message when verifying it is shown. 
        /// </en>
        /// </summary>
        string ErrorMessage {
            get;
            set;
        }
    }

    /// <summary>
    /// <ja>
    /// ���؂����s�����Ƃ��̗�O�������N���X�ł��B
    /// </ja>
    /// <en>
    /// Class that shows exception when verification fails
    /// </en>
    /// </summary>
    public class ValidationException : Exception {
        private IPreferenceItemBase _sourceItem;
        private IPreferenceValidationResult _result;

        /// <summary>
        /// <ja>
        /// ���؂����s�����Ƃ��̗�O�𐶐����܂��B
        /// </ja>
        /// <en>
        /// The exception when the verification fails is generated. 
        /// </en>
        /// </summary>
        /// <param name="source"><ja>��O�̌����ƂȂ����\�[�X�ł��B</ja><en>Source that causes exception</en></param>
        /// <param name="result"><ja>���،��ʂł��B</ja><en>Verification result</en></param>
        public ValidationException(IPreferenceItemBase source, IPreferenceValidationResult result)
            : base(result.ErrorMessage) {
            _sourceItem = source;
            _result = result;
        }

        /// <summary>
        /// <ja>
        /// ��O�̌����ƂȂ����\�[�X�������܂��B
        /// </ja>
        /// <en>
        /// The source that causes the exception is shown. 
        /// </en>
        /// </summary>
        public IPreferenceItemBase SourceItem {
            get {
                return _sourceItem;
            }
        }

        /// <summary>
        /// <ja>
        /// ���،��ʂ������܂��B
        /// </ja>
        /// <en>
        /// The verification result is shown. 
        /// </en>
        /// </summary>
        public IPreferenceValidationResult Result {
            get {
                return _result;
            }
        }
    }

    //Preference Plugin����
    /// <summary>
    /// <ja>
    /// PreferencePlugin�v���O�C�����񋟂���C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that PreferencePlugin plug-in offers.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// ���̃C���^�[�t�F�C�X�́A<seealso cref="Poderosa.Plugins.ICoreServices">ICoreServices</seealso>��<see cref="Poderosa.Plugins.ICoreServices.Preferences">Preferences�v���p�e�B</see>
    /// ����擾�ł��܂��B
    /// </ja>
    /// <en>
    /// This interface can be get from the <see cref="Poderosa.Plugins.ICoreServices.Preferences">Preferences property</see> of <seealso cref="Poderosa.Plugins.ICoreServices">ICoreServices</seealso>. 
    /// </en>
    /// </remarks>
    /// <example>
    /// <ja>
    /// IPreferences�𓾂܂��B
    /// <code>
    /// ICoreServices cs = PoderosaWorld.GetAdapter(typeof(ICoreServices));
    /// // IPreferences���擾���܂�
    /// IPreferences pref = cs.Preferences;
    /// </code>
    /// </ja>
    /// <en>
    /// Get IPreferences.
    /// <code>
    /// ICoreServices cs = PoderosaWorld.GetAdapter(typeof(ICoreServices));
    /// // Get IPreferences.
    /// IPreferences pref = cs.Preferences;
    /// </code>
    /// </en>
    /// </example>
    public interface IPreferences {
        /// <summary>
        /// <ja>
        /// �t�H���_���������܂��B
        /// </ja>
        /// <en>
        /// Retrieve the folder.
        /// </en>
        /// </summary>
        /// <param name="id"><ja>��������t�H���_��</ja><en>Retrieved folder name</en></param>
        /// <returns><ja>���������t�H���_������IPreferenceFolder�B������Ȃ��Ƃ��ɂ�null</ja><en>IPreferenceFolder that shows found folder. When not found, null returns.</en></returns>
        IPreferenceFolder FindPreferenceFolder(string id);
        /// <summary>
        /// <ja>���ׂẴt�H���_��z��Ƃ��ē��܂��B</ja>
        /// <en>All folders are obtained as an array.</en>
        /// </summary>
        /// <returns><ja>�ێ����Ă��邷�ׂẴt�H���_</ja><en>All held folders</en></returns>
        IPreferenceFolder[] GetAllFolders();
    }


    //�ݒ荀�ڂ��`���鑤����
    /// <summary>
    /// <ja>
    /// ���[�U�[�ݒ荀�ڂ��`����v���O�C�����������ׂ��C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that plug-in that defines user setting item should implement.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// <para>
    /// ���[�U�[�ݒ荀�ڂ�ǂݏ�������v���O�C���́A���̃C���^�[�t�F�C�X�����������I�u�W�F�N�g��p�ӂ��A
    /// PreferencePlugin�v���O�C�����񋟂���uorg.poderosa.core.preferences�v�Ƃ����g���|�C���g�ւ�
    /// �o�^���܂��B
    /// </para>
    /// <para>
    /// �uorg.poderosa.core.preferences�v�g���|�C���g�́A<seealso cref="Poderosa.Plugins.ICoreServices">ICoreServices</seealso>��
    /// <see cref="Poderosa.Plugins.ICoreServices.PreferenceExtensionPoint">PreferenceExtensionPoint�v���p�e�B</see>����擾�ł��܂��B
    /// </para>
    /// <code>
    /// ICoreServices cs = PoderosaWorld.GetAdapter(typeof(ICoreServices));
    /// // PreferencesPlugin�v���O�C���̊g���|�C���g���擾���܂�
    /// IExtensionPoint prefext =cs.PreferenceExtensionPoint;
    /// 
    /// // ���g��o�^���܂��B
    /// prefext.RegisterExtension(this);
    /// </code>
    /// <para>
    /// ��̓I�Ȏg�����ɂ��ẮA<see href="/chap04_05.html">���[�U�[�ݒ�l�̑���</see>���Q�Ƃ��Ă��������B
    /// </para>
    /// </ja>
    /// <en>
    /// <para>
    /// The object that implements this interface is prepared, and the plug-in to read and 
    /// write the user setting item is registered to the extension point of "org.poderosa.core.preferences" 
    /// that the PreferencePlugin plug-in offers. 
    /// </para>
    /// <para>
    /// "org.poderosa.core.preferences" The extension point can be got from the 
    /// <see cref="Poderosa.Plugins.ICoreServices.PreferenceExtensionPoint">PreferenceExtensionPoint property</see> 
    /// of <seealso cref="Poderosa.Plugins.ICoreServices">ICoreServices</seealso>. 
    /// </para>
    /// <code>
    /// ICoreServices cs = PoderosaWorld.GetAdapter(typeof(ICoreServices));
    /// // Get the extension point of PreferencesPlugin plug-in.
    /// IExtensionPoint prefext =cs.PreferenceExtensionPoint;
    /// 
    /// // Regist this.
    /// prefext.RegisterExtension(this);
    /// </code>
    /// <para>
    /// Please refer to <see href="/chap04_05.html">Operation of user setting value</see> for a concrete usage. 
    /// </para>
    /// </en>
    /// </remarks>
    public interface IPreferenceSupplier {
        /// <summary>
        /// <ja>
        /// �ݒ�l���v���O�C�����ƂɎ��ʂ��鍀�ږ��ł��B
        /// </ja>
        /// <en>
        /// Item name that identifies set value of each plug-in.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <para>
        /// ���̒l�́Aoptions.conf�ɏ������܂��Ƃ��̃��[�g�����̖��O�Ƃ��č̗p����܂��B���̃v���O�C���Əd�����Ȃ��悤�ɂ��邽�߁A
        /// �v���O�C��ID�Ɠ������̂�ݒ肷�邱�Ƃ���������܂��i�����đ��̃v���O�C���̐ݒ�l��ǂݏ����������ꍇ�ɂ́A���̌���ł͂���܂���j�B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// This value is adopted as a name right under the route when written in options.conf. 
        /// To make it not overlap with other plug-ins, the same one as plug-in ID is recommended to be set 
        /// (It is not this to dare to read and write a set value of other plug-ins). 
        /// </para>
        /// </en>
        /// </remarks>
        string PreferenceID {
            get;
        }
        /// <summary>
        /// <ja>
        /// ����������PreferencesPlugin�v���O�C������Ăяo����郁�\�b�h�ł��B
        /// </ja>
        /// <en>
        /// Method that calls from PreferencesPlugin plug-in when initializing it
        /// </en>
        /// </summary>
        /// <param name="builder"><ja>�ݒ荀�ڂ�o�^���邽�߂̃C���^�[�t�F�C�X�ł��B</ja><en>Interface to register set item</en></param>
        /// <param name="folder"><ja>�e�ƂȂ�t�H���_�ł��B</ja><en>Folder that becomes parents</en></param>
        /// <remarks>
        /// <ja>
        /// �J���҂́A���̃��\�b�h����<paramref name="bulder"/>�̊e���\�b�h���Ăяo���āA
        /// �ݒ�l��o�^���܂��B
        /// </ja>
        /// <en>
        /// The developer calls each method of <paramref name="bulder"/> in this method, and registers a set value. 
        /// </en>
        /// </remarks>
        void InitializePreference(IPreferenceBuilder builder, IPreferenceFolder folder);
        /// <summary>
        /// <ja>
        /// ���[�U�[�ݒ�l����L�̃C���^�[�t�F�C�X�ւƕϊ��������Ƃ��ɗp���܂��B
        /// </ja>
        /// <en>
        /// It uses it to convert the user setting value into a peculiar interface. 
        /// </en>
        /// </summary>
        /// <param name="folder"><ja>�e�ƂȂ�t�H���_�ł��B</ja><en>Folder that becomes parents</en></param>
        /// <param name="type"><ja>�ϊ����悤�Ƃ���^�ł��B</ja><en>Type that tries to be converted</en></param>
        /// <returns><ja>�ϊ���̌^��Ԃ��܂��B</ja><en>The type after it converts it is returned. </en></returns>
        /// <remarks>
        /// <ja>
        /// �^�̕ϊ��@�\��K�v�Ƃ��Ȃ��Ƃ��ɂ́A�P����null��Ԃ��悤�Ɏ������Ă��������B
        /// </ja>
        /// <en>
        /// Please implement to return null simply when the conversion function of the type is not needed. 
        /// </en>
        /// </remarks>
        object QueryAdapter(IPreferenceFolder folder, Type type);

        /// <summary>
        /// <ja>
        /// �����t�H���_�Ɋ܂܂�镡���̃��[�U�[�ݒ�l�������������Ƃ��ɗp���܂��B
        /// </ja>
        /// <en>
        /// It uses it to inspect two or more user setting values included in the same folder. 
        /// </en>
        /// </summary>
        /// <param name="folder"><ja>���؂̑ΏۂƂȂ�t�H���_���n����܂��B</ja><en>Target folder for verification</en></param>
        /// <param name="output"><ja>���،��ʂ�ݒ肵�܂��B</ja><en>The verification result is set. </en></param>
        /// <remarks>
        /// <ja>
        /// �����̃��[�U�[�ݒ�l����������@�\��K�v�Ƃ��Ȃ��Ƃ��ɂ́A���̏����͋�ł��܂��܂���B
        /// </ja>
        /// <en>
        /// This processing is not cared about in the sky when the function to inspect two or more user setting values is not needed. 
        /// </en>
        /// </remarks>
        void ValidateFolder(IPreferenceFolder folder, IPreferenceValidationResult output);
    }

    //�ύX�ʒm
    /// <summary>
    /// <ja>
    /// �t�H���_���̃��[�U�[�ݒ�l���ω������Ƃ��ɒʒm���󂯎��C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that receives notification when user setting value in folder changes.
    /// </en>
    /// </summary>
    public interface IPreferenceChangeListener {
        /// <summary>
        /// <ja>
        /// ���[�U�[�ݒ�l���ω������Ƃ��ɌĂяo����郁�\�b�h�ł��B
        /// </ja>
        /// <en>
        /// Method of call when user setting value changes.
        /// </en>
        /// </summary>
        /// <param name="oldvalues"><ja>�ݒ�O�̌Â��l�ł��B</ja><en>Old value before it sets it</en></param>
        /// <param name="newvalues"><ja>�ݒ��̐V�����l�ł��B</ja><en>New value after it sets it</en></param>
        void OnPreferenceImport(IPreferenceFolder oldvalues, IPreferenceFolder newvalues);
    }

    //���������̂ݗL��
    /// <summary>
    /// <ja>
    /// ���[�U�[�ݒ�l�iPreference�j�Ƃ��č��ڂ�o�^����@�\��񋟂��܂��B
    /// </ja>
    /// <en>
    /// The function to register the item as user setting value (Preference) is offered. 
    /// </en>
    /// </summary>
    public interface IPreferenceBuilder {
        /// <summary>
        /// <ja>
        /// �K�w������t�H���_���`���܂��B
        /// </ja>
        /// <en>
        /// The hierarchized folder is defined. 
        /// </en>
        /// </summary>
        /// <param name="parent"><ja>�e�ƂȂ�t�H���_</ja><en>Folder that becomes parents</en></param>
        /// <param name="supplier"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// <ja>
        /// <paramref name="parent"/>�ɂ́A<seealso cref="IPreferenceSupplier">IPreferenceSupplier</seealso>��
        /// <see href="IPreferenceSupplier.InitializePreference">InitializePreference</see>�̑�2�����ɓn���ꂽ
        /// �l�����̂܂܈����n���̂��ʗ�ł��B
        /// </ja>
        /// <en>
        /// In <paramref name="parent"/>, it is usual to pass the value passed to the second argument of 
        /// <see href="IPreferenceSupplier.InitializePreference">InitializePreference</see> of 
        /// <seealso cref="IPreferenceSupplier">IPreferenceSupplier</seealso> in off as it is. 
        /// </en>
        /// </remarks>
        /// <exclude/>
        IPreferenceFolder DefineFolder(IPreferenceFolder parent, IPreferenceSupplier supplier, string id); //�q��Supplier��null
        /// <exclude/>
        IPreferenceFolder DefineFolderArray(IPreferenceFolder parent, IPreferenceSupplier supplier, string id); //Array��Ԃ��킯�ł͂Ȃ����Ƃɒ���
        /// <exclude/>
        IPreferenceLooseNode DefineLooseNode(IPreferenceFolder parent, IPreferenceLooseNodeContent content, string id);

        //validator�s�v�ȂƂ���null
        /// <summary>
        /// <ja>
        /// bool�^�̃��[�U�[�ݒ�l���`���܂��B
        /// </ja>
        /// <en>
        /// The user setting value of the bool type is defined. 
        /// </en>
        /// </summary>
        /// <param name="parent"><ja>�e�ƂȂ�t�H���_</ja><en>Folder that becomes parents</en></param>
        /// <param name="id"><ja>�l�̃L�[�ƂȂ�ݒ薼</ja><en>Set name that becomes key to value.</en></param>
        /// <param name="initial_value"><ja>�����l</ja><en>Initial value</en></param>
        /// <param name="validator"><ja>�l�����؂���ۂ̃o���f�[�^</ja><en>Validator when value is verified</en></param>
        /// <returns><ja>�l��ǂݏ������邽�߂�<seealso cref="IBoolPreferenceItem">IBoolPreferenceItem</seealso></ja><en><seealso cref="IBoolPreferenceItem">IBoolPreferenceItem</seealso> to read and write value.</en></returns>
        /// <remarks>
        /// <ja>
        /// <para>
        /// <paramref name="parent"/>�ɂ́A<seealso cref="IPreferenceSupplier">IPreferenceSupplier</seealso>��
        /// <see href="IPreferenceSupplier.InitializePreference">InitializePreference</see>�̑�2�����ɓn���ꂽ
        /// �l�����̂܂܈����n���̂��ʗ�ł��B
        /// </para>
        /// <para>
        /// <paramref name="id"/>�́A���[�U�[�ݒ�l�����ʂ��邽�߂̔C�ӂ̖��O�ł��B<paramref name="parent"/>
        /// �ŊK�w������邽�߁A�ق��̃v���O�C���Ƃ̖��O�̏d�����l����K�v�͂���܂���B
        /// </para>
        /// <para>
        /// <paramref name="initial_value"/>�́A�Y�����郆�[�U�[�ݒ�l���܂����݂��Ȃ��ꍇ�̏����l�ł��B
        /// ���łɂ��̃��[�U�[�ݒ�l�����݂���ꍇ�ɂ͖�������A�����̒l���ǂݍ��܂�܂��B
        /// </para>
        /// <para>
        /// ���؋@�\��K�v�Ƃ��Ȃ��Ƃ��ɂ́A<paramref name="validator"/>��null�ɂ��邱�Ƃ��ł��܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// In <paramref name="parent"/>, it is usual to pass the value passed to the second argument of 
        /// <see href="IPreferenceSupplier.InitializePreference">InitializePreference</see> of 
        /// <seealso cref="IPreferenceSupplier">IPreferenceSupplier</seealso> in off as it is. 
        /// </para>
        /// <para>
        /// <paramref name="id"/> is an arbitrary name to identify the user setting value. 
        /// It is not necessary to think about the repetition of the name with other plug-ins 
        /// because it is hierarchized by <paramref name="parent"/>. 
        /// </para>
        /// <para>
        /// <paramref name="initial_value"/> is a value in the early the case where the corresponding user 
        /// setting value has not existed yet. It is disregarded when this user setting value already exists, 
        /// and an existing value is read. 
        /// </para>
        /// <para>
        /// When the verification function is not needed, <paramref name="validator"/> can be made null. 
        /// </para>
        /// </en>
        /// </remarks>
        IBoolPreferenceItem DefineBoolValue(IPreferenceFolder parent, string id, bool initial_value, PreferenceItemValidator<bool> validator);
        /// <summary>
        /// <ja>
        /// int�^�̃��[�U�[�ݒ�l���`���܂��B
        /// </ja>
        /// <en>
        /// The user setting value of the int type is defined. 
        /// </en>
        /// </summary>
        /// <param name="parent"><ja>�e�ƂȂ�t�H���_</ja><en>Folder that becomes parents</en></param>
        /// <param name="id"><ja>�l�̃L�[�ƂȂ�ݒ薼</ja><en>Set name that becomes key to value.</en></param>
        /// <param name="initial_value"><ja>�����l</ja><en>Initial value</en></param>
        /// <param name="validator"><ja>�l�����؂���ۂ̃o���f�[�^</ja><en>Validator when value is verified</en></param>
        /// <returns><ja>�l��ǂݏ������邽�߂�<seealso cref="IIntPreferenceItem">IIntPreferenceItem</seealso></ja><en><seealso cref="IIntPreferenceItem">IIntPreferenceItem</seealso> to read and write value.</en></returns>
        /// <remarks>
        /// <ja>
        /// <para>
        /// <paramref name="parent"/>�ɂ́A<seealso cref="IPreferenceSupplier">IPreferenceSupplier</seealso>��
        /// <see href="IPreferenceSupplier.InitializePreference">InitializePreference</see>�̑�2�����ɓn���ꂽ
        /// �l�����̂܂܈����n���̂��ʗ�ł��B
        /// </para>
        /// <para>
        /// <paramref name="id"/>�́A���[�U�[�ݒ�l�����ʂ��邽�߂̔C�ӂ̖��O�ł��B<paramref name="parent"/>
        /// �ŊK�w������邽�߁A�ق��̃v���O�C���Ƃ̖��O�̏d�����l����K�v�͂���܂���B
        /// </para>
        /// <para>
        /// <paramref name="initial_value"/>�́A�Y�����郆�[�U�[�ݒ�l���܂����݂��Ȃ��ꍇ�̏����l�ł��B
        /// ���łɂ��̃��[�U�[�ݒ�l�����݂���ꍇ�ɂ͖�������A�����̒l���ǂݍ��܂�܂��B
        /// </para>
        /// <para>
        /// ���؋@�\��K�v�Ƃ��Ȃ��Ƃ��ɂ́A<paramref name="validator"/>��null�ɂ��邱�Ƃ��ł��܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// In <paramref name="parent"/>, it is usual to pass the value passed to the second argument of 
        /// <see href="IPreferenceSupplier.InitializePreference">InitializePreference</see> of 
        /// <seealso cref="IPreferenceSupplier">IPreferenceSupplier</seealso> in off as it is. 
        /// </para>
        /// <para>
        /// <paramref name="id"/> is an arbitrary name to identify the user setting value. 
        /// It is not necessary to think about the repetition of the name with other plug-ins 
        /// because it is hierarchized by <paramref name="parent"/>. 
        /// </para>
        /// <para>
        /// <paramref name="initial_value"/> is a value in the early the case where the corresponding user 
        /// setting value has not existed yet. It is disregarded when this user setting value already exists, 
        /// and an existing value is read. 
        /// </para>
        /// <para>
        /// When the verification function is not needed, <paramref name="validator"/> can be made null. 
        /// </para>
        /// </en>
        /// </remarks>
        IIntPreferenceItem DefineIntValue(IPreferenceFolder parent, string id, int initial_value, PreferenceItemValidator<int> validator);

        /// <summary>
        /// <ja>
        /// string�^�̃��[�U�[�ݒ�l���`���܂��B
        /// </ja>
        /// <en>
        /// The user setting value of the string type is defined. 
        /// </en>
        /// </summary>
        /// <param name="parent"><ja>�e�ƂȂ�t�H���_</ja><en>Folder that becomes parents</en></param>
        /// <param name="id"><ja>�l�̃L�[�ƂȂ�ݒ薼</ja><en>Set name that becomes key to value.</en></param>
        /// <param name="initial_value"><ja>�����l</ja><en>Initial value</en></param>
        /// <param name="validator"><ja>�l�����؂���ۂ̃o���f�[�^</ja><en>Validator when value is verified</en></param>
        /// <returns><ja>�l��ǂݏ������邽�߂�<seealso cref="IIntPreferenceItem">IIntPreferenceItem</seealso></ja><en><seealso cref="IIntPreferenceItem">IIntPreferenceItem</seealso> to read and write value.</en></returns>
        /// <remarks>
        /// <ja>
        /// <para>
        /// <paramref name="parent"/>�ɂ́A<seealso cref="IPreferenceSupplier">IPreferenceSupplier</seealso>��
        /// <see href="IPreferenceSupplier.InitializePreference">InitializePreference</see>�̑�2�����ɓn���ꂽ
        /// �l�����̂܂܈����n���̂��ʗ�ł��B
        /// </para>
        /// <para>
        /// <paramref name="id"/>�́A���[�U�[�ݒ�l�����ʂ��邽�߂̔C�ӂ̖��O�ł��B<paramref name="parent"/>
        /// �ŊK�w������邽�߁A�ق��̃v���O�C���Ƃ̖��O�̏d�����l����K�v�͂���܂���B
        /// </para>
        /// <para>
        /// <paramref name="initial_value"/>�́A�Y�����郆�[�U�[�ݒ�l���܂����݂��Ȃ��ꍇ�̏����l�ł��B
        /// ���łɂ��̃��[�U�[�ݒ�l�����݂���ꍇ�ɂ͖�������A�����̒l���ǂݍ��܂�܂��B
        /// </para>
        /// <para>
        /// ���؋@�\��K�v�Ƃ��Ȃ��Ƃ��ɂ́A<paramref name="validator"/>��null�ɂ��邱�Ƃ��ł��܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// In <paramref name="parent"/>, it is usual to pass the value passed to the second argument of 
        /// <see href="IPreferenceSupplier.InitializePreference">InitializePreference</see> of 
        /// <seealso cref="IPreferenceSupplier">IPreferenceSupplier</seealso> in off as it is. 
        /// </para>
        /// <para>
        /// <paramref name="id"/> is an arbitrary name to identify the user setting value. 
        /// It is not necessary to think about the repetition of the name with other plug-ins 
        /// because it is hierarchized by <paramref name="parent"/>. 
        /// </para>
        /// <para>
        /// <paramref name="initial_value"/> is a value in the early the case where the corresponding user 
        /// setting value has not existed yet. It is disregarded when this user setting value already exists, 
        /// and an existing value is read. 
        /// </para>
        /// <para>
        /// When the verification function is not needed, <paramref name="validator"/> can be made null. 
        /// </para>
        /// </en>
        /// </remarks>
        IStringPreferenceItem DefineStringValue(IPreferenceFolder parent, string id, string initial_value, PreferenceItemValidator<string> validator);
    }


    /// <summary>
    /// <ja>
    /// ���[�U�[�ݒ�l�iPreference�j�̍��ڂ�t�H���_�̊��ƂȂ�C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// It is an interface that becomes the item of user setting value (Preference) and basic of the folder. 
    /// </en>
    /// </summary>
    public interface IPreferenceItemBase {
        /// <summary>
        /// <ja>
        /// �ݒ薼�ł��B
        /// </ja>
        /// <en>
        /// Name of setting
        /// </en>
        /// </summary>
        string Id {
            get;
        }
        /// <summary>
        /// <ja>
        /// �t�H���_�����܂߂����S�Ȑݒ薼�ł��B
        /// </ja>
        /// <en>
        /// Complete set name including folder name
        /// </en>
        /// </summary>
        string FullQualifiedId {
            get;
        }
        /// <summary>
        /// <ja>
        /// �e����̃C���f�b�N�X�ʒu�ł��B
        /// </ja>
        /// <en>
        /// It is an index position from parents. 
        /// </en>
        /// </summary>
        int Index {
            get;
        }
        //cast: NOTE:�p�~�\��
        /// <exclude/>
        IPreferenceItem AsItem();
        /// <exclude/>
        IPreferenceFolder AsFolder();
        /// <exclude/>
        IPreferenceFolderArray AsFolderArray();
        //���ׂď�����
        /// <summary>
        /// <ja>
        /// �l�����ׂď��������܂��B
        /// </ja>
        /// <en>
        /// All the values are initialized. 
        /// </en>
        /// </summary>
        void ResetValue();
    }

    /// <summary>
    /// <ja>
    /// ���[�U�[�ݒ�l�iPreference�j���K�w������t�H���_�𑀍삷��C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// It is an interface that operates the folder that hierarchizes user setting value (Preference). 
    /// </en>
    /// </summary>
    public interface IPreferenceFolder : IPreferenceItemBase {
        /// <summary>
        /// <ja>
        /// �������쐬���܂��B
        /// </ja>
        /// <en>
        /// Create a copy.
        /// </en>
        /// </summary>
        /// <returns><ja>��������IPreferenceFolder</ja><en>Duplicated IPreferenceFolder</en></returns>
        IPreferenceFolder Clone();
        /// <summary>
        /// <ja>
        /// �ʂ̃t�H���_����C���|�[�g���܂��B
        /// </ja>
        /// <en>
        /// Import from another folder.
        /// </en>
        /// </summary>
        /// <param name="newvalues"><ja>�C���|�[�g����l���܂ރt�H���_</ja><en>Folder including value in which import.</en></param>
        void Import(IPreferenceFolder newvalues);

        //TODO �O������̖����Ifolder validation��������

        /// <summary>
        /// <ja>
        /// �q�̃t�H���_���������܂��B
        /// </ja>
        /// <en>
        /// Child's folder is retrieved. 
        /// </en>
        /// </summary>
        /// <param name="id"><ja>�q�̃t�H���_��ID</ja><en>ID of child's folder.</en></param>
        /// <returns><ja>���������t�H���_������IPreferenceFolder�B������Ȃ��Ƃ��ɂ�null</ja><en>IPreferenceFolder that shows found folder. When not found, null returns.</en></returns>
        IPreferenceFolder FindChildFolder(string id);
        /// <summary>
        /// <ja>
        /// �q�̃t�H���_�̔z����������܂��B
        /// </ja>
        /// <en>
        /// Array of child's folder is retrieved. 
        /// </en>
        /// </summary>
        /// <param name="id">
        /// <ja>�q�̃t�H���_��ID</ja>
        /// <en>ID of child's folder.</en>
        /// </param>
        /// <returns><ja>���������t�H���_������IPreferenceFolder�B������Ȃ��Ƃ��ɂ�null</ja><en>IPreferenceFolder that shows found folder. When not found, null returns.</en></returns>
        IPreferenceFolderArray FindChildFolderArray(string id);
        /// <summary>
        /// <ja>
        /// ���̃t�H���_���̐ݒ�l���������܂��B
        /// </ja>
        /// <en>
        /// A set value in this folder is retrieved. 
        /// </en>
        /// </summary>
        /// <param name="id"><ja>��������ݒ薼</ja><en>Retrieved set name</en></param>
        /// <returns><ja>���������t�H���_������IPreferenceFolder�B������Ȃ��Ƃ��ɂ�null</ja><en>IPreferenceFolder that shows found folder. When not found, null returns.</en></returns>
        IPreferenceItem FindItem(string id);
        /// <summary>
        /// <ja>
        /// �q�̐��������܂��B
        /// </ja>
        /// <en>
        /// Number of children
        /// </en>
        /// </summary>
        int ChildCount {
            get;
        }
        /// <summary>
        /// <ja>
        /// �w�肵���C���f�b�N�X�ʒu�ɂ���ݒ荀�ڂ�Ԃ��܂��B
        /// </ja>
        /// <en>
        /// A set item at the specified index position is returned. 
        /// </en>
        /// </summary>
        /// <param name="index"><ja>�C���f�b�N�X�ʒu</ja><en>Position of index.</en></param>
        /// <returns><ja>���̃C���f�b�N�X�ʒu�ɂ���ݒ荀�ڂ�����IPreferenceItemBase�B������Ȃ��Ƃ��ɂ�null</ja><en>IPreferenceItemBase that shows set item at the index position. When not found, returns null.</en></returns>
        IPreferenceItemBase ChildAt(int index);

        //UserFriendly interface�ւ̃L���X�g�p
        /// <summary>
        /// <ja>
        /// ���ꂼ��̃v���O�C���ɓ��L��Preference�ւƕϊ����܂��B
        /// </ja>
        /// <en>
        /// It converts it into peculiar Preference to each plug-in. 
        /// </en>
        /// </summary>
        /// <param name="type"><ja>�ϊ�����C���^�[�t�F�C�X�̌^</ja><en>Type in converted interface</en></param>
        /// <returns><ja>�ϊ���̃C���^�[�t�F�C�X�B�ϊ��ł��Ȃ��Ƃ��ɂ�null</ja><en>Interface after it converts it. When it is not possible to convert it, returns null. </en></returns>
        object QueryAdapter(Type type);

        //��c�ւ��`�d
        /// <summary>
        /// <ja>
        /// ���̃t�H���_���̐ݒ�l���ω������Ƃ��ɒʒm����I�u�W�F�N�g��o�^���܂��B
        /// </ja>
        /// <en>
        /// The object notified when a set value in this folder changes is registered. 
        /// </en>
        /// </summary>
        /// <param name="listener"><ja>�ʒm��̃I�u�W�F�N�g</ja><en>Object at notification destination</en></param>
        void AddChangeListener(IPreferenceChangeListener listener);
        /// <summary>
        /// <ja>
        /// <seealso cref="AddChangeListener">AddChangeListener</seealso>�œo�^�����I�u�W�F�N�g���������܂��B
        /// </ja>
        /// <en>
        /// The object registered with AddChangeListener is released. 
        /// </en>
        /// </summary>
        /// <param name="listener"><ja>��������I�u�W�F�N�g</ja><en>Object to release.</en></param>
        void RemoveChangeListener(IPreferenceChangeListener listener);
    }

    /// <summary>
    /// <ja>
    /// ���[�U�[�ݒ�l�iPreference�j�̃t�H���_���܂Ƃ߂Ĉ������߂̃C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface to handle the folder of the user setting value collectively. 
    /// </en>
    /// </summary>
    public interface IPreferenceFolderArray : IPreferenceItemBase {
        /// <summary>
        /// <ja>
        /// �������쐬���܂��B
        /// </ja>
        /// <en>
        /// Create a copy.
        /// </en>
        /// </summary>
        /// <returns><ja>��������IPreferenceFolderArray</ja><en>Duplicated IPreferenceFolderArray</en></returns>
        IPreferenceFolderArray Clone();
        /// <summary>
        /// <ja>
        /// �ʂ̃t�H���_����C���|�[�g���܂��B
        /// </ja>
        /// <en>
        /// Import from another folder.
        /// </en>
        /// </summary>
        /// <param name="newvalues"><ja>�C���|�[�g����l���܂ރt�H���_</ja><en>Folder including value in which import is done</en></param>
        void Import(IPreferenceFolderArray newvalues);

        /// <summary>
        /// <ja>
        /// �ێ����Ă�����e��IPreferenceFolder�̔z��Ƃ��ē��܂��B
        /// </ja>
        /// <en>
        /// The held content is obtained as an array of IPreferenceFolder. 
        /// </en>
        /// </summary>
        IPreferenceFolder[] Folders {
            get;
        }

        /// <summary>
        /// <ja>
        /// �ێ����Ă�����e���N���A���܂��B
        /// </ja>
        /// <en>
        /// Clear the held content.
        /// </en>
        /// </summary>
        void Clear();

        /// <summary>
        /// <ja>
        /// �V�����t�H���_���쐬���܂��B
        /// </ja>
        /// <en>
        /// Create a new folder.
        /// </en>
        /// </summary>
        /// <returns><ja>���ꂽ�V�����t�H���_������IPreferenceFolder</ja>
        /// <en>IPreferenceFolder that shows made new folder</en>
        /// </returns>
        IPreferenceFolder CreateNewFolder();

        /// <summary>
        /// <ja>
        /// �e���v���[�g��p���āA�q�̃t�H���_���A�C�e���ւƕϊ����܂��B
        /// </ja>
        /// <en>
        /// Child's folder is converted into the item with a template. 
        /// </en>
        /// </summary>
        /// <param name="child_folder"><ja>�ϊ�����q�t�H���_</ja><en>Converted child folder</en></param>
        /// <param name="item_in_template"><ja>�p����e���v���[�g</ja><en>Used template.</en></param>
        /// <returns><ja>�e���v���[�g�ɂ���ĕϊ����ꂽ���ڂ�����IPreferenceItem</ja><en>IPreferenceItem that shows item converted with template</en></returns>
        IPreferenceItem ConvertItem(IPreferenceFolder child_folder, IPreferenceItem item_in_template);
    }

    /// <summary>
    /// <ja>
    /// ���[�U�[�ݒ�l�����؂��邽�߂̃f���Q�[�g�ł��B
    /// </ja>
    /// <en>
    /// It is delegate to verify the user setting value. 
    /// </en>
    /// </summary>
    /// <typeparam name="T"><ja>���[�U�[�ݒ�l�̌^���ł��B</ja><en>It is type information on the user setting value. </en></typeparam>
    /// <param name="value"><ja>���؂��ׂ��l�ł��B</ja><en>It is a value that should be verified. </en></param>
    /// <param name="result"><ja>���،��ʂ��i�[���܂��B</ja><en>The verification result is stored. </en></param>
    /// <remarks>
    /// <ja>
    /// ���[�U�[�ݒ�l�����؂���@�\��񋟂���v���O�C���ł́A<paramref name="value"/>�̒l�̑Ó��������؂��A
    /// ���̌��ʂ�<paramref name="result"/>�ɐݒ肵�Ă��������B
    /// </ja>
    /// <en>Please verify the validity of the value of <paramref name="value"/>, and set the result to <paramref name="result"/> in the plug-in that offers the function to verify the user setting value. </en>
    /// </remarks>
    public delegate void PreferenceItemValidator<T>(T value, IPreferenceValidationResult result);

    /// <summary>
    /// <ja>
    /// ���[�U�[�ݒ�l�iPreference�j�̍��ڂ������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows item of user setting value (Preference).
    /// </en>
    /// </summary>
    public interface IPreferenceItem : IPreferenceItemBase {
        /// <summary>
        /// <ja>
        /// bool�^�Ƃ��Ēl�𓾂邽�߂̃C���^�[�t�F�C�X���擾���܂��B
        /// </ja>
        /// <en>
        /// The interface to obtain the value as bool type is got. 
        /// </en>
        /// </summary>
        /// <returns><ja>bool�l�Ƃ��ăA�N�Z�X���邽�߂�IBoolPreferenceItem</ja><en>IBoolPreferenceItem to access it as bool value</en></returns>
        IBoolPreferenceItem AsBool();
        /// <summary>
        /// <ja>
        /// int�^�Ƃ��Ēl�𓾂邽�߂̃C���^�[�t�F�C�X���擾���܂��B
        /// </ja>
        /// <en>
        /// The interface to obtain the value as int type is got. 
        /// </en>
        /// </summary>
        /// <returns><ja>int�l�Ƃ��ăA�N�Z�X���邽�߂�IIntPreferenceItem</ja><en>IIntPreferenceItem to access it as int value</en></returns>
        IIntPreferenceItem AsInt();
        /// <summary>
        /// <ja>
        /// string�^�Ƃ��Ēl�𓾂邽�߂̃C���^�[�t�F�C�X���擾���܂��B
        /// </ja>
        /// <en>
        /// The interface to obtain the value as string type is got. 
        /// </en>
        /// </summary>
        /// <returns><ja>string�l�Ƃ��ăA�N�Z�X���邽�߂�IStringPreferenceItem</ja><en>IStringPreferenceItem to access it as string value</en></returns>
        IStringPreferenceItem AsString();

    }

    /// <summary>
    /// <ja>
    /// IPoderosaItem���^�t�������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that puts the type as for IPoderosaItem. 
    /// </en>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITypedPreferenceItem<T> : IPreferenceItem {
        /// <summary>
        /// <ja>
        /// ���[�U�[�ݒ�l��ǂݏ������܂��B
        /// </ja>
        /// <en>
        /// Read and write the user setting value.
        /// </en>
        /// </summary>
        T Value {
            get;
            set;
        }

        /// <summary>
        /// <ja>
        /// ���[�U�[�ݒ�l�̏����l�������܂��B
        /// </ja>
        /// <en>
        /// Show the initial value of the user setting value.
        /// </en>
        /// </summary>
        T InitialValue {
            get;
        }

        /// <summary>
        /// <ja>
        /// ���[�U�[�ݒ�l�����؂��邽�߂�PreferenceItemValidator���擾�^�ݒ肵�܂��B
        /// </ja>
        /// <en>
        /// Get / set the PreferenceItemValidator to verify the user setting value.
        /// </en>
        /// </summary>
        PreferenceItemValidator<T> Validator {
            get;
            set;
        }
    }

    //Generic Parameter���v���O���}�ɖ���w�肳����̂�����炵�����AIPreferenceItem���̃L���X�g�p���\�b�h���C�y�Ɏg�������̂�
    /// <summary>
    /// <ja>
    /// bool�^�̃��[�U�[�ݒ�l��ǂݏ�������@�\��񋟂��܂��B
    /// </ja>
    /// <en>
    /// Offered the function to read and write the user setting value of the bool type.
    /// </en>
    /// </summary>
    public interface IBoolPreferenceItem : ITypedPreferenceItem<bool> {
    }

    /// <summary>
    /// <ja>
    /// int�^�̃��[�U�[�ݒ�l��ǂݏ�������@�\��񋟂��܂��B
    /// </ja>
    /// <en>
    /// Offered the function to read and write the user setting value of the int type.
    /// </en>
    /// </summary>
    public interface IIntPreferenceItem : ITypedPreferenceItem<int> {
    }

    /// <summary>
    /// <ja>
    /// string�^�̃��[�U�[�ݒ�l��ǂݏ�������@�\��񋟂��܂��B
    /// </ja>
    /// <en>
    /// Offered the function to read and write the user setting value of the string type.
    /// </en>
    /// </summary>
    public interface IStringPreferenceItem : ITypedPreferenceItem<string> {
    }

    //Loose Node

    /// <summary>
    /// </summary>
    /// <exclude/>
    public interface IPreferenceLooseNode : IPreferenceItemBase {
        IPreferenceLooseNodeContent Content {
            get;
        }
    }

    //TODO Folder�̓N���[���ł���̂�Content�͂ł��Ȃ��͕̂ς��B�ΏƓI�łȂ�
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IPreferenceLooseNodeContent {
        IPreferenceLooseNodeContent Clone();
        void Reset();
        void LoadFrom(StructuredText node);
        void SaveTo(StructuredText node);
    }

    //End Exported Part
}
