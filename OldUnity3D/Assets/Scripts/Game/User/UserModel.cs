using UnityEngine;
using System.Collections;

public class UserModel : BaseModel
{
    /// <summary> 玩家昵称 </summary>
    public string name
    {
        set;
        get;
    }
    /// <summary> 玩家性别 </summary>
    public int sex
    {
        get;
        set;
    }

    #region 金币

    private int mGold;
    /// <summary> 玩家金币 </summary>
    public int gold
    {
        get
        {
            return mGold;
        }
        set
        { 
            if(mGold == value)
            {
                return;
            }
            if(value <= 0)
            {
                mGold = 0;
            }

            ValueUpdateEventArgs ve = new ValueUpdateEventArgs();
            ve.key = "gold";
            ve.oldValue = mGold;
            ve.newValue = value;
            mGold = value;
            DispatchValueUpdateEvent(ve);
        }
    }
    #endregion

    #region 元宝
    private int mIngot;
    /// <summary> 元宝 </summary>
    public int ingot
    {
        get
        {
            return mIngot;
        }
        set
        {
            if (mIngot == value)
            {
                return;
            }
            if (value <= 0)
            {
                mIngot = 0;
            }

            ValueUpdateEventArgs ve = new ValueUpdateEventArgs();
            ve.key = "ingot";
            ve.oldValue = mIngot;
            ve.newValue = value;
            mIngot = value;
            DispatchValueUpdateEvent(ve);
        }
    }
    #endregion

    #region 经验
    private int mExp;
    /// <summary> 经验 </summary>
    public int exp
    {
        get
        {
            return mExp;
        }
        set
        {
            if (mExp == value)
            {
                return;
            }
            if (value <= 0)
            {
                mExp = 0;
            }

            ValueUpdateEventArgs ve = new ValueUpdateEventArgs();
            ve.key = "exp";
            ve.oldValue = mExp;
            ve.newValue = value;
            mExp = value;
            DispatchValueUpdateEvent(ve);
        }
    }
    #endregion

    #region 等级
    private int mLv;
    /// <summary> 等级 </summary>
    public int lv
    {
        get
        {
            return mLv;
        }
        set
        {
            if (mLv == value)
            {
                return;
            }
            if (value <= 0)
            {
                mLv = 0;
            }

            ValueUpdateEventArgs ve = new ValueUpdateEventArgs();
            ve.key = "lv";
            ve.oldValue = mLv;
            ve.newValue = value;
            mLv = value;
            DispatchValueUpdateEvent(ve);
        }
    }
    #endregion

    #region 体力
    private int mAction;
    /// <summary> 等级 </summary>
    public int action
    {
        get
        {
            return mAction;
        }
        set
        {
            if (mAction == value)
            {
                return;
            }
            if (value <= 0)
            {
                mLv = 0;
            }

            ValueUpdateEventArgs ve = new ValueUpdateEventArgs();
            ve.key = "action";
            ve.oldValue = mAction;
            ve.newValue = value;
            mAction = value;
            DispatchValueUpdateEvent(ve);
        }
    }
    #endregion
}
