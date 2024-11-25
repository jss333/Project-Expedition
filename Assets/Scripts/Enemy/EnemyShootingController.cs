using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingController : MonoBehaviour
{
    [Range(0,100)]
    [SerializeField] private int stunShotChance;


    public bool CanShootStunBullet()
    { 
        int randomNumber = Random.Range(0,101);

        if(randomNumber <= stunShotChance)
        {
            return true;
        }

        return false;
    }

    public void SetStunBulletChance(int chance)
    {
        stunShotChance = chance;
    }
}
