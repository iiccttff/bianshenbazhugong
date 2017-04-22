using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using YouKe.Common;

namespace YouKe.Web.Logic
{
    /// <summary>
    /// 登陆
    /// </summary>
    public class Logic102
    {
        public byte[] Init(Stream inputSteam, string httpMethod,uint user_id)
        {

            byte[] bytes = new byte[inputSteam.Length];
            inputSteam.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            inputSteam.Seek(0, SeekOrigin.Begin);

            object obj = SerializationHelper.Deserialize(NetMessageDef.ReqLogin, bytes);

            protos.Login.ReqLogin reqLogin = obj as protos.Login.ReqLogin;


            protos.ReturnMessage.ResDefaultInfo resInfo = new protos.ReturnMessage.ResDefaultInfo();

            if (string.IsNullOrEmpty(reqLogin.account))
            {
                resInfo.results = 0;
                resInfo.details = "账号不能为空!";
            }
            else if (string.IsNullOrEmpty(reqLogin.password))
            {
                resInfo.results = 0;
                resInfo.details = "密码不能为空!";
            }

            Model.users userModel = new BLL.users().GetModel(reqLogin.account, reqLogin.password);

            if (userModel != null)
            {
                resInfo.results = 1;
                resInfo.details = userModel.id.ToString();
            }
            else
            {
                resInfo.results = 0;
                resInfo.details = "账号或密码错误";
            }


            return SerializationHelper.Serialize(new MuffinMsg(NetMessageDef.ResReturnDefaultInfo, resInfo)); 
        }
    }
}