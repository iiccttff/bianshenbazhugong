using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LayerMgr : MonoBehaviour
{
    private static LayerMgr mInstance;

    /// <summary>
    /// 获取资源加载实例
    /// </summary>
    /// <returns></returns>
    public static LayerMgr GetInstance()
    {
        if(mInstance == null)
        {
            mInstance = new GameObject("_LayerMgr").AddComponent<LayerMgr>();
        }
        return mInstance;
    }
    LayerMgr()
    {
        mLayerDic = new Dictionary<LayerType, GameObject>();
    }
    private Dictionary<LayerType, GameObject> mLayerDic;
    private GameObject mParent;
    public void LayerInit()
    {
        mParent = GameObject.Find("UI Root");
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
        layer.transform.parent = mParent.transform;
        layer.transform.localPosition = new Vector3(0,0,((int)type) * -1);
        layer.transform.localEulerAngles = Vector3.zero;
        layer.transform.localScale = Vector3.one;
        return layer;
    }

    public void SetLayer(GameObject current,LayerType type)
    {
        if(mLayerDic.Count < Enum.GetNames(typeof(LayerType)).Length)
        {
            LayerInit();
        }
        current.transform.parent = mLayerDic[type].transform;
        UIPanel[] panelArr = current.GetComponentsInChildren<UIPanel>(true);
        foreach(UIPanel panel in panelArr)
        {
            panel.depth += (int)type;
        }
    }
}
/// <summary>
/// 分层类型
/// </summary>
public enum LayerType
{
    /// <summary>场景</summary>
    Scene = 50,
    /// <summary>弹框</summary>
    Panel = 200,
    /// <summary>提示</summary>
    Tips = 400,
    /// <summary>公告层</summary>
    Notice = 1000,
}
