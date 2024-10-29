using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeowTraverseState : EnemyState
{
    private MeowEnemy enemy;
    private Color color;

    public MeowTraverseState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, MeowEnemy enemy, Color color) : base(enemyBase, stateMachine, animBoolName)
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

        Debug.Log("Agro");

        if (enemy.IsAlerted())
        {
           stateMachine.ChangeState(enemy.meowAlertedState);
        }
    }
}
