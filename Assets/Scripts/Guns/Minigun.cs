using UnityEngine;

public class Minigun : MonoBehaviour
{
    [Header("Minigun Settings")]
    public Transform shootingPosition;
    public GameObject bulletPrefab;

    [Header("Bullet Settings")]
    public float bulletSpeed = 20f;
    public float fireRate = 0.1f;
    private float _nextFireTime = 0f;

    public Animation idleAnim;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= _nextFireTime)
        {
            Shoot();
            _nextFireTime = Time.time + Mathf.Max(0.0001f, fireRate);
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootingPosition.position, shootingPosition.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        bulletRb.linearVelocity = shootingPosition.forward * bulletSpeed;
    }
}