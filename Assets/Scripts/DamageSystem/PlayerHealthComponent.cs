using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthComponent : HealthComponent
{
    private EntityActionVisualController entityActionVisualController;
    [SerializeField] private GameObject robObj;
    [SerializeField] private EventReference playerTakesDamageSFX;
    private Animator animator;
    public override void Start()
    {
        base.Start();
        entityActionVisualController = GetComponent<EntityActionVisualController>();
        animator = GetComponent<Animator>();
    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        //AudioManagerNoMixers.Singleton.PlaySFXByName("PlayerTakesDamage");
        AudioManagerNoMixers.Singleton.PlayOneShot(playerTakesDamageSFX, this.transform.position);

        entityActionVisualController.ApplyGettingHitVisuals();
        DamageEventsManager.OnPlayerDamaged?.Invoke((float)damage / getMaxHealth());
        PlayHitAnim();
    }
    public void PlayerDeath()
    {
        //challengeRoomBGM.PlayDefeatBGM();
        AudioManagerNoMixers.Singleton.PlayDefeatMusic();
        EndGameEventManager.OnDefeatAchieved?.Invoke();
        Destroy(robObj);
    }

    private void PlayHitAnim()
    {
        animator.SetTrigger("gotHit");
    }
}
