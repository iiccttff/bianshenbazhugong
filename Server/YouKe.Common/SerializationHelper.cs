using ProtoBuf;
using System;
using System.IO;
using System.Xml.Serialization;

namespace YouKe.Common
{
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
                case NetMessageDef.ReqGetFriendList:
                    {
                        protos.friend.ReqGetFriendList request = msg.pBObject as protos.friend.ReqGetFriendList;
                        MemoryStream memStream = new MemoryStream();
                        ProtoBuf.Serializer.Serialize<protos.friend.ReqGetFriendList>(memStream, request);
                        byte[] x = memStream.ToArray();
                        memStream.Close();
                        return x;
                    }
                case NetMessageDef.ResFriendList:
                    {
                        protos.friend.ResFriendList request = msg.pBObject as protos.friend.ResFriendList;
                        MemoryStream memStream = new MemoryStream();
                        ProtoBuf.Serializer.Serialize<protos.friend.ResFriendList>(memStream, request);
                        byte[] x = memStream.ToArray();
                        memStream.Close();
                        return x;
                    }
                default:
                    {
                        Console.WriteLine("MuffinMsg.Serialize: unhandled cmdId->" + msg.cmdId.ToString());
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
                case NetMessageDef.ReqGetFriendList:
                    {
                        return ProtoBuf.Serializer.Deserialize<protos.friend.ReqGetFriendList>(new MemoryStream(bytes));
                    }
                case NetMessageDef.ResFriendList:
                    {
                        return ProtoBuf.Serializer.Deserialize<protos.friend.ResFriendList>(new MemoryStream(bytes));
                    }
                default:
                    {
                        Console.WriteLine("MuffinMsg.Deserialize: unhandled cmdId->" + cmdId.ToString());
                        return null;
                    }
            }
            //return ProtoBuf.Serializer.Deserialize<test.HeroLevel>(new MemoryStream(bytes));
        }
        #endregion
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filename">文件路径</param>
        /// <returns></returns>
        public static object Load(Type type, string filename)
        {
            FileStream fs = null;
            try
            {
                // open the stream...
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }


        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static void Save(object obj, string filename)
        {
            FileStream fs = null;
            // serialize it...
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }

        }

    }
}
