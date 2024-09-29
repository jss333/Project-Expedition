using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingDamagedAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ApplyGettingHitAnimation()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);

        AnimationClip currentClip = clipInfo[0].clip;

        Debug.Log(currentClip.name);

        if(!(currentClip.name == "A_Boss_Getting_Damaged"))
        {
            animator.CrossFadeInFixedTime("GettingHit", 0);
        }

    }
}
