/*
 * 脚本名(ScriptName)：    TestResMgr.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;

public class TestResMgr : MonoBehaviour 
{

	// Use this for initialization
    void Start()
    {
        //GameObject obj = ResourcesMgr.GetInstance().CreateGameObject("Game/UI/SceneMail", false);
        SceneMgr.Instatance.SwitchScene(SceneType.SceneLogin, "haha", 22, false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
