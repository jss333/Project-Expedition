using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeowIdleState : EnemyState
{
    private MeowEnemy meowEnemy;
    private Color color;
    private Vector2 destination;
    private float remainingDistance;
    private bool hasReached = false;
    public MeowIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, MeowEnemy enemy, Color color) : base(enemy, stateMachine, animBoolName)
    {
        this.meowEnemy = enemy;
        this.color = color;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        meowEnemy.spriteRenderer.color = color;

        PickRandomPoint();
    }

    public override void OnExit()
    {
        LeanTween.cancel(meowEnemy.gameObject);

        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //Debug.Log("Idle");

        Debug.Log(hasReached);

        if (hasReached)
        {
            PickRandomPoint();
        }                                                                                   

        if (meowEnemy.IsAgro())
        {
            stateMachine.ChangeState(meowEnemy.meowTraverseState);
        }

        if (meowEnemy.IsAlerted())
        {
            stateMachine.ChangeState(meowEnemy.meowAlertedState);
        }
    }

    private void PickRandomPoint()
    {
        Vector2 randomPoint = (Vector2)meowEnemy.transform.position + Random.insideUnitCircle * 3f;

        RaycastHit2D hit = Physics2D.Raycast(randomPoint, Vector2.down, Mathf.Infinity, meowEnemy.GroundLayerMask);

        destination = hit.point;

        HasReachedDestination();
        //Debug.Log(destination);
    }

    private void HasReachedDestination()
    {
        LeanTween.cancel(meowEnemy.gameObject);
        hasReached = false;

        LeanTween.moveLocalX(meowEnemy.gameObject, destination.x, 2f).setOnUpdate((float value) =>
        {

            float dist = Vector2.Distance(meowEnemy.transform.position, destination);

            //Debug.Log(dist);
            if(dist < 0.2f)
            {
                hasReached = true;
            }
            else
            {
                hasReached = false;
            }
        });
    }
}
