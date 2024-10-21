using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HighLightShadingEffect))]
public class EntityActionVisualController : MonoBehaviour //This class is specific to Boss, should be renamed
{
    private Animator animator;
    private HighLightShadingEffect highLightShadingEffect;
    private BossController bossController;
    private void Start()
    {
        animator = GetComponent<Animator>();
        highLightShadingEffect = GetComponent<HighLightShadingEffect>();
        bossController = GetComponent<BossController>();
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
        // Why can't this be handled by the Controller state machine??
        if (bossController.hurtStateTriggered)
        {
            animator.CrossFadeInFixedTime("BossDamaged_Shooting", 0.2f);
        }
        else 
        {
            animator.CrossFadeInFixedTime("Boss_Shooting", 0.2f);
        }
        

    }
}
