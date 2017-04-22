/*
 * 脚本名(ScriptName)：    MonsterPlayerStateHit.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;

public class MonsterPlayerStateHit : StateBase 
{
    public MonsterPlayerStateHit(PlayerBase player)
        : base(player)
    {

    }

    public override uint GetStateID()
    {
        return StateDef.hit;
    }

    public override void OnEnter(StateMachine machine, IState prevState, object param1, object param2)
    {
        mPlayer.Play("hurt");
    }

    public override void OnLeave(IState nextState, object param1, object param2)
    {
        GameServerMgr.GetInstance().RequsterNotifier(CombatDef.AttackOver, mPlayer, nextState.GetStateID());
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

    public override void OnAnimationAttackOver(string clipName)
    {
        Debug.Log(22);
    }

    public override void OnAnimationEventEnd(string clipName)
    {
        Debug.Log(11);
        mPlayer.SwitchState(StateDef.idle);
    }
}
