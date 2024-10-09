using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal adjacentPortal;
    [SerializeField] Transform spawnPoint;
    bool canTelePort;

    public bool CanTelePort 
    {
        get { return canTelePort; }
        set { canTelePort = value; }
    }

    private void Start()
    {
        canTelePort = true;
    }

    public void SpawnTarget(Transform targetObject)
    {

        if(targetObject.GetComponent<PlayerMovement>() != null )
        {
            targetObject.transform.position = spawnPoint.position;
            targetObject.transform.rotation = spawnPoint.rotation;

            SpriteRenderer sprite = targetObject.GetComponent<SpriteRenderer>();
            if (sprite != null)
            {
                sprite.flipX = true;
            }
        }

        if(targetObject.GetComponent<PlayerProjectile>()  != null )
        {
            targetObject.transform.position = spawnPoint.position;
            targetObject.GetComponent<Rigidbody2D>().velocity *= -1f; 
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!canTelePort) return;

        if( collision != null )
        {
            adjacentPortal.SpawnTarget(collision.transform);
            adjacentPortal.CanTelePort = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if( collision != null )
        {
            canTelePort = true;
        }
    }
}
