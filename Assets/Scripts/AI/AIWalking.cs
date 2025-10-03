using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIWalking : MonoBehaviour
{
    [Header("References")]
    public Transform pointA;
    public Transform pointB;
    public NavMeshAgent agent;
    public ParticleSystem coins;
    public Transform coinSpawnPoint;
    public GameObject replacementObject;
    public GameObject noMoney;

    [Header("Settings")]
    public float waitTime = 3f;
    public float stoppingDistance = 0.5f;
    public float timerDuration = 10f;

    private Transform currentTarget;
    private bool isWaiting;
    private float waitTimer;
    private float productTimer;
    private bool hasReceivedProduct;

    [Header("Audio")]
    private AudioSource _impact;


    void OnEnable()
    {
        productTimer = timerDuration;
        hasReceivedProduct = false;
    }

    void Start()
    {

        _impact = GetComponent<AudioSource>();
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

        if (pointA == null || pointB == null)
        {
            Debug.LogError("PointA or PointB not assigned on " + gameObject.name);
            enabled = false;
            return;
        }

        if (coins == null)
        {
            Debug.LogError("Coin ParticleSystem not assigned on " + gameObject.name);
        }
        if (coinSpawnPoint == null)
        {
            Debug.LogError("Coin spawn point Transform not assigned on " + gameObject.name);
        }

        if (replacementObject == null)
        {
            Debug.LogError("Replacement GameObject not assigned on " + gameObject.name);
        }

        currentTarget = pointB;
        agent.SetDestination(currentTarget.position);
        isWaiting = false;
    }

    void Update()
    {
        if (!hasReceivedProduct)
        {
            productTimer -= Time.deltaTime;
            if (productTimer <= 0f)
            {
                SpawnReplacementAndDisable();
                return;
            }
        }

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                SwitchTarget();
            }
            return;
        }

        if (currentTarget != null && agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            SwitchTarget();
        }
    }

    // Stops the AI at a waypoint for the specified wait time
    void StartWaiting()
    {
        isWaiting = true;
        waitTimer = waitTime;
        agent.isStopped = true;
    }

    // Switches the AI's target to the other waypoint
    void SwitchTarget()
    {
        currentTarget = (currentTarget == pointA) ? pointB : pointA;
        agent.SetDestination(currentTarget.position);
        agent.isStopped = false;
    }

    // Handles the AI receiving a product (hit by bullet)
    public void ReceivedProduct()
    {
        Debug.Log("ReceivedProduct");

        hasReceivedProduct = true;
        isWaiting = false;
        // agent.isStopped = true;
        _impact.Play();
        if (coins != null && coinSpawnPoint != null)
        {
            Debug.Log("Coins and spawn point not null");
            coins.transform.position = coinSpawnPoint.position;
            coins.transform.rotation = Quaternion.Euler(-90f, 0f, 0f); // Ensures 3D objects emit upward
            coins.Play();
            noMoney.SetActive(true);
            StartCoroutine(StopParticleSystem(coins));
        }

        Collider aiCollider = GetComponent<Collider>();
        if (aiCollider != null)
        {
            aiCollider.enabled = false;
        }
    }

    // Spawns a replacement GameObject and disables the Customer
    private void SpawnReplacementAndDisable()
    {
        if (replacementObject != null)
        {
            replacementObject.SetActive(true);
            replacementObject.transform.position = transform.position;
        }
        gameObject.SetActive(false);
    }

    // Stops the particle system after one burst, allowing 3D objects to fade
    private System.Collections.IEnumerator StopParticleSystem(ParticleSystem particleSystem)
    {
        float burstDuration = particleSystem.main.startLifetime.constantMax;
        yield return new WaitForSeconds(burstDuration);
        particleSystem.Stop();
    }
}