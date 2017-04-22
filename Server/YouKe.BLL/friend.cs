using System;
using System.Data;
using System.Collections.Generic;


namespace YouKe.BLL
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public partial class friend
    {
        private readonly DAL.friend dal;
        public friend()
        {
            dal = new DAL.friend("mb_");
        }

        #region 基本方法===================================
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int uid, int friend_id)
        {
            return dal.Exists(uid, friend_id);
        }

        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(Model.friend model)
        {
            return dal.Add(model);
        }

        #endregion
    }
}
