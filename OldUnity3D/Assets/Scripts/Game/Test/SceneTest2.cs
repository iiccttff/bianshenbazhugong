using UnityEngine;
using System.Collections;

public class SceneTest2 : SceneBase
{
    #region 界面加载相关
    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type = SceneType.SceneTest2;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Game/Test/SceneTest2");
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

    #region 初始化相关

    void InitData()
    {
        
    }


    #endregion

    #region 点击事件

    void ButtonClick(GameObject click)
    {
        if (click.name.Equals("BtnOpen"))
        {
            SceneMgr.GetInstance().SwitchingScene(SceneType.SceneTest);
        }
    }

    #endregion
}
