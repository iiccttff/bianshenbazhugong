using System;
using ZyGames.Framework.Common;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Game.Cache;

namespace GameServer.Model
{
    public static class UserCacheGlobal
    {
        private const int LoadMaxCount = 10000;

        //private static int periodTime = 600;//秒

        public static void ReLoad(string userId)
        {
            DoLoad(userId, true, false);
        }

        public static GameUser LoadOffline(string userId)
        {
            var cacheSet = new GameDataCacheSet<GameUser>();
            DoLoad(userId, false, true);
            return cacheSet.FindKey(userId);
        }

        public static void Load(string userId)
        {
            DoLoad(userId, false, false);
        }

        public static GameUser CheckLoadUser(string userId)
        {
            var cacheSet = new GameDataCacheSet<GameUser>();
            GameUser gameUser = cacheSet.FindKey(userId);
            if (gameUser == null)
            {
                
            }
            return gameUser;
        }

        private static void DoLoad(string personalId, bool isReload, bool offline)
        {
            int userId = personalId.ToInt();
            if (string.IsNullOrEmpty(personalId))
            {
                TraceLog.WriteInfo("Load userid:\"{0}\" is null", userId);
                return;
            }
            lock (personalId)
            {
               

                var cacheSet = new GameDataCacheSet<GameUser>();
                GameUser gameUser = cacheSet.FindKey(personalId);
                string pid = gameUser != null ? gameUser.PassportId : string.Empty;

                if (gameUser == null)
                {
                    Console.WriteLine(personalId);
                    //新注册用户会为null
                    return;
                }
                if (offline && gameUser != null)
                {
                    //gameUser.IsOnline = false;
                    //gameUser.OnlineDate = DateTime.Now;
                }


                //gameUser.IsRefreshing = true;
                //gameUser.IsRefreshing = false;
            }
        }

        public static GameUser GetGameUser(string userID)
        {
            var cacheSet = new GameDataCacheSet<GameUser>();
            GameUser userInfo = cacheSet.FindKey(userID);
            if (userInfo == null)
            {
                UserCacheGlobal.Load(userID);//重新刷缓存
                userInfo = cacheSet.FindKey(userID);
            }
            return userInfo;
        }

    }
}
