using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldController : MonoBehaviour
{
    private Collider2D col;
    private SpriteRenderer sr;
    [SerializeField] private GameObject pointer;
    
    public float shield_offset = 1.5F;
    public InputAction activate;
    public bool acquired;
    [SerializeField] private float duration = .3F;
    [SerializeField] private float charge;
    [SerializeField] private float regen_rate = .5F;
    [SerializeField] private bool on_cooldown = false;
    [SerializeField] private bool active = false;


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;

        col = GetComponent<Collider2D>();
        col.enabled = false;

        activate.Enable();
        charge = duration;
    }

    // Update is called once per frame
    void Update()
    {
        if(!acquired){return;}
        if(!on_cooldown){
            if(charge <= 0){
                charge = 0;
                on_cooldown = true;
                active = false;
                sr.enabled = false;
                col.enabled = false;
            }
            else if(active)  {charge -= Time.deltaTime;}
            else{
                if(activate.IsPressed()){
                //Activate ability visuals...
                    sr.enabled = true;
                    col.enabled = true;
                    active = true;
                    Debug.Log("Whoosh!");
                }
            }
            }
        else{
            if(charge < duration)       {charge += regen_rate * Time.deltaTime;}
            else{
                on_cooldown = false;
                charge = duration;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision){
        //Needs implementation...
        return;
    }
}
