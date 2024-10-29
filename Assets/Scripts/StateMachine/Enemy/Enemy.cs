using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask playerLayer;

    public Animator animator { get; private set; }

    public SpriteRenderer spriteRenderer { get; private set; }

    public EnemyStateMachine stateMachine { get; private set; }

    Collider2D playerCollider;

    bool isAgro = false;
    bool isAlerted = false;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        stateMachine = new EnemyStateMachine();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.OnUpdate();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //playerCollider = Physics2D.OverlapCircle(transform.position, agroDistance, playerLayer);

            float playerDist = Vector2.Distance(collision.transform.position, transform.transform.position);

            if (playerDist > agroDistance && playerDist < alertedDistance)
            {
                isAgro = false;
                isAlerted = true;
            }
            else if (playerDist < agroDistance)
            {
                isAgro = true;
                isAlerted = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isAlerted = false;
            isAgro = false;
        }
    }

    public virtual bool IsAgro()
    {
        return isAgro;
    }

    public virtual bool IsAlerted()
    {
        return isAlerted && !isAgro;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, alertedDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, agroDistance);
    }
}
