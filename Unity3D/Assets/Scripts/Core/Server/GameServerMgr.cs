/*
 * 脚本名(ScriptName)：    GameServerMgr.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using protos.Login;

public class GameServerMgr : MonoBehaviour
{
    private static GameServerMgr mInstance;

    public static bool hasIntance
    {
        get
        {
            return mInstance != null;
        }
    }

    /// <summary>
    /// 是否正在删除，当程序退出时设置为true
    /// </summary>
    public static bool isDestorying = false;

    private string Sendlink = "http://127.0.0.1:45/test.aspx";//取消直接传协议ID，改为用包头发送
    /// <summary>
    /// 消息节点列表(收听器列表)
    /// key为resId
    /// </summary>
    protected Dictionary<uint, Register> registers;

    void OnApplicationQuit()
    {
        isDestorying = true;
    }

    /// <summary>
    /// 获取单例
    /// </summary>
    /// <returns></returns>
    public static GameServerMgr GetInstance()
    {
        if(!hasIntance)
        {
            if(isDestorying)
            {
                return null;
            }
            mInstance = new GameObject("_GameServerMgr").AddComponent<GameServerMgr>();
        }
        return mInstance;
    }

    internal void Awake()
    {
        registers = new Dictionary<uint, Register>();
    }
    internal void OnDestroy()
    {
        StopAllCoroutines();

        registers.Clear();
        registers = null;
    }
    public void Test()
    {
        ReqLogin loginTest = new ReqLogin();
        loginTest.account = "111";
        loginTest.password = "111";
        byte[] bytes = SerializationHelper.Serialize(new MuffinMsg(NetMessageDef.ReqLogin,loginTest));
        StartCoroutine(SendMessage(bytes, NetMessageDef.ReqLogin));
    }
    /// <summary>
    /// 发送服务器消息
    /// </summary>
    /// <param name="muffin"></param>
    public void SendMessage(MuffinMsg muffin)
    {
        byte[] bytes = SerializationHelper.Serialize(muffin);
        StartCoroutine(SendMessage(bytes, muffin.cmdId));
    }
    /// <summary>
    /// www发送接收数据
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    IEnumerator SendMessage(byte[] bytes, uint cmdId)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("CONTENT_TYPE", "multipart/form-data");
        headers.Add("cmdId", cmdId.ToString());
        headers.Add("userId", "1");
        Debug.Log("发送消息:"+cmdId);
        WWW getData = new WWW(Sendlink, bytes, headers);
        yield return getData;
        try
        {
            if (getData.error != null)
            {
                Debug.LogError(string.Format("GetData={0}", getData.error));
            }
            else
            {
                //解包头
                Dictionary<string,string> header = getData.responseHeaders;

                uint id = uint.Parse(header["CMDID"]);
                Debug.Log(header["CMDID"]);

                object obj = SerializationHelper.Deserialize((uint)id, getData.bytes);
                if(id == NetMessageDef.ResReturnDefaultInfo)
                {
                    protos.ReturnMessage.ResDefaultInfo resDefaultInfo = obj as protos.ReturnMessage.ResDefaultInfo;
                    if(resDefaultInfo.results == 0)
                    {
                        Debug.LogError(resDefaultInfo.details);
                    }
                    else
                    {
                        Debug.Log(resDefaultInfo.details);
                    }
                }
                RequsterNotifier(id, obj, null);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning(ex.Message);
        }
    }

    /// <summary>
    /// 通知该协议已注册的收听器
    /// </summary>
    /// <param name="resId"></param>
    /// <param name="req"></param>
    /// <param name="res"></param>
    public void RequsterNotifier(uint resId, object param1, object param2)
    {
        if (registers.ContainsKey(resId))
        {
            INotifier[] notifiers = registers[resId].notifiers.ToArray();//notifier.OnReceiveData中有可能改变notifiers的长度。所以用notifiers的副本
            foreach (INotifier notifier in notifiers)
            {
                notifier.OnReceiveData(resId, param1, param2);
            }
        }
    }

    /// <summary>
    /// 注册收听器
    /// </summary>
    /// <param name="resId"></param>
    /// <param name="notifier"></param>
    public void RegisterNotifier(uint resId, INotifier notifier)
    {
        Debug.Log(string.Format("注册收听器:{0}", resId));
        Register register;
        if (registers.ContainsKey(resId))
        {
            register = registers[resId];
        }
        else
        {
            //如果不存在。则创建一个消息节点
            register = new Register();
            register.resId = resId;
            registers.Add(resId, register);
        }
        register.notifiers.Add(notifier);
    }
    /// <summary>
    /// 取消注册收听器
    /// </summary>
    /// <param name="resId"></param>
    /// <param name="notifier"></param>
    public void UnregisterNotifier(uint resId, INotifier notifier)
    {
        Debug.Log(string.Format("取消注册收听器:{0}", resId));
        if (registers.ContainsKey(resId))
        {
            Register register = registers[resId];
            if (!register.notifiers.Contains(notifier))
            {
                Debug.LogError(string.Format("试图对没有注册过的收听器取消注册：{0}", resId));
            }
            else
            {
                register.notifiers.Remove(notifier);
                if (register.notifiers.Count == 0)
                {
                    registers.Remove(resId);
                }
            }
        }
        else
        {
            Debug.LogError("试图对没有注册过的收听器取消注册：" + resId); 
        }
    }
}
