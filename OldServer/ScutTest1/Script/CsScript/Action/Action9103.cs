using GameServer.CsScript.Lang;
using GameServer.CsScript.Model.Enum;
using GameServer.Model;
using System;
using System.Collections.Generic;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Game.Service;
using ZyGames.Tianjiexing.Component.Chat;

namespace GameServer.CsScript.Action
{

    /// <summary>
    /// 添加好友接口
    /// </summary>
    /// <remarks>继续BaseStruct类:允许无身份认证的请求;AuthorizeAction:需要身份认证的请求</remarks>
    public class Action9103 : AuthorizeAction
    {

        #region class object
        #endregion

        /// <summary>
        /// 好友ID好友ID,其实就是userID
        /// </summary>
        private string _friendID;
        /// <summary>
        /// 好友名称好友名称
        /// </summary>
        private string _friendName;

        private int _isSuccess;

        public Action9103(ActionGetter actionGetter)
            : base((short)9103, actionGetter)
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
                httpGet.GetString("FriendName", ref _friendName);
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
            var cacheSet = new ShareCacheStruct<UserFriends>();
            _isSuccess = 1;
            if (_friendID != "")
            {
                //找到本玩家的数据
                List<UserFriends> friendArray = cacheSet.FindAll(m => m.UserID.ToString() == ContextUser.UserId.ToString());
                int friendNum = 100;//ConfigEnvSet.GetInt("UserFriends.MaxFriendNum");
                //添加的好友上限
                if (friendArray.Count >= friendNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St9103_TheMaximumReachedAFriend;
                    return false;
                }
                //查看是否在user库中有该玩家
                GameUser userInfo = new GameDataCacheSet<GameUser>().FindKey(_friendID);
                if (userInfo == null)
                {
                    UserCacheGlobal.LoadOffline(_friendID);
                    userInfo = new GameDataCacheSet<GameUser>().FindKey(_friendID);
                }
                if (userInfo == null)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St9103_NotFriendsUserID;
                    return false;
                }

                //在好友表中查找本玩家 和添加好友的关系
                var userFriend = cacheSet.FindKey(ContextUser.UserId, _friendID);
                var userFriend1 = cacheSet.FindKey(_friendID, ContextUser.UserId);

                if (userFriend == null)
                {
                    //创建新的数据 并且添加成关注类型
                    var friends = new UserFriends
                    {
                        UserID = ContextUser.UserId.ToString(),
                        FriendID = _friendID,
                        FriendType = FriendType.Attention
                    };
                    cacheSet.Add(friends);
                    //todo test
                    friends.ChatTime = DateTime.Now;

                }
                //如果玩家数据不为空
                else
                {
                    //判断两个玩家的关系
                    if (userFriend.FriendType == FriendType.Friend)
                    {
                        ErrorCode = LanguageManager.GetLang().ErrorCode;
                        ErrorInfo = LanguageManager.GetLang().St9103_TheUserHasAFriendIn;
                        return false;
                    }

                    ////如果已经发送请求就不在继续发
                    //if (userFriend.FriendType == FriendType.Attention)
                    //{
                    //    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    //    ErrorInfo = LanguageManager.GetLang().St_FirendNoticeTip;
                    //    return false;
                    //}

                    //加好友都是变为关注 从仇敌那里也能转换为关注
                    if (userFriend.FriendType != FriendType.Friend)
                    {
                        userFriend.FriendType = FriendType.Attention;
                    }

                }

                //判断对方是否有和本玩家的数据 如果没有创建 有改状态
                if (userFriend1 == null)
                {
                    var friends2 = new UserFriends
                    {
                        UserID = _friendID,
                        FriendID = ContextUser.UserId.ToString(),
                        FriendType = FriendType.Fans,
                    };
                    cacheSet.Add(friends2);
                    //todo test
                    friends2.ChatTime = DateTime.Now;
                }

                // 发送系统信件       
                try
                {
                    Guid newGuid = Guid.NewGuid();
                    UserMail userMail = new UserMail(newGuid);
                    userMail.UserId = Int32.Parse(_friendID);
                    userMail.MailType = (int)MailType.Friends;
                    userMail.Title = LanguageManager.GetLang().St_AskFirendMailTitle;
                    userMail.Content = string.Format(LanguageManager.GetLang().St_AskFirendTip, ContextUser.NickName);
                    userMail.SendDate = DateTime.Now;
                    userMail.IsReply = true;
                    userMail.ReplyStatus = 0;
                    userMail.FromUserId = ContextUser.UserId;//Int32.Parse(ContextUser.UserId);
                    userMail.FromUserName = ContextUser.NickName;
                    kpMailService mailService = new kpMailService(ContextUser);
                    mailService.Send(userMail);
                }
                catch (Exception)
                {

                }
            }
            //上传的好友名字不为空
            else if (_friendName != null)
            {
                List<UserFriends> friendArray = cacheSet.FindAll(m => m.UserID.ToString() == ContextUser.UserId.ToString());
                int friendNum = 100;//ConfigEnvSet.GetInt("UserFriends.MaxFriendNum");
                if (friendArray.Count >= friendNum)
                {
                    ErrorCode = LanguageManager.GetLang().ErrorCode;
                    ErrorInfo = LanguageManager.GetLang().St9103_TheMaximumReachedAFriend;
                    return false;
                }
                GameUser friend = null;
                new GameDataCacheSet<GameUser>().Foreach((personalId, key, user) =>
                {
                    if (user.NickName == _friendName)
                    {
                        friend = user;
                        return false;
                    }
                    return true;
                });
                if (friend != null)
                {
                    GameUser gameUser = new GameDataCacheSet<GameUser>().FindKey(friend.UserId.ToString());
                    UserFriends userFriend = cacheSet.FindKey(ContextUser.UserId, _friendID);
                    if (userFriend != null)
                    {
                        if (userFriend.FriendType == FriendType.Fans)
                        {
                            this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                            this.ErrorInfo = LanguageManager.GetLang().St9103_TheUserHasTheFansIn;
                            return false;
                        }
                        //else if (userFriend.FriendType == FriendType.Blacklist)
                        //{
                        //    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                        //    this.ErrorInfo = LanguageManager.GetLang().St9103_TheUserHasTheBlacklist;
                        //    return false;
                        //}
                        else
                        {
                            this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                            this.ErrorInfo = LanguageManager.GetLang().St9103_TheUserHasAFriendIn;
                            return false;
                        }
                    }
                    UserFriends friends = new UserFriends()
                    {
                        UserID = ContextUser.UserId.ToString(),
                        FriendID = gameUser.UserId.ToString(),
                        FriendType = FriendType.Attention
                    };
                    cacheSet.Add(friends);

                    UserFriends friends2 = new UserFriends()
                    {
                        UserID = gameUser.UserId.ToString(),
                        FriendID = ContextUser.UserId.ToString(),
                        FriendType = FriendType.Fans,
                    };
                    cacheSet.Add(friends2);
                }
                else
                {
                    this.ErrorCode = LanguageManager.GetLang().ErrorCode;
                    this.ErrorInfo = LanguageManager.GetLang().St9103_DoesNotExistTheUser;
                    return false;
                }
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
