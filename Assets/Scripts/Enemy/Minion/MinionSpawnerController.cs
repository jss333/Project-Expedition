using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawnerController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private GameObject entityToSpawn;
    [SerializeField] private VisualLinkController visualLink;
    [SerializeField] private List<GameObject> freeAnchors; //without minion

    [Header("State")]
    [SerializeField] private int numEntitiesToSpawn = 2;
    [SerializeField] private int activeEntityInstances = 0;
    [SerializeField] private List<GameObject> activeAnchors; //with minion
    private Transform bossTransform;

    public int NumEntitiesToSpawn => numEntitiesToSpawn;

    void Awake()
    {
        activeAnchors = new List<GameObject>(freeAnchors.Count);
    }

    private void Start()
    {
        bossTransform = FindFirstObjectByType<BossController>().transform;
        numEntitiesToSpawn = RoundManager.Singleton.RoundSettings.NumMinionsToSpawn;

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
