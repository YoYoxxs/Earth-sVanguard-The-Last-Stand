using UnityEngine;

public class SpaceshipShooting : MonoBehaviour
{
    public GameObject bulletPrefab;  // Assign the sphere prefab in the Inspector
    public Transform firePoint;      // The position from where bullets spawn
    public float bulletSpeed = 10f;
    public float fireRate = 0.2f;

    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = transform.up * bulletSpeed; // Moves upwards
         
        }
    }
}
