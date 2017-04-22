using System;
namespace YouKe.Model
{
    /// <summary>
    /// 会员主表
    /// </summary>
    [Serializable]
    public partial class users
    {
        public users()
        { }
        #region Model
        private int _id;
        private string _user_account;
        private string _user_passworld;
        private DateTime _reg_time;
        private DateTime _login_time;
        private string _user_name;
        private int _endurance;
        private int _gold;
        private int _wing;
        private int _lv;
        private int _vip;
        private string _head;
        private int _exp;

        /// <summary>
        /// 自增ID
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string user_account
        {
            set { _user_account = value; }
            get { return _user_account; }
        }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string user_passworld
        {
            set { _user_passworld = value; }
            get { return _user_passworld; }
        }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime reg_time
        {
            set { _reg_time = value; }
            get { return _reg_time; }
        }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime login_time
        {
            set { _login_time = value; }
            get { return _login_time; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string user_name
        {
            set { _user_name = value; }
            get { return _user_name; }
        }
        /// <summary>
        /// 体力
        /// </summary>
        public int endurance
        {
            set { _endurance = value; }
            get { return _endurance; }
        }
        /// <summary>
        /// 金币
        /// </summary>
        public int gold
        {
            set { _gold = value; }
            get { return _gold; }
        }
        /// <summary>
        /// 元宝
        /// </summary>
        public int wing
        {
            set { _wing = value; }
            get { return _wing; }
        }
        /// <summary>
        /// 等级
        /// </summary>
        public int lv
        {
            set { _lv = value; }
            get { return _lv; }
        }
        /// <summary>
        /// VIP等级
        /// </summary>
        public int vip
        {
            set { _vip = value; }
            get { return _vip; }
        }
        /// <summary>
        /// VIP等级
        /// </summary>
        public string head
        {
            set { _head = value; }
            get { return _head; }
        }
        /// <summary>
        /// 经验值
        /// </summary>
        public int exp
        {
            set { _exp = value; }
            get { return _exp; }
        }
        #endregion

    }
}