using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HazardPlatform : MonoBehaviour
{
    [SerializeField] protected float disappearTime;
    [SerializeField] protected float stayHazardousTime;
    [SerializeField] protected int shakeTime = 1;
    [SerializeField] protected Collider2D mainCollider;

    private Transform startPoint;
    protected bool isAlreadyActive;


    protected virtual void Start()
    {
        GameObject startPointGO = new GameObject(this.gameObject.name + "_StartPoint");
        startPoint = startPointGO.transform;

        startPoint.position = transform.position;
        startPoint.rotation = transform.rotation;
        startPoint.localScale = transform.localScale;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("test");
            TiggerAction();
        }
    }

    protected virtual void TiggerAction()
    {
        LeanTween.cancel(gameObject);

        LeanTween.rotateAround(gameObject, Vector3.forward, 3f, 0.02f).setEaseInOutSine().setLoopPingPong((shakeTime+1) * 10).setOnComplete(() =>
        {
            ActivateAction();
        });

        
    }

    protected virtual void ActivateAction()
    {
        LeanTween.cancel(gameObject);

        LeanTween.delayedCall(gameObject, stayHazardousTime, () => ReActivateAgain());

        Debug.Log("Started Action");
    }

    protected virtual void ReActivateAgain()
    {
        ResetToSpwanPoint();

        Debug.Log("Reactivated");
    }

    protected virtual void ResetToSpwanPoint()
    {
        transform.position = startPoint.position;
        transform.localScale = startPoint.localScale;
        transform.rotation = startPoint.rotation;

    }
}