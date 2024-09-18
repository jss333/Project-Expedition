using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenLeavingPlaySpace : MonoBehaviour
{
    [Tooltip("Logs a message when this object leaves the Play Space Boundary trigger area")]
    public bool debug;

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Play Space Boundary")
        {
            if (debug) Debug.Log("object " + this.gameObject.name + " is leaving the Play Space Boundary and will be destroyed");
         
            Destroy(this.gameObject);
        }
    }
}
