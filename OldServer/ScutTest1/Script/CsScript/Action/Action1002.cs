using System;
using System.Collections.Generic;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Game.Service;
using ZyGames.Framework.Game.Sns;
using ZyGames.Framework.Game.Lang;
using ZyGames.Framework.Game.Runtime;

namespace GameServer.CsScript.Action
{

    /// <summary>
    /// 获取通行证接口
    /// </summary>
    /// <remarks>继续BaseStruct类:允许无身份认证的请求;AuthorizeAction:需要身份认证的请求</remarks>
    public class Action1002 : BaseStruct
    {
        /// <summary>
        /// 用户手机类型
        /// </summary>
        private int _mobileType;
        /// <summary>
        /// 游戏类型
        /// </summary>
        private int _gameType;
        /// <summary>
        /// 渠道ID游戏推广渠道编号
        /// </summary>
        private int _retailID;
        /// <summary>
        /// 客户端版本号末填写则默认为1.0版本
        /// </summary>
        private string _clientAppVersion;
        /// <summary>
        /// 屏幕宽度（像素）
        /// </summary>
        private int _screenX;
        /// <summary>
        /// 屏幕高度（像素）
        /// </summary>
        private int _screenY;
        /// <summary>
        /// 设备ID
        /// </summary>
        private string _deviceID;
        /// <summary>
        /// 分服ID
        /// </summary>
        private int _serverID;
        /// <summary>
        /// 通行证ID
        /// </summary>
        private string _passportID;
        /// <summary>
        /// 密码
        /// </summary>
        private string _password;


        public Action1002(ActionGetter actionGetter)
            : base((short)1002, actionGetter)
        {

        }



        /// <summary>
        /// 客户端请求的参数较验
        /// </summary>
        /// <returns>false:中断后面的方式执行并返回Error</returns>
        public override bool GetUrlElement()
        {

            if (httpGet.GetInt("MobileType", ref _mobileType)
                && httpGet.GetInt("GameType", ref _gameType)
                && httpGet.GetInt("RetailID", ref _retailID)
                && httpGet.GetString("ClientAppVersion", ref _clientAppVersion)
                && httpGet.GetString("DeviceID", ref _deviceID)
                && httpGet.GetInt("ServerID", ref _serverID))
            {

                httpGet.GetInt("ScreenX", ref _screenX);
                httpGet.GetInt("ScreenY", ref _screenY);
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
            try
            {
                
                Console.WriteLine("_deviceID:" + _deviceID);
                string[] userList = SnsManager.GetRegPassport(_deviceID);
                _passportID = userList[0];
                _password = userList[1];
                return true;
            }
            catch (Exception ex)
            {
                this.SaveLog(ex);

                return false;
            }

        }

        /// <summary>
        /// 下发给客户的包结构数据
        /// </summary>
        public override void BuildPacket()
        {
            this.PushIntoStack(_passportID);
            this.PushIntoStack(_password);

        }

    }
}
