using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public Button resetButton;
    public GameObject gameOverPanel;
    private bool isGameOver = false;
    public Player player;
    public float fadeSpeed = 1f;

    private void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Game Over panel not assigned to GameOverManager.");
        }
        
        resetButton.onClick.AddListener(ResetGame);
    }

    private void OnDisable()
    {
        resetButton.onClick.RemoveListener(ResetGame);
    }

    public void TriggerGameOver()
    {
        StartCoroutine(ShowGameOverPanel());
    }

    private IEnumerator ShowGameOverPanel()
    {
        isGameOver = true;
        Time.timeScale = 0f;

        // Fade in the game over panel
        if (gameOverPanel != null)
        {
            CanvasGroup canvasGroup = gameOverPanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameOverPanel.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 0f;
            gameOverPanel.SetActive(true);

            while (canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha += Time.unscaledDeltaTime * fadeSpeed;
                yield return null;
            }
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResetGame()
    {
        StartCoroutine(ResetGameCoroutine());
    }

    private IEnumerator ResetGameCoroutine()
    {
        // Fade out the game over panel
        if (gameOverPanel != null)
        {
            CanvasGroup canvasGroup = gameOverPanel.GetComponent<CanvasGroup>();
            while (canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= Time.unscaledDeltaTime * fadeSpeed;
                yield return null;
            }
            gameOverPanel.SetActive(false);
        }

        // Reset the time scale
        Time.timeScale = 1f;

        // Reset player
        if (player != null)
        {
            player.ResetPlayer();
        }
        else
        {
            Debug.LogError("Player reference not set in GameOverManager.");
        }

        // Reset other game elements (add as needed)
        // For example, respawn enemies, reset collectibles, etc.

        isGameOver = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}