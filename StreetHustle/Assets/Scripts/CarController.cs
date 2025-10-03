using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] waypoints;
    public int currentWaypointIndex = 0;

    [Header("Movement Settings")]
    public float maxSpeed = 5f;
    public float minSpeed = 2f;
    public float rotationSpeed = 5f;
    public float waypointThreshold = 0.5f;
    public float slowDownDistance = 3f;

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypointIndex];
        Vector3 direction = target.position - transform.position;
        float distanceToWaypoint = direction.magnitude;

        // Adjust speed based on proximity to waypoint
        float currentSpeed = maxSpeed;
        if (distanceToWaypoint < slowDownDistance)
        {
            currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, distanceToWaypoint / slowDownDistance);
        }

        // Move towards the waypoint
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            currentSpeed * Time.deltaTime
        );

        // Rotate only on Y-axis (stay upright)
        Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z);
        if (flatDirection.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(flatDirection, Vector3.up);

            // Adjust model orientation if needed
            Quaternion modelOffset = Quaternion.Euler(-90f, 0f, 0f); // Modify if your model faces differently
            Quaternion targetRotation = lookRotation * modelOffset;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Switch to next waypoint if close enough
        if (distanceToWaypoint < waypointThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}