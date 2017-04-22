/*
 * 脚本名(ScriptName)：    MainPlayerStateIdle.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;

public class MainPlayerStateIdle : StateBase 
{
    public MainPlayerStateIdle(PlayerBase player)
        : base(player)
    {

    }

    public override uint GetStateID()
    {
        return StateDef.idle;
    }

    public override void OnEnter(StateMachine machine, IState prevState, object param1, object param2)
    {
        mPlayer.Play("idle");
    }

    public override void OnLeave(IState nextState, object param1, object param2)
    {
    }

    public override void OnUpate()
    {
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnLeteUpdate()
    {
    }
}
