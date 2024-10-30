using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeowAlertedState : EnemyState
{
    private MeowEnemy enemy;
    private Color color;

    public MeowAlertedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, MeowEnemy enemy, Color color) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
        this.color = color;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        enemy.spriteRenderer.color = color;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //Debug.Log("Alerted");

        if(enemy.IsAgro())
        {
            stateMachine.ChangeState(enemy.meowTraverseState);
        }

        if (!enemy.IsAgro() && !enemy.IsAlerted())
        {
            stateMachine.ChangeState(enemy.meowIdleState);
        }
    }
}
