using UnityEngine;

public class GameTester : MonoBehaviour
{
    public GameManagerScore gameManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Test Win triggered!");
            gameManager.Win();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Test Game Over triggered!");
            gameManager.GameOver();
        }
    }
}

