using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public float charge = 4;
    public float shield_offset = 1.5F;
    private Mouse mouse;
    public Vector3 cursor_pos;
    public InputAction activate;
    [SerializeField] private Camera cam;


    // Start is called before the first frame update
    void Start()
    {
        mouse = Mouse.current;
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
        activate.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(cam != null){
            cursor_pos = (Vector2)mouse.position.ReadValue();
            Vector2 dir = (Vector2)(cursor_pos - cam.WorldToScreenPoint(this.transform.parent.position)).normalized;
        }
        else{
            Debug.Log("Error: no camera...");
        }
    }
}
