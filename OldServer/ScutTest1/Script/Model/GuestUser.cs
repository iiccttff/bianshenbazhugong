/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

using System;
using ProtoBuf;
using ZyGames.Framework.Game.Context;
using ZyGames.Framework.Model;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Data;
using System.Data;

namespace GameServer.Model
{
    [Serializable, ProtoContract, EntityTable(DbConfig.Data, "GameUser", DbConfig.PeriodTime, DbConfig.PersonalName)]
    public class GameUser : BaseUser
    {
        public const string Index_UserID = "Index_UserID";

        [ProtoMember(1)]
        [EntityField(true, IsKey = true)]
        public int UserId { get; set; }

        [ProtoMember(2)]
        [EntityField]
        public String NickName
        {
            get;
            set;
        }

        [ProtoMember(3)]
        [EntityField]
        public String PassportId
        {
            get;
            set;
        }

        [ProtoMember(4)]
        [EntityField]
        public String RetailID
        {
            get;
            set;
        }

        /// <summary> 性别 </summary>
        [ProtoMember(5)]
        [EntityField]
        public int Sex { get; set; }

        /// <summary> 体力 </summary>
        [ProtoMember(6)]
        [EntityField]
        public int Action { get; set; }

        /// <summary> 经验 </summary>
        [ProtoMember(7)]
        [EntityField]
        public int Exp { get; set; }

        /// <summary> 等级 </summary>
        [ProtoMember(8)]
        [EntityField]
        public int Lv { get; set; }

        /// <summary> 金币 </summary>
        [ProtoMember(9)]
        [EntityField]
        public int Gold { get; set; }

        /// <summary> 元宝 </summary>
        [ProtoMember(10)]
        [EntityField]
        public int Ingot { get; set; }


        private DateTime _LoginTime = DateTime.Now;
        /// <summary>
        /// 登录时间
        /// </summary>
        [ProtoMember(11)]
        [EntityField]
        public DateTime LoginTime
        {
            get
            {
                return _LoginTime;
            }
            set
            {
                _LoginTime = value;
                SetChange("LoginTime", value);
            }
        }

        /// <summary>
        /// 服务器ID
        /// </summary>
        [ProtoMember(12)]
        public int ServerId { get; set; }

        private string _sessionID;


        /// <summary>
        /// 会话id
        /// </summary>
        //[ProtoMember(45)]
        public string SessionID
        {
            get { return _sessionID; }
            set
            {
                _sessionID = value;
            }
        }

        /// <summary>
        /// 是否在线
        /// </summary>
        [ProtoMember(50)]
        public bool IsOnline
        {
            get;
            set;
        }
        /// <summary>
        /// 游戏id
        /// </summary>
        [ProtoMember(46)]
        public int GameId { get; set; }


        public string SId { get; set; }

        protected override int GetIdentityId()
        {
            return UserId;
        }
        
        public override int GetUserId()
        {
            return UserId;
        }

        public override string GetNickName()
        {
            return NickName;
        }

        public override string GetPassportId()
        {
            return PassportId;
        }

        public override string GetRetailId()
        {
            return RetailID;
        }

        public  int GetSex()
        {
            return Sex;
        }

        public  int GetAction()
        {
            return Action;
        }

        public int GetExp()
        {
            return Exp;
        }

        public override bool IsLock
        {
            get { return false; }
        }

        /// <summary>
        /// 昵称是否已使用
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsNickName(string name)
        {
            //return new GameDataCacheSet<GameUser>().IsExist(u => u.NickName.ToLower() == name.ToLower().Trim());
            bool bl = false;
            bl = new GameDataCacheSet<GameUser>().IsExist(u => u.NickName.ToLower() == name.ToLower().Trim());
            if (!bl)
            {
                var dbProvider = DbConnectionProvider.CreateDbProvider(DbConfig.Data);

                var command = dbProvider.CreateCommandStruct("GameUser", CommandMode.Inquiry, "NickName");
                command.Filter = dbProvider.CreateCommandFilter();
                command.Filter.Condition = command.Filter.FormatExpression("NickName");
                command.Filter.AddParam("NickName", name);
                command.Parser();
                using (var reader = dbProvider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
                {
                    while (reader.Read())
                    {
                        bl = true;
                    }
                }
            }

            return bl;
        }

        public override DateTime OnlineDate
        {
            get;
            set;
        }
    }

}