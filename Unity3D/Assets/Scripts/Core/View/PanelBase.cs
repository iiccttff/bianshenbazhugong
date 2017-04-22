/*
 * 脚本名(ScriptName)：    PanelBase.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelBase : UIBase 
{
    protected PanelType _type;
    public PanelType type
    {
        get
        {
            return _type;
        }
    }
    /// <summary> 面板打开时间 </summary>
    protected float _openDuration = 0.2f;
    /// <summary> 面板打开时间 </summary>
    public float openDuration
    {
        get
        {
            return _openDuration;
        }
    }

    protected PanelMgr.PanelShowStyle _showStyle = PanelMgr.PanelShowStyle.CenterScaleBigNomal;
    /// <summary> 面板显示方式 </summary>
    public PanelMgr.PanelShowStyle panelShowStyle
    {
        get
        {
            return _showStyle;
        }
    }

    #region 页面处理方法

    /// <summary>
    /// 初始化场景
    /// </summary>
    /// <param name="sceneArgs"></param>
    public virtual void OnInit(params object[] sceneArgs)
    {
        _sceneArgs = sceneArgs;
        Init();
    }

    /// <summary>
    /// 发起关闭，播放效果
    /// </summary>
    protected void Colse()
    {
        //Destroy(this.gameObject);
        PanelMgr.Instatance.HidePanel(type);
    }
    /// <summary>
    /// 立刻关闭
    /// </summary>
    protected void ColseImmediate()
    {
        PanelMgr.Instatance.DestroyPanel(type);
    }
    #endregion
}
public enum PanelType
{
    /// <summary> 玩家信息面板 </summary>
    PanelPlayInfo,
    PanelPlayInfoAll,
    /// <summary> 商品购买页面 </summary>
    PanelShop,
}