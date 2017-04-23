using GameServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Common;
using ZyGames.Framework.Net;

namespace GameServer.CsScript.Action
{

    /// <summary>
    /// 可添加好友列表
    /// </summary>
    /// <remarks>继续BaseStruct类:允许无身份认证的请求;AuthorizeAction:需要身份认证的请求</remarks>
    public class Action9102 : AuthorizeAction
    {
        ///// <summary>
        ///// 
        ///// </summary>
        //private int _pageIndex;
        ///// <summary>
        ///// 
        ///// </summary>
        //private int _pageSize;
        ///// <summary>
        ///// 
        ///// </summary>
        //private int _pageCount;
        /// <summary>
        /// 
        /// </summary>
        //private List<UserProInfo> gameUserList = new List<UserProInfo>();
        List<GameUser> friendArray;

        public Action9102(ActionGetter actionGetter)
            : base((short)9102, actionGetter)
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
            //if (httpGet.GetInt("PageIndex", ref _pageIndex)
            //    && httpGet.GetInt("PageSize", ref _pageSize))
            //{
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
            friendArray = new List<GameUser>();

            var cacheSet = new GameDataCacheSet<GameUser>();
           
            GameUser _user = cacheSet.FindKey(Current.UserId.ToString());

            cacheSet.Foreach((personId, key, user) =>
            {
                //查找相同等级的 并且好友类型非好友和关注的类型 
                if (user.Lv == _user.Lv&& user.UserId != UserId)
                {
                    UserFriends userInfo = new ShareCacheStruct<UserFriends>().FindKey(UserId, user.UserId);
                    if (userInfo == null)
                    {
                        friendArray.Add(user);
                    }
                    else
                    {
                        if (userInfo.FriendType != FriendType.Friend)
                        {
                            friendArray.Add(user);
                        }
                    }

                }
                return true;
            });
            Console.WriteLine("推荐好友个数：" + friendArray.Count);
            return true;
        }

        /// <summary>
        /// 下发给客户的包结构数据
        /// </summary>
        public override void BuildPacket()
        {
            //this.PushIntoStack(_pageCount);
            this.PushIntoStack(friendArray.Count);
            foreach (var item in friendArray)
            {
                DataStruct dsItem = new DataStruct();
                dsItem.PushIntoStack(item.UserId.ToString());
                dsItem.PushIntoStack(item.NickName);
                dsItem.PushIntoStack(item.Sex);
                dsItem.PushIntoStack(item.Lv);
                dsItem.PushIntoStack(item.LoginTime);
                this.PushIntoStack(dsItem);
            }

        }

    }
}

