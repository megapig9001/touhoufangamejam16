using EventManager;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class LevelClearMenu : MonoBehaviour
{
    [SerializeField] string titleScreenSceneName = "Title Screen";

    [SerializeField] CanvasGroup levelClearMenuCanvasGroup;

    [SerializeField] TextMeshProUGUI finalTimeText;

    [SerializeField] private GameObject defaultSelectedButton;

    [SerializeField] TimerController timer;

    private bool levelClearMenuOpen = false;

    private Coroutine returningToTitle;

    private void Awake()
    {
        levelClearMenuCanvasGroup.alpha = 0;
        levelClearMenuCanvasGroup.gameObject.SetActive(false);
    }

    public void OnClickRetry()
    {
        CloseMenu();
        new PlayerDeathEvent().InvokeEvent();
    }

    public void OnClickToTitle()
    {
        if (returningToTitle == null)
        {
            returningToTitle = StartCoroutine(LoadScene(titleScreenSceneName));
        }
    }

    private void CloseMenu()
    {
        levelClearMenuOpen = false;
        levelClearMenuCanvasGroup.gameObject.SetActive(false);
    }

    private IEnumerator LoadScene(string sceneName)
    {
        yield return levelClearMenuCanvasGroup.DOFade(0, 0.2f).WaitForCompletion();

        levelClearMenuCanvasGroup.gameObject.SetActive(false);
        yield return GameManager.instance.TransitionExpandAndCollapseIn();
        yield return new WaitForEndOfFrame();

        yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        returningToTitle = null;
    }

    #region Event Handling

    private void OnEnable()
    {
        LevelCompleteEvent.AddListener(HandleLevelCompleteEvent);
    }

    private void OnDisable()
    {
        LevelCompleteEvent.RemoveListener(HandleLevelCompleteEvent);
    }
    private void HandleLevelCompleteEvent(LevelCompleteEvent info)
    {
        if (levelClearMenuOpen)
            return;

        levelClearMenuOpen = true;
        levelClearMenuCanvasGroup.gameObject.SetActive(true);
        finalTimeText.text = "Final Time: " + timer.TimerText;
        levelClearMenuCanvasGroup.DOFade(1, 0.2f);
        EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
    }

    #endregion
}
