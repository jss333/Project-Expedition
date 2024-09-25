using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorInvisible : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
