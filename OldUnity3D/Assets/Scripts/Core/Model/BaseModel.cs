using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// 基础数据模型
/// 实现dispatcher
/// </summary>
public class BaseModel
{
    #region 属性事件
    /// <summary> 属性变更事件定义 </summary>
    public event EventHandler<ValueUpdateEventArgs> ValueUpdateEvent;

    /// <summary>
    /// 属性事件触发
    /// </summary>
    /// <param name="key">事件key</param>
    /// <param name="oldValue">旧的值</param>
    /// <param name="newValue">新值</param>
    protected void DispatchValueUpdateEvent(string key, object oldValue, object newValue)
    {
        EventHandler<ValueUpdateEventArgs> handler = ValueUpdateEvent;
        if (handler != null)
        {
            handler(this, new ValueUpdateEventArgs(key, oldValue, newValue));
        }
    }

    /// <summary> 属性事件触发 </summary>
    protected void DispatchValueUpdateEvent(ValueUpdateEventArgs args)
    {
        EventHandler<ValueUpdateEventArgs> handler = ValueUpdateEvent;
        if (handler != null)
        {
            handler(this, args);
        }
    }
    #endregion

    #region 模型事件

    /// <summary> 模型事件定义 </summary>
    public event EventHandler<ModelEventArgs> ModelEvent;
    /// <summary>  模型事件触发  </summary>
    protected void DispatchModelEvent(string type, params object[] args)
    {
        EventHandler<ModelEventArgs> handler = ModelEvent;
        if (handler != null)
        {
            handler(this, new ModelEventArgs(type, args));
        }
    }

    /// <summary> 模型事件触发 </summary>
    protected void DispatchModelEvent(ModelEventArgs args)
    {
        EventHandler<ModelEventArgs> handler = ModelEvent;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    #endregion

    /// <summary>  清理 </summary>
    virtual public void Destroy()
    {
        ModelEvent = null;
        ValueUpdateEvent = null;
    }
}



/// <summary>
/// 模型事件
/// </summary>
public class ModelEventArgs : EventArgs
{
    public string type { get; set; }
    public object[] args;

    public ModelEventArgs(String type, params object[] args)
    {
        this.type = type;
        this.args = args;
    }

    public ModelEventArgs()
    {
    }
}

/// <summary>
/// 数据更新事件
/// </summary>
public class ValueUpdateEventArgs : EventArgs
{
    public string key { get; set; }

    public object oldValue { get; set; }

    public object newValue { get; set; }

    public ValueUpdateEventArgs(String key, object oldValue, object newValue)
    {
        this.key = key;
        this.oldValue = oldValue;
        this.newValue = newValue;
    }

    public ValueUpdateEventArgs()
    {
    }
}