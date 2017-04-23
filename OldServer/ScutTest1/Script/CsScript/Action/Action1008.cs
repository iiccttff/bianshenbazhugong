using GameServer.Model;
using System;
using System.Collections.Generic;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Game.Service;

namespace GameServer.CsScript.Action
{

    /// <summary>
    /// 主界面数据推送
    /// </summary>
    /// <remarks>继续BaseStruct类:允许无身份认证的请求;AuthorizeAction:需要身份认证的请求</remarks>
    public class Action1008 : AuthorizeAction
    {

        #region class object
        #endregion

        /// <summary>
        /// 玩家昵称
        /// </summary>
        private string _nickName;
        /// <summary>
        /// 用户id
        /// </summary>
        private int _userId;
        /// <summary>
        /// 通信证id
        /// </summary>
        private string _passportId;
        /// <summary>
        /// 性别
        /// </summary>
        private int _sex;
        /// <summary>
        /// 体力
        /// </summary>
        private int _action;
        /// <summary>
        /// 经验值
        /// </summary>
        private int _exp;
        /// <summary>
        /// 等级
        /// </summary>
        private int _lv;
        /// <summary>
        /// 金币
        /// </summary>
        private int _gold;
        /// <summary>
        /// 元宝
        /// </summary>
        private int _ingot;

        private GameUser _user;
        public Action1008(ActionGetter actionGetter)
            : base((short)1008, actionGetter)
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
            return true;
        }

        /// <summary>
        /// 业务逻辑处理
        /// </summary>
        /// <returns>false:中断后面的方式执行并返回Error</returns>
        public override bool TakeAction()
        {
            var cacheSet = new GameDataCacheSet<GameUser>();
            _user = cacheSet.FindKey(Current.UserId.ToString());
            Console.WriteLine(Current.UserId);
            if (_user == null) return false;
            
            //GameUser _user = Current.User as GameUser;
            //if (_user == null)
            //{
            //    Console.WriteLine("222222");
            //    return false;
            //}

            return true;
        }

        /// <summary>
        /// 下发给客户的包结构数据
        /// </summary>
        public override void BuildPacket()
        {
            Console.WriteLine("name" + _user.NickName);
            this.PushIntoStack(_user.NickName);
            this.PushIntoStack(_user.UserId);
            this.PushIntoStack(_user.PassportId);
            this.PushIntoStack(_user.Sex);
            this.PushIntoStack(_user.Action);
            this.PushIntoStack(_user.Exp);
            this.PushIntoStack(_user.Lv);
            this.PushIntoStack(_user.Gold);
            this.PushIntoStack(_user.Ingot);

        }

    }
}
