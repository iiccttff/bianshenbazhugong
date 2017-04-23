using System;

public enum ActionType
{
    RankAdd = 100,
    RankSelect = 1001,
    /// <summary> 测试注册 </summary>
    Reg = 101,
    /// <summary> 注册 </summary>
    Regis = 1002,
    /// <summary> 登陆 </summary>
    Login = 1004,
    /// <summary> 创建角色 </summary>
    CreateRote = 1005,
    /// <summary> 好友列表 </summary>
    FriendList = 9101,
    /// <summary> 推荐好友列表 </summary>
    IntroduceFriend = 9102,
    /// <summary> 添加好友接口 </summary>
    AddFriend = 9103,
    /// <summary> 删除好友接口 </summary>
    RemoveFriend = 9104,

}
