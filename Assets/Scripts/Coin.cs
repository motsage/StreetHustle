using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddScore(1);

            Destroy(gameObject);
        }
    }
}
