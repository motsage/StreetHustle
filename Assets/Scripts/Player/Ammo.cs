using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AmmoType
{
    public string name;
    public GameObject bulletPrefab;
    public int poolSize = 5;
}

public class Ammo : MonoBehaviour
{
    [Header("Ammo Types")]
    public List<AmmoType> ammoTypes;

    private Dictionary<string, List<GameObject>> ammoPools;

    void Start()
    {
        ammoPools = new Dictionary<string, List<GameObject>>();

        foreach (AmmoType type in ammoTypes)
        {
            if (type == null)
            {
                Debug.LogWarning("AmmoType is null in ammoTypes list.");
                continue;
            }

            if (type.bulletPrefab == null)
            {
                Debug.LogWarning($"Bullet prefab not assigned for ammo type: {type.name}");
                continue;
            }

            List<GameObject> pool = new List<GameObject>();
            for (int i = 0; i < type.poolSize; i++)
            {
                GameObject obj = Instantiate(type.bulletPrefab);
                obj.SetActive(false);
                pool.Add(obj);
            }
            ammoPools[type.name] = pool;
        }
    }

    /// <summary>
    /// Always returns a random ammo type
    /// </summary>
    public GameObject GetBullet()
    {
        if (ammoTypes.Count == 0) return null;

        int randomIndex = Random.Range(0, ammoTypes.Count);
        string ammoName = ammoTypes[randomIndex].name;

        return GetBulletFromPool(ammoName);
    }

    private GameObject GetBulletFromPool(string ammoName)
    {
        List<GameObject> pool = ammoPools[ammoName];

        foreach (GameObject bullet in pool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        // expand pool if all bullets are used
        AmmoType type = ammoTypes.Find(t => t.name == ammoName);
        GameObject newBullet = Instantiate(type.bulletPrefab);
        newBullet.SetActive(true);
        pool.Add(newBullet);
        return newBullet;
    }
}
