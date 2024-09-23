using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInformation : MonoBehaviour
{
    // Start is called before the first frame update
    
    private bool immune;
    private int minionCount;
    [Header("References")]
    [SerializeField] private MinionSpawnerController msController;
    private BossShield bossShield;
    void Start()
    {
        minionCount = msController.entityCount;
        immune = true;
    }

    // Update is called once per frame
    public bool getImmune()
    {
        return immune;
    }
    private void setImmune(bool i)
    {
        immune = i;
    }
    public void minionDestroyed()
    {
        bossShield = FindFirstObjectByType<BossShield>();
        minionCount--;
        if(minionCount == 2)
        {
            bossShield.shieldDamaged(1);
        }
        else if (minionCount == 1)
        {
            bossShield.shieldDamaged(2);
        }
        if(minionCount <= 0)
        {
            setImmune(false);
            bossShield.playShieldBreakAnimation();
        }
    }
}
