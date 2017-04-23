using UnityEngine;
using System.Collections;

public class PanelTest : PanelBase
{

    #region 界面加载相关
    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type = PanelName.PanelTest;//面板名称
        _openDuration = 0.2f;//面板打开时间
        _maskStyle = PanelMgr.PanelMaskSytle.Alpha;//面板打开遮罩类型
        _showStyle = PanelMgr.PanelShowStyle.CenterScaleBigNomal;//面板打开方式
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Game/Test/PanelTest");
    }
    protected override void OnInitDone()
    {
        base.OnInitDone();
        //InitData();

    }
    protected override void OnDestroyDone()
    {
        base.OnDestroyDone();

    }
    protected override void OnDestroyFront()
    {
        base.OnDestroyFront();
    }
    protected override void OnClick(GameObject target)
    {
        base.OnClick(target);
        Close();
        //ButtonClick(target);
    }

    public override void OnInit(params object[] sceneArgs)
    {
        base.OnInit(sceneArgs);
    }
    public override void OnShowed()
    {
        base.OnShowed();

    }
    #endregion
}
