using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class GunScript : MonoBehaviour
{
    [SerializeField] private PlayerVerticalAim playerVerticalAim;
    [SerializeField] private Camera playerCam;

    [Header ("Planet Transform")]
    [SerializeField]
    private Transform planetTransform;

    [SerializeField]
    private Transform BulletSpawn;
    [SerializeField]
    private GameObject BulletPrefab;
    [SerializeField] private GameObject chargeBulletPrefab;

    public float BulletSpeed = 20f;
    public float TimeBetweenShots = 0.5f;
    private float Cooldown = 0f;
    [SerializeField] private float delay = 0f;
    private float delayTime = 0f;
    bool alreadyShot = false;

    public bool ChargeShot;
    public float ChargeTimer = 200f;
    public float BulletSize = 1f;
    private bool chargeFired = false;

    Animator animator;
    public float DefaultSpinSpeed = 1.0f;
    public float MaxSpinSpeed = 10.0f;

    public bool IsGun2;

    [Header ("Audio")]
    [SerializeField] private AudioClip gatlingClip;
    [SerializeField] private AudioClip chargingClip;
    [SerializeField] private AudioClip chargeShotClip;
    [SerializeField] private AudioClip chargeFailClip;
    [SerializeField] private AudioSource gunAudioSource;
    [SerializeField] private float pitchVariance = 0.5f;

    private void Start()
    {
        animator = GetComponent<Animator>();

        delayTime = delay;
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

        //Allows the player to switch between weapons using 1 and 2.

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
            if (Input.GetKey(KeyCode.Space) && !chargeFired)
            {
                if (gunAudioSource.clip != chargingClip)
                {
                    gunAudioSource.pitch = 1.2f;
                    gunAudioSource.volume = .4f;

                    gunAudioSource.clip = chargingClip;
                    gunAudioSource.Play();
                }

                BarrelSpinFaster();
                
                if (Cooldown >= ChargeTimer)
                {
                    chargeFired = true;
                    float BulletScale = 1f * (Cooldown / ChargeTimer); 
                    Cooldown = 0f;
                    ChargeShoot(BulletScale);
                }
                else 
                {
                    Cooldown += Time.deltaTime * 100;
                }
            }
            else
            {
                if (Cooldown > 0f && Cooldown < ChargeTimer)
                {
                    float BulletScale = 1f * (Cooldown / ChargeTimer);
                    if (BulletScale > .5f) ChargeShoot(BulletScale);
                    else
                    {
                        float timeStamp = gunAudioSource.time;
                        gunAudioSource.Stop();
                        gunAudioSource.clip = null;
                        gunAudioSource.clip = chargeFailClip;
                        gunAudioSource.pitch = -1.2f;
                        gunAudioSource.volume = .4f;
                        gunAudioSource.time = timeStamp;
                        gunAudioSource.Play();
                    }
                }
                animator.speed = 1.0f;
                Cooldown = 0f;
            }
            
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            alreadyShot = false;
            delayTime = delay;
            chargeFired = false;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(BulletPrefab, BulletSpawn.position, BulletSpawn.rotation);

        Vector3 dir = GetShootDir();

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = dir * BulletSpeed;
        Destroy(bullet, 2f);

        // Audio
        float randomPitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);
        float randomVolume = Random.Range(0.4f, 1f);
        gunAudioSource.pitch = randomPitch;
        gunAudioSource.volume = randomVolume;
        gunAudioSource.PlayOneShot(gatlingClip);

    }


    void ChargeShoot(float BulletScale)
    {
        GameObject bullet = Instantiate(chargeBulletPrefab, BulletSpawn.position, BulletSpawn.rotation);

        Vector3 dir = GetShootDir();

        bullet.transform.localScale = 1f * Vector3.one;
        bullet.transform.localScale *= BulletScale;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = dir * BulletSpeed;
        bullet.tag = "ChargeBullet";
        bullet.GetComponent<Bullet>().damage = Mathf.Lerp(0f, 25f, BulletScale);
        Destroy(bullet, 6.7f);

        gunAudioSource.Stop();
        gunAudioSource.clip = null;
        gunAudioSource.volume = Mathf.Clamp(BulletScale, .3f, .5f);
        gunAudioSource.PlayOneShot(chargeShotClip);
    }

    void BarrelSpinFaster()
    {
        if (animator.speed < MaxSpinSpeed)
        {
            animator.speed += 0.1f;
        }
    }
    
    Vector3 GetShootDir()
    {
        // --- Vertical Aim Logic --- ///

        // Multiply the position of the crosshair by the desired sensitivity
        //float pitch = playerVerticalAim.crosshairTransform.anchoredPosition.y * shotPitchSens;

        // Rotate pitch amount around the "X axis" and then multiply by forward to get the desired direction
        //Vector3 dir = Quaternion.AngleAxis(-pitch, playerCam.transform.right) * playerCam.transform.forward;


        Vector3 crosshairScreenPosition = playerCam.WorldToScreenPoint(playerVerticalAim.crosshairTransform.position);

        Ray ray = playerCam.ScreenPointToRay(crosshairScreenPosition);
        RaycastHit hit;
        Vector3 rayTargetPoint;

        if (Physics.Raycast(ray, out hit, 999f))
        {
            if (hit.collider.tag == "Enemy")
            {
                rayTargetPoint = hit.point;
            }
            else
            {
                rayTargetPoint = ray.GetPoint(999f);
            }
        }
        else
        {
            rayTargetPoint = ray.GetPoint(999f);
        }

        Vector3 dir = (rayTargetPoint - BulletSpawn.position).normalized;

        return dir;
            
        // -------------------------- ///
    }
}