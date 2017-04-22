/*
 * 脚本名(ScriptName)：    LayerMgr.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LayerMgr : MonoBehaviour
{
    #region 初始化
    private static LayerMgr mInstance;
    public static bool hasIntance
    {
        get
        {
            return mInstance != null;
        }
    }

    /// <summary>
    /// 是否正在删除，当程序退出时设置为true
    /// </summary>
    public static bool isDestorying = false;

    public static LayerMgr GetInstance()
    {
        if (!hasIntance)
        {
            if (isDestorying)
            {
                return null;
            }
            mInstance = new GameObject("_LayerMgr").AddComponent<LayerMgr>();
        }
        return mInstance;
    }
    void OnApplicationQuit()
    {
        isDestorying = true;
    }

    private LayerMgr()
    {
        mLayerDic = new Dictionary<LayerType, GameObject>();
        
    }
    private void OnDestroy()
    {
        mLayerDic.Clear();
        mLayerDic = null;
    }
    #endregion

    private Dictionary<LayerType, GameObject> mLayerDic;
    private Transform mParent;

    public void SetLayer(GameObject current,LayerType type)
    {
        if(mLayerDic.Count < Enum.GetNames(typeof(LayerType)).Length)
        {
            //初始化
            LayerInit();
        }
        current.transform.parent = mLayerDic[type].transform;
        UIPanel[] panels = current.GetComponentsInChildren<UIPanel>(true);
        foreach(UIPanel panel in panels)
        {
            panel.depth += (int)type;
        }
    }

    public void LayerInit()
    {
        mParent = GameObject.Find("UI Root").transform;
        int nums = Enum.GetNames(typeof(LayerType)).Length;
        for (int i = 0; i < nums; i++)
        {
            object obj = Enum.GetValues(typeof(LayerType)).GetValue(i);
            mLayerDic.Add((LayerType)obj, CreateLayerGameObject(obj.ToString(), (LayerType)obj));
        }
    }
    GameObject CreateLayerGameObject(string name,LayerType type)
    {
        GameObject layer = new GameObject(name);
        layer.transform.parent = mParent;
        layer.transform.localEulerAngles = Vector3.zero;
        layer.transform.localScale = Vector3.one;
        layer.transform.localPosition = new Vector3(0, 0, ((int)type) * -1);
        return layer;
    }
}

public enum LayerType
{
    /// <summary> 场景 </summary>
    Scene = 50,
    /// <summary> 面板 </summary>
    Panel = 200,
    /// <summary> 提示 </summary>
    Tips = 400,
    /// <summary> 公告跑马灯 </summary>
    Notice = 1000,
}
