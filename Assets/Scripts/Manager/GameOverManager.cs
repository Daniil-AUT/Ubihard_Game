using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameOverManager : MonoBehaviour
{
    public Button resetButton;
    public GameObject gameOverPanel;
    [Tooltip("Add GameObjects here that should be hidden when player dies")]
    public List<GameObject> objectsToHideOnDeath = new List<GameObject>();
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

        // Hide all objects in the list
        foreach (GameObject obj in objectsToHideOnDeath)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }

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

        // Show all objects in the list
        foreach (GameObject obj in objectsToHideOnDeath)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
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

        isGameOver = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}