/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: SerializeEx.cs,v 1.2 2011/10/27 23:21:55 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace Poderosa.Serializing {
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface ISerializeService {
        StructuredText Serialize(object obj);
        StructuredText Serialize(Type type, object obj); //�^�𖾎�
        object Deserialize(StructuredText node);
    }

    //ExtensionPoint�ɐڑ�����C���^�t�F�[�X�B
    //ConcreteType�ɑΉ�����I�u�W�F�N�g�ɑ΂��Ďg�p����B
    //����StructuredText�̌`��
    // <ConcreteType.FullName> {
    //   ...
    // }
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface ISerializeServiceElement {
        Type ConcreteType {
            get;
        }
        StructuredText Serialize(object obj);
        object Deserialize(StructuredText node);
    }
}
