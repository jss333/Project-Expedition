using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInformation : MonoBehaviour
{
    // Start is called before the first frame update

    public bool immune;
    void Start()
    {
        immune = false;
    }

    // Update is called once per frame
    void Update()
    {
        var minions = FindAnyObjectByType<MinionController>();
        if (minions != null)
        {
            immune = true;
        }
        else
        {
            immune = false;
        }

    }
}
