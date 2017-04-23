using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// model管理
/// </summary>
public class ModelMgr
{
    protected static ModelMgr mInstance;
    public static bool hasInstance
    {
        get
        {
            return mInstance != null;
        }
    }

    public static ModelMgr GetInstance()
    {
        if (!hasInstance)
        {
            mInstance = new ModelMgr();
        }
        return mInstance;
    }

    /// <summary>
    /// 存储所有model
    /// </summary>
    private Dictionary<string, BaseModel> dictionary;

    private ModelMgr()
    {
        dictionary = new Dictionary<string, BaseModel>();
    }
    
    /// <summary>
    /// 获取model
    /// </summary>
    public T GetModel<T>() where T : BaseModel
    {
        Type type = typeof(T);
        if (dictionary.ContainsKey(type.Name))
        {
            return dictionary[type.Name] as T;
        }

        T model = System.Activator.CreateInstance(type) as T;
        dictionary.Add(type.Name, model);
        return model;
    }

    /// <summary>
    /// 清理
    /// </summary>
    public void Destroy()
    {
        Clear();
        dictionary = null;
    }
    public void Clear()
    {
        foreach (BaseModel m in dictionary.Values)
        {
            m.Destroy();
        }
        dictionary.Clear();
    }
}