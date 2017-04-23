using System;
using System.Collections.Generic;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Game.Contract;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Model;
using GameServer.Model;

namespace GameServer.CsScript.Action
{

    /// <summary>
    /// 1005_创建角色接口
    /// </summary>
    public class Action1005 : RegisterAction
    {

        public Action1005(ActionGetter actionGetter)
            : base((short)1005, actionGetter)
        {

        }

        protected override bool GetActionParam()
        {
            return true;
        }


        protected override bool CreateUserRole(out IUser user)
        {
            user = null;
            if (UserName.Length < 2 || UserName.Length > 12)//如果名字过长或过短
            {
                ErrorCode = Language.Instance.ErrorCode;
                ErrorInfo = Language.Instance.St1005_UserNameNotEnough;
                return false;
            }

            if (GameUser.IsNickName(UserName))
            {
                ErrorCode = Language.Instance.ErrorCode;
                ErrorInfo = Language.Instance.St1005_UserNameNotEnough;
                return false;
            }

            var userCache = new PersonalCacheStruct<GameUser>();
            GameUser gameUser;
            
            if (userCache.TryFindKey(UserId.ToString(), out gameUser) == LoadingStatus.Success)
            {
                if (gameUser == null)
                {
                    gameUser = new GameUser
                    {
                        UserId = UserId,
                        PassportId = Pid,
                        NickName = UserName,
                        Sex = Sex,
                    };
                }
                gameUser.Exp = 0;
                gameUser.Lv = 1;
                gameUser.LoginTime = DateTime.Now;
                gameUser.Action = 100;
                gameUser.Gold = 5000;
                gameUser.Ingot = 100;
                userCache.Add(gameUser);
                return true;
            }
            return false;
        }

        public override void BuildPacket()
        {

        }

        public override void TakeActionAffter(bool state)
        {
            Console.WriteLine("1005>发送World通知...");
            var notifyUsers = new List<IUser>();
            notifyUsers.Add(Current.User);
            ActionFactory.SendAsyncAction(notifyUsers, (int)1008, null, t =>
            {
                Console.WriteLine("1005>发送World通知结果:{0}", t.Result.ToString());
            });
            base.TakeActionAffter(state);
        }
    }
}
