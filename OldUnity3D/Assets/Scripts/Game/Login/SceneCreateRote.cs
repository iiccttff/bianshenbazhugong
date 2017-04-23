using UnityEngine;
using System.Collections;

public class SceneCreateRote : SceneBase
{
    #region 界面加载相关
    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type = SceneType.SceneCreateRote;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Game/Login/SceneCreateRote");
    }
    protected override void OnInitDone()
    {
        base.OnInitDone();

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
        int i1 = (int)sceneArgs[0];
        int i2 = (int)sceneArgs[1];
        Debug.LogError(i1 + "--" + i2);
    }
    public override void OnShowed()
    {
        base.OnShowed();
        InitData();
    }
    #endregion

    #region 数据定义

    private UIInput mInputAccount;
    private UITexture mBtnMan;
    private UITexture mBtnWoman;
    private int mSex = 0;
    #endregion

    #region ui逻辑

    void InitData()
    {
        mInputAccount = skinTransform.Find("InputAccount").GetComponent<UIInput>();
        mBtnMan = skinTransform.Find("BtnMan").GetComponent<UITexture>();
        mBtnWoman = skinTransform.Find("BtnWoman").GetComponent<UITexture>();
    }

    void ButtonClick(GameObject click)
    {
        if (click.name.Equals("BtnCreate"))
        {
            ActionParam ap = new ActionParam();
            ap["roleName"] = mInputAccount.value;
            ap["Sex"] = mSex;
            Net.Instance.Send((int)ActionType.CreateRote, CreateRoteReturn, ap);
        }
        else if (click.name.Equals("BtnMan"))
        {
            mSex = 0;
            mBtnWoman.shader = Shader.Find("Unlit/Transparent Colored Gray");
            mBtnMan.shader = Shader.Find("Unlit/Transparent Colored");
        }
        else if (click.name.Equals("BtnWoman"))
        {
            mSex = 1;
            mBtnMan.shader = Shader.Find("Unlit/Transparent Colored Gray");
            mBtnWoman.shader = Shader.Find("Unlit/Transparent Colored");
        }
    }

    #endregion

    #region 服务器返回
    void CreateRoteReturn(ActionResult action)
    {
        SceneMgr.GetInstance().SwitchingScene(SceneType.SceneLoading);
    }
    #endregion
}
