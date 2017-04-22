/*
 * 脚本名(ScriptName)：    SceneChat.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;

public class SceneChat : SceneBase 
{
    #region 初始化相关
    protected override void OnInitSkin()
    {
        base.SetMainSkinPath("Game/UI/Chat/SceneChat");
        base.OnInitSkin();
    }

    protected override void OnInitDone()
    {
        base.OnInitDone();


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

    #region 点击事件

    void ClickButton(GameObject click)
    {
        if (click.name.Equals("BtnReturn"))
        {
            SceneMgr.Instatance.SwitchToPrevScene();
        }
    }
    #endregion
	
}
