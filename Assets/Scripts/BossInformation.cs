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

    void Awake()
    {
        if (msController == null)
        {
            msController = FindObjectOfType<MinionSpawnerController>(); // Attempt to find the reference
        }
    }

    void Start()
    {
        minionCount = msController.entityCount;
        immune = true;
    }

   

    void OnEnable()
    {
        if (msController != null)
        {
            minionCount = msController.entityCount;
        }
        immune = true;
    }


    // Update is called once per frame
    public bool getImmune()
    {
        return immune;
    }
    public void setImmune(bool i)
    {
        immune = i;
    }
    //public void minionDestroyed()
    //{
    //    bossShield = FindFirstObjectByType<BossShield>();
    //    minionCount--;
    //    if(minionCount >= 2)
    //    {
    //        bossShield.shieldDamaged(1);
    //    }
    //    else if (minionCount == 1)
    //    {
    //        bossShield.shieldDamaged(2);
    //    }
    //    if(minionCount <= 0)
    //    {
    //        setImmune(false);
    //        bossShield.playShieldBreakAnimation();
    //    }
    //}

    public void minionDestroyed()
    {
<<<<<<< HEAD
        bossShield = FindFirstObjectByType<BossShield>();
        minionCount--;
<<<<<<< HEAD
=======
        if(minionCount >= 2)
=======
        if (bossShield == null)
>>>>>>> 83c9ade (Defeat Screen)
        {
            bossShield = FindFirstObjectByType<BossShield>();
        }

        if (bossShield != null) // Check if bossShield is still valid
        {
            minionCount--;
            if (minionCount >= 2)
            {
                bossShield.shieldDamaged(1);
            }
            else if (minionCount == 1)
            {
                bossShield.shieldDamaged(2);
            }
            if (minionCount <= 0)
            {
                setImmune(false);
                bossShield.playShieldBreakAnimation();
            }
        }
<<<<<<< HEAD
>>>>>>> 63d9bd3 (Minion and Shield Respawn Threashhold)
        if(minionCount <= 0)
        {
            setImmune(false);
            bossShield.playShieldBreakAnimation();
            bossShield.playShieldBreakAnimation();
=======
        else
        {
            Debug.LogError("BossShield reference not found!");
>>>>>>> 83c9ade (Defeat Screen)
        }
    }

}
