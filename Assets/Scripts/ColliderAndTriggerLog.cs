using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderAndTriggerLog : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        string msg = string.Format("Trigger enter between this {0} and {1}",
            this.gameObject.name,
            other.gameObject.name);

        Debug.Log(msg);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        string msg = string.Format("Collision enter in this {0}: {1} and {2}",
            this.gameObject.name,
            nameAndLayer(collision.collider.gameObject),
            nameAndLayer(collision.otherCollider.gameObject));

        Debug.Log(msg);
    }

    private string nameAndLayer(GameObject gameObject)
    {
        return gameObject.name + " (layer " + LayerMask.LayerToName(gameObject.layer) + ")";
    }
}
