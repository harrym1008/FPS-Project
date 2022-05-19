using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public enum PauseStates
    {
        Gameplay,
        PauseMenu,
        SettingsMenu,
        GFXMenu
    }



    public PauseStates pauseState;

    public InputMaster controls;

    public GameObject blackBG;

    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject graphicsMenuUI;

    public GameObject playerHUD;

    public static bool gameIsPaused = false;

    public void Resume()
    {
        pauseState = PauseStates.Gameplay;

        blackBG.SetActive(false);
        pauseMenuUI.SetActive(false);
        playerHUD.SetActive(true);
        settingsMenuUI.SetActive(false);
        graphicsMenuUI.SetActive(false);

        Time.timeScale = 1f;
        gameIsPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        pauseState = PauseStates.PauseMenu;

        blackBG.SetActive(true);
        pauseMenuUI.SetActive(true);
        playerHUD.SetActive(false);
        settingsMenuUI.SetActive(false);
        graphicsMenuUI.SetActive(false);

        Time.timeScale = 0f;
        gameIsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Quit()
    {
        Debug.Log("Quitting the game... this will work in the build");
        Application.Quit();
    }

    public void SettingsMenu()
    {
        pauseState = PauseStates.SettingsMenu;

        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
        graphicsMenuUI.SetActive(false);
    }

    public void GraphicsMenu()
    {
        pauseState = PauseStates.GFXMenu;

        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        graphicsMenuUI.SetActive(true);
        GetComponent<GraphicalSettingsMenu>().UpdateEntries();
    }


    public void ReadMenuKey()
    {
        if (pauseState == PauseStates.Gameplay || pauseState == PauseStates.SettingsMenu)
        {
            Pause();
        }
        else if (pauseState == PauseStates.PauseMenu)
        {
            Resume();
        }
        else if (pauseState == PauseStates.GFXMenu)
        {
            SettingsMenu();
        }
    }


    private void Awake()
    {
        controls = new InputMaster();
        controls.Menu.PauseMenu.started += _ => ReadMenuKey();

        Resume();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
