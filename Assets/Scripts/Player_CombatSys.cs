using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_CombatSys : MonoBehaviour
{
    public Rigidbody2D rb;
    public float max_vel = 2;
    public float health;
    public InputAction fire;
    public InputAction aim;

    public GameObject proj;
    public float cd = .5F;
    private float cd_rm = 0;

    public ProjectileType type = ProjectileType.Precision;

    [Header("Maximum angular displacement of launched projectiles from player's Vector3.right")]
    [SerializeField] private float MuzzleSpread = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fire.Enable();
        aim.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(cd_rm <= 0){
            if(fire.IsPressed()){
                LaunchProjectile();
            }
        }
        else{
            cd_rm -= Time.deltaTime;
        }
    }

    private void LaunchProjectile(){
        Projectile_Sys p;
        if(type == ProjectileType.Tracking){
            p = ((GameObject)Instantiate(proj, transform.position, transform.rotation * Quaternion.AngleAxis(Random.Range(-MuzzleSpread, MuzzleSpread), Vector3.forward)))
                .GetComponent<Projectile_Sys>();
        }
        else{
            p = ((GameObject)Instantiate(proj, transform.position, transform.rotation)).GetComponent<Projectile_Sys>();
        }
        p.type = type;
        cd_rm = cd;
    }
}