using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouKe.Common
{
    public class NetMessageDef
    {
        /// <summary>
        /// 创建账号请求
        /// </summary>
        public const uint ReqCreateAccount = 101;
        /// <summary>
        /// 登陆请求
        /// </summary>
        public const uint ReqLogin = 102;

        /// <summary>
        /// 创建角色请求
        /// </summary>
        public const uint ReqCreateRole = 1001;

        /// <summary>
        /// 获取角色基本信息
        /// </summary>
        public const uint ReqGetRole = 1002;

        /// <summary>
        /// 获取角色基本信息请求
        /// </summary>
        public const uint ResGetRole = 1003;

        /// <summary>
        /// 响应结果(结果 or 细节)
        /// </summary>
        public const uint ResReturnDefaultInfo = 6001;

        /// <summary>
        /// 请求好友列表
        /// </summary>
        public const uint ReqGetFriendList = 201;
        /// <summary>
        /// 返回好友列表
        /// </summary>
        public const uint ResFriendList = 202;
    }
}
