using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogicTips : LogicBase
{
    protected override void Awake()
    {
        mTipsQueue = new List<string>();
    }

    protected override void OnDestroy()
    {
        mTipsQueue.Clear();
        mTipsQueue = null;
    }
    private List<string> mTipsQueue;

    public void AddTips(string content)
    {
        GameObject tipsObj = ResourceMgr.GetInstance().CreateGameObject("Public/prefab/PanelTips", true);
        LayerMgr.GetInstance().SetLayer(tipsObj, LayerType.Tips);
        tipsObj.transform.localPosition = Vector3.zero;
        tipsObj.transform.localScale = Vector3.one;
        tipsObj.transform.localEulerAngles = Vector3.zero;
        TipsView tv = tipsObj.GetComponent<TipsView>();
        tv.StartTips(content);
    }
}
