using GameServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Game.Cache;
using ZyGames.Framework.Game.Message;

namespace ZyGames.Tianjiexing.Component.Chat
{
    public class kpMailService : MailService<UserMail>
    {
        //static Main()
        //{
        //    GameUser gameUser;
        //    var ser = new TjxMailService(gameUser);
        //    ser.Send(t);
        //    ser.GetMail();
        //}
        private GameUser _gameUser;
        public kpMailService(GameUser gameUser)
        {
            _gameUser = gameUser;
        }

        public override List<UserMail> GetMail(out int unReadCount)
        {
            var mailList = MailCacheSet.FindAll(_gameUser.PersonalId, true);
            unReadCount = mailList.FindAll(x => !x.IsRead).Count;
            return mailList;
        }

        protected override bool IsUnRead(UserMail message)
        {
            return !message.IsRead;
        }

        protected override void SetRead(UserMail t)
        {
            t.IsRead = true;
        }
    }
}