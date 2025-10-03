using System.Collections.Generic;
using UnityEngine;

public class Magazin : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int poolSize = 10;

    private List<GameObject> bullets;

    void Start()
    {
        // Initialize pool
        bullets = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            bullets.Add(obj);
        }
    }

    public GameObject GetBullet()
    {
        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        // Optional: expand pool if needed
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(true);
        bullets.Add(newBullet);
        return newBullet;
    }
}
