using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class HazardPlatform : MonoBehaviour
{
    [SerializeField] protected float disappearTime;
    [SerializeField] protected float stayHiddenTime;
    [SerializeField] protected float shakeTime = 1;
    [SerializeField] protected Collider2D mainCollider;
    [SerializeField] protected Collider2D triggerCollider;
    [SerializeField] protected Animator animator;
    [SerializeField] protected TextMeshProUGUI timerText;
    [SerializeField] protected GameObject timerHolder;

    private Transform startPoint;
    protected bool isAlreadyTriggered;


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
            if (isAlreadyTriggered) return;
            TiggerAction();
        }
    }

    protected virtual void TiggerAction()
    {
        LeanTween.cancel(gameObject);
        isAlreadyTriggered = true;

        /*
        LeanTween.rotateAround(gameObject, Vector3.forward, 3f, 0.02f).setEaseInOutSine().setTime(shakeTime).setOnComplete(() =>
        {
            LeanTween.cancel(gameObject);
            transform.rotation = Quaternion.identity;
            ActivateAction();
        }); */

        animator.SetBool("Shake", true);

        float timer = shakeTime;
        timerHolder.SetActive(true);

        LeanTween.value(gameObject, timer, 0, shakeTime).setOnUpdate((value) =>
        {
            timerText.text = ((int)value).ToString();
        }).setOnComplete(() =>
        {
            ActivateAction();
            timerHolder.SetActive(false);
            animator.SetBool("Shake", false);
        });
    }

    protected virtual void ActivateAction()
    {
        LeanTween.cancel(gameObject);

        Debug.Log("Started Action");
        LeanTween.delayedCall(gameObject, stayHiddenTime, () => ReActivateAgain());

    }

    protected virtual void ReActivateAgain()
    {
        ResetToSpwanPoint();
        triggerCollider.enabled = true;

        Debug.Log("Reactivated");
    }

    protected virtual void ResetToSpwanPoint()
    {
        transform.position = startPoint.position;
        transform.localScale = startPoint.localScale;
        transform.rotation = startPoint.rotation;

        isAlreadyTriggered = false;
    }

    private void OnDisable()
    {
        LeanTween.cancel(gameObject);
    }
}