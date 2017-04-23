using UnityEngine;
using System.Collections;

public class SceneMain : SceneBase
{
    #region 界面加载相关
    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type = SceneType.SceneMain;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Game/Main/SceneMain");
    }
    protected override void OnInitDone()
    {
        base.OnInitDone();
        InitData();

    }
    protected override void OnDestroyDone()
    {
        base.OnDestroyDone();

    }
    protected override void OnDestroyFront()
    {
        base.OnDestroyFront();
        mUserModel.ValueUpdateEvent -= OnValueUpdateEventArgs;
        mUserModel = null;
    }
    protected override void OnClick(GameObject target)
    {
        base.OnClick(target);
        ButtonClick(target);
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

    #region 数据定义
    private UISprite mHeadIcon;
    private UILabel mNameLabel;
    private UILabel mVipLabel;
    private UILabel mActionLabel;
    private UILabel mGoldLabel;
    private UILabel mIngotLabel;
    private UILabel mZhanDouLiLabel;
    private UILabel mLvLabel;
    private UserModel mUserModel;
    #endregion

    #region 界面初始化
    void InitData()
    {
        mUserModel = ModelMgr.GetInstance().GetModel<UserModel>();
        mUserModel.ValueUpdateEvent += OnValueUpdateEventArgs;
        mHeadIcon = skinTransform.Find("HeadInfo/Head/HeadIcon").GetComponent<UISprite>();
        mNameLabel = skinTransform.Find("HeadInfo/NameLabel").GetComponent<UILabel>();
        mVipLabel = skinTransform.Find("HeadInfo/VipLabel").GetComponent<UILabel>();
        mActionLabel = skinTransform.Find("HeadInfo/ActionSprite/Label").GetComponent<UILabel>();
        mGoldLabel = skinTransform.Find("HeadInfo/GoldSprite/Label").GetComponent<UILabel>();
        mIngotLabel = skinTransform.Find("HeadInfo/IngotSprite/Label").GetComponent<UILabel>();
        mZhanDouLiLabel = skinTransform.Find("HeadInfo/ZhanDouLi").GetComponent<UILabel>();
        mLvLabel = skinTransform.Find("HeadInfo/LvSprite/Label").GetComponent<UILabel>();

        UIEventListener listener = UIEventListener.Get(mHeadIcon.transform.parent.gameObject);
        listener.onClick = ButtonClick;

        mNameLabel.text = mUserModel.name;
        mLvLabel.text = mUserModel.lv.ToString();
        mActionLabel.text = string.Concat(mUserModel.action.ToString(), "/ 100");
        mGoldLabel.text = mUserModel.gold.ToString();
        mIngotLabel.text = mUserModel.ingot.ToString();
    }

    #endregion

    #region 属性变更事件

    void OnValueUpdateEventArgs(object obj, ValueUpdateEventArgs v)
    {
        switch(v.key)
        {
            case "gold":
                ChangeGold((int)v.newValue, (int)v.oldValue);
                break;
        }
    }

    void ChangeGold(int newNums,int oldNums)
    {
        mGoldLabel.text = newNums.ToString();
    }
    #endregion

    #region 按钮点击事件

    void ButtonClick(GameObject click)
    {
        if(click.name.Equals("Head"))
        {
            PanelMgr.GetInstance().ShowPanel(PanelName.PanelUserInfo);
        }
        else if(click.name.Equals("BtnFriends"))
        {
            SceneMgr.GetInstance().SwitchingScene(SceneType.SceneFriends);
        }
    }
    #endregion
}
