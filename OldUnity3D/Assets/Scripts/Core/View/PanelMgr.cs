using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 面板管理
/// </summary>
public class PanelMgr
{
    #region 初始化
    protected static PanelMgr mInstance;
    public static bool hasInstance
    {
        get
        {
            return mInstance != null;
        }
    }

    public static PanelMgr GetInstance()
    {
        if (!hasInstance)
        {
            mInstance = new PanelMgr();
        }
        return mInstance;
    }
    private PanelMgr()
    {
        panels = new Dictionary<PanelName, PanelBase>();
    }
    #endregion

    #region 数据定义

    /// <summary>
    /// 面板显示方式
    /// </summary>
    public enum PanelShowStyle
    {
        /// <summary>
        /// //正常出现--
        /// </summary>
        Nomal,
        /// <summary>
        /// //中间变大--
        /// </summary>
        CenterScaleBigNomal,
        /// <summary>
        /// //上往中滑动--
        /// </summary>
        UpToSlide,
        /// <summary>
        /// //下往中滑动
        /// </summary>
        DownToSlide,
        /// <summary>
        /// //左往中--
        /// </summary>
        LeftToSlide,
        /// <summary>
        /// //右往中--
        /// </summary>
        RightToSlide,
    }

    /// <summary>
    /// 面板遮罩
    /// </summary>
    public enum PanelMaskSytle
    {
        /// <summary>
        /// 无背景
        /// </summary>
        None,
        /// <summary>
        /// 半透明背景
        /// </summary>
        BlackAlpha,
        /// <summary>
        /// 无背景.但有BOX关闭组件
        /// </summary>
        Alpha,
    }

    /// <summary>
    /// 存储当前已经实例化的面板
    /// </summary>
    public Dictionary<PanelName, PanelBase> panels;
    #endregion


    /// <summary>
    /// 当前面板
    /// </summary>
    public PanelBase current;

    public void Destroy()
    {

    }


    /// <summary>
    /// 打开指定场景
    /// </summary>
    /// <param name="sceneType"></param>
    /// <param name="sceneArgs">场景参数</param>
    public void ShowPanel(PanelName panelName, params object[] sceneArgs)
    {

        if (panels.ContainsKey(panelName))
        {
            Debug.LogError("该面板已打开！");
            current = panels[panelName];
            current.gameObject.SetActive(false);
            current.OnInit(sceneArgs);
            current.OnShowing();
            LayerMgr.GetInstance().SetLayer(current.gameObject, LayerType.Panel);
        }
        else
        {
            GameObject go = new GameObject(panelName.ToString());
            current = UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(go, "Assets/Scripts/Core/View/PanelMgr.cs (124,23)", panelName.ToString()) as PanelBase; //sceneType.tostring等于该场景的classname
            current.gameObject.SetActive(false);
            current.OnInit(sceneArgs);
            panels.Add(current.type, current);
            current.OnShowing();
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localRotation = Quaternion.identity;
            MaskStyle(current);
        }
        StartShowPanel(current, current.PanelShowStyle,true);
    }

    /// <summary> 打开关闭面板效果 </summary>
    private void StartShowPanel(PanelBase go, PanelShowStyle showStyle, bool isOpen)
    {
        switch(showStyle)
        {
            case PanelShowStyle.Nomal:
                ShowNomal(go, isOpen);
                break;
            case PanelShowStyle.CenterScaleBigNomal:
                CenterScaleBigNomal(go, isOpen);
                break;
            case PanelShowStyle.LeftToSlide:
                LeftAndRightToSlide(go, false, isOpen);
                break;
            case PanelShowStyle.RightToSlide:
                LeftAndRightToSlide(go, true, isOpen);
                break;
            case PanelShowStyle.UpToSlide:
                TopAndDownToSlide(go, true, isOpen);
                break;
            case PanelShowStyle.DownToSlide:
                TopAndDownToSlide(go, false, isOpen);
                break;
        }

        
    }

    #region 显示方式
    /// <summary> 默认显示 </summary>
    void ShowNomal(PanelBase go, bool isOpen)
    {
        if (isOpen)
        {
            current.gameObject.SetActive(true);
            current.OnShowed();
        }
        else DestroyPanel(go.type);
    }
    /// <summary> 中间变大 </summary>
    void CenterScaleBigNomal(PanelBase go, bool isOpen)
    {
        TweenScale ts = go.gameObject.GetComponent<TweenScale>();
        if (ts == null)ts = go.gameObject.AddComponent<TweenScale>();
        //
        ts.from = Vector3.zero;
        ts.to = Vector3.one;
        ts.duration = go.OpenDuration;
        ts.method = UITweener.Method.EaseInOut;
        ts.SetOnFinished(() =>
        {
            if(isOpen) go.OnShowed();
            else DestroyPanel(go.type);
        });
        go.gameObject.SetActive(true);
        if (!isOpen) ts.Play(isOpen);
    }
    /// <summary> 左右往中 </summary>
    void LeftAndRightToSlide(PanelBase go, bool isRight,bool isOpen)
    {
        TweenPosition tp = go.gameObject.GetComponent<TweenPosition>();
        if (tp == null) tp = go.gameObject.AddComponent<TweenPosition>();
        tp.from = isRight == true ? new Vector3(640, 0, 0) : new Vector3(-640, 0, 0);
        tp.to = Vector3.zero;
        tp.duration = go.OpenDuration;
        tp.method = UITweener.Method.EaseInOut;
        tp.SetOnFinished(() =>
        {
            if (isOpen) go.OnShowed();
            else DestroyPanel(go.type);
        });
        go.gameObject.SetActive(true);
        if (!isOpen) tp.Play(isOpen);
    }
    /// <summary> 上下往中 </summary>
    void TopAndDownToSlide(PanelBase go, bool isTop,bool isOpen)
    {
        TweenPosition tp = go.gameObject.GetComponent<TweenPosition>();
        if (tp == null) tp = go.gameObject.AddComponent<TweenPosition>();
        //
        tp.from = isTop == true ? new Vector3(0, 640, 0) : new Vector3(0, -640, 0);
        tp.to = Vector3.zero;
        tp.duration = go.OpenDuration;
        tp.method = UITweener.Method.EaseInOut;
        tp.SetOnFinished(() =>
        {
            if (isOpen) go.OnShowed();
            else DestroyPanel(go.type);
        });
        go.gameObject.SetActive(true);
        if (!isOpen) tp.Play(isOpen);
    }

    #endregion

    #region 遮罩方式

    void MaskStyle(PanelBase go)
    {
        float alpha = 1;
        switch(go.PanelMaskStyle)
        {
            case PanelMaskSytle.Alpha:
                alpha = 0.001f;
                break;
            case PanelMaskSytle.BlackAlpha:
                alpha = 0.5f;
                break;
        }
        GameObject mask = ResourceMgr.GetInstance().CreateGameObject("Public/prefab/PanelMask", true);
        mask.transform.parent = go.gameObject.transform;
        mask.transform.localPosition = Vector3.zero;
        mask.transform.localEulerAngles = Vector3.zero;
        mask.transform.localScale = Vector3.one;

        UIPanel panel = mask.GetComponent<UIPanel>();
        panel.alpha = alpha;
        LayerMgr.GetInstance().SetLayer(go.gameObject, LayerType.Panel);
    }

    #endregion

    /// <summary>
    /// 发起关闭
    /// </summary>
    public void HidePanel(PanelName type)
    {
        if (panels.ContainsKey(type))
        {
            PanelBase pb = null;
            pb = panels[type];
            StartShowPanel(pb, pb.PanelShowStyle, false);
        }
        else Debug.LogError("关闭的面板不存在!");
    }


    /// <summary>
    /// 强制摧毁面板
    /// </summary>
    /// <param name="panel"></param>
    public void DestroyPanel(PanelName type)
    {
        if (panels.ContainsKey(type))
        {
            PanelBase pb = panels[type];
            if (!pb.cache)
            {
                GameObject.Destroy(pb.gameObject);
                panels.Remove(type);
            }
        }
    }
}
