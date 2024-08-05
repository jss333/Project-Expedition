using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_CombatSys : MonoBehaviour
{
    public Rigidbody2D rb;
    public float max_vel = 2;
    public float health;
    public InputAction mv;
    public InputAction jump;

    public GameObject proj;

    //Stores direction player is facing. 0 -> left, 1 -> right
    private bool player_dir = false;
    public SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        jump.Enable();
        mv.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(jump.IsPressed()){
            Instantiate(proj, transform.position, transform.rotation);
        }
    }
}
