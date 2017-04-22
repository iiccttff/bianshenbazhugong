/*
 * 脚本名(ScriptName)：    MonsterPlayerStateAttack.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MonsterPlayerStateAttack : StateBase 
{
    public MonsterPlayerStateAttack(PlayerBase player)
        : base(player)
    {

    }

    public override uint GetStateID()
    {
        return StateDef.attack;
    }

    public override void OnEnter(StateMachine machine, IState prevState, object param1, object param2)
    {
        Debug.Log("attack1");
        //mPlayer.Play("attack1");
        Vector3 v3 = mPlayer.OtherRole[0].transform.position + new Vector3(1f,0,0);
        Tween tw = mPlayer.transform.DOMove(v3, 0.3f);
        tw.OnComplete(delegate()
        {
            mPlayer.Play("attack1");
        });
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
        Debug.Log("Monster OnAnimationAttackOver");
        mPlayer.OtherRole[0].SwitchState(StateDef.hit);
    }

    public override void OnAnimationEventEnd(string clipName)
    {
        mPlayer.transform.DOLocalMove(Vector3.zero, 0.3f).OnComplete(delegate() 
        {
            mPlayer.SwitchState(StateDef.idle);
        });

        
    }
}
