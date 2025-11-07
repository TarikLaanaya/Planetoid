using Unity.VisualScripting;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [Header ("Planet Transform")]
    [SerializeField]
    private Transform planetTransform;

    [SerializeField]
    private Transform BulletSpawn;
    [SerializeField]
    private GameObject BulletPrefab;

    public float BulletSpeed = 20f;
    public float TimeBetweenShots = 0.5f;
    private float Cooldown = 0f;
    [SerializeField] private float delay = 0f;
    private float delayTime = 0f;
    bool alreadyShot = false;

    public bool ChargeShot;
    public float ChargeTimer = 60f;
    public float BulletSize = 1f;

    Animator animator;
    public float DefaultSpinSpeed = 1.0f;
    public float MaxSpinSpeed = 10.0f;

    public bool IsGun2;

    private void Start()
    {
        animator = GetComponent<Animator>();

        delayTime = delay;
    }

    void Update()
    {
        if (!ChargeShot)    // Gatling shooting mode
        {
            Cooldown -= Time.deltaTime;
            delayTime -= Time.deltaTime;
            if (Input.GetKey(KeyCode.Space))
            {
                BarrelSpinFaster();
                if (Cooldown <= 0f && delayTime <= 0f)
                {
                    Shoot();
                    Cooldown = TimeBetweenShots;

                    if(alreadyShot) delayTime = 0f;
                    else delayTime = delay;

                }
                alreadyShot = true;
            }
            else
            {
                animator.speed = 1.0f;
            }
        }
        if (ChargeShot)     // Charge shot mode
        {
            if (Input.GetKey(KeyCode.Space))
            {
                BarrelSpinFaster();
                if (Cooldown >= ChargeTimer)
                {
                    float BulletScale = 2f;
                    Cooldown = 0f;
                    ChargeShoot(BulletScale);
                }
                else 
                {
                    Cooldown += 1;
                } 
            }
            else
            {
                if (Cooldown > 0f && Cooldown < ChargeTimer)
                {
                    float BulletScale = 1f + (Cooldown / ChargeTimer);
                    ChargeShoot(BulletScale);
                }
                animator.speed = 1.0f;
                Cooldown = 0f;
            }
            
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            alreadyShot = false;
        }

    }

    void Shoot()
    {
        GameObject bullet = Instantiate(BulletPrefab, BulletSpawn.position, BulletSpawn.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.GetComponent<PlanetGravitySim>().planetTransform = planetTransform;
        rb.linearVelocity = BulletSpawn.forward * BulletSpeed;
        Destroy(bullet, 2f);
    }

    void ChargeShoot(float BulletScale)
    {
        GameObject bullet = Instantiate(BulletPrefab, BulletSpawn.position, BulletSpawn.rotation);
        bullet.transform.localScale *= BulletScale;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = BulletSpawn.forward * BulletSpeed;
        Destroy(bullet, 2f);
    }

    void BarrelSpinFaster()
    {
        if(animator.speed < MaxSpinSpeed)
        {
            animator.speed += 0.1f;
        }
    }
}

