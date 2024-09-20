using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInformation : MonoBehaviour
{
    // Start is called before the first frame update

    private bool immune;
    private int minionCount;
    [SerializeField] private MinionSpawnerController msController;
    void Start()
    {
        minionCount = msController.entityCount;
        immune = false;
    }

    // Update is called once per frame
    void Update()
    {
        //var minions = FindAnyObjectByType<MinionController>();
        if (minionCount > 0)
        {
            setImmune(true);
        }
        else
        {
            setImmune(false);
        }
    }
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
        minionCount--;
    }
}
