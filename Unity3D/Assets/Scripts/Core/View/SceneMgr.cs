/*
 * 脚本名(ScriptName)：    SceneMgr.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SceneMgr
{
    #region 初始化

    protected static SceneMgr mInstance;

    public static SceneMgr Instatance
    {
        get
        {
            if(mInstance == null)
            {
                mInstance = new SceneMgr();
            }
            return mInstance;
        }
    }

    private SceneMgr()
    {
        switchRecorder = new List<SwitchRecorder>();
    }

    public void OnDestroy()
    {
        switchRecorder.Clear();
        switchRecorder = null;
    }
    #endregion

    private GameObject curren;
    private Transform parentObj = null;
    private List<SwitchRecorder> switchRecorder;
    private string mainSceneName = "SceneHome";

    public void SwitchScene(SceneType sceneType, params object[] sceneArgs)
    {
        //GameObject scene = ResourcesMgr.GetInstance().CreateGameObject("Game/UI/" + name, false);
        string name = sceneType.ToString();
        GameObject scene = new GameObject(name);
        SceneBase baseObj = scene.AddComponent(Type.GetType(name)) as SceneBase;
        //baseObj.Init(sceneArgs);
        baseObj.OnInit(sceneArgs);
        if(parentObj == null)
        {
            parentObj = GameObject.Find("UI Root").transform;
        }
        scene.transform.parent = parentObj;
        scene.transform.localEulerAngles = Vector3.zero;
        scene.transform.localScale = Vector3.one;
        scene.transform.localPosition = Vector3.zero;
        LayerMgr.GetInstance().SetLayer(baseObj.gameObject, LayerType.Scene);

        if(name.Equals(mainSceneName))//如果进入主场景。则清空记录
        {
            switchRecorder.Clear();
        }

        switchRecorder.Add(new SwitchRecorder(sceneType, sceneArgs));
        if (curren != null)
        {
            GameObject.Destroy(curren);
        }
        curren = scene;
    }
    /// <summary>
    /// 切换到上一个场景
    /// </summary>
    public void SwitchToPrevScene()
    {
        SwitchRecorder sr = switchRecorder[switchRecorder.Count - 2];
        switchRecorder.RemoveRange(switchRecorder.Count - 2, 2);//将当前场景，以及要切换的场景。从记录中清空
        SwitchScene(sr.sceneType, sr.sceneArgs);
    }

    /// <summary>
    /// 记录结构体
    /// </summary>
    internal struct SwitchRecorder
    {
        internal SceneType sceneType;
        internal object[] sceneArgs;

        internal SwitchRecorder(SceneType sceneType, params object[] sceneArgs)
        {
            this.sceneType = sceneType;
            this.sceneArgs = sceneArgs;
        }
    }
}
