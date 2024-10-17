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

    // TODO: REWORK NEXT

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

        // Increment game timer but only when the fight has started
        //if (PrototypeBoss.instance.bFightStarted)
        //{
        //    fGameTimer += Time.deltaTime;
        //    gameTimeText.text = ConvertToMinSecs(fGameTimer);
        //}

        // Pause key input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void PlayGame()
    {
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.LoadScene("MainScene");
    }

    public void PauseUnpause()
    {
        //if (!bIsPaused)
        //{
        //    bIsPaused = true;
        //
        //    // Pause game and reenable cursor
        //    Time.timeScale = 0.0f;
        //    Cursor.visible = true;
        //    Cursor.lockState= CursorLockMode.None;
        //
        //    // Activate pause screen & disable player HUD
        //    pauseScreen.SetActive(true);
        //    playerHUD.SetActive(false);
        //}
        //else
        //{
        //    bIsPaused = false;
        //
        //    // Pause game and reenable cursor
        //    Time.timeScale = 1.0f;
        //    Cursor.visible = false;
        //    Cursor.lockState = CursorLockMode.Locked;
        //
        //    // Deactivate pause screen
        //    pauseScreen.SetActive(false);
        //    playerHUD.SetActive(true);
        //}
    }

    public void GameOver(bool _bVictory)
    {
        Time.timeScale = 0.0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Get game time
        endTimeText.text = ConvertToMinSecs(fGameTimer);

        // Change game screen text based on win or lose
        gameOverText.text = _bVictory ? "Victory!" : "Game Over...";

        gameOverScreen.SetActive(true);
    }
    
    public void ReturnToMenu()
    {
        Time.timeScale = 1.0f;

        SceneManager.LoadScene(0);
    }

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

    string ConvertToMinSecs(float _fTime)
    {
        int iMins = Mathf.FloorToInt(_fTime / 60.0f);
        int iSecs = Mathf.FloorToInt(_fTime - (iMins * 60.0f));

        string sTime = iMins.ToString("00") + ":" + iSecs.ToString("00");
        return sTime;
    }    
}
