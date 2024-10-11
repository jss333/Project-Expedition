using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal adjacentPortal;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool spawnOnlyInOneDirection;
    bool canTelePort;
    Transform targetObject;

    public bool CanTelePort 
    {
        get { return canTelePort; }
        set { canTelePort = value; }
    }

    public Transform TargetObject
    {
        get { return targetObject; }
        set { targetObject = value; }
    }

    private void Start()
    {
        canTelePort = true;
    }

    public void SpawnTarget()
    {

        /*if(targetObject.GetComponent<PlayerMovement>() != null )
        {
            targetObject.transform.position = spawnPoint.position;

            SpriteRenderer sprite = targetObject.GetComponent<SpriteRenderer>();
            if (sprite != null)
            {
                sprite.flipX = true;
            }
        } */

        if(targetObject.GetComponent<PlayerProjectile>()  != null )
        {
            targetObject.transform.position = spawnPoint.position;
            
            if(spawnOnlyInOneDirection)
            {
                targetObject.transform.rotation = spawnPoint.rotation;
                targetObject.GetComponent<Rigidbody2D>().velocity = spawnPoint.right * 20; 
            }
            else
            {
                targetObject.GetComponent<Rigidbody2D>().velocity *= -1f;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!canTelePort) return;

        if( collision != null )
        {
            if (collision.GetComponent<PlayerMovement>() != null)
            {
                targetObject = collision.transform;
            }
            
            adjacentPortal.TargetObject = collision.transform;
            adjacentPortal.SpawnTarget();
            adjacentPortal.CanTelePort = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if( collision != null )
        {
            canTelePort = true;
            targetObject = null;
        }
    }


    public void TelePortPlayer()   //Interaction Event Call back
    {
        targetObject.transform.position = adjacentPortal.spawnPoint.position;

        SpriteRenderer sprite = targetObject.GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.flipX = true;
        }
    }

    public void SpawnPlayerAtAdjacentPortal()
    {
        targetObject.transform.position = adjacentPortal.spawnPoint.position;

        SpriteRenderer sprite = targetObject.GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.flipX = true;
        }
    }
}
