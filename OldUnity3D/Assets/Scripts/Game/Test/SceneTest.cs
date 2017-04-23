using UnityEngine;
using System.Collections;

public class SceneTest : SceneBase 
{
    #region 界面加载相关
    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type = SceneType.SceneTest;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Game/Test/SceneTest");
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

    private GameObject mItem;
    void InitData()
    {
        mItem = skinTransform.Find("PanelMove/Items/Item").gameObject;
        ShowItems();
    }

    private void ShowItems()
    {
        Profiler.BeginSample("TestTTTTT");
        for(int i = 0;i < 50;i++)
        {
            GameObject item = Instantiate(mItem) as GameObject;
            item.transform.parent = mItem.transform.parent;
            item.transform.localEulerAngles = Vector3.zero;
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = new Vector3(0, 190 - i * 110, 0);
            item.SetActive(true);
        }
        Profiler.EndSample();
    }

    #endregion

    #region 点击事件

    void ButtonClick(GameObject click)
    {
        if (click.name.Equals("BtnOpen"))
        {
            PanelMgr.GetInstance().ShowPanel(PanelName.PanelTest);
        }
    }

    #endregion
}
