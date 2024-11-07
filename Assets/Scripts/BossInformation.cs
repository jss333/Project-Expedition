using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInformation : MonoBehaviour
{
    // Start is called before the first frame update
    
    private bool immune;
    private int minionCount;
    private MinionSpawnerController msController;
    private BossShield bossShield;

    void Start()
    {
        msController = FindFirstObjectByType<MinionSpawnerController>();
        minionCount = msController.entityCount;
        immune = true;
    }

    // Update is called once per frame
    public bool GetImmune()
    {
        return immune;
    }
    public void SetImmune(bool i)
    {
        immune = i;
    }
    public void MinionDestroyed()
    {
        if(FindFirstObjectByType<BossShield>()!= null)
        {
            bossShield = FindFirstObjectByType<BossShield>();
            minionCount--;
            if (minionCount <= 0)
            {
                SetImmune(false);
                bossShield.playShieldBreakAnimation();
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
    }
    public void SetMinionCount(int count)
    {
        minionCount = count;
    }
}
