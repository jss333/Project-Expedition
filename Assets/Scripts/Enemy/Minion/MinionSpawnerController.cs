using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawnerController : MonoBehaviour
{
    public GameObject entityToSpawn;
    private int activeEntityInstances = 0;
    public int entityCount = 2;
    private float timer = 0;
    [SerializeField] private List<GameObject> freeAnchors; //without minion
    [SerializeField] private List<GameObject> activeAnchors; //with minion


    void Awake(){
        activeAnchors = new List<GameObject>(freeAnchors.Count);

    }
    private void Start()
    {
        spawnWave();
    }
    public void decrementActiveCount(GameObject anchorToRemove){
        for(int counter = 0; counter < activeAnchors.Count; counter++){
            if (activeAnchors[counter].transform == anchorToRemove.transform){
                freeAnchors.Add(activeAnchors[counter]);
                activeAnchors.RemoveAt(counter);
                activeEntityInstances--;
            }
        }

    }
    public void spawnWave(){
        while(activeEntityInstances < entityCount){
            int randomIndex = Random.Range(0, freeAnchors.Count);
            activeAnchors.Add(freeAnchors[randomIndex]);
            GameObject newMinion = Instantiate(entityToSpawn, this.transform.position, this.transform.rotation);
            newMinion.GetComponent<MinionController>().anchor = freeAnchors[randomIndex];
            freeAnchors.RemoveAt(randomIndex);
            activeEntityInstances++;
            //timer = 0;
        }
    }
    public void handleMinionRespawn()
    {
        if(activeEntityInstances ==0)
        {
            spawnWave();
        }
    }
    public void handleMinionRespawn()
    {
        if(activeEntityInstances ==0)
        {
            spawnWave();
        }
    }
}
