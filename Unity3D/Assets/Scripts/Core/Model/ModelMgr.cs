/*
 * 脚本名(ScriptName)：    ModelMgr.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ModelMgr 
{
    private static ModelMgr mInstance;

    public static ModelMgr Instatance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new ModelMgr();
            }
            return mInstance;
        }
    }
    /// <summary>
    /// 存储所有的实例化的Model
    /// </summary>
    private Dictionary<string, BaseModel> dictionary;
    private ModelMgr()
    {
        dictionary = new Dictionary<string, BaseModel>();
    }

    void OnDestroy()
    {
        dictionary.Clear();
        dictionary = null;
    }

    public T GetModel<T>() where T : BaseModel
    {
        Type type = typeof(T);

        if(dictionary.ContainsKey(type.Name))
        {
            return dictionary[type.Name] as T;
        }
        T model = System.Activator.CreateInstance(type) as T;
        dictionary.Add(type.Name, model);
        return model;
    }
}
