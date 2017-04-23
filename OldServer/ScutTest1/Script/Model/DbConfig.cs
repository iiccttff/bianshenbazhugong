using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    public class DbConfig
    {
        public const string Config = "KaPaiConfig";
        public const string Data = "KaPaiData";
        public const string Log = "KaPaiLog";
        public const int GlobalPeriodTime = 0;
        /// <summary>
        /// 过期时间
        /// </summary>
        public const int PeriodTime = 86400;
        /// <summary>
        /// 
        /// </summary>
        public const string PersonalName = "UId";

        //public static string ConfigConnectString
        //{
        //    get
        //    {
        //        return ConfigurationManager.ConnectionStrings[Config].ConnectionString;
        //    }
        //}

        //public static string DataConnectString
        //{
        //    get
        //    {
        //        return ConfigurationManager.ConnectionStrings[Data].ConnectionString;
        //    }
        //}

        //public static string LogConnectString
        //{
        //    get
        //    {
        //        return ConfigurationManager.ConnectionStrings[Log].ConnectionString;
        //    }
        //}
    }
}
