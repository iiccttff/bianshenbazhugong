using UnityEngine;
using System.Collections;

public class PanelUserInfo : PanelBase
{
    #region 界面加载相关
    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type = PanelName.PanelUserInfo;//面板名称
        _maskStyle = PanelMgr.PanelMaskSytle.BlackAlpha;//面板打开遮罩类型
        _showStyle = PanelMgr.PanelShowStyle.CenterScaleBigNomal;//面板打开方式
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Game/Main/PanelUserInfo");
    }
    protected override void OnInitDone()
    {
        base.OnInitDone();
    }
    protected override void OnDestroyDone()
    {
        base.OnDestroyDone();
        mUserModel = null;
    }
    protected override void OnDestroyFront()
    {
        base.OnDestroyFront();
    }
    protected override void OnClick(GameObject target)
    {
        base.OnClick(target);
        ButtonClick(target);
    }

    public override void OnInit(params object[] sceneArgs)
    {
        base.OnInit(sceneArgs);
        InitData();
    }
    public override void OnShowed()
    {
        base.OnShowed();
    }
    #endregion

    #region 数据定义
    private UILabel mName;
    private UILabel mLv;
    private UILabel mID;
    private UILabel mExp;
    private UserModel mUserModel;
    #endregion

    #region ui逻辑

    void InitData()
    {
        mUserModel = ModelMgr.GetInstance().GetModel<UserModel>();
        mName = skinTransform.Find("Panel/Name").GetComponent<UILabel>();
        mLv = skinTransform.Find("Panel/Lv").GetComponent<UILabel>();
        mID = skinTransform.Find("Panel/ID").GetComponent<UILabel>();
        mExp = skinTransform.Find("Panel/Exp").GetComponent<UILabel>();

        mName.text = mUserModel.name;
        mLv.text = "Lv "+ mUserModel.lv.ToString();
        mID.text = "ID 123456789";
        mExp.text = "经验 " + mUserModel.exp.ToString() + " / 100";
    }
    void ButtonClick(GameObject click)
    {
        Close();
    }
    #endregion
}
