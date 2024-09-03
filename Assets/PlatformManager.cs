using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject horizontalPlatformPrefab;
    public GameObject verticalPlatformPrefab;
    public int numberOfHorizontalPlatforms = 3;
    public int numberOfVerticalPlatforms = 2;

    void Start()
    {
        // Instantiate horizontal platforms
        for (int i = 0; i < numberOfHorizontalPlatforms; i++)
        {
            Instantiate(horizontalPlatformPrefab, Vector3.zero, Quaternion.identity);
        }

        // Instantiate vertical platforms
        for (int i = 0; i < numberOfVerticalPlatforms; i++)
        {
            Instantiate(verticalPlatformPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
