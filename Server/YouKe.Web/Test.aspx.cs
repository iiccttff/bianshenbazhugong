using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YouKe.Common;
using YouKe.Web.Logic;

namespace YouKe.Web
{
    public partial class Test : System.Web.UI.Page
    {
        //private byte[] reqId;
        private byte[] bytes;
        private uint cmdId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            NameValueCollection header = Request.Headers;

            string[] reqCmdId = header.GetValues("cmdId");
            string[] reqUserId = header.GetValues("userId");


            uint id = uint.Parse(reqCmdId[0]);
            uint userId = uint.Parse(reqUserId[0]);
            
            string httpMethod = HttpContext.Current.Request.HttpMethod;
            Stream inputSteam = HttpContext.Current.Request.InputStream;

            switch (id)
            {
                case NetMessageDef.ReqLogin:
                    {
                        cmdId = NetMessageDef.ResReturnDefaultInfo;
                        Logic102 a = new Logic102();
                        bytes = a.Init(inputSteam, httpMethod, userId);
                    }
                    break;
                case NetMessageDef.ReqCreateAccount:
                    {
                        cmdId = NetMessageDef.ResReturnDefaultInfo;
                        Logic101 a = new Logic101();
                        bytes = a.Init(inputSteam, httpMethod, 0);
                    }
                    break;
                case NetMessageDef.ReqGetRole:
                    {
                        cmdId = NetMessageDef.ResGetRole;
                        Logic1002 a = new Logic1002();
                        bytes = a.Init(inputSteam, httpMethod, userId);
                    }
                    break;
                case NetMessageDef.ReqGetFriendList:
                    {
                        cmdId = NetMessageDef.ResFriendList;
                        Logic201 a = new Logic201();
                        bytes = a.Init(inputSteam, httpMethod, userId);
                    }
                    break;
            }
            Response.AddHeader("cmdId", cmdId.ToString());
            Response.AddHeader("userId", cmdId.ToString());
            //Response.BinaryWrite(reqId);
            Response.BinaryWrite(bytes);
            
        }
    }
}