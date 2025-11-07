using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

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
    public float ChargeTimer = 200f;
    public float BulletSize = 1f;

    Animator animator;
    public float DefaultSpinSpeed = 1.0f;
    public float MaxSpinSpeed = 10.0f;

    public bool IsGun2;

    [Header ("Audio")]
    [SerializeField] private AudioClip gatlingClip;
    [SerializeField] private AudioClip chargingClip;
    [SerializeField] private AudioClip chargeShotClip;
    [SerializeField] private AudioSource GatlingAudio;
    [SerializeField] private AudioSource ChargingAudio;
    [SerializeField] private AudioSource ChargeShotAudio;
    [SerializeField] private float pitchVariance = 0.5f;

    private void Start()
    {
        animator = GetComponent<Animator>();

        delayTime = delay;

        GatlingAudio.enabled = true;
        ChargingAudio.enabled = true;
        ChargeShotAudio.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChargeShot = false;
            Debug.Log("Gatling Mode");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChargeShot = true;
            Debug.Log("Charge Shot Mode");
        }

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
                if (!ChargingAudio.isPlaying)
                {
                    ChargingAudio.PlayOneShot(chargingClip);
                }
                BarrelSpinFaster();
                if (Cooldown >= ChargeTimer)
                {
                    float BulletScale = 1f * (Cooldown / ChargeTimer); 
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
                ChargingAudio.Stop();
                if (Cooldown > 0f && Cooldown < ChargeTimer)
                {
                    float BulletScale = 1f * (Cooldown / ChargeTimer);
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

        float randomPitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);
        float randomVolume = Random.Range(0.4f, 6f);
        GatlingAudio.pitch = randomPitch;
        GatlingAudio.volume = randomVolume;
        GatlingAudio.PlayOneShot(gatlingClip);

    }


    void ChargeShoot(float BulletScale)
    {
        GameObject bullet = Instantiate(BulletPrefab, BulletSpawn.position, BulletSpawn.rotation);
        bullet.transform.localScale = 1f * Vector3.one;
        bullet.transform.localScale *= BulletScale;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.GetComponent<PlanetGravitySim>().planetTransform = planetTransform;
        rb.linearVelocity = BulletSpawn.forward * BulletSpeed;
        bullet.tag = "ChargeBullet";
        Destroy(bullet, 6.7f);
        ChargingAudio.Stop();
        ChargeShotAudio.PlayOneShot(chargeShotClip);

    }

    void BarrelSpinFaster()
    {
        if(animator.speed < MaxSpinSpeed)
        {
            animator.speed += 0.1f;
        }
    }
}

