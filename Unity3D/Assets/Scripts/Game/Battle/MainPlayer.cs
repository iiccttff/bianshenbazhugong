/*
 * 脚本名(ScriptName)：    MainPlayer.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;

public class MainPlayer : PlayerBase 
{
    public override void SwitchState(uint stateId, object param1 = null, object param2 = null)
    {
        base.SwitchState(stateId, param1, param2);
    }

    public override void OnAwake()
    {
        base.OnAwake();
        this.camp = 0;
        //注册状态
        mPlayerStateMachine.RegisterState(new MainPlayerStateAttack(this));
        mPlayerStateMachine.RegisterState(new MainPlayerStateIdle(this));
        mPlayerStateMachine.RegisterState(new MainPlayerStateHit(this));
    }
}
