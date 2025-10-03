using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public NavMeshAgent agent;

    [Header("Settings")]
    public float chaseSpeed = 5f;

    private bool isActive = true; // Flag to control chasing

    void Start()
    {
        // Ensure NavMeshAgent is assigned
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
                enabled = false;
                return;
            }
        }

        // Validate player reference
        if (player == null)
        {
            Debug.LogError("Player Transform not assigned on " + gameObject.name);
            enabled = false;
            return;
        }

        // Set initial speed
        agent.speed = chaseSpeed;
    }

    void Update()
    {
        if (isActive)
        {
            agent.SetDestination(player.position);
        }
    }

    public void HandleHit()
    {
        // Stop chasing
        isActive = false;
        agent.isStopped = true; // Stop NavMeshAgent movement

        // Disable the zombie's Collider
        Collider zombieCollider = GetComponent<Collider>();
        if (zombieCollider != null)
        {
            zombieCollider.enabled = false;
        }
    }
}
