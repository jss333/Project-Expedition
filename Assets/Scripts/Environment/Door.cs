using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float openAnimationTime;

    public void OpenDoor()
    {
        Vector3 upPoint = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
        LeanTween.move(gameObject, upPoint, openAnimationTime);


        Material mat = GetComponent<SpriteRenderer>().material;

        LeanTween.value(1, 0, openAnimationTime).setOnUpdate((value) => mat.SetVector("_Color", new Vector4(1, 1, 1, value)));
    }
}
