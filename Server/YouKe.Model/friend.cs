using System;
namespace YouKe.Model
{
    /// <summary>
    /// 会员表
    /// </summary>
    [Serializable]
    public partial class friend
    {
        public friend()
        { }
        #region Model
        private int _uid;
        private int _friden_id;

        /// <summary>
        /// uID
        /// </summary>
        public int uid
        {
            set { _uid = value; }
            get { return _uid; }
        }

        /// <summary>
        /// 好友id
        /// </summary>
        public int friden_id
        {
            set { _friden_id = value; }
            get { return _friden_id; }
        }
        #endregion
    }
}
