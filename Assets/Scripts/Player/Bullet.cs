using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    public int pointsOnHit = 1; // Points awarded when hitting a Customer
    
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the "Customer" tag
        if (collision.gameObject.CompareTag("Customer"))
        {
            Debug.Log("Collided with Customer");
            // Add points to the score
            GameManager.Instance.AddScore(pointsOnHit);

            AIWalking aIWalking = collision.gameObject.GetComponent<AIWalking>();

            aIWalking.ReceivedProduct();

            // Deactivate bullet on hit
            gameObject.SetActive(false);
        }
    }
}
