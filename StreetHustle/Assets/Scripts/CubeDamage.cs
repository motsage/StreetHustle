using UnityEngine;
using TMPro;

public class CubeDamage : MonoBehaviour
{
    public int points = 100;              // Starting points
    public int collisionPenalty = 10;     // Points lost per collision
    public TextMeshProUGUI pointsText;    // UI display for points
    public GameObject[] stockCubes;       // Assign your 10 cubes here in Inspector

    private float stockTimer = 1f;        // 1 second countdown
    private bool isColliding = false;     // Tracks if we're colliding with a "Cube"
    private int currentStock;             // Tracks remaining stock cubes

    void Start()
    {
        currentStock = stockCubes.Length; // Start with all cubes active
        UpdateUI();
    }

    void Update()
    {
        // Continuous stock loss while colliding
        if (isColliding && currentStock > 0)
        {
            stockTimer -= Time.deltaTime;
            if (stockTimer <= 0f)
            {
                LoseStock();
                stockTimer = 1f;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            isColliding = true;

            // Immediate stock loss
            LoseStock();

            // Immediate points penalty
            points -= collisionPenalty;
            if (points < 0) points = 0;

            UpdateUI();

            // Reset timer for next loss
            stockTimer = 1f;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            isColliding = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            isColliding = false;
            stockTimer = 1f; // reset for next collision
        }
    }

    void LoseStock()
    {
        if (currentStock > 0)
        {
            currentStock--;
            stockCubes[currentStock].SetActive(false); // Hide one cube
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (pointsText != null) pointsText.text = "Points: " + points;
    }
}