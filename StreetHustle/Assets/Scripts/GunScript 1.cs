using UnityEngine;
using System.Collections;
//using OVR.OpenVR;

public class GunScript : MonoBehaviour
{
    public Magazin bulletPool;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float fireRate = 0.5f;

    private float nextFireTime = 2f;
    public bool attack = false;

    // public AudioSource GunSound; // Commented out

    // [SerializeField] private ParticleSystem _shoot; // Commented out

    public bool playerShooterl = false;

    void Update()
    {
        if (Time.timeScale == 0f) return;

        if (Input.GetMouseButtonDown(0) && playerShooterl)
        {
            Shoot();
            // StartCoroutine(PlayGunshot()); // Commented out
            Debug.Log("Shooting");
        }
        else if (attack && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
        else
        {
            // _shoot.Stop(); // Commented out
        }
    }

    void Shoot()
    {
        GameObject bullet = bulletPool.GetBullet();
        bullet.transform.position = firePoint.position;

        Vector3 shootDirection;
        if (playerShooterl)
        {
            Vector3 targetPoint = GetTargetPoint();
            shootDirection = (targetPoint - firePoint.position).normalized;
        }
        else
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector3 targetPoint = player.transform.position;
                shootDirection = (targetPoint - firePoint.position).normalized;
            }
            else
            {
                shootDirection = firePoint.forward;
            }
        }

        bullet.transform.rotation = Quaternion.LookRotation(shootDirection);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = shootDirection * bulletSpeed;

        // _shoot.Play(); // Commented out

        // deactivate bullet after 3 seconds
        StartCoroutine(DeactivateBulletAfterTime(bullet, 3f));
    }

    Vector3 GetTargetPoint()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        return Physics.Raycast(ray, out RaycastHit hit, 1000f)
            ? hit.point
            : ray.GetPoint(1000f);
    }

    IEnumerator DeactivateBulletAfterTime(GameObject bullet, float time)
    {
        yield return new WaitForSeconds(time);
        bullet.SetActive(false);
    }

    // IEnumerator PlayGunshot()
    // {
    //     GunSound.PlayOneShot(GunSound.clip);
    //     yield return new WaitForSeconds(0.3f);
    //     GunSound.Stop();
    // }
}
