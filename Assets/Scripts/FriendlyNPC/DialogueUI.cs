using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    public TextMeshProUGUI nameText;  // Assign through Inspector
    public TextMeshProUGUI contentText;  // Assign through Inspector
    public Button nextButton;  // Assign through Inspector

    private List<string> contentList;
    private int contentIndex = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        nextButton.onClick.AddListener(OnNextButtonClick);
        Hide();  // Start with the dialogue UI hidden.
    }

    public void Show(string npcName, string[] content)
    {
        nameText.text = npcName;
        contentList = new List<string>(content);
        contentIndex = 0;
        contentText.text = contentList[contentIndex];
        gameObject.SetActive(true);

        // Unlock and show the cursor when dialogue is active
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        // Lock and hide the cursor when dialogue is not active
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnNextButtonClick()
    {
        contentIndex++;
        if (contentIndex >= contentList.Count)
        {
            Hide();
        }
        else
        {
            contentText.text = contentList[contentIndex];
        }
    }
}
