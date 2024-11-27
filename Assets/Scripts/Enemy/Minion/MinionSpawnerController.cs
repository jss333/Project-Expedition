using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawnerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject entityToSpawn;

    [Header("Parameters")]
    [SerializeField] private int numEntitiesToSpawn = 2;
    public int NumEntitiesToSpawn => numEntitiesToSpawn;

    [Header("State")]
    private Transform bossTransform;
    [SerializeField] private int activeEntityInstances = 0;
    [SerializeField] private VisualLinkController visualLink;
    [SerializeField] private List<GameObject> freeAnchors; //without minion
    [SerializeField] private List<GameObject> activeAnchors; //with minion

    void Awake()
    {
        activeAnchors = new List<GameObject>(freeAnchors.Count);
    }

    private void Start()
    {
        bossTransform = FindFirstObjectByType<BossController>().transform;

        if (FindAnyObjectByType<BossInformation>().GetImmune() == true) 
        {
            SpawnWave();
        }
    }

    public void SpawnWave()
    {
        while(activeEntityInstances < numEntitiesToSpawn)
        {
            int randomIndex = Random.Range(0, freeAnchors.Count);
            activeAnchors.Add(freeAnchors[randomIndex]);
            GameObject newMinion = Instantiate(entityToSpawn, bossTransform.position, this.transform.rotation);

            VisualLinkController visualLinkInstance = Instantiate(visualLink, bossTransform.position, Quaternion.identity).GetComponent<VisualLinkController>();
            visualLinkInstance.SetUpPoints(bossTransform, newMinion.transform);


            newMinion.GetComponent<MinionController>().anchor = freeAnchors[randomIndex];
            freeAnchors.RemoveAt(randomIndex);
            activeEntityInstances++;
        }
    }

    public void HandleMinionRespawn()
    {
        if(activeEntityInstances == 0) 
        {
            SpawnWave();
            BossInformation info = FindFirstObjectByType<BossInformation>();
            info.SetMinionCount(numEntitiesToSpawn);
        }
    }

    public void DecrementActiveCount(GameObject anchorToRemove)
    {
        for (int counter = 0; counter < activeAnchors.Count; counter++)
        {
            if (activeAnchors[counter].transform == anchorToRemove.transform)
            {
                freeAnchors.Add(activeAnchors[counter]);
                activeAnchors.RemoveAt(counter);
                activeEntityInstances--;
            }
        }
    }
}
