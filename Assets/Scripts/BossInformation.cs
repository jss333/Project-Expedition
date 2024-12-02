using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInformation : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private BossShield bossShield;
    [SerializeField] private MinionSpawnerController msController;
    [SerializeField] private bool immune = false;
    [SerializeField] private int minionCount;

    void Start()
    {
        msController = FindFirstObjectByType<MinionSpawnerController>();
        minionCount = msController.NumEntitiesToSpawn;
        Debug.Log("Initial minions: " + minionCount);
    }

    public bool MinionDestroyed()
    {
        if(FindFirstObjectByType<BossShield>() != null)
        {
            bossShield = FindFirstObjectByType<BossShield>();
            minionCount--;
            if (minionCount <= 0)
            {
                bossShield.playShieldBreakAnimation();
                FindFirstObjectByType<BossHealthComponent>().SetIsImmune(false);
                return true;
            }
        }

        if (minionCount >= 2)
        {
            bossShield.shieldDamaged(1);
        }
        else if (minionCount == 1)
        {
            bossShield.shieldDamaged(2);
        }

        return false;
    }

    public void SetMinionCount(int count)
    {
        minionCount = count;
    }

    public bool GetImmune()
    {
        return immune;
    }

    public void SetImmune(bool i)
    {
        immune = i;
    }
}
