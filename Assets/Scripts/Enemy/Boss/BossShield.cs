using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShield : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("References")]
    private Animator animator;
    private BossController boss;

    void Start()
    {
        boss = FindFirstObjectByType<BossController>();
        animator = GetComponent<Animator>();
        this.transform.localScale = new Vector3(.55f, .55f, 0);
        //shield = GetComponent<SpriteRenderer>();
    }
    //change the damaged stage of the sprite renderer for the shield.
    public void shieldDamaged(int spriteNum)
    {
        
        if (spriteNum == 1)
        {
            animator.SetTrigger("ShieldBreak1");
            AudioManagerNoMixers.Singleton.PlaySFXByName("BossShieldCracksFirst");
        }
        else if (spriteNum == 2)
        {
            animator.SetTrigger("ShieldBreak2");
            AudioManagerNoMixers.Singleton.PlaySFXByName("BossShieldCracksNext");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            //play shield absorb sound prob
            Destroy(collision.gameObject);
        }
    }
    public void playShieldBreakAnimation()
    {
        animator.SetTrigger("BreakShield");
        AudioManagerNoMixers.Singleton.PlaySFXByName("BossShieldBreaks");
        boss.setHasShield(false);
    }
    public void endShieldSprite()
    {
        Destroy(this.gameObject);
    }
    public void adjustSpriteSize()
    {
        this.transform.localScale = new Vector3(.78f, .79f, 0);
        this.transform.position = FindFirstObjectByType<BossController>().transform.position;
    }
}
