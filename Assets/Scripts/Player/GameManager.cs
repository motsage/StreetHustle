using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI timerText;
    private int score = 0;
    public Text scoreText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        UpdateScoreUI();
    }

    // Adds points to the score and updates the UI
    public void AddScore(int points)
    {
        score += points;
        Debug.Log($"Score increased by {points}. Total Score: {score}");
        UpdateScoreUI();
       
    }

    // Returns the current score
    public int GetScore()
    {
        return score;
    }

    // Updates the UI text to display the current score
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
            timerText.text = scoreText.text;
        }
    }
}