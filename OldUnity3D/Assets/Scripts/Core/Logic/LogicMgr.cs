using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 逻辑管理
/// </summary>
public class LogicMgr : MonoBehaviour
{
    #region 初始化

    protected static LogicMgr mInstance;
    public static bool hasInstance
    {
        get
        {
            return mInstance != null;
        }
    }
    /// <summary>
    /// 是否正在删除，当程序退出时设置为true
    /// </summary>
    public static bool isDestroying = false;
   
    public static LogicMgr GetInstance()
    {
        if (!hasInstance)
        {
            if (isDestroying)
            {
                return null;
            }
            mInstance = new GameObject("_LogicMgr").AddComponent<LogicMgr>();
        }
        return mInstance;
    }

    internal void Awake()
    {
        dictionary = new Dictionary<string, LogicBase>();
    }

    void OnApplicationQuit()
    {
        isDestroying = true;
    }

    internal void OnDestroy()
    {
        StopAllCoroutines();
        dictionary.Clear();
        dictionary = null;

        LogicMgr.mInstance = null;
    }
    #endregion

    /// <summary>
    /// 存储所有logic
    /// </summary>
    private Dictionary<string, LogicBase> dictionary;

    /// <summary>
    /// get logic
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetLogic<T>() where T : LogicBase
    {
        Type type = typeof(T);
        if (dictionary.ContainsKey(type.Name))
        {
            return dictionary[type.Name] as T;
        }

        T logic = gameObject.AddComponent<T>();
        dictionary.Add(type.Name, logic);
        return logic;
    }
}
