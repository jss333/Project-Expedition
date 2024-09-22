using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShield : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("References")]
    [SerializeField] SpriteRenderer shield;
    private Animator animator;
    [SerializeField] Sprite[] shieldSprites;
    private Sprite test;
    void Start()
    {
        animator = GetComponent<Animator>();
        shield.sprite = shieldSprites[0];
    }
    //change the damaged stage of the sprite renderer for the shield.
    public void shieldDamaged(int spriteNum)
    {
        Debug.Log("ChangeSprite");
        shield.sprite = shieldSprites[spriteNum];
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
        this.transform.localScale = new Vector3(1.5f,1.5f,0);
        Debug.Log("play sb anim");
    }
    public void endShieldSprite()
    {
        shield.sprite = shieldSprites[3];
        this.transform.localScale = Vector3.zero;
    }
    
}
