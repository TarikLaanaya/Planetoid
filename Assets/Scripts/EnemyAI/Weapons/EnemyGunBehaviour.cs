using UnityEngine;

public class EnemyGunBehaviour : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private float distanceBeforeShoot = 20f;
    [SerializeField] private Transform BulletSpawn;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private float BulletSpeed = 20f;
    [SerializeField] private float TimeBetweenShots = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioClip gatlingClip;
    private AudioSource audioSource;
    private BasicEnemyBrain basicEnemyBrain;
    private float shootCooldown;

    void Start()
    {
        basicEnemyBrain = GetComponent<BasicEnemyBrain>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (basicEnemyBrain.currentState == BasicEnemyBrain.EnemyState.Attack)
        {
            if (Vector3.Distance(transform.position, basicEnemyBrain.playerRootTransform.position) < distanceBeforeShoot)
            {
                shootCooldown -= Time.deltaTime;

                if (shootCooldown <= 0f)
                {
                    Shoot();
                    shootCooldown = TimeBetweenShots;
                }
            }
        }

        void Shoot()
        {
            GameObject bullet = Instantiate(BulletPrefab, BulletSpawn.position, BulletSpawn.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            bullet.GetComponent<PlanetGravitySim>().planetTransform = basicEnemyBrain.planetTransform;
            rb.linearVelocity = BulletSpawn.forward * BulletSpeed;
            Destroy(bullet, 2f);

            // Audio
            float randomPitch = Random.Range(.5f, 1f);
            float randomVolume = Random.Range(0.4f, .7f);
            audioSource.pitch = randomPitch;
            audioSource.volume = randomVolume;
            audioSource.PlayOneShot(gatlingClip);

        }
    }
}