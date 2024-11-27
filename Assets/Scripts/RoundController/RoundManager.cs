using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    #region Singleton;
    public static RoundManager Singleton;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            if (Singleton != this)
                Destroy(gameObject);
        }
    }
    #endregion

    [SerializeField] private RoundSettingsSO roundSettings;

    private BossHealthComponent bossHealthComponent;
    private PlayerHealthComponent playerHealthComponent;
    private EnemyShootingController bossShootingController;

    public RoundSettingsSO RoundSettings => roundSettings;

    private void Start()
    {
        bossHealthComponent = FindFirstObjectByType<BossHealthComponent>();
        bossShootingController = bossHealthComponent.GetComponent<EnemyShootingController>();
        bossHealthComponent.InitializeHealth(roundSettings.BossHealth);
        bossShootingController.SetStunBulletChance(roundSettings.BossStunBulletChance);

        playerHealthComponent = FindFirstObjectByType<PlayerHealthComponent>();
        playerHealthComponent.InitializeHealth(roundSettings.PlayerHealth);
    }
}
