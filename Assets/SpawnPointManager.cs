using UnityEngine;
using System.Collections.Generic;

public class SpawnPointManager : MonoBehaviour
{
    private HashSet<Transform> occupiedSpawnPoints = new HashSet<Transform>();

    public void RegisterSpawnPoint(Transform spawnPoint)
    {
        occupiedSpawnPoints.Add(spawnPoint);
    }

    public void UnregisterSpawnPoint(Transform spawnPoint)
    {
        occupiedSpawnPoints.Remove(spawnPoint);
    }

    public Transform GetAvailableSpawnPoint(Transform[] spawnPoints)
    {
        List<Transform> availablePoints = new List<Transform>();

        foreach (var point in spawnPoints)
        {
            if (!occupiedSpawnPoints.Contains(point))
            {
                availablePoints.Add(point);
            }
        }

        if (availablePoints.Count > 0)
        {
            return availablePoints[Random.Range(0, availablePoints.Count)];
        }
        else
        {
            Debug.LogWarning("No available spawn points!");
            return null;
        }
    }
}