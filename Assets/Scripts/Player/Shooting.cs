using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("References")]
    public Ammo bulletPool; // Reference to Magazin script for bullet pooling
    public Transform firePoint; // Point where bullets spawn
    public Camera cam; // Main camera for aiming

    [Header("Settings")]
    public float bulletSpeed = 20f; // Initial speed of the bullet
    public float arcHeight = 2f; // Height of the arc
    public float bulletLifetime = 3f; // How long bullet remains active
    public float fireRate = 0.5f; // Time between shots

    private float nextFireTime;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Check for fire input (left mouse button) and fire rate
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            StartCoroutine(ShootCoroutine());
        }
    }

    private IEnumerator ShootCoroutine()
    {
        ShootBullet();
        nextFireTime = Time.time + fireRate;
        yield return null; // Ensure one frame delay
    }

   public void ShootBullet()
    {
        // Get bullet from Magazin pool
        GameObject bullet = bulletPool.GetBullet();
        if (bullet == null)
        {
            Debug.LogWarning("No inactive bullets available in Magazin pool!");
            return;
        }

        // Activate and position bullet at firePoint
        bullet.SetActive(true);
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.identity;

        // Get bullet Rigidbody
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = bullet.AddComponent<Rigidbody>();
        }

        // Reset Rigidbody properties
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Calculate direction (camera forward with slight upward arc)
        Vector3 shootDirection = cam.transform.forward;
        shootDirection.y += arcHeight * 0.1f; // Add upward component for arc
        shootDirection = shootDirection.normalized;

        // Apply velocity to bullet
        rb.linearVelocity = shootDirection * bulletSpeed;
        rb.useGravity = true; // Enable gravity for arc effect

        // Set bullet rotation to face direction
        bullet.transform.rotation = Quaternion.LookRotation(shootDirection);

        // Deactivate bullet after lifetime
        StartCoroutine(DeactivateBulletAfterTime(bullet, bulletLifetime));
    }

    private IEnumerator DeactivateBulletAfterTime(GameObject bullet, float time)
    {
        yield return new WaitForSeconds(time);
        if (bullet != null)
        {
            bullet.SetActive(false);
        }
    }
}
