using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HighLightShadingEffect))]
public class EntityActionVisualController : MonoBehaviour
{
    private Animator animator;
    private HighLightShadingEffect highLightShadingEffect;

    private void Start()
    {
        animator = GetComponent<Animator>();
        highLightShadingEffect = GetComponent<HighLightShadingEffect>();
    }

    public void ApplyGettingHitVisuals()
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);

        if(clipInfo != null) 
        {
            if(clipInfo.Length > 0)
            {
                AnimationClip currentClip = clipInfo[0].clip;

                if (!(currentClip.name == "A_Boss_Getting_Damaged"))
                {
                    //animator.CrossFadeInFixedTime("GettingHit", 0);

                    highLightShadingEffect.FlashOnImapct();

                }
            }
        }

    }

    public void ApplyShootAnimation()
    {
        animator.CrossFadeInFixedTime("Boss_Shot", 0.2f);

    }
}
