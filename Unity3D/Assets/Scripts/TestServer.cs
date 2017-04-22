/*
 * 脚本名(ScriptName)：    TestServer.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;

public class TestServer : MonoBehaviour 
{

	// Use this for initialization
    void Start()
    {
        GameServerMgr.GetInstance().Test();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
