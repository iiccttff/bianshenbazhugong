/*
 * 脚本名(ScriptName)：    BaseModel.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;
using System;

public class BaseModel  
{
    /// <summary>
    /// 事件触发
    /// </summary>
    /// <param name="key"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public void DispatchValueUpdateEvent(string key,object oldValue,object newValue)
    {
        EventHandler<ValueUpdateEvenArgs> handler = ValueUpdateEvent;
        if(handler != null)
        {
            handler(this, new ValueUpdateEvenArgs(key, oldValue, newValue));
        }
    }
    /// <summary>
    /// 事件触发
    /// </summary>
    /// <param name="args"></param>
    public void DispatchValueUpdateEvent(ValueUpdateEvenArgs args)
    {
        EventHandler<ValueUpdateEvenArgs> handler = ValueUpdateEvent;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    public event EventHandler<ValueUpdateEvenArgs> ValueUpdateEvent;
}
public class ValueUpdateEvenArgs : EventArgs
{
    public string key { set; get; }
    public object oldValue { set; get; }
    public object newValue { set; get; }

    public ValueUpdateEvenArgs(string key,object oldValue,object newValue)
    {
        this.key = key;
        this.oldValue = oldValue;
        this.newValue = newValue;
    }
    public ValueUpdateEvenArgs() { }
}
