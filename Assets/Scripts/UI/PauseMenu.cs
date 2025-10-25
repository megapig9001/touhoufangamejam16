using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using EventManager;
using System;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] CanvasGroup pauseMenuCanvasGroup;
    
    [SerializeField] private GameObject defaultSelectedButton;

    private bool pauseMenuOpen = false;

    private bool pauseEnabled = false;

    private void Awake()
    {
        pauseMenuCanvasGroup.gameObject.SetActive(false);
    }

    void Update()
    {
        if (pauseEnabled && (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame))
        {
            if (!pauseMenuOpen)
            {
                pauseMenuOpen = true;
                pauseMenuCanvasGroup.gameObject.SetActive(true);
                EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
                Time.timeScale = 0;
            }
            else
            {
                ClosePauseMenu();
            }
        }
    }

    public void OnClickResume()
    {
        ClosePauseMenu();
    }

    public void OnClickRestart()
    {
        Debug.Log("Restart");
    }

    public void OnClickToTitle()
    {
        Debug.Log("To Title");
    }

    private void ClosePauseMenu()
    {
        pauseMenuOpen = false;
        pauseMenuCanvasGroup.gameObject.SetActive(false);
        Time.timeScale = 1;
    }



    #region Event Handling

    private void OnEnable()
    {
        LevelStartEvent.AddListener(HandleLevelStartEvent);
        PlayerDeathEvent.AddListener(HandlePlayerDeathEvent);
        PlayerReachGoalEvent.AddListener(HandlePlayerReachGoalEvent);
        LevelRestartEvent.AddListener(HandleLevelRestartEvent);
    }

    private void OnDisable()
    {
        LevelStartEvent.RemoveListener(HandleLevelStartEvent);
        PlayerDeathEvent.RemoveListener(HandlePlayerDeathEvent);
        PlayerReachGoalEvent.RemoveListener(HandlePlayerReachGoalEvent);
        LevelRestartEvent.RemoveListener(HandleLevelRestartEvent);
    }
    private void HandlePlayerReachGoalEvent(PlayerReachGoalEvent info)
    {
        pauseEnabled = false;
    }

    private void HandlePlayerDeathEvent(PlayerDeathEvent info)
    {
        pauseEnabled = false;
    }

    private void HandleLevelStartEvent(LevelStartEvent info)
    {
        pauseEnabled = true;
    }

    private void HandleLevelRestartEvent(LevelRestartEvent info)
    {
        pauseEnabled = true;
    }


    #endregion
}
