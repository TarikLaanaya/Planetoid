using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GunScript_V2 : MonoBehaviour
{
    [SerializeField]
    private bool AddBulletSpread = true;
    [SerializeField]
    private float muzzleFlashDuration;
    [SerializeField]
    private Vector3 BulletSpreadVariance = new() { x = 0.1f, y = 0.1f, z = 0.1f };
    [SerializeField]
    private ParticleSystem ShootingSystem;
    [SerializeField]
    private Transform BulletSpawnPoint1;
    [SerializeField]
    private Transform BulletSpawnPoint2;
    [SerializeField]
    private ParticleSystem ImpactParticleSystem;
    [SerializeField]
    private TrailRenderer BulletTrail;
    [SerializeField]
    private float ShootDelay = 0.5f;
    [SerializeField]
    private LayerMask Mask;
    [SerializeField]
    private float BulletSpeed = 100;
    public float bulletDamage = 2;
    public int ammocount = 300;
    public bool CanShoot = true;
    public TMP_Text ammoUI;

    public int damage = 2;

    private Animator Animator;
    private float LastShootTime;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (ammocount > 0 && CanShoot)
            {
                Shoot();
            }
        }

        if (ammoUI != null)
            ammoUI.text = "Ammo: " + ammocount;

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    public void Shoot()
    {
        if (LastShootTime + ShootDelay >= Time.time) return;

        // Play shooting VFX/sound once per shot (both barrels)
        if (ShootingSystem != null)
            ShootingSystem.Play();

        // Left gun
        var muzzleFlashInstance1 = ShootingSystem != null
            ? Instantiate(ShootingSystem, BulletSpawnPoint1.position, BulletSpawnPoint1.rotation * Quaternion.Euler(0, 180, 0))
            : null;
        if (muzzleFlashInstance1 != null)
        {
            muzzleFlashInstance1.transform.SetParent(BulletSpawnPoint1);
            muzzleFlashInstance1.transform.localPosition = Vector3.zero;
        }

        Vector3 direction1 = GetDirection(BulletSpawnPoint1);
        PerformRaycastAndTrail(BulletSpawnPoint1, direction1, muzzleFlashInstance1);

        ammocount = Mathf.Max(0, ammocount - 1);

        // Right gun
        var muzzleFlashInstance2 = ShootingSystem != null
            ? Instantiate(ShootingSystem, BulletSpawnPoint2.position, BulletSpawnPoint2.rotation * Quaternion.Euler(0, 180, 0))
            : null;
        if (muzzleFlashInstance2 != null)
        {
            muzzleFlashInstance2.transform.SetParent(BulletSpawnPoint2);
            muzzleFlashInstance2.transform.localPosition = Vector3.zero;
        }

        Vector3 direction2 = GetDirection(BulletSpawnPoint2);
        PerformRaycastAndTrail(BulletSpawnPoint2, direction2, muzzleFlashInstance2);

        ammocount = Mathf.Max(0, ammocount - 1);

        // Start coroutines to destroy muzzle flashes
        StartCoroutine(DestroyMuzzleFlash(muzzleFlashInstance1, muzzleFlashInstance2));

        LastShootTime = Time.time;
    }

    // Shared helper that performs the raycast, spawns trail and handles hit.
    private void PerformRaycastAndTrail(Transform spawnPoint, Vector3 direction, ParticleSystem muzzleFlashInstance)
    {
        if (Physics.Raycast(spawnPoint.position, direction, out RaycastHit hit, Mathf.Infinity, Mask, QueryTriggerInteraction.Ignore))
        {
            if (BulletTrail != null)
            {
                var trail = Instantiate(BulletTrail, spawnPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));
            }

            HandleHit(hit);
        }
        else
        {
            if (BulletTrail != null)
            {
                var trail = Instantiate(BulletTrail, spawnPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, spawnPoint.position + direction * 100f, Vector3.zero, false));
            }
        }
    }

    private IEnumerator Reload()
    {
        CanShoot = false;
        yield return new WaitForSeconds(2);
        ammocount = 300;
        CanShoot = true;
    }

    private IEnumerator DestroyMuzzleFlash(ParticleSystem muzzleFlashInstance1, ParticleSystem muzzleFlashInstance2)
    {
        yield return new WaitForSeconds(muzzleFlashDuration);

        if (muzzleFlashInstance1 != null)
            Destroy(muzzleFlashInstance1.gameObject);

        if (muzzleFlashInstance2 != null)
            Destroy(muzzleFlashInstance2.gameObject);
    }

    private void HandleHit(RaycastHit hit)
    {
        // Accept either tag "Enemy" or "Enemies" (supports both project conventions)
        if (!hit.collider.CompareTag("Enemy") && !hit.collider.CompareTag("Enemies"))
            return;

        var penguin = hit.collider.GetComponent<PenguinEnemy>() ??
                      hit.collider.GetComponentInParent<PenguinEnemy>();

        if (penguin != null)
            penguin.TakeDamage(damage);
    }

    private Vector3 GetDirection(Transform spawnPoint)
    {
        Vector3 direction = spawnPoint.forward;

        if (AddBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
                Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
                Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)
            );

            direction.Normalize();
        }

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact)
    {
        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));
            remainingDistance -= BulletSpeed * Time.deltaTime;
            yield return null;
        }

        Trail.transform.position = HitPoint;
        if (MadeImpact && ImpactParticleSystem != null)
        {
            Instantiate(ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
        }

        Destroy(Trail.gameObject, Trail.time);
    }
}
