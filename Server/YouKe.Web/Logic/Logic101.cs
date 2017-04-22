using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using YouKe.Common;

namespace YouKe.Web.Logic
{
    /// <summary>
    /// 创建账号
    /// </summary>
    public class Logic101
    {
        public byte[] Init(Stream inputSteam, string httpMethod,uint user_id)
        {

            byte[] bytes = new byte[inputSteam.Length];
            inputSteam.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            inputSteam.Seek(0, SeekOrigin.Begin);

            object obj = SerializationHelper.Deserialize(NetMessageDef.ReqCreateAccount, bytes);

            protos.Login.ReqCreateAccount reqCreateAccount = obj as protos.Login.ReqCreateAccount;


            protos.ReturnMessage.ResDefaultInfo resInfo = new protos.ReturnMessage.ResDefaultInfo();

            

            if (string.IsNullOrEmpty(reqCreateAccount.account))
            {
                resInfo.results = 0;
                resInfo.details = "账号不能为空!";
            }
            else if(string.IsNullOrEmpty(reqCreateAccount.password))
            {
                resInfo.results = 0;
                resInfo.details = "密码不能为空!";
            }
            
            if (new BLL.users().ExistsAcc(reqCreateAccount.account))
            {
                resInfo.results = 0;
                resInfo.details = "账号已存在";
            }
            else
            {
                Model.users newUser = new Model.users();
                newUser.user_account = reqCreateAccount.account;
                newUser.user_passworld = reqCreateAccount.password;
                newUser.reg_time = DateTime.Now;
                newUser.login_time = DateTime.Now;
                newUser.endurance = 100;
                newUser.exp = 0;
                newUser.gold = 1000;
                newUser.head = "";
                newUser.lv = 1;
                newUser.user_name = "";
                newUser.vip = 0;
                newUser.wing = 0;


                if (new BLL.users().Add(newUser) > 0)
                {
                    resInfo.results = 2;
                    resInfo.details = "注册成功";
                }
                else
                {
                    resInfo.results = 0;
                    resInfo.details = "注册失败";
                }
            }

            return SerializationHelper.Serialize(new MuffinMsg(NetMessageDef.ResReturnDefaultInfo, resInfo)); 
        }
    }
}