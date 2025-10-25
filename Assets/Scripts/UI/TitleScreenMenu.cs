using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleScreenMenu : MonoBehaviour
{
    [SerializeField] CanvasGroup mainMenuCanvasGroup;
    [SerializeField] CanvasGroup levelSelectMenuCanvasGroup;

    private void Start()
    {
        mainMenuCanvasGroup.gameObject.SetActive(true);
        levelSelectMenuCanvasGroup.gameObject.SetActive(false);
        StartCoroutine(GameManager.instance.TransitionFadeOut(3));
    }

    public void OnClickStart(string firstStageScene)
    {
        StartCoroutine(LoadScene(firstStageScene));
    }

    public void OnClickLevelSelect()
    {
        mainMenuCanvasGroup.gameObject.SetActive(false);
        levelSelectMenuCanvasGroup.gameObject.SetActive(true);
    }

    public void OnClickControls()
    {
        Debug.Log("Control");
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void OnClickLevelSelectMenu_LoadLevel(string sceneToLoad)
    {
        StartCoroutine(LoadScene(sceneToLoad));
    }

    public void OnClickLevelSelectMenu_Back()
    {
        mainMenuCanvasGroup.gameObject.SetActive(true);
        levelSelectMenuCanvasGroup.gameObject.SetActive(false);
    }

    public void MenuUtil_SetNewActiveGameObject(GameObject gameObject)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    private IEnumerator LoadScene(string sceneName)
    {
        yield return GameManager.instance.TransitionExpandAndCollapseIn();
        yield return new WaitForEndOfFrame();

        yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
    }
}
