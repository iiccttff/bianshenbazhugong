/*
 * 脚本名(ScriptName)：    SceneHome.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;

public class SceneHome : SceneBase
{
    #region 初始化相关
    protected override void OnInitSkin()
    {
        base.SetMainSkinPath("Game/UI/Home/SceneHome");
        base.OnInitSkin();
    }

    protected override void OnInitDone()
    {
        base.OnInitDone();
        mUserModel = ModelMgr.Instatance.GetModel<UserModel>();
        Init();
    }

    protected override void OnClick(GameObject click)
    {
        base.OnClick(click);
        ClickButton(click);
    }

    public override void OnResetArgs(params object[] sceneArgs)
    {
        base.OnResetArgs(sceneArgs);
    }

    #endregion

    #region 数据定义
    private UserModel mUserModel;
    private UILabel mName;
    private UILabel mVIP;
    private UILabel mLv;
    /// <summary>
    /// 体力
    /// </summary>
    private UILabel mPhysical;
    /// <summary>
    /// 金币
    /// </summary>
    private UILabel mGold;
    /// <summary>
    /// 元宝
    /// </summary>
    private UILabel mWing;
    #endregion

    #region 模块初始化
    void Init()
    {
        mName = skinTransform.Find("InfoBg/Name").GetComponent<UILabel>();
        mVIP = skinTransform.Find("InfoBg/VIP").GetComponent<UILabel>();
        mLv = skinTransform.Find("InfoBg/Lv").GetComponent<UILabel>();
        mPhysical = skinTransform.Find("TopInfo/Physical/Nums").GetComponent<UILabel>();
        mGold = skinTransform.Find("TopInfo/Gold/Nums").GetComponent<UILabel>();
        mWing = skinTransform.Find("TopInfo/Wing/Nums").GetComponent<UILabel>();

        mName.text = mUserModel.userName;
        mWing.text = mUserModel.wing.ToString();
        mGold.text = mUserModel.gold.ToString();
        mVIP.text = "VIP" + mUserModel.vip.ToString();
        mPhysical.text = mUserModel.endurance.ToString() + "/100";
        mLv.text = mUserModel.lv.ToString();
    }


    #endregion
    #region 点击事件

    void ClickButton(GameObject click)
    {
        if(click.name.Equals("BtnMail"))
        {
            SceneMgr.Instatance.SwitchScene(SceneType.SceneMail);
        }
        else if(click.name.Equals("BtnHeadIcon"))
        {
            PanelMgr.Instatance.ShowPanel(PanelType.PanelPlayInfo);
        }
        else if(click.name.Equals("BtnShop"))
        {
            SceneMgr.Instatance.SwitchScene(SceneType.SceneShop);
        }
    }
    #endregion
}
