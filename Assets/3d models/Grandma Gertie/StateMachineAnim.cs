using UnityEngine;
using System.Collections;

public class StateMachineAnim : MonoBehaviour
{

    public Animator animator;
    public int numberOfAttacks = 3;

    void Update()
    {
    
            animator.SetTrigger("StartCombat");

            // Pick a random attack index
            int attackIndex = Random.Range(0, numberOfAttacks);

            // Set the AttackIndex to select Attack1,2,3
            animator.SetInteger("int", attackIndex);
        
    }
}
