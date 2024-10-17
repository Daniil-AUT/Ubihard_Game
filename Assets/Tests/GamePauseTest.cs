using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

public class GamePauseTest
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pauseMenuPanel;
        public Button resumeButton;
        public Button saveButton;
        public Button loadButton;
        public Button quitButton;

        public bool isPaused = false;

        public void PauseGame()
        {
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void ResumeGame()
        {
            pauseMenuPanel.SetActive(false);
            Time.timeScale = 1;
            isPaused = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void SaveGame()
        {
            Debug.Log("Game saved!");
        }

        public void LoadGame()
        {
            Debug.Log("Game loaded!");
            ResumeGame();
        }

        public void QuitGame()
        {
            Debug.Log("Quit Game called");
            string quitMessage = "Are you sure you want to quit? Any unsaved progress will be lost!";
            Debug.Log(quitMessage);

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }

    private GameObject pauseMenuObject;
    private PauseMenu pauseMenu;

    [SetUp]
    public void Setup()
    {
        pauseMenuObject = new GameObject();
        pauseMenu = pauseMenuObject.AddComponent<PauseMenu>();

        pauseMenu.pauseMenuPanel = new GameObject();
        pauseMenu.resumeButton = new GameObject().AddComponent<Button>();
        pauseMenu.saveButton = new GameObject().AddComponent<Button>();
        pauseMenu.loadButton = new GameObject().AddComponent<Button>();
        pauseMenu.quitButton = new GameObject().AddComponent<Button>();

        pauseMenu.pauseMenuPanel.SetActive(false);
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(pauseMenuObject); // Use DestroyImmediate instead of Destroy
        Time.timeScale = 1; // Resetting the time scale to avoid impacting other tests.
        Cursor.lockState = CursorLockMode.Locked; // Resetting cursor state
        Cursor.visible = false; // Resetting cursor visibility
    }

    [UnityTest]
    public IEnumerator PauseGame_ShouldDisplayPauseMenu()
    {
        pauseMenu.PauseGame();

        Assert.IsTrue(pauseMenu.pauseMenuPanel.activeSelf, "Pause menu should be visible when the game is paused.");
        Assert.IsTrue(pauseMenu.isPaused, "Game should be marked as paused.");

        yield return null;
    }

    [UnityTest]
    public IEnumerator QuitGame_ShouldNotCrash()
    {
        pauseMenu.PauseGame();
        pauseMenu.QuitGame();

        Assert.IsTrue(pauseMenu.isPaused, "Game should be paused when quitting.");
        Debug.Log("Quit game action triggered successfully.");

        yield return null;
    }

    [UnityTest]
    public IEnumerator SaveGame_SaveWithoutErrors()
    {
        pauseMenu.PauseGame();
        LogAssert.Expect(LogType.Log, "Game saved!");
        pauseMenu.SaveGame();

        Assert.IsTrue(pauseMenu.isPaused, "Game should be paused while saving.");

        yield return null;
    }

    [UnityTest]
    public IEnumerator PressPauseKey_ShouldPauseGame()
    {
        Assert.IsFalse(pauseMenu.isPaused, "The game should not be paused when the player is playing.");

        pauseMenu.PauseGame();

        Assert.IsTrue(pauseMenu.isPaused, "The game should be paused after pressing the pause key.");
        Assert.AreEqual(0, Time.timeScale, "Time scale should be 0 when the game is paused.");

        yield return null;
    }
}
