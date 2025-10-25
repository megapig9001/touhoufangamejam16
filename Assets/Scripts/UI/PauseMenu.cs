using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using EventManager;
using System;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] string titleScreenSceneName = "Title Screen";

    [SerializeField] CanvasGroup pauseMenuCanvasGroup;
    
    [SerializeField] private GameObject defaultSelectedButton;

    private bool pauseMenuOpen = false;

    private bool pauseEnabled = false;

    private Coroutine returningToTitle;

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
        ClosePauseMenu();
        new PlayerDeathEvent().InvokeEvent();
    }

    public void OnClickToTitle()
    {
        if (returningToTitle == null)
        {
            Time.timeScale = 1;
            returningToTitle = StartCoroutine(LoadScene(titleScreenSceneName));
        }
    }

    private void ClosePauseMenu()
    {
        pauseMenuOpen = false;
        pauseMenuCanvasGroup.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private IEnumerator LoadScene(string sceneName)
    {
        pauseMenuCanvasGroup.gameObject.SetActive(false);
        yield return GameManager.instance.TransitionExpandAndCollapseIn();
        yield return new WaitForEndOfFrame();

        yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        returningToTitle = null;
    }

    #region Event Handling

    private void OnEnable()
    {
        LevelStartEvent.AddListener(HandleLevelStartEvent);
        PlayerDeathEvent.AddListener(HandlePlayerDeathEvent);
        PlayerReachGoalEvent.AddListener(HandlePlayerReachGoalEvent);
    }

    private void OnDisable()
    {
        LevelStartEvent.RemoveListener(HandleLevelStartEvent);
        PlayerDeathEvent.RemoveListener(HandlePlayerDeathEvent);
        PlayerReachGoalEvent.RemoveListener(HandlePlayerReachGoalEvent);
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


    #endregion
}
