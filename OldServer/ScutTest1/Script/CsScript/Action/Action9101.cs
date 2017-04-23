using GameServer.Model;
using System;
using System.Collections.Generic;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;

namespace GameServer.CsScript.Action
{

    /// <summary>
    /// 好友列表接口
    /// </summary>
    /// <remarks>继续BaseStruct类:允许无身份认证的请求;AuthorizeAction:需要身份认证的请求</remarks>
    public class Action9101 : AuthorizeAction
    {

        #region class object

        #endregion

        /// <summary>
        /// 好友类型
        /// </summary>
        private FriendType _friendType;
        /// <summary>
        /// 
        /// </summary>
        private int _pageIndex;
        /// <summary>
        /// 
        /// </summary>
        private int _pageSize;
        /// <summary>
        /// 是否只显示在线
        /// </summary>
        private int _isOnLine;
        /// <summary>
        /// 
        /// </summary>
        private int _pageCount;
        /// <summary>
        /// 
        /// </summary>
        private List<UserFriends> userFriendsesArray = new List<UserFriends>();


        public Action9101(ActionGetter actionGetter)
            : base((short)9101, actionGetter)
        {

        }

        /// <summary>
        /// 检查的Action是否需要授权访问
        /// </summary>
        protected override bool IgnoreActionId
        {
            get { return true; }
        }

        /// <summary>
        /// 客户端请求的参数较验
        /// </summary>
        /// <returns>false:中断后面的方式执行并返回Error</returns>
        public override bool GetUrlElement()
        {
            //if (httpGet.GetEnum("FriendType", ref _friendType)
            //    && httpGet.GetInt("PageIndex", ref _pageIndex)
            //    && httpGet.GetInt("PageSize", ref _pageSize))
            //{
            //    httpGet.GetInt("IsOnLine", ref _isOnLine);
            //    return true;
            //}
            return true;
        }

        /// <summary>
        /// 业务逻辑处理
        /// </summary>
        /// <returns>false:中断后面的方式执行并返回Error</returns>
        public override bool TakeAction()
        {
            userFriendsesArray = new ShareCacheStruct<UserFriends>().FindAll(m => m.UserID.ToString() == UserId.ToString());
            Console.WriteLine("好友个数:"+userFriendsesArray.Count);
            return true;
        }

        /// <summary>
        /// 下发给客户的包结构数据
        /// </summary>
        public override void BuildPacket()
        {
            this.PushIntoStack(_pageCount);
            this.PushIntoStack(userFriendsesArray.Count);
            foreach (var item in userFriendsesArray)
            {
                GameUser gameUser = UserCacheGlobal.LoadOffline(item.FriendID);
                Console.WriteLine("---"+gameUser.UserId+"---"+item.FriendID);
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.FriendID);
                dsItem.PushIntoStack(gameUser.NickName);
                Console.WriteLine(gameUser.NickName);
                dsItem.PushIntoStack(gameUser.Lv);
                this.PushIntoStack(dsItem);
            }
        }

    }
}
