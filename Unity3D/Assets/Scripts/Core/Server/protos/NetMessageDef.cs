/*
 * 脚本名(ScriptName)：    NetMessageDef.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;

public class NetMessageDef
{
    /// <summary>
    /// 创建账号请求
    /// </summary>
    public const uint ReqCreateAccount = 101;
    /// <summary>
    /// 登陆请求
    /// </summary>
    public const uint ReqLogin = 102;

    /// <summary>
    /// 创建角色请求
    /// </summary>
    public const uint ReqCreateRole = 1001;

    /// <summary>
    /// 获取角色基本信息
    /// </summary>
    public const uint ReqGetRole = 1002;

    /// <summary>
    /// 获取角色基本信息请求
    /// </summary>
    public const uint ResGetRole = 1003;

    /// <summary>
    /// 响应结果(结果 or 细节)
    /// </summary>
    public const uint ResReturnDefaultInfo = 6001;
}

public class CombatDef
{
    /// <summary>
    /// 攻击结束
    /// </summary>
    public const uint AttackOver = 10001;
}
