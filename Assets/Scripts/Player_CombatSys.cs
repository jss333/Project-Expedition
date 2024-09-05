using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class B3RTCombat : MonoBehaviour
{
    [Header("References")]
    public GameObject proj;
    public GameObject pointer;
    public Rigidbody2D rb;

    [Header("Input Controls")]
    public InputAction fire;
    public InputAction aim;

    [Header("Parameters")]
    public float despawnTimer = 0;
    [SerializeField] private float timeToDespawn = 0;
    public float max_vel = 2;
    public bool rot_to_pointer = true;
    public float cd = .5F;
    [SerializeField] private float cd_rm = 0;

    //public ProjectileType type = ProjectileType.Precision;
    //[SerializeField] private float MuzzleSpread = 0;
   

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
                cd_rm = cd;
            }
        }
        else{
            cd_rm -= Time.deltaTime;
        }
        if(rot_to_pointer){
            float angle = Vector2.SignedAngle(transform.right, Vector3.Normalize(pointer.transform.position - this.transform.position));
            this.transform.rotation *= Quaternion.Euler(0, 0, angle);
        }
    }

    private void LaunchProjectile(){
        Projectile_Sys p = ((GameObject)Instantiate(proj, transform.position, transform.rotation)).GetComponent<Projectile_Sys>();
    }

    /*
    private void LaunchProjectile(){
        Projectile_Sys p;
        if(type == ProjectileType.Tracking){
            p = ((GameObject)Instantiate(proj, transform.position
                , transform.rotation * Quaternion.AngleAxis(Random.Range(-MuzzleSpread, MuzzleSpread), Vector3.forward)))
                .GetComponent<Projectile_Sys>();
        }
        else{
            p = ((GameObject)Instantiate(proj, transform.position, transform.rotation)).GetComponent<Projectile_Sys>();
        }
        p.type = type;
    }
    */
}