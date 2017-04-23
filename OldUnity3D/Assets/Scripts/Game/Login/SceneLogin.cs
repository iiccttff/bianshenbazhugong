using UnityEngine;
using System.Collections;

public class SceneLogin : SceneBase
{
    #region 界面加载相关
    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type = SceneType.SceneLogin;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Game/Login/SceneLogin");
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

    private UIInput mInputAccount;
    private UIInput mInputPassword;

    #endregion

    #region ui逻辑

    void InitData()
    {
        mInputAccount = skinTransform.Find("InputAccount").GetComponent<UIInput>();
        mInputPassword = skinTransform.Find("InputPassword").GetComponent<UIInput>();
        if (PlayerPrefs.GetString("Account") != null && PlayerPrefs.GetString("Account") != "")
        {
            mInputAccount.value = PlayerPrefs.GetString("Account");
        }
        if (PlayerPrefs.GetString("Password") != null && PlayerPrefs.GetString("Password") != "")
        {
            mInputPassword.value = PlayerPrefs.GetString("Password");
        }
    }

    void ButtonClick(GameObject click)
    {
        if(click.name.Equals("BtnLogin"))
        {

            if (mInputAccount.value == "")
            {
                LogicMgr.GetInstance().GetLogic<LogicTips>().AddTips("没有输入账号！");
            }
            else if (mInputPassword.value == "")
            {
                LogicMgr.GetInstance().GetLogic<LogicTips>().AddTips("没有输入密码！");
            }
            else
            {
                ActionParam ap = new ActionParam();
                GameSetting.Instance.Pid = mInputAccount.value;
                GameSetting.Instance.Password = mInputPassword.value;
                Net.Instance.Send((int)ActionType.Login, LoginReturn, null);
            }
        }
        else if(click.name.Equals("BtnRegis"))
        {
            Net.Instance.Send((int)ActionType.Regis, RegisReturn, null);
            //ActionParam ap = new ActionParam();
            //ap["account"] = mInputAccount.value;
            //ap["password"] = mInputPassword.value;
            //Net.Instance.Send((int)ActionType.Reg, RegisReturnTest, ap);
        }
    }
    #endregion

    #region 服务器返回

    void LoginReturn(ActionResult action)
    {
        
        if(action != null)
        {
            PlayerPrefs.SetString("Account", mInputAccount.value);
            PlayerPrefs.SetString("Password", mInputPassword.value);
        }
        if (action != null && action.Get<int>("GuideID") == (int)ActionType.CreateRote)
        {
            //登陆返回未创建角色。需要跳转创建角色
            SceneMgr.GetInstance().SwitchingScene(SceneType.SceneCreateRote,111,222,333,444);
        }
        else
        {
            Debug.LogError(action.Get<string>("UserID"));
            SceneMgr.GetInstance().SwitchingScene(SceneType.SceneLoading);
        }
    }

    void RegisReturn(ActionResult action)
    {
        if (action == null) return;
        Debug.LogError("一键注册成功：账号:" + action.Get<string>("passportID") + "  密码：" + action.Get<string>("password"));
        mInputAccount.value = action.Get<string>("passportID");
        mInputPassword.value = action.Get<string>("password");
    }
    void RegisReturnTest(ActionResult action)
    {

    }

    #endregion
}
