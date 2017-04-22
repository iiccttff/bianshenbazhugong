using UnityEngine;
using System.Collections;
using protos.ReturnMessage;
using protos.Login;

public class SceneLogin : SceneBase, INotifier
{
    private UIInput mInputAcc;
    private UIInput mInputPass;

    protected override void OnInitSkin()
    {
        base.SetMainSkinPath("Game/UI/SceneLogin");
        base.OnInitSkin();
    }

    protected override void OnInitDone()
    {
        base.OnInitDone();

        GameServerMgr.GetInstance().RegisterNotifier(NetMessageDef.ResReturnDefaultInfo, this);

        mInputAcc = skinTransform.Find("InputAcc").GetComponent<UIInput>();
        mInputPass = skinTransform.Find("InputPass").GetComponent<UIInput>();

        string str = (string)sceneArgs[0];
        int i = (int)sceneArgs[1];
        bool bo = (bool)sceneArgs[2];

        mInputAcc.value = PlayerPrefs.GetString("LoginAcc");
        mInputPass.value = PlayerPrefs.GetString("LoginPass");

        Debug.LogError(str + "---" + i + "---" + bo);
    }
    void OnDestroy()
    {
        GameServerMgr.GetInstance().UnregisterNotifier(NetMessageDef.ResReturnDefaultInfo, this);
    }
    public void OnReceiveData(uint cmdId, object param1, object param2)
    {

        switch(cmdId)
        {
            case NetMessageDef.ResReturnDefaultInfo:
                ResDefaultInfo info = param1 as ResDefaultInfo;
                if(info.results == 1)
                {
                    SceneMgr.Instatance.SwitchScene(SceneType.SceneLoding, "Hello youke.pro");
                    PlayerPrefs.SetString("LoginAcc", mInputAcc.value);
                    PlayerPrefs.SetString("LoginPass", mInputPass.value);
                }
                else if(info.results == 2)
                {
                    //跳转到创建角色界面
                    SceneMgr.Instatance.SwitchScene(SceneType.SceneLoding, "Hello youke.pro");
                }
                break;
        }
    }


    protected override void OnClick(GameObject click)
    {
        base.OnClick(click);
        ClickButton(click);
    }
    protected override void OnDestroyFront()
    {
        base.OnDestroyFront();
        Debug.Log("OnDestroyFront");
        mInputAcc = null;
        mInputPass = null;
    }
    protected override void OnDestroyEnd()
    {
        base.OnDestroyEnd();
        Debug.Log("OnDestroyEnd");
    }


    void ClickButton(GameObject click)
    {
        if (click.name.Equals("BtnLogin"))
        {
            if (string.IsNullOrEmpty(mInputAcc.value) || string.IsNullOrEmpty(mInputPass.value))
            {
                Debug.LogError("账号或密码不能为空");
                return;
            }
            ReqLogin login = new ReqLogin();
            login.account = mInputAcc.value;
            login.password = mInputPass.value;
            GameServerMgr.GetInstance().SendMessage(new MuffinMsg(NetMessageDef.ReqLogin, login));
        }
        else if (click.name.Equals("BtnReg"))
        {
            if (string.IsNullOrEmpty(mInputAcc.value) || string.IsNullOrEmpty(mInputPass.value))
            {
                Debug.LogError("注册的账号或密码不能为空，请输入");
                return;
            }

            ReqCreateAccount createAccount = new ReqCreateAccount();
            createAccount.account = mInputAcc.value;
            createAccount.password = mInputPass.value;
            GameServerMgr.GetInstance().SendMessage(new MuffinMsg(NetMessageDef.ReqCreateAccount, createAccount));
        }
    }

}
