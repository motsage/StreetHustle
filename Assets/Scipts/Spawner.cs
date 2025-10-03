using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public float respawnDelay = 3f;
    public float deathAnimLength = 2f;

    public void StartKillAndRespawn(GameObject zombie, Animator anim)
    {
        StartCoroutine(KillAndRespawn(zombie, anim));
    }

    private IEnumerator KillAndRespawn(GameObject zombie, Animator anim)
    {
        var enemyScript = zombie.GetComponent<Zombie>();
        

        anim.SetTrigger("DeathTrig");
        Debug.Log("Death animation triggered for " + zombie.name);

        yield return new WaitForSeconds(deathAnimLength);

        zombie.SetActive(false);

        yield return new WaitForSeconds(respawnDelay);

        int randomIndex = Random.Range(0, spawnPoints.Length);
        zombie.transform.position = spawnPoints[randomIndex].position;

        
        zombie.SetActive(true);
    }
}
