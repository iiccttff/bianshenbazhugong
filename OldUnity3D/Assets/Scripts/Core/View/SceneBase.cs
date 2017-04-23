using UnityEngine;
using System.Collections;

public class SceneBase : LayerBase
{

    protected bool _cache = false;
    /// <summary>
    /// 缓存标识
    /// 如为false,则在关闭时destroy。
    /// </summary>
    public bool cache
    {
        get
        {
            return _cache;
        }
    }

    protected SceneType _type;
    /// <summary>
    /// 场景ID
    /// </summary>
    public SceneType type
    {
        get
        {
            return _type;
        }
    }
    protected object[] _sceneArgs;
    /// <summary>
    /// 记录场景init时参数
    /// </summary>
    public object[] sceneArgs
    {
        get
        {
            return _sceneArgs;
        }
    }

    /// <summary>
    /// 初始化场景
    /// </summary>
    /// <param name="sceneArgs">场景参数</param>
    public virtual void OnInit(params object[] sceneArgs)
    {
        _sceneArgs = sceneArgs;
        Init();
    }

    /// <summary>
    /// 开始显示
    /// </summary>
    public virtual void OnShowing()
    {

    }
    /// <summary>
    /// 重值数据
    /// </summary>
    /// <param name="sceneArgs"></param>
    public virtual void OnResetArgs(params object[] sceneArgs)
    {
        _sceneArgs = sceneArgs;
    }
    /// <summary>
    /// 显示场景后
    /// </summary>
    public virtual void OnShowed()
    {

    }
    /// <summary>
    /// 开始隐藏
    /// </summary>
    public virtual void OnHiding()
    {

    }
    /// <summary>
    /// 隐藏后
    /// </summary>
    public virtual void OnHided()
    {

    }
}

/// <summary>
/// 场景定义
/// 有一个特殊要求，SceneType.tostring等于该场景的classname。原因是：在切换场景时，参数为scenetype,此时需要根据string反射得到该className对应的对象。
/// </summary>
public enum SceneType
{
    /// <summary> 测试场景 </summary>
    SceneTest,
    SceneTest2,
    /// <summary> 登录 </summary>
    SceneLogin,
    /// <summary> 加载 </summary>
    SceneLoading,
    /// <summary> 创建角色 </summary>
    SceneCreateRote,
    /// <summary> 主界面 </summary>
    SceneMain,
    /// <summary> 好友 </summary>
    SceneFriends,
}