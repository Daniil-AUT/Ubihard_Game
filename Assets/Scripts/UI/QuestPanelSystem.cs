using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanelSystem : MonoBehaviour
{
    public static QuestPanelSystem Instance { get; private set; }

    [System.Serializable]
    public class QuestInfo
    {
        public string questTitle;
        public string questDescription;
        public bool isCompleted;
    }

    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questDescriptionText;
    public QuestInfo currentQuest;

    public CanvasGroup canvasGroup; 
    public float fadeDuration = 1f; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        gameObject.SetActive(false);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0; 
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    // Method to activate a quest and show the quest panel
    public void ActivateQuest(QuestInfo quest)
    {
        currentQuest = quest; 
        UpdateQuestDisplay();

        gameObject.SetActive(true); 

        StartCoroutine(FadeInQuestPanel());
    }


    // Coroutine to fade in the quest panel
    private IEnumerator FadeInQuestPanel()
    {
        gameObject.SetActive(true); 
        if (canvasGroup != null)
        {
            // Gradually increase the alpha value to create a fade-in effect
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null; 
            }
            canvasGroup.alpha = 1; 
            canvasGroup.interactable = true; 
            canvasGroup.blocksRaycasts = true; 
        }
    }

    // Method to complete a quest
    public void CompleteQuest()
    {
        if (currentQuest != null)
        {
            currentQuest.isCompleted = true;
            HideQuestPanel(); 
            DialogueUI.Instance.CompleteQuest(); 
        }
    }

    // Method to hide the quest panel
    public void HideQuestPanel()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0; 
            canvasGroup.interactable = false; 
            canvasGroup.blocksRaycasts = false;
        }
        gameObject.SetActive(false); 
    }

    // Method to update the quest display text
    private void UpdateQuestDisplay()
    {
        if (questTitleText != null && currentQuest != null)
            questTitleText.text = currentQuest.questTitle;

        if (questDescriptionText != null && currentQuest != null)
            questDescriptionText.text = currentQuest.questDescription;
    }
}