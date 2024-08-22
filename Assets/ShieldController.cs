using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldController : MonoBehaviour
{
    private Collider2D col;
    private SpriteRenderer sr;
    [SerializeField] private GameObject cursor;
    public float charge = 4;
    public float shield_offset = 1.5F;
    private Mouse mouse;
    private Vector3 cursor_pos;
    public InputAction activate;
    [SerializeField] private Camera cam;
    private bool run = true;


    // Start is called before the first frame update
    void Start()
    {
        mouse = Mouse.current;
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;

        col = GetComponent<Collider2D>();
        col.enabled = false;

        activate.Enable();

        if(cam == null || cursor == null){
            Debug.Log("Error: Undefined object parameters...");
            run = false;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!run)    {return;}
        cursor_pos = mouse.position.ReadValue();
        Debug.Log(cursor_pos);
        cursor.transform.position = cam.ScreenToWorldPoint(cursor_pos);
        Debug.Log(cursor.transform.position);
        Vector2 dir = (Vector2)(cursor_pos - cam.WorldToScreenPoint(this.transform.parent.position)).normalized;
        
    }
}
