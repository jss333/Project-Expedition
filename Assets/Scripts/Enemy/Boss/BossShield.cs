using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShield : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("References")]
    private Animator animator;
    [SerializeField] private AudioClip crack1SFX;
    [SerializeField] private AudioClip crack2SFX;
    [SerializeField] private AudioClip shieldBreakSFX;
    private RandomPitchAudioSource audioSource;
    private BossController boss;

    void Start()
    {
        boss = FindFirstObjectByType<BossController>();
        animator = GetComponent<Animator>();
        this.transform.localScale = new Vector3(.55f, .55f, 0);
        //shield = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<RandomPitchAudioSource>();
    }
    //change the damaged stage of the sprite renderer for the shield.
    public void shieldDamaged(int spriteNum)
    {
        
        if (spriteNum == 1)
        {
            animator.SetTrigger("ShieldBreak1");
            audioSource.PlayAudioWithNormalPitch(crack1SFX);
        }
        else if (spriteNum == 2)
        {
            animator.SetTrigger("ShieldBreak2");
            audioSource.PlayAudioWithNormalPitch(crack2SFX);
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
        audioSource.PlayAudioWithNormalPitch(shieldBreakSFX);
        boss.setHasShield(false);
        Debug.Log("play sb anim");
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
