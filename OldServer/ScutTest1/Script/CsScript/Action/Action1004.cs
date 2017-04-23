using GameServer.Model;
using System;
using System.Collections.Generic;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Game.Service;

namespace GameServer.CsScript.Action
{
    /// <summary>
    /// 1004_用户登录
    /// </summary>
    public class Action1004 : LoginExtendAction
    {
        public Action1004(ActionGetter actionGetter)
            : base((short)1004, actionGetter)
        {
        }

        protected override bool DoSuccess(int userId, out IUser user)
        {
            user = null;
            Console.WriteLine("登录成功！");
            user = null;
            //原因：重登录时，数据会回档问题
            var cacheSet = new GameDataCacheSet<GameUser>();
            GameUser userInfo = cacheSet.FindKey(userId.ToString());
            if (userInfo == null)
            {
                //通知客户跳转到创建角色接口
                GuideId = 1005;
                return true;
            }
            else
            {
                user = new SessionUser(userInfo);
                userInfo.LoginTime = DateTime.Now;
                Console.WriteLine(DateTime.Now);
                userInfo.SessionID = Sid;
                userInfo.IsOnline = true;
                userInfo.ServerId = ServerID;
                userInfo.GameId = GameType;
            }
            return true;
            //如写登录日志
        }

        public override void TakeActionAffter(bool state)
        {
            Console.WriteLine("1004>发送World通知...");
            var notifyUsers = new List<IUser>();
            notifyUsers.Add(Current.User);
            ActionFactory.SendAsyncAction(notifyUsers, (int)1008, null, t =>
            {
                Console.WriteLine("1004>发送World通知结果:{0}", t.Result.ToString());
            });
            base.TakeActionAffter(state);
        }
    }
}
