using AbilitySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Singleton;

    [SerializeField] private List<PooledObjectInfo> objectPools = new List<PooledObjectInfo>();


    private void Awake()
    {
        if (Singleton != null)
        {
            Destroy(Singleton);
        }
        else
        {
            Singleton = this;
        }
    }

    public GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        PooledObjectInfo pool = objectPools.Find(p => p.lookUpString == objectToSpawn.name);

        if (pool == null)
        {
            pool = new PooledObjectInfo() { lookUpString = objectToSpawn.name };
            objectPools.Add(pool);
        }

        GameObject spawnableObj = pool.inactiveObjects.FirstOrDefault();

        if (spawnableObj == null)
        {
            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
        }
        else
        {
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.inactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7);
        PooledObjectInfo pool = objectPools.Find(p =>p.lookUpString == goName);

        if(pool == null)
        {
            Debug.LogWarning("Trying To release object that is not pooled" + obj.name);
        }
        else
        {
            obj.SetActive(false);
            pool.inactiveObjects.Add(obj);
        }
    }
}

[Serializable]
public class PooledObjectInfo
{
    public string lookUpString;
    public List<GameObject> inactiveObjects = new List<GameObject>();
}
