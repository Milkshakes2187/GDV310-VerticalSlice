using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using VInspector;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool bInMainMenu;
    public bool bIsPaused;

    float fGameTimer = 0.0f;

    [Header("Menu Screens")]
    public GameObject pauseScreen;
    public GameObject gameOverScreen;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI endTimeText;

    [Header("Vars")]
    public TextMeshProUGUI gameTimeText;
    public GameObject playerHUD;

    [Foldout("Dev Keys")]
    public KeyCode toggleCursor = KeyCode.BackQuote;
    public KeyCode togglePause = KeyCode.Escape;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (bInMainMenu) return;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // If in main menu, do nothing
        if (bInMainMenu) return;

        // Pause key input
        if (Input.GetKeyDown(togglePause))
        {
            TogglePause();
        }

        // Toggle free cursor
        if (Input.GetKeyDown(toggleCursor))
        {
            bool cursorState = Cursor.visible;
            ToggleFreeCursor(!cursorState);
        }
    }

    /***********************************************
    * PlayGame: Plays the game from beginning by loading main game scene.
    * @author: Justhine Nisperos
    * @parameter: 
    * @return: void
    ************************************************/
    public void PlayGame()
    {
        Time.timeScale = 1.0f;
        ToggleFreeCursor(false);

        SceneManager.LoadScene("MainScene");
    }

    /***********************************************
    * TogglePause: Pauses/Unpauses the game.
    * @author: Justhine Nisperos
    * @parameter: 
    * @return: void
    ************************************************/
    public void TogglePause()
    {
        bIsPaused = !bIsPaused;
        ToggleFreeCursor(bIsPaused);

        if (bIsPaused)
        {
            // Pause game and reenable cursor
            Time.timeScale = 0.0f;

            // Activate pause screen & disable player HUD
            pauseScreen.SetActive(true);
            playerHUD.SetActive(false);
        }
        else
        {
            // Unpause game and reenable cursor
            Time.timeScale = 1.0f;

            // Deactivate pause screen
            pauseScreen.SetActive(false);
            playerHUD.SetActive(true);
        }
    }

    /***********************************************
    * GameOver: Ends the game and display the end game screen.
    * @author: Justhine Nisperos
    * @parameter: bool
    * @return: void
    ************************************************/
    public void GameOver(bool _bVictory)
    {
        Time.timeScale = 0.0f;
        ToggleFreeCursor(true);

        // Get game time
        endTimeText.text = ConvertToMinSecs(fGameTimer);

        // Change game screen text based on win or lose
        gameOverText.text = _bVictory ? "Victory!" : "Game Over...";

        gameOverScreen.SetActive(true);
    }

    /***********************************************
    * ReturnToMenu: Returns to main menu scene.
    * @author: Justhine Nisperos
    * @parameter: 
    * @return: void
    ************************************************/
    public void ReturnToMenu()
    {
        Time.timeScale = 1.0f;

        // TODO: Reloads current scene. Swap for commented option below when main menu scene implemented.
        SceneManager.LoadScene(0);
        //SceneManager.LoadScene(0);
    }

    /***********************************************
    * QuitGame: Exits the application.
    * @author: Justhine Nisperos
    * @parameter: 
    * @return: void
    ************************************************/
    public void QuitGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /***********************************************
    * ConvertToMinSecs: Converts a float timer to 00:00 mins & secs format as a string.
    * @author: Justhine Nisperos
    * @parameter: float
    * @return: string
    ************************************************/
    string ConvertToMinSecs(float _fTime)
    {
        int iMins = Mathf.FloorToInt(_fTime / 60.0f);
        int iSecs = Mathf.FloorToInt(_fTime - (iMins * 60.0f));

        string sTime = iMins.ToString("00") + ":" + iSecs.ToString("00");
        return sTime;
    }

    /***********************************************
    * ToggleFreeCursor: Toggles the cursor visibility and lock state.
    * @author: Justhine Nisperos
    * @parameter: bool
    * @return: void
    ************************************************/
    public void ToggleFreeCursor(bool _toggleOn)
    {
        if (!_toggleOn)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
