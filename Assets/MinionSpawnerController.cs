using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawnerController : MonoBehaviour
{
    public GameObject entityToSpawn;
    private int activeEntityInstances = 0;
    public int entityCount = 3;
    public float delayTimeToNextWave = 5F;
    private float timer = 0;
    [SerializeField] private List<GameObject> anchors;

    void Awake(){
        anchors = new List<GameObject>(entityCount);
    }

    void Update(){
        if(activeEntityInstances == 0){
            
        }
    }
}
