using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    public enum FriendType
    {
        /// <summary>
        /// 等待验证好友
        /// </summary>
        Validation = 0,
        /// <summary>
        /// 好友
        /// </summary>
        Friend = 1,
        /// <summary>
        /// 关注 ，等待验证
        /// </summary>
        Attention = 2,
        /// <summary>
        /// 粉丝
        /// </summary>
        Fans = 3,
        
    }
}
