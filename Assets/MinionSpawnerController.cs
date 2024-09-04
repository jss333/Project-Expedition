using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawnerController : MonoBehaviour
{
    public GameObject entityToSpawn;
    private int activeEntityInstances = 0;
    public int entityCount = 2;
    public float waveCooldown = 5F;
    private float timer = 0;
    [SerializeField] private List<GameObject> freeAnchors;
    [SerializeField] private List<GameObject> activeAnchors;


    void Awake(){
        activeAnchors = new List<GameObject>(freeAnchors.Count);
    }

    void Update(){
        if(activeEntityInstances == 0){
            if(timer >= waveCooldown){
                spawnWave();
            }
            else{
                timer += Time.deltaTime;
            }
        }
    }

    public void decrementActiveCount(GameObject anchorToRemove){
        for(int counter = 0; counter < activeAnchors.Count; counter++){
            if (activeAnchors[counter].transform == anchorToRemove.transform){
                freeAnchors.Add(activeAnchors[counter]);
                activeAnchors.RemoveAt(counter);
            }
        }
        if((activeEntityInstances--) < 0)   {activeEntityInstances = 0;}
    }

    public void spawnWave(){
        while(activeEntityInstances < entityCount){
            int randomIndex = Random.Range(0, freeAnchors.Count);
            activeAnchors.Add(freeAnchors[randomIndex]);
            GameObject newMinion = Instantiate(entityToSpawn, this.transform.position, this.transform.rotation);
            newMinion.GetComponent<MinionController>().anchor = freeAnchors[randomIndex];
            freeAnchors.RemoveAt(randomIndex);
            activeEntityInstances++;
            timer = 0;
        }
    }
}
