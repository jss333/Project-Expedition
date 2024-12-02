using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArmSpawnPoint : MonoBehaviour
{
    public enum SpawnType
    {
        Vertical,
        Horizontal
    }

    public SpawnType spawnType; 
}
