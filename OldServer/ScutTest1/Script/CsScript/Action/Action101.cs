using System;
using System.Collections.Generic;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Game.Sns;

namespace GameServer.CsScript.Action
{

    /// <summary>
    /// 测试登陆
    /// </summary>
    /// <remarks>继续BaseStruct类:允许无身份认证的请求;AuthorizeAction:需要身份认证的请求</remarks>
    public class Action101 : BaseStruct
    {

        #region class object
        #endregion

        /// <summary>
        /// 登陆密码
        /// </summary>
        private string _password;
        /// <summary>
        /// 登陆账号
        /// </summary>
        private string _account;
        /// <summary>
        /// 登陆详情
        /// </summary>
        private int _relu;


        public Action101(ActionGetter actionGetter)
            : base((short)101, actionGetter)
        {

        }

        

        /// <summary>
        /// 客户端请求的参数较验
        /// </summary>
        /// <returns>false:中断后面的方式执行并返回Error</returns>
        public override bool GetUrlElement()
        {
            if (httpGet.GetString("Password", ref _password)
                && httpGet.GetString("Account", ref _account))
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
             //Console.WriteLine(SnsManager.RegisterPassport(_password));
             //_relu = SnsManager.QuickRegisterPassport(_account, _password);
            //_relu = 1;
            //Console.WriteLine(SnsManager.CheckPassport(_account));
            //SnsManager.RegisterPassportTest(_account, _password);
            if (SnsManager.Register(_account, _password))
            {
                _relu = 0;
                Console.WriteLine("注册成功 账号：{0}  密码：{1}", _account, _password);
            }
            else
            {
                Console.WriteLine("注册失败");
                _relu = 1;
            }
            
            return true;
        }

        /// <summary>
        /// 下发给客户的包结构数据
        /// </summary>
        public override void BuildPacket()
        {
            this.PushIntoStack(_relu);

        }

    }
}
