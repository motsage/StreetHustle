using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManagerScore : MonoBehaviour
{
    public StarManager starManager;
    public GameObject winPanel;
    public GameObject gameOverPanel;
    public LevelTimer timer;
    public TextMeshProUGUI timeTakenText;

    public void Win()
    {
        winPanel.SetActive(true);

        timer.timerIsRunning = false;

        float timeRemaining = timer.timeRemaining;
        int starsEarned = 0;

        if (timeRemaining > 30) starsEarned = 3;
        else if (timeRemaining > 15) starsEarned = 2;
        else if (timeRemaining > 0) starsEarned = 1;

        starManager.ShowStars(starsEarned);

        float timeTaken = timer.totalTime - timeRemaining;
        int minutes = Mathf.FloorToInt(timeTaken / 60);
        int seconds = Mathf.FloorToInt(timeTaken % 60);

        timeTakenText.text = $"Time: {minutes:00}:{seconds:00}";
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);

        timer.timerIsRunning = false;

        starManager.ResetStars();
    }

    public void RetryLevel()
    {
        Debug.Log("Retry pressed!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Debug.Log("Main Menu pressed!");
        SceneManager.LoadScene("MainMenu");
    }

}
