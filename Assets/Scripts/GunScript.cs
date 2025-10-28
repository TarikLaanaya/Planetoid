using Unity.VisualScripting;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField]
    private Transform BulletSpawn;
    [SerializeField]
    private GameObject BulletPrefab;

    public float BulletSpeed = 20f;
    public float TimeBetweenShots = 0.5f;
    private float Cooldown = 0f;

    public bool ChargeShot;
    public float ChargeTimer = 60f;

    Animator animator;
    public float DefaultSpinSpeed = 1.0f;
    public float MaxSpinSpeed = 10.0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!ChargeShot)
        {
            Cooldown -= Time.deltaTime;

            if (Input.GetKey(KeyCode.Space) && Cooldown <= 0f)
            {
                BarrelSpinFaster();
                Shoot();
                Cooldown = TimeBetweenShots;
            }
            else
            {
                animator.speed = 1.0f;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                BarrelSpinFaster();
                if (Cooldown >= ChargeTimer)
                {
                    Cooldown = 0f;
                    Shoot();
                }
                else 
                {
                    Cooldown += 1;
                } 
            }
            else
            {
                animator.speed = 1.0f;
                Cooldown = 0f;
            }
            
        }
        
     }

    void Shoot()
    {
        GameObject bullet = Instantiate(BulletPrefab, BulletSpawn.position, BulletSpawn.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = BulletSpawn.forward * BulletSpeed;
    }

    void BarrelSpinFaster()
    {
        if(animator.speed < MaxSpinSpeed)
        {
            animator.speed += 0.1f;
        }
    }

}

