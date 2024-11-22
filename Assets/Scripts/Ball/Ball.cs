using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static Action<Vector2> OnBallDisappear;

    void Start()
    {
        Invoke("Die", 5f);
    }

    private void Die()
    {
        OnBallDisappear?.Invoke(transform.position);
        Destroy(gameObject);
    }
}
