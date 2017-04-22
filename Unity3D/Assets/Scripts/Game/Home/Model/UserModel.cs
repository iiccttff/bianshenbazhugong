/*
 * 脚本名(ScriptName)：    UserModel.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;

public class UserModel : BaseModel
{
    #region 基础数据，只允许设置一次

    /// <summary>
    /// 玩家id
    /// </summary>
    public int userId
    {
        set;
        get;
    }

    /// <summary>
    /// 玩家信息
    /// </summary>
    public string userName
    {
        set;
        get;
    }

    #endregion

    #region 体力
    private int _endurance = 0;
    /// <summary>
    /// 体力
    /// </summary>
    public int endurance
    {
        get
        {
            return _endurance;
        }
        set
        {
            if(_endurance == value)
            {
                return;
            }
            if(_endurance < 0)
            {
                _endurance = 0;
            }
            _endurance = value;
        }
    }
    #endregion

    #region 金币
    private int _gold = 0;
    /// <summary>
    /// 金币
    /// </summary>
    public int gold
    {
        get
        {
            return _gold;
        }
        set
        {
            if (_gold == value)
            {
                return;
            }
            if (_gold < 0)
            {
                _gold = 0;
            }
            //ValueUpdateEvenArgs ve = new ValueUpdateEvenArgs();
            //ve.key = "gold";
            //ve.oldValue = _gold;
            //ve.newValue = value;
            DispatchValueUpdateEvent("gold", _gold, value);
            _gold = value;
        }
    }
    #endregion

    #region 元宝
    private int _wing = 0;
    /// <summary>
    /// 元宝
    /// </summary>
    public int wing
    {
        get
        {
            return _wing;
        }
        set
        {
            if (_wing == value)
            {
                return;
            }
            if (_wing < 0)
            {
                _wing = 0;
            }
            _wing = value;
        }
    }
    #endregion

    #region 等级
    private int _lv = 0;
    /// <summary>
    /// 等级
    /// </summary>
    public int lv
    {
        get
        {
            return _lv;
        }
        set
        {
            if (_lv == value)
            {
                return;
            }
            if (_lv < 0)
            {
                _lv = 0;
            }
            _lv = value;
        }
    }
    #endregion

    #region VIP等级
    private int _vip = 0;
    /// <summary>
    /// vip等级
    /// </summary>
    public int vip
    {
        get
        {
            return _vip;
        }
        set
        {
            if (_vip == value)
            {
                return;
            }
            if (_vip < 0)
            {
                _vip = 0;
            }
            _vip = value;
        }
    }
    #endregion

    #region 头像
    private int _head = 0;
    /// <summary>
    /// 头像ID
    /// </summary>
    public int head
    {
        get
        {
            return _head;
        }
        set
        {
            if (_head == value)
            {
                return;
            }
            if (_head < 0)
            {
                _head = 0;
            }
            _head = value;
        }
    }
    #endregion

}
