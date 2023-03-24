using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	
    #region Variables
    
    [HideInInspector] public static bool game_paused = false;

    [Header("References")]
    [SerializeField] GameObject pause_menu;
    [SerializeField] GameObject settings_menu;
    [SerializeField] GameObject hud;
     
    #endregion

    
    #region Events
    
    
     
    #endregion


    #region Mono Callbacks

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            if (game_paused)
                Resume();
            else
                Pause();
        }
    }
     
    #endregion


    #region Private Methods
    
    
     
    #endregion


    #region Public Methods
    
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pause_menu?.SetActive(false);
        settings_menu?.SetActive(false);
        hud?.SetActive(true);

        Time.timeScale = 1;

        game_paused = false;
    }
    
    public void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        pause_menu?.SetActive(true);
        settings_menu?.SetActive(false);
        hud?.SetActive(false);

        Time.timeScale = 0;

        game_paused = true;
    }

    public void SettingsMenu()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        pause_menu?.SetActive(false);
        settings_menu?.SetActive(true);
        hud?.SetActive(false);

        Time.timeScale = 0;

        game_paused = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    #endregion
    
}
