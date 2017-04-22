/*
 * 脚本名(ScriptName)：    PlayerBase.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBase : MonoBehaviour 
{
    /// <summary> 角色阵营 </summary>
    public uint camp;
    /// <summary>
    /// 角色状态机
    /// </summary>
    protected StateMachine mPlayerStateMachine = new StateMachine();

    /// <summary>
    /// 角色状态机
    /// </summary>
    public StateMachine playerStateMachine
    {
        get
        {
            return mPlayerStateMachine;
        }
    }

    private Animator mPlayerAnimator;
    /// <summary>
    /// 动画控件
    /// </summary>
    public Animator palyerAnimator
    {
        get
        {
            if(mPlayerAnimator == null)
            {
                mPlayerAnimator = GetComponent<Animator>();
            }
            return mPlayerAnimator;
        }
    }


    protected List<PlayerBase> mOtherRole = null;
    /// <summary>
    /// 对方角色
    /// </summary>
    public List<PlayerBase> OtherRole
    {
        get
        {
            return mOtherRole;
        }
    }
    /// <summary>
    /// 播放动画剪辑
    /// </summary>
    /// <param name="clipName"></param>
    public void Play(string clipName)
    {
        palyerAnimator.Play(clipName);
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="stateId">状态id</param>
    /// <param name="param1">参数1</param>
    /// <param name="param2">参数2</param>
    public virtual void SwitchState(uint stateId,object param1 = null,object param2 = null)
    {
        if(param1 != null)
        {
            mOtherRole = param1 as List<PlayerBase>;
        }
        mPlayerStateMachine.SwitchState(stateId, param1, param2);
    }


    /// <summary>
    /// 角色动画播放结束
    /// </summary>
    /// <param name="clipNmae"></param>
    public void AnimationEventEnd(string clipName)
    {
        Debug.Log("角色动画播放结束" + clipName);
        if(mPlayerStateMachine.CurrentState != null)
        {
            StateBase state = mPlayerStateMachine.CurrentState as StateBase;
            state.OnAnimationEventEnd(clipName);
        }
    }
    /// <summary>
    /// 角色攻击结束
    /// </summary>
    /// <param name="clipName"></param>
    public void AnimationAttackOver(string clipName)
    {
        Debug.Log("攻击结束" + clipName);
        if (mPlayerStateMachine.CurrentState != null)
        {
            StateBase state = mPlayerStateMachine.CurrentState as StateBase;
            state.OnAnimationAttackOver(clipName);
        }
    }

    #region 系统方区
    void Awake()
    {
        OnAwake();
    }

    void Update()
    {
        OnUpdate();
        mPlayerStateMachine.OnUpate();
    }

    void FixedUpdate()
    {
        OnFixedUpdate();
        mPlayerStateMachine.OnFixedUpdate();
    }

    void LeteUpdate()
    {
        OnLeteUpdate();
        mPlayerStateMachine.OnLeteUpdate();
    }
    #endregion

    public virtual void OnAwake() { }
    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }
    public virtual void OnLeteUpdate() { }
}
