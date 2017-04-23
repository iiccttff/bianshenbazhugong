using UnityEngine;
using System.Collections;

public class SceneLoading : SceneBase
{
    #region 界面加载相关
    protected override void OnInitFront()
    {
        base.OnInitFront();
        _type = SceneType.SceneLoading;
    }
    protected override void OnInitSkinFront()
    {
        base.OnInitSkinFront();
        SetMainSkinPath("Game/Loading/SceneLoading");
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

    private UILabel mSliderLabel;
    private UISlider mSlider;

    void InitData()
    {
        mSliderLabel = skinTransform.Find("SliderLabel").GetComponent<UILabel>();
        mSlider = skinTransform.Find("Slider").GetComponent<UISlider>();
        StartCoroutine(PlayTest());
    }

    IEnumerator PlayTest()
    {
        mSlider.value += 0.04f;
        mSliderLabel.text = (mSlider.value * 100).ToString("0.00") + " %";

        mSliderLabel.transform.localPosition = new Vector3(mSliderLabel.transform.localPosition.x + mSlider.foregroundWidget.width * 0.04f, mSliderLabel.transform.localPosition.y, 0);
        yield return new WaitForSeconds(0.1f);
        if(mSlider.value >= 1)
        {
            //LogicMgr.GetInstance().GetLogic<LogicTips>().AddTips("加载完成!");  
            SceneMgr.GetInstance().SwitchingScene(SceneType.SceneMain);
        }
        else StartCoroutine(PlayTest());
        
    }
}
