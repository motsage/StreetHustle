using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayAnimation : MonoBehaviour
{

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PlaySprayAnim()
    {
        anim.Play("Charge");
    }

    public void PlayIdleAnim()
    {
        anim.Play("Charge@Standing Idle");
    }
}
