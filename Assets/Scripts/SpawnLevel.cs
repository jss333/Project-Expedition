using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLevel : MonoBehaviour
{
    public float appearTime = 3f;  // Time the platform stays active
    public float disappearTime = 2f;  // Time the platform stays inactive
    public GameObject[] spawnPoints;  // Array of spawn point GameObjects
    public bool isHorizontal;  // True if the platform is horizontal, false if vertical
    public SpawnPointManager spawnPointManager;  // Reference to the SpawnPointManager

    private float timer;
    private bool isActive = true;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Transform currentSpawnPoint;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        timer = appearTime;
        RandomizePosition();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (isActive && timer <= 0)
        {
            DeactivatePlatform();
        }
        else if (!isActive && timer <= 0)
        {
            ActivatePlatform();
        }
    }

    void DeactivatePlatform()
    {
        isActive = false;
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;

        if (currentSpawnPoint != null)
        {
            spawnPointManager.UnregisterSpawnPoint(currentSpawnPoint);
        }

        timer = disappearTime;
    }

    void ActivatePlatform()
    {
        isActive = true;
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
        RandomizePosition();
        timer = appearTime;
    }

    void RandomizePosition()
    {
        currentSpawnPoint = spawnPointManager.GetAvailableSpawnPoint(spawnPoints);

        if (currentSpawnPoint != null)
        {
            transform.position = currentSpawnPoint.position;
            spawnPointManager.RegisterSpawnPoint(currentSpawnPoint);

            if (isHorizontal)
            {
                // Adjust the size or rotation if necessary for horizontal platforms
                transform.localScale = new Vector3(2f, 0.5f, 1f); // Example scale for horizontal
            }
            else
            {
                // Adjust the size or rotation if necessary for vertical platforms
                transform.localScale = new Vector3(0.5f, 2f, 1f); // Example scale for vertical
            }
        }
    }
}



