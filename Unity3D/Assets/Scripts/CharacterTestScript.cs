/*
 * 脚本名(ScriptName)：    CharacterTestScript.cs
 * 作者(Author):           Circle
 * 官网(Url):              http://blog.csdn.net/u013108312
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterTestScript : MonoBehaviour,INotifier
{
    //Animator anim;
    public PlayerBase player;
    /// <summary>
    /// 主角信息
    /// </summary>
    public List<PlayerBase> mainRole = new List<PlayerBase>();

    public List<PlayerBase> monsterRole = new List<PlayerBase>();
    /// <summary> 当前行动队列 </summary>
    private Queue<PlayerBase> mActionQueue = new Queue<PlayerBase>();
    /// <summary>
    /// 是否开始攻击
    /// </summary>
    private bool isAttack = false;

	// Use this for initialization
    void Start()
    {
        GameServerMgr.GetInstance().RegisterNotifier(CombatDef.AttackOver, this);
    }

    void InitActionQueue()
    {
        for (int i = 0; i < mainRole.Count; i++)
        {
            mActionQueue.Enqueue(mainRole[i]);
        }

        for (int i = 0; i < monsterRole.Count; i++)
        {
            mActionQueue.Enqueue(monsterRole[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InitActionQueue();
            isAttack = true;
            //Attack();
            //player.SwitchState(1);
        }
        else if(Input.GetKeyDown(KeyCode.Return))
        {
            
        }

        if(isAttack && mActionQueue.Count > 0)
        {
            PlayerBase player = mActionQueue.Peek();
            if(player.playerStateMachine.CurrentID == StateDef.idle)
            {
                PlayerBase attackRole = player.camp == 0 ? monsterRole[0] : mainRole[0];
                player.SwitchState(1, new List<PlayerBase>() { attackRole });
            }
        }
    }

    void Attack()
    {
        mainRole[0].SwitchState(1, new List<PlayerBase>() { monsterRole[0] });
    }


    public void OnReceiveData(uint cmdId, object param1, object param2)
    {
        Debug.LogError(cmdId);
        switch(cmdId)
        {
            case CombatDef.AttackOver:
                PlayerBase player = param1 as PlayerBase;
                uint nextStateId = (uint)param2;
                if(mActionQueue.Peek() == player && nextStateId == StateDef.idle)
                {
                    Debug.Log("调整攻击队列");
                    mActionQueue.Dequeue();//将攻击完成的对象移除队列
                    //mActionQueue.Enqueue(player);// 如果需要循环攻击
                }
                break;
        }
    }

    void OnDestroy()
    {
        GameServerMgr.GetInstance().UnregisterNotifier(CombatDef.AttackOver, this);
    }
}
