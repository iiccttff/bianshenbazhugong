/*
 * 脚本名(ScriptName)：    SceneBase.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneBase : UIBase
{
    //protected object[] _sceneArgs;
    ///// <summary>
    ///// 场景init参数
    ///// </summary>
    //public object[] sceneArgs
    //{
    //    get
    //    {
    //        return _sceneArgs;
    //    }
    //}

    /// <summary>
    /// 初始化场景
    /// </summary>
    /// <param name="sceneArgs"></param>
    public virtual void OnInit(params object[] sceneArgs)
    {
        _sceneArgs = sceneArgs;
        Init();
    }

    public virtual void OnShowing()
    {

    }

    
    /// <summary>
    /// 重置数据
    /// </summary>
    /// <param name="sceneArgs"></param>
    public virtual void OnResetArgs(params object[] sceneArgs)
    {
        _sceneArgs = sceneArgs;
    }

}

public enum SceneType
{
    /// <summary> 登陆界面 </summary>
    SceneLogin,
    /// <summary> 主界面 </summary>
    SceneHome,
    /// <summary> 加载 </summary>
    SceneLoding,
    /// <summary> 邮件 </summary>
    SceneMail,
    /// <summary> 聊天 </summary>
    SceneChat,
    /// <summary> 商城 </summary>
    SceneShop,
}
