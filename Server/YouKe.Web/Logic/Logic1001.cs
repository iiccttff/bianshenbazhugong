using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using YouKe.Common;

namespace YouKe.Web.Logic
{
    /// <summary>
    /// 创建角色
    /// </summary>
    public class Logic1001
    {
        public byte[] Init(Stream inputSteam, string httpMethod,uint user_id)
        {

            byte[] bytes = new byte[inputSteam.Length];
            inputSteam.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            inputSteam.Seek(0, SeekOrigin.Begin);

            object obj = SerializationHelper.Deserialize(NetMessageDef.ReqCreateRole, bytes);

            protos.Login.ReqCreateRole reqCreateRole = obj as protos.Login.ReqCreateRole;


            protos.ReturnMessage.ResDefaultInfo resInfo = new protos.ReturnMessage.ResDefaultInfo();

            if (string.IsNullOrEmpty(reqCreateRole.user_name))
            {
                resInfo.results = 0;
                resInfo.details = "昵称不能为空!";
            }

            Model.users mUser = new BLL.users().GetModel(user_id);

            if (mUser != null)
            {
                mUser.user_name = reqCreateRole.user_name;
                resInfo.results = 2;
                resInfo.details = "创建成功!";
            }
            else
            {
                resInfo.results = 0;
                resInfo.details = "账号不存在!";
            }

            return SerializationHelper.Serialize(new MuffinMsg(NetMessageDef.ResReturnDefaultInfo, resInfo)); 
        }
    }
}