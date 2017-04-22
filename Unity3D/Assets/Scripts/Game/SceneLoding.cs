/*
 * 脚本名(ScriptName)：    SceneLoding.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneLoding : SceneBase, INotifier
{
    private UISlider mSlider;
    private UILabel mLabel;
    protected override void OnInitSkin()
    {
        base.SetMainSkinPath("Game/UI/SceneLoding");
        base.OnInitSkin();
    }
    public void OnReceiveData(uint cmdId, object param1, object param2)
    {
        switch(cmdId)
        {
            case NetMessageDef.ResGetRole:
                protos.Login.ResGetRole info = param1 as protos.Login.ResGetRole;
                UserModel userModel = ModelMgr.Instatance.GetModel<UserModel>();
                userModel.userName = info.user_name;
                userModel.endurance = info.endurance;
                userModel.gold = info.gold;
                userModel.head = info.head;
                userModel.lv = info.lv;
                userModel.userId = int.Parse(info.uid);
                userModel.vip = info.vip;
                userModel.wing = info.wing;
                Debug.Log(info.user_name);
                StartCoroutine(Test());
                break;
        }
    }
    protected override void OnInitDone()
    {
        base.OnInitDone();
        GameServerMgr.GetInstance().RegisterNotifier(NetMessageDef.ResGetRole, this);
        string str = (string)sceneArgs[0];

        Debug.LogError(str);

        mSlider = skinTransform.Find("Slider").GetComponent<UISlider>();
        mLabel = skinTransform.Find("Label").GetComponent<UILabel>();
        mSlider.value = 0;
        SetLabelInfo(mSlider.value);

        protos.Login.ReqGetRole reqCreateRole = new protos.Login.ReqGetRole();
        reqCreateRole.uid = "0";
        GameServerMgr.GetInstance().SendMessage(new MuffinMsg(NetMessageDef.ReqGetRole, reqCreateRole));
        //StartCoroutine(Test());
    }

    void OnDestroy()
    {
        GameServerMgr.GetInstance().UnregisterNotifier(NetMessageDef.ResGetRole, this);
    }

    /// <summary>
    /// 通过异步。递归。实现模拟进度条
    /// </summary>
    /// <returns></returns>
    IEnumerator Test()
    {
        yield return 1;//暂停帧
        yield return new WaitForSeconds(0.001f);//暂停秒
        mSlider.value += 0.02f;
        SetLabelInfo(mSlider.value);
        if(mSlider.value < 1)
        {
            StartCoroutine(Test());
        }
        else
        {
            Debug.Log("加载完成了！");
            SceneMgr.Instatance.SwitchScene(SceneType.SceneHome, "haha22", 44, false);
        }
    }

    void SetLabelInfo(float value)
    {
        if(mLabel != null)
        {
            mLabel.text = (mSlider.value * 100.0f).ToString("f2") + "%";
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    
}
