/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: AdapterEx.cs,v 1.2 2011/10/27 23:21:56 kzmi Exp $
 */
using System;

namespace Poderosa {
    /*

    */

    //Adapter�֌W
    // �����P�F�C���X�^���X�̋�ʂ����Ȃ��B�^�ɂ���Ă̂ݐ������邩�ǂ��������܂�
    // �����Q�F�Ώ̗��E���ڗ������BCOM��QueryInterface�Ɠ����B
    /// <summary>
    /// <ja>
    /// �w�肵���C���^�[�t�F�C�X��Ԃ��@�\��񋟂��܂��B
    /// </ja>
    /// <en>
    /// Return the mechanism of specified interface.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// �I�u�W�F�N�g���T�|�[�g����C���^�[�t�F�C�X��Ԃ��@�\��񋟂��܂��B
    /// COM�iComponent Object Model�j�ɂ�����QueryInterface�Ɠ����ł��B
    /// �����҂́A���̗v��������Ă��������B
    /// <ol>
    /// <li>�C���X�^���X�̋�ʂ����Ȃ��ł��������B�^�ɂ���Ă̂ݐ������邩�ǂ��������߂Ă�������</li>
    /// <li>�Ώ̗��E���ڗ�������Ă��������B</li>
    /// </ol>
    /// </ja>
    /// <en>
    /// The mechanism that returns the interface supported by the object is offered. 
    /// It's same as QueryInterface on COM (Component Object Model).
    /// Developers must defend the following requirement.
    /// <ol>
    /// <li>Please do not distinguish the instance. Please decide whether to succeed only by the type. </li>
    /// <li>Please defend the symmetric law and the transition law. </li>
    /// </ol>
    /// </en>
    /// </remarks>
    public interface IAdaptable {
        /// <summary>
        /// <ja>
        /// ����̌^�̃C���^�[�t�F�C�X��Ԃ��܂��B
        /// </ja>
        /// <en>
        /// Return the interface of the specified type
        /// </en>
        /// </summary>
        /// <param name="adapter">
        /// <ja>
        /// �v������C���^�[�t�F�C�X�̌^
        /// </ja>
        /// <en>Type of required interface type.</en></param>
        /// <returns>
        /// <ja>�v�������C���^�[�t�F�C�X���߂�܂��B�I�u�W�F�N�g�����̃C���^�[�t�F�C�X���������Ă��Ȃ��ꍇ�ɂ�null���߂�܂��B</ja>
        /// <en>Return the interface of required. Return null if the interface is not implemented on the object.</en>
        /// </returns>
        /// <remarks>
        /// <ja>
        /// �����҂́A���̃��\�b�h���ŗ�O��Ԃ��Ă͂Ȃ�܂���B�����Ȃ��C���^�[�t�F�C�X�̌^���n���ꂽ�ꍇ�ɂ�null��Ԃ��Ă��������B<br/>
        /// �����̎����ł́A���L�̃R�[�h���g���AIAdapterManager�C���^�[�t�F�C�X��<seealso cref="IAdapterManager.GetAdapter">GetAdapter���\�b�h</seealso>
        /// ���g���āAAdapterManager�ɕϊ���C����悤�ɂ��܂��B
        /// <code>
        /// public IAdaptable GetAdapter(Type adapter)
        /// {
        ///     return poderosa_world.AdapterManager.GetAdapter(this, adapter);
        /// }
        /// </code>
        /// </ja>
        /// <en>
        /// When he or she returns the exception in this method, those who mount do not become it.
        /// Please return null when the type in the interface with which it doesn't provide is passed. 
        /// In a lot of mounting, conversion is left to AdapterManager by using the following code,
        /// and using the <seealso cref="IAdapterManager.GetAdapter">GetAdapter method</seealso> of the IAdapterManager interface. 
        /// <code>
        /// public IAdaptable GetAdapter(Type adapter)
        /// {
        ///     return poderosa_world.AdapterManager.GetAdapter(this, adapter);
        /// }
        /// </code>
        /// </en>
        /// </remarks>
        IAdaptable GetAdapter(Type adapter);
        //Note: ������Generics��( T GetAdapter<T>() )����邱�Ƃ��l�������A�����R�[�h���������I�ɍs����Ƃ���΋N�����ԂɈ��e���o�邩������Ȃ��̂ł�߂Ă����B���̂������Ƃ����邩��
    }

    /*
     * �Â��^�C�v��AdapterFactory. Eclipse��^���Ă����Ȃ����悤�ȋL�����邪�s�m���B����͎g�����舫���̂ŉ��߂�
    public interface IAdapterFactory {
        Type SourceType {
            get;
        }
        Type[] Adapters {
            get;
        }
        IAdaptable GetAdapter(IAdaptable obj, Type adapter);
    }
    */

    //�o�����ɕϊ��ł��Ȃ���΂Ȃ�Ȃ��B
    /// <summary>
    /// <ja>
    /// �A�_�v�^�t�@�N�g�����\������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// The interface that compose the adapter factory.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// ���̃C���^�[�t�F�C�X�́A�A�_�v�^�}�l�[�W���i<seealso cref="IAdapterManager">IAdapterManager</seealso>�j
    /// ���g���Č^�ϊ���`����Ƃ��Ɏg���܂��B
    /// </ja>
    /// </remarks>
    public interface IDualDirectionalAdapterFactory {
        /// <summary>
        /// <ja>�\�[�X�̌^�������܂��B</ja>
        /// <en>Type of source</en>
        /// </summary>
        Type SourceType {
            get;
        }
        /// <summary>
        /// <ja>�A�_�v�^�̌^�������܂��B
        /// </ja>
        /// <en>Type of adapter
        /// </en>
        /// </summary>
        Type AdapterType {
            get;
        }
        /// <summary>
        /// <ja>
        /// �\�[�X����A�_�v�^�ւƕϊ����܂��B
        /// </ja>
        /// <en>
        /// Convert from source to adapter.
        /// </en>
        /// </summary>
        /// <param name="obj">
        /// <ja>�\�[�X�̌^</ja>
        /// <en>Type of source</en>
        /// </param>
        /// <returns>
        /// <ja>�A�_�v�^�̌^���Ԃ���܂��B</ja>
        /// <en>Return the type of the adapter</en>
        /// </returns>
        IAdaptable GetAdapter(IAdaptable obj); //SourceType -> AdapterType
        /// <summary>
        /// <ja>
        /// �A�_�v�^����\�[�X�ւƕϊ����܂��B
        /// </ja>
        /// <en>
        /// Convert from adapter to source.
        /// </en>
        /// </summary>
        /// <param name="obj">
        /// <ja>�A�_�v�^�̌^</ja>
        /// <en>Type of adapter</en>
        /// </param>
        /// <returns>
        /// <ja>�\�[�X�̌^���Ԃ���܂��B</ja>
        /// <en>Return the type of the source</en>
        /// </returns>
        IAdaptable GetSource(IAdaptable obj);  //AdapterType -> SourceType
    }

    //Generics�� IAdapterFactory
    /// <summary>
    /// <ja>
    /// Generics�ł̃A�_�v�^�t�@�N�g���ł��B
    /// </ja>
    /// <en>
    /// Adapter factory of the Generics version.
    /// </en>
    /// </summary>
    /// <typeparam name="S">
    /// <ja>�\�[�X�̌^</ja>
    /// <en>Type of the source</en>
    /// </typeparam>
    /// <typeparam name="T">
    /// <ja>�A�_�v�^�̌^</ja>
    /// <en>Type of adapter</en>
    /// </typeparam>
    /// <remarks>
    /// <ja>
    /// ���̃C���^�[�t�F�C�X�́A�A�_�v�^�}�l�[�W���i<seealso cref="IAdapterManager">IAdapterManager</seealso>�j
    /// ���g���Č^�ϊ���`����Ƃ��Ɏg���܂��B
    /// </ja>
    /// <en>
    /// This interface is used when it defines the type conversation by adapter manager(<seealso cref="IAdapterManager">IAdapterManager</seealso>)
    /// </en>
    /// </remarks>
    public abstract class ITypedDualDirectionalAdapterFactory<S, T> : IDualDirectionalAdapterFactory
        where T : IAdaptable
        where S : IAdaptable {

        /// <summary>
        /// <ja>�\�[�X�̌^�������܂��B</ja>
        /// <en>The type of the source</en>
        /// </summary>
        public Type SourceType {
            get {
                return typeof(S);
            }
        }

        /// <summary>
        /// <ja>�A�_�v�^�̌^�������܂��B</ja>
        /// <en>the type of the adapter</en>
        /// </summary>
        public Type AdapterType {
            get {
                return typeof(T);
            }
        }

        /// <summary>
        /// <ja>�\�[�X����A�_�v�^�ւƕϊ����܂��B</ja>
        /// <en>Convert from the source th the adapter.</en>
        /// </summary>
        /// <param name="obj"><ja>�\�[�X�̌^</ja><en>Type of the source</en></param>
        /// <returns><ja>�A�_�v�^�̌^���Ԃ���܂��B</ja><en>Return the type of the adapter.</en></returns>
        public IAdaptable GetAdapter(IAdaptable obj) {
            return GetAdapter((S)obj);
        }

        /// <summary>
        /// <ja>�A�_�v�^����\�[�X�ւƕϊ����܂��B</ja>
        /// <en>Convert from the adapter to the source.</en>
        /// </summary>
        /// <param name="obj">
        /// <ja>�A�_�v�^�̌^</ja>
        /// <en>Type of adapter</en>
        /// </param>
        /// <returns>
        /// <ja>�\�[�X�̌^���Ԃ���܂��B</ja>
        /// <en>Return the type of the source.</en>
        /// </returns>
        public IAdaptable GetSource(IAdaptable obj) {
            return GetSource((T)obj);
        }

        /// <summary>
        /// <ja>�\�[�X����A�_�v�^�ւƕϊ����܂��B</ja>
        /// <en>Convert from the source to the adapter.</en>
        /// </summary>
        /// <param name="obj">
        /// <ja>�\�[�X�̌^</ja>
        /// <en>Type of the source</en>
        /// </param>
        /// <returns>
        /// <ja>�A�_�v�^�̌^���Ԃ���܂��B</ja>
        /// <en>Return the type of the adapter.</en>
        /// </returns>
        public abstract T GetAdapter(S obj);

        /// <summary>
        /// <ja>�A�_�v�^����\�[�X�ւƕϊ����܂��B</ja>
        /// <en>Convert from the adapter to the source</en>
        /// </summary>
        /// <param name="obj">
        /// <ja>�A�_�v�^�̌^</ja>
        /// <en>Type of the adapter</en>
        /// </param>
        /// <returns>
        /// <ja>�\�[�X�̌^���Ԃ���܂��B</ja>
        /// <en>Return the type of the source.</en>
        /// </returns>
        public abstract S GetSource(T obj);
    }


    /// <summary>
    /// <ja>�A�_�v�^�}�l�[�W���������C���^�[�t�F�C�X�ł��B</ja>
    /// <en>Interface that shows the adapter manager</en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// �A�_�v�^�}�l�[�W���́A<seealso cref="Poderosa.Plugins.IPoderosaWorld">IPoderosaWorld</seealso>��
    /// <see cref="Poderosa.Plugins.IPoderosaWorld.AdapterManager">AdapterManager�v���p�e�B</see>����擾�ł��܂��B
    /// </ja>
    /// <en>
    /// The adapter manager can be got by <see cref="Poderosa.Plugins.IPoderosaWorld.AdapterManager">AdapterManager property</see>
    /// on <seealso cref="Poderosa.Plugins.IPoderosaWorld">IPoderosaWorld</seealso>.
    /// </en>
    /// </remarks>
    public interface IAdapterManager {
        /// <summary>
        /// <ja>�A�_�v�^�t�@�N�g����o�^���܂��B</ja>
        /// <en>Regist the adapter factory.</en>
        /// </summary>
        /// <param name="factory">
        /// <ja>�o�^����A�_�v�^�t�@�N�g��</ja>
        /// <en>Adapter factory to be regist.</en>
        /// </param>
        void RegisterFactory(IDualDirectionalAdapterFactory factory);
        /// <summary>
        /// <ja>
        /// �A�_�v�^�t�@�N�g�����������܂��B
        /// </ja>
        /// <en>
        /// Remove the adapter factory.
        /// </en>
        /// </summary>
        /// <param name="factory">
        /// <ja>��������A�_�v�^�t�@�N�g��</ja>
        /// <en>The adapter factory to remove.</en>
        /// </param>
        void RemoveFactory(IDualDirectionalAdapterFactory factory);
        /// <summary>
        /// <ja>
        /// �A�_�v�^�t�@�N�g�����g�����^�ϊ��@�\��񋟂��܂��B
        /// </ja>
        /// <en>
        /// Offers type conversation function by using adapter factory.
        /// </en>
        /// </summary>
        /// <param name="obj">
        /// <ja>�ϊ��ΏۂƂȂ�I�u�W�F�N�g</ja>
        /// <en>The object to convert.</en>
        /// </param>
        /// <param name="adapter">
        /// <ja>�擾�������C���^�[�t�F�C�X</ja>
        /// <en>The interface to get.</en>
        /// </param>
        /// <returns>
        /// <ja>�ϊ����ꂽ�C���^�[�t�F�C�X</ja>
        /// <en>The converted interface</en>
        /// </returns>
        /// <remarks>
        /// <ja>
        /// �J���҂́A����GetAdapter���\�b�h���g���āA�W���̌^�ϊ��@�\�i<seealso cref="IAdaptable">IAdaptable</seealso>��GetAdapter�̎����j�����̂悤�ɂł��܂��B
        /// <code>
        /// public IAdaptable GetAdapter(Type adapter)
        /// {
        ///     return poderosa_world.AdapterManager.GetAdapter(this, adapter);
        /// }
        /// </code>
        /// </ja>
        /// <en>
        /// The developer is good at a standard type conversion mechanism as follows by the use of this GetAdapter method. 
        /// <code>
        /// public IAdaptable GetAdapter(Type adapter)
        /// {
        ///     return poderosa_world.AdapterManager.GetAdapter(this, adapter);
        /// }
        /// </code>
        /// </en>
        /// </remarks>
        IAdaptable GetAdapter(IAdaptable obj, Type adapter);
        /// <summary>
        /// <ja>
        /// �A�_�v�^�t�@�N�g�����g�����^�ϊ��@�\��񋟂��܂��B
        /// </ja>
        /// <en>
        /// The type conversion function to use the adaptor factory is offered. 
        /// </en>
        /// </summary>
        /// <typeparam name="T">
        /// <ja>�ϊ��������C���^�[�t�F�C�X�̌^</ja>
        /// <en>Type in interface that wants to be converted</en>
        /// </typeparam>
        /// <param name="obj">
        /// <ja>�ϊ��ΏۂƂȂ�I�u�W�F�N�g</ja>
        /// <en>The object that wants to be converted.</en>
        /// </param>
        /// <returns>
        /// <ja>�ϊ����ꂽ�C���^�[�t�F�C�X</ja>
        /// <en>Converted interface</en>
        /// </returns>
        T GetAdapter<T>(IAdaptable obj) where T : IAdaptable;
    }
}
