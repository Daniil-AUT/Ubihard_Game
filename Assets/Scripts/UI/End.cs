using UnityEngine;
using UnityEngine.UI;

public class GiftConfirmationPanel : MonoBehaviour
{
    public GameObject giftConfirmationPanel;
    public Button quitButton;

    private void Start()
    {
        giftConfirmationPanel.SetActive(false);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void ShowPanel()
    {
        giftConfirmationPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HidePanel()
    {
        giftConfirmationPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop playing in the editor
#endif
    }
}
