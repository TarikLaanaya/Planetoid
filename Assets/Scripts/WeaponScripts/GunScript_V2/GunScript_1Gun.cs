using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GunScript_1Gun : MonoBehaviour
{
    #region Variables
    [SerializeField] private bool AddBulletSpread = true;
    [SerializeField] public float muzzleFlashDuration = 0.05f;
    [SerializeField] private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private ParticleSystem ShootingSystem;
    [SerializeField] private Transform BulletSpawnPoint;
    [SerializeField] private ParticleSystem ImpactParticleSystem;
    [SerializeField] private TrailRenderer BulletTrail;
    [SerializeField] private float ShootDelay = 0.01f;
    [SerializeField] private float chargeupDelay = 0.5f;
    [SerializeField] private LayerMask Mask; // Must include enemies
    [SerializeField] private float BulletSpeed = 100;
    [SerializeField] private ParticleSystem MuzzleFlash;

    [Header("Damage")]
    public int damage = 1; // Each shot deals 1 damage

    public Animator Animator;
    private float LastShootTime;
    private Coroutine shootCoroutine;
    public bool CanShoot = true;
    public bool IsShooting = false;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Animator.SetBool("IsShooting", false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (CanShoot)
            {
                Animator.SetBool("IsShooting", true);
                shootCoroutine = StartCoroutine(ShootWithDelay());
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (shootCoroutine != null)
            {
                StopCoroutine(shootCoroutine);
                shootCoroutine = null;
            }
            Animator.SetBool("IsShooting", false);
        }
    }
    #endregion

    #region Shooting Logic
    private IEnumerator ShootWithDelay()
    {
        yield return new WaitForSeconds(chargeupDelay);
        while (Input.GetMouseButton(0))
        {
            Shoot();
            yield return new WaitForSeconds(ShootDelay);
        }
    }

    public void Shoot()
    {
        if (LastShootTime + ShootDelay >= Time.time) return;

        ShootingSystem.Play();

        var muzzleFlashInstance = Instantiate(MuzzleFlash, BulletSpawnPoint.position,
            BulletSpawnPoint.rotation * Quaternion.Euler(0, 180, 0));
        muzzleFlashInstance.transform.SetParent(BulletSpawnPoint);
        muzzleFlashInstance.transform.localPosition = Vector3.zero;
        StartCoroutine(DestroyMuzzleFlash(muzzleFlashInstance));

        Vector3 direction = GetDirection();

        if (Physics.Raycast(BulletSpawnPoint.position, direction, out RaycastHit hit, Mathf.Infinity, Mask,
                QueryTriggerInteraction.Ignore))
        {
            var trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));

            HandleHit(hit);
            LastShootTime = Time.time;
        }
        else
        {
            var trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, BulletSpawnPoint.position + direction * 100f, Vector3.zero, false));
            LastShootTime = Time.time;
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = BulletSpawnPoint.forward;

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
        float distance = Vector3.Distance(startPosition, HitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0f)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));
            remainingDistance -= BulletSpeed * Time.deltaTime;
            yield return null;
        }

        Trail.transform.position = HitPoint;

        if (MadeImpact)
        {
            Instantiate(ImpactParticleSystem, HitPoint,
                HitNormal == Vector3.zero ? Quaternion.identity : Quaternion.LookRotation(HitNormal));
        }

        Destroy(Trail.gameObject, Trail.time);
    }

    private IEnumerator DestroyMuzzleFlash(ParticleSystem muzzleFlashInstance)
    {
        yield return new WaitForSeconds(muzzleFlashDuration);
        if (muzzleFlashInstance != null)
            Destroy(muzzleFlashInstance.gameObject);
    }
    #endregion

    #region Hitting Enemies
    private void HandleHit(RaycastHit hit)
    {
        if (!hit.collider.CompareTag("Enemy")) return;

        // Try on the collider first, then its parents (in case collider is child object)
        var penguin = hit.collider.GetComponent<PenguinEnemy>() ??
                      hit.collider.GetComponentInParent<PenguinEnemy>();

        if (penguin != null)
        {
            penguin.TakeDamage(damage);
        }
    }
    #endregion
}
