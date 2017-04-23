using System;
using System.Collections.Generic;
using ZyGames.Framework.Game.Service;


namespace GameServer.CsScript.Action
{

    /// <summary>
    /// helloworld
    /// </summary>
    /// <remarks>继续BaseStruct类:允许无身份认证的请求;AuthorizeAction:需要身份认证的请求</remarks>
    public class Action100 : BaseStruct
    {

        #region class object
        #endregion

        /// <summary>
        /// 
        /// </summary>
        private string _str;
        /// <summary>
        /// 
        /// </summary>
        private string _content;


        public Action100(ActionGetter actionGetter)
            : base((short)100, actionGetter)
        {

        }


        /// <summary>
        /// 客户端请求的参数较验
        /// </summary>
        /// <returns>false:中断后面的方式执行并返回Error</returns>
        public override bool GetUrlElement()
        {
            Console.WriteLine(httpGet.GetString("str"));
            if (httpGet.GetString("str", ref _str))
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
            _content = _str;
            return true;
        }

        /// <summary>
        /// 下发给客户的包结构数据
        /// </summary>
        public override void BuildPacket()
        {
            this.PushIntoStack(_content);
        }

    }
}
