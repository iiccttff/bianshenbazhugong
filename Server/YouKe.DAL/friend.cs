using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using YouKe.DBUtility;
using YouKe.Common;

namespace YouKe.DAL
{
    /// <summary>
    /// 数据访问类:用户
    /// </summary>
    public partial class friend
    {
        private string databaseprefix; //数据库表名前缀
        public friend(string _databaseprefix)
        {
            databaseprefix = _databaseprefix;
        }

        #region 基本方法================================
        /// <summary>
        /// 是否已经是好友了
        /// </summary>
        public bool Exists(int uid,int friend_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from " + databaseprefix + "friend");
            strSql.Append(" where uid=@uid and friend_id=@friend_id");
            SqlParameter[] parameters = {
					new SqlParameter("@uid", SqlDbType.Int,4),
                    new SqlParameter("@friend_id", SqlDbType.Int,4)};
            parameters[0].Value = uid;
            parameters[1].Value = friend_id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.friend model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into " + databaseprefix + "friend(");
            strSql.Append("uid,friend_id)");
            strSql.Append(" values (");
            strSql.Append("@uid,@friend_id)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@uid", SqlDbType.Int,4),
                    new SqlParameter("@friend_id", SqlDbType.Int,4)};
            parameters[0].Value = model.uid;
            parameters[1].Value = model.friden_id;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
      
        #endregion


        #region 扩展方法================================

        /// <summary>
        /// 修改一列数据
        /// </summary>
        public int UpdateField(int id, string strValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update " + databaseprefix + "users set " + strValue);
            strSql.Append(" where id=" + id);
            return DbHelperSQL.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.users DataRowToModel(DataRow row)
        {
            Model.users model = new Model.users();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["user_account"] != null)
                {
                    model.user_account = row["user_account"].ToString();
                }
                if (row["user_passworld"] != null)
                {
                    model.user_passworld = row["user_passworld"].ToString();
                }
                if (row["reg_time"] != null && row["reg_time"].ToString() != "")
                {
                    model.reg_time = DateTime.Parse(row["reg_time"].ToString());
                }
                if (row["login_time"] != null && row["login_time"].ToString() != "")
                {
                    model.login_time = DateTime.Parse(row["login_time"].ToString());
                }
                if (row["user_name"] != null)
                {
                    model.user_name = row["user_name"].ToString();
                }
                if (row["endurance"] != null && row["endurance"].ToString() != "")
                {
                    model.endurance = int.Parse(row["endurance"].ToString());
                }
                if (row["gold"] != null && row["gold"].ToString() != "")
                {
                    model.gold = int.Parse(row["gold"].ToString());
                }
                if (row["wing"] != null && row["wing"].ToString() != "")
                {
                    model.wing = int.Parse(row["wing"].ToString());
                }
                if (row["lv"] != null && row["lv"].ToString() != "")
                {
                    model.lv = int.Parse(row["lv"].ToString());
                }
                if (row["vip"] != null && row["vip"].ToString() != "")
                {
                    model.vip = int.Parse(row["vip"].ToString());
                }
                if (row["head"] != null)
                {
                    model.head = row["head"].ToString();
                }
                if (row["exp"] != null && row["exp"].ToString() != "")
                {
                    model.exp = int.Parse(row["exp"].ToString());
                }

            }
            return model;
        }
        #endregion
    }
}
