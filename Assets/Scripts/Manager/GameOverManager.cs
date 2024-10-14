using UnityEngine;
using UnityEngine.UI; // For UI elements
using UnityEngine.SceneManagement; // For loading scenes

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel; // Reference to the Game Over panel
    private bool isGameOver = false;

    private void Start()
    {
        // Hide the game over panel at the start
        gameOverPanel.SetActive(false);
    }

    public void TriggerGameOver()
    {
        isGameOver = true;
        // Pause the game
        Time.timeScale = 0f;
        // Show the game over panel
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        // Reset the time scale and reload the scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
