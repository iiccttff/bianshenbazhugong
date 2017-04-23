using GameServer.Model;
using System;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Contract.Action;
using ZyGames.Framework.Game.Service;

namespace GameServer.CsScript.Action
{
    public abstract class BaseAction : AuthorizeAction
    {
        protected BaseAction(short actionID, ZyGames.Framework.Game.Contract.HttpGet httpGet)
            : base(actionID, httpGet)
        {
        }

        public string Uid
        {
            get { return Current.UserId.ToString(); }
        }
        /// <summary>
        /// 上下文玩家
        /// </summary>
        public GameUser ContextUser
        {
            get
            {
                return Current.User as GameUser;
                //return PersonalCacheStruct.Get<GameUser>(Current.UserId.ToString());
            }
        }



        ///// <summary>
        ///// 格式化日期显示，昨天，前天
        ///// </summary>
        ///// <param name="sendDate"></param>
        ///// <returns></returns>
        //protected string FormatDate(DateTime sendDate)
        //{
        //    string result = sendDate.ToString("HH:mm:ss");
        //    if (sendDate.Date == DateTime.Now.Date)
        //    {
        //        return result;
        //    }
        //    if (DateTime.Now > sendDate)
        //    {
        //        TimeSpan timeSpan = DateTime.Now.Date - sendDate.Date;
        //        int day = (int)Math.Floor(timeSpan.TotalDays);
        //        if (day == 1)
        //        {
        //            return string.Format("{0} {1}", LanguageManager.GetLang().Date_Yesterday, result);
        //        }
        //        if (day == 2)
        //        {
        //            return string.Format("{0} {1}", LanguageManager.GetLang().Date_BeforeYesterday, result);
        //        }
        //        return string.Format("{0} {1}", string.Format(LanguageManager.GetLang().Date_Day, day), result);
        //    }
        //    return result;
        //}

    }

}