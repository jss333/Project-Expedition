using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pointer_Sys : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private Mouse mouse;
    public bool hide_pointer = false;
    private Vector3 pointer_pos = Vector3.zero;

    void Start()
    {
        if(cam == null){
            Debug.Log("No camera attached -- pointer non-functional.");
            hide_pointer = true;
        }
        
        mouse = Mouse.current;
    }

    // Update is called once per frame
    void Update()
    {
        if(hide_pointer)    {return;}
        pointer_pos = mouse.position.ReadValue();
        Debug.Log(pointer_pos);
        this.transform.position = cam.ScreenToWorldPoint(pointer_pos);
        Debug.Log(this.transform.position);
        }
}
