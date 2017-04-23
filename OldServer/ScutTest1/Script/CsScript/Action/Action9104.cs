using GameServer.CsScript.Lang;
using GameServer.Model;
using System;
using System.Collections.Generic;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Game.Service;

namespace GameServer.CsScript.Action
{

    /// <summary>
    /// 删除好友接口
    /// </summary>
    /// <remarks>继续BaseStruct类:允许无身份认证的请求;AuthorizeAction:需要身份认证的请求</remarks>
    public class Action9104 : AuthorizeAction
    {

        #region class object
        #endregion

        /// <summary>
        /// 
        /// </summary>
        private string _friendID;
        private int _isSuccess;

        public Action9104(ActionGetter actionGetter)
            : base((short)9104, actionGetter)
        {

        }

        public GameUser ContextUser
        {
            get
            {
                //return Current.User as GameUser;
                return PersonalCacheStruct.Get<GameUser>(Current.UserId.ToString());
            }
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
            if (httpGet.GetString("FriendID", ref _friendID))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 业务逻辑处理
        /// </summary>
        /// <returns>false:中断后面的方式执行并返回Error</returns>
        public override bool TakeAction()
        {
            _isSuccess = 1;

            var cacheSet = new ShareCacheStruct<UserFriends>();
            UserFriends userFriend = cacheSet.FindKey(ContextUser.UserId, _friendID);
            UserFriends userFriend1 = cacheSet.FindKey(_friendID, ContextUser.UserId);
            
            if(userFriend == null)
            {
                this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                this.ErrorInfo = LanguageManager.GetLang().St9104_NotFriendsUserID;
                return false;
            }

            //Console.WriteLine("***" + userFriend1.UserID);
            //如果原来是好友 要将对方的状态改为关注 其他的直接删除
            if (userFriend.FriendType == FriendType.Friend)
            {
                cacheSet.Delete(userFriend);
                if (userFriend1 != null) userFriend1.FriendType = FriendType.Attention;
            }
            else
            {
                cacheSet.Delete(userFriend);
            }

            return true;
        }

        /// <summary>
        /// 下发给客户的包结构数据
        /// </summary>
        public override void BuildPacket()
        {
            PushIntoStack(_isSuccess);
        }

    }
}
