using GameServer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZyGames.Framework.Cache.Generic;
using ZyGames.Framework.Common.Configuration;
using ZyGames.Framework.Data;
using ZyGames.Framework.Net;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Common.Timing;
using ZyGames.Framework.Common;
using ZyGames.Framework.Game.Cache;

namespace GameServer.CsScript.Base
{
    public static class SystemGlobal
    {
        private const int LoadDay = 100;
        private static int maxCount = 1000;//ConfigUtils.GetSetting("MaxLoadCount", "100").ToInt();


        public static void Run()
        {
            TryRecoverFromDb(); //如果需要从服务器初始化数据
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            LoadGlobalData();
            LoadUser();
            stopwatch.Stop();
            Console.WriteLine("系统全局运行环境加载所需时间:" + stopwatch.Elapsed.TotalMilliseconds + "ms");
        }

        public static void TryRecoverFromDb()
        {
            int capacity = int.MaxValue;
            //todo Load
            var dbFilter = new DbDataFilter(capacity);
            //new GameUser().TryRecoverFromDb();
            new ShareCacheStruct<UserFriends>().TryRecoverFromDb(dbFilter);
            new GameDataCacheSet<GameUser>().TryRecoverFromDb(dbFilter);
           
        }

        /// <summary> 加载所有用户 </summary>
        private static void LoadUser()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var userList = GetLoadUser(LoadDay, maxCount);
            Console.WriteLine("系统加载所有用户数:{0}/最大{1}", userList.Count, maxCount);
            //new BaseLog().SaveLog("系统加载当天用户数:" + userList.Count + "/最大:" + maxCount);
            foreach (string userId in userList)
            {
                GameUser user = UserCacheGlobal.LoadOffline(userId);
                //if (user == null) continue;
                //Console.WriteLine(user.NickName +"---"+user.UserId);
            }
            stopwatch.Stop();
            Console.WriteLine("系统加载所有用户所需时间:{0}ms  共加载用户：{1}", stopwatch.Elapsed.TotalMilliseconds, userList.Count.ToString());
            //new BaseLog().SaveLog("系统加载当天用户所需时间:" + stopwatch.Elapsed.TotalMilliseconds + "ms");
        }
        /// <summary>
        /// 开始加载所有用户
        /// </summary>
        /// <param name="days"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public static List<string> GetLoadUser(int days, int maxCount)
        {
            var dbProvider = DbConnectionProvider.CreateDbProvider(DbConfig.Data);

            var command = dbProvider.CreateCommandStruct("GameUser", CommandMode.Inquiry, "UserID");
            command.OrderBy = "LoginTime desc";
            command.Filter = dbProvider.CreateCommandFilter();
            command.Filter.Condition = command.Filter.FormatExpression("LoginTime", ">");
            command.Filter.AddParam("LoginTime", DateTime.Now.AddDays(-days));
            command.Parser();

            List<string> userList = new List<string>();
            using (IDataReader reader = dbProvider.ExecuteReader(CommandType.Text, command.Sql, command.Parameters))
            {
                while (reader.Read())
                {
                    userList.Add(reader["UserID"].ToString());
                }
            }
            return userList;
        }

        /// <summary>
        /// 系统加载单服配置
        /// </summary>
        public static void LoadGlobalData()
        {
            Console.WriteLine("系统加载单服配置开始...");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int capacity = int.MaxValue;
            //todo Load
            var dbFilter = new DbDataFilter(capacity);

            new ShareCacheStruct<UserFriends>().AutoLoad(dbFilter);

            stopwatch.Stop();
            //AddTestFriends("1380026");
            //new ShareCacheStruct<UserFriends>().AddOrUpdate(user);
            List<UserFriends> friendsList = new ShareCacheStruct<UserFriends>().FindAll();

            Console.WriteLine("系统加载单服配置所需时间:{0}ms   加载好友数{1}", stopwatch.Elapsed.TotalMilliseconds, friendsList.Count);
        }



        public static void Stop()
        {
            //CountryCombat.Stop();
            //GameActiveCenter.Stop();
            //GuildGameActiveCenter.Stop();
        }

        public static void AddTestFriends(string id1)
        {

            var userList = GetLoadUser(LoadDay, maxCount);

            foreach(string id in userList)
            {
                if(id != id1)
                {
                    UserFriends user = new UserFriends();
                    user.UserID = id1;
                    user.FriendID = id;
                    user.FriendType = FriendType.Friend;
                    new ShareCacheStruct<UserFriends>().AddOrUpdate(user);
                }
            }

        }
    }
}
