using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using YouKe.Common;

namespace YouKe.Web.Logic
{
    /// <summary>
    /// 获取角色信息
    /// </summary>
    public class Logic1002
    {
        public byte[] Init(Stream inputSteam, string httpMethod,uint user_id)
        {
            protos.Login.ResGetRole resMessageGetRole = new protos.Login.ResGetRole();


            Model.users userModel = new BLL.users().GetModel(user_id);
            if(userModel != null)
            {
                resMessageGetRole.uid = userModel.id.ToString();
                resMessageGetRole.gold = userModel.gold;
                resMessageGetRole.endurance = userModel.endurance;
                resMessageGetRole.head = 0;//userModel.head;
                resMessageGetRole.lv = userModel.lv;
                resMessageGetRole.user_name = userModel.user_name;
                resMessageGetRole.vip = userModel.vip;
                resMessageGetRole.wing = userModel.wing;
            }


            return SerializationHelper.Serialize(new MuffinMsg(NetMessageDef.ResGetRole, resMessageGetRole)); 
        }
    }
}