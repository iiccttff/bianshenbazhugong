/*
 * 脚本名(ScriptName)：    SerializationHelper.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;
using System.IO;

public class SerializationHelper  
{

    public SerializationHelper() { }
    #region 序列化proto
    /// <summary>
    /// 序列化成proto
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static byte[] Serialize(MuffinMsg msg)
    {
        switch (msg.cmdId)
        {
            case NetMessageDef.ReqLogin:
                {

                    protos.Login.ReqLogin request = msg.pBObject as protos.Login.ReqLogin;
                    MemoryStream memStream = new MemoryStream();
                    ProtoBuf.Serializer.Serialize<protos.Login.ReqLogin>(memStream, request);
                    byte[] x = memStream.ToArray();
                    memStream.Close();
                    return x;
                }
            case NetMessageDef.ReqCreateAccount:
                {

                    protos.Login.ReqCreateAccount request = msg.pBObject as protos.Login.ReqCreateAccount;
                    MemoryStream memStream = new MemoryStream();
                    ProtoBuf.Serializer.Serialize<protos.Login.ReqCreateAccount>(memStream, request);
                    byte[] x = memStream.ToArray();
                    memStream.Close();
                    return x;
                }
            case NetMessageDef.ReqGetRole:
                {
                    protos.Login.ReqGetRole request = msg.pBObject as protos.Login.ReqGetRole;
                    MemoryStream memStream = new MemoryStream();
                    ProtoBuf.Serializer.Serialize<protos.Login.ReqGetRole>(memStream, request);
                    byte[] x = memStream.ToArray();
                    memStream.Close();
                    return x;
                }
            case NetMessageDef.ResGetRole:
                {
                    protos.Login.ResGetRole request = msg.pBObject as protos.Login.ResGetRole;
                    MemoryStream memStream = new MemoryStream();
                    ProtoBuf.Serializer.Serialize<protos.Login.ResGetRole>(memStream, request);
                    byte[] x = memStream.ToArray();
                    memStream.Close();
                    return x;
                }
            case NetMessageDef.ResReturnDefaultInfo:
                {
                    protos.ReturnMessage.ResDefaultInfo request = msg.pBObject as protos.ReturnMessage.ResDefaultInfo;
                    MemoryStream memStream = new MemoryStream();
                    ProtoBuf.Serializer.Serialize<protos.ReturnMessage.ResDefaultInfo>(memStream, request);
                    byte[] x = memStream.ToArray();
                    memStream.Close();
                    return x;
                }
            default:
                {
                    //Console.WriteLine("MuffinMsg.Serialize: unhandled cmdId->" + msg.cmdId.ToString());
                    return null;
                }
        }
    }
    #endregion

    #region 反序列化proto
    /// <summary>
    /// 反序列化proto
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static object Deserialize(uint cmdId, byte[] bytes)
    {
        switch (cmdId)
        {
            case NetMessageDef.ReqLogin:
                {
                    return ProtoBuf.Serializer.Deserialize<protos.Login.ReqLogin>(new MemoryStream(bytes));
                }
            case NetMessageDef.ReqCreateAccount:
                {
                    return ProtoBuf.Serializer.Deserialize<protos.Login.ReqCreateAccount>(new MemoryStream(bytes));
                }
            case NetMessageDef.ResReturnDefaultInfo:
                {
                    return ProtoBuf.Serializer.Deserialize<protos.ReturnMessage.ResDefaultInfo>(new MemoryStream(bytes));
                }
            case NetMessageDef.ReqGetRole:
                {
                    return ProtoBuf.Serializer.Deserialize<protos.Login.ReqGetRole>(new MemoryStream(bytes));
                }
            case NetMessageDef.ResGetRole:
                {
                    return ProtoBuf.Serializer.Deserialize<protos.Login.ResGetRole>(new MemoryStream(bytes));
                }
            default:
                {
                    //Console.WriteLine("MuffinMsg.Deserialize: unhandled cmdId->" + cmdId.ToString());
                    return null;
                }
        }
        //return ProtoBuf.Serializer.Deserialize<test.HeroLevel>(new MemoryStream(bytes));
    }
    #endregion
}

public class MuffinMsg
{
    public uint cmdId = 0;
    /// <summary>
    /// ProtocalBuffer的数据类
    /// </summary>
    public object pBObject = null;

    public MuffinMsg(uint cmd, object pb)
    {
        cmdId = cmd;
        pBObject = pb;
    }
}
