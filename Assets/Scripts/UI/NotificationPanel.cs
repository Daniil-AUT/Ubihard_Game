using UnityEngine;
using TMPro;

public class NotificationPanel : MonoBehaviour
{
    public GameObject notificationPanel; 
    public TextMeshProUGUI notificationText; 

    private void Start()
    {
        notificationPanel.SetActive(false); 
    }

    // Method to show the notification with a message
    public void ShowNotification(string message, float duration)
    {
        notificationText.text = message; 
        notificationPanel.SetActive(true); 
        StartCoroutine(FadeInCoroutine(duration));
    }

    private System.Collections.IEnumerator FadeInCoroutine(float duration)
    {
        float timeElapsed = 0f;

        CanvasGroup canvasGroup = notificationPanel.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0; 
            canvasGroup.interactable = true; 
            canvasGroup.blocksRaycasts = true; 

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0, 1, timeElapsed / duration);
                yield return null;
            }

            canvasGroup.alpha = 1; 
        }

        // Wait for the specified duration before fading out
        yield return new WaitForSeconds(duration);
        FadeOutPanel(1f); 
    }

    public void FadeOutPanel(float duration)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    private System.Collections.IEnumerator FadeOutCoroutine(float duration)
    {
        CanvasGroup canvasGroup = notificationPanel.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            float timeElapsed = 0f;

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1, 0, timeElapsed / duration);
                yield return null;
            }

            canvasGroup.alpha = 0; 
            canvasGroup.interactable = false; 
            canvasGroup.blocksRaycasts = false; 
            notificationPanel.SetActive(false); 
        }
    }

    public void HideNotification()
    {
        notificationPanel.SetActive(false); 
    }
}
