using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayScript : MonoBehaviour
{
    public ParticleSystem sprayEffect;
    public Collider sprayCollider;

    public Transform cameraPos;
    public Transform sprayPos;

    private Transform sprayDirection;
    [Header("References")]
    public SprayAnimation sprayAnimation;
    public Spawner spawner;
    public AudioSource _spraySound;
    public AudioClip _sprayEffect;
    public float invokeTIme = 2f;
    // Start is called before the first frame update
    void Start()
    {
        sprayDirection = cameraPos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            if (!sprayEffect.isPlaying)
            {
                Spray();
                cameraPos = sprayPos;
            }
        }
        else
        {
            if (sprayEffect.isPlaying)
            {
                StopSpray();
                cameraPos = sprayDirection;
            }
        }
    }
    void Spray()
    {
        sprayEffect.Play();
        sprayAnimation.PlaySprayAnim();
        Invoke("PlaySpraySound", invokeTIme);
        sprayCollider.enabled = true;
    }

    void StopSpray()
    {
        sprayEffect.Stop();
        sprayAnimation.PlayIdleAnim();
        sprayCollider.enabled = false;

    }

    void PlaySpraySound()
    {
       
        _spraySound.PlayOneShot(_sprayEffect);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy Hit");

            Animator anim = other.GetComponent<Animator>();
            var enemyScript = other.GetComponent<Zombie>();

            if (anim != null && spawner != null)
            {

                spawner.StartKillAndRespawn(other.gameObject, anim);
            }
        }
    }


}
