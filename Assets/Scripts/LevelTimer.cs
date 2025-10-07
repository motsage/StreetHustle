using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    public float timeRemaining = 60f;
    public bool timerIsRunning = false;
    public TextMeshProUGUI timerText;
    public float totalTime;

    void Start()
    {
        totalTime = timeRemaining;
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;

                FindFirstObjectByType<GameManagerScore>().GameOver();
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
