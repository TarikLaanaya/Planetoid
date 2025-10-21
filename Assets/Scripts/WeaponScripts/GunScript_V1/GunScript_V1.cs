using System.Collections;
using UnityEngine;

public class GunScript_V1 : MonoBehaviour
{
    public Camera playerCamera;

    //shooting
    public bool isShooting, isReadyToShoot;
    public float shootingDelay = 0.01f; // Adjusted for faster firing rate

    //spread
    public float spreadIntensity = 0.1f;

    //hitscan
    public float hitscanRange = 100f;
    [SerializeField] private LayerMask hitscanLayers;
    public Transform hitscanBulletSpawn;

    public GameObject bulletPrefab;
    public GameObject muzzleFlash;
    public Transform bulletSpawn;
    public Transform bulletSpawn2;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    private void Awake()
    {
        isReadyToShoot = true;
    }

    void Update()
    {
        isShooting = Input.GetKey(KeyCode.Mouse0);

        if (isShooting && isReadyToShoot)
        {
            StartCoroutine(FireWeapon());
        }
    }

    private IEnumerator FireWeapon()
    {
        isReadyToShoot = false;

        FireWeaponProjectile();
        muzzleShoot();

        yield return new WaitForSeconds(shootingDelay);
        isReadyToShoot = true;

        if (isShooting)
        {
            StartCoroutine(FireWeapon());
        }
    }

    private void muzzleShoot()
    {
        GameObject Flash = Instantiate(muzzleFlash, bulletSpawn.position, bulletSpawn.rotation);
        GameObject Flash2 = Instantiate(muzzleFlash, bulletSpawn2.position, bulletSpawn2.rotation);
        Destroy(Flash2, 0.12f);
        Destroy(Flash, 0.12f);
    }

    private void FireWeaponProjectile()
    {
        Vector3 spreadDirection1 = GetSpreadDirection(bulletSpawn);
        Vector3 spreadDirection2 = GetSpreadDirection(bulletSpawn2);

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.LookRotation(spreadDirection1));
        GameObject bullet2 = Instantiate(bulletPrefab, bulletSpawn2.position, Quaternion.LookRotation(spreadDirection2));

        bullet.GetComponent<Rigidbody>().AddForce(spreadDirection1 * bulletVelocity, ForceMode.Impulse);
        bullet2.GetComponent<Rigidbody>().AddForce(spreadDirection2 * bulletVelocity, ForceMode.Impulse);

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));
        StartCoroutine(DestroyBulletAfterTime(bullet2, bulletPrefabLifeTime));
    }

    private Vector3 GetSpreadDirection(Transform spawnPoint)
    {
        Vector3 direction = spawnPoint.forward;
        direction.x += Random.Range(-spreadIntensity, spreadIntensity);
        direction.y += Random.Range(-spreadIntensity, spreadIntensity);
        direction.z += Random.Range(-spreadIntensity, spreadIntensity);
        return direction.normalized;
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

    private void FireWeaponHitscan()
    {
        if (Physics.Raycast(hitscanBulletSpawn.position, hitscanBulletSpawn.forward, out RaycastHit hit, hitscanLayers))
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(hitscanBulletSpawn.position, hitscanBulletSpawn.position + hitscanBulletSpawn.forward * hitscanRange);
    }
}
