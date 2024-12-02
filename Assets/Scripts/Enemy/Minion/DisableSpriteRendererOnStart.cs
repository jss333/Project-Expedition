using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSpriteRendererOnStart : MonoBehaviour
{
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
