using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private float defaultTransitionLength = 0.5f;

    [SerializeField] CanvasGroup transitionCanvasGroup;
    [SerializeField] Image transitionImage;

    [SerializeField] CanvasGroup storyCanvasGroup;
    [SerializeField] Image storyImage;
    [SerializeField] TextMeshProUGUI storyText;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
            transitionCanvasGroup.gameObject.SetActive(true);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {

    }

    public IEnumerator GoThroughStoryEvent(StoryEvent storyEvent, float transitionLength = 0)
    {
        storyCanvasGroup.gameObject.SetActive(true);
        storyCanvasGroup.alpha = 0;

        yield return TransitionExpandAndCollapseIn(transitionLength);

        foreach (StoryEventStep step in storyEvent.storyEventSteps)
        {
            //Wait briefly after completing first half of transition to make changing images less jarring.
            yield return new WaitForSeconds(0.1f);
            DisplayStoryEvent(step);

            yield return TransitionExpandAndCollapseOut(transitionLength);

            //TODO: Move on via user input before going to next step
            yield return new WaitForSeconds(2);

            if(step.playTransitionToNextStep)
                yield return TransitionExpandAndCollapseIn(transitionLength);

        }

        yield return TransitionExpandAndCollapseIn(transitionLength);

        storyCanvasGroup.alpha = 0;
        storyCanvasGroup.gameObject.SetActive(false);
    }

    private void DisplayStoryEvent(StoryEventStep step)
    {
        storyCanvasGroup.alpha = 1;
        storyImage.sprite = step.eventImage;
        storyText.text = step.eventText;
    }


    #region Transitions

    private bool hasTransitionedIn = true;

    /// <summary>
    /// Fade in transition. Second step to complete transition would be TransitionFadeOut
    /// </summary>
    /// <param name="time">Seconds to complete transition. Default's to GameManager's configured defaultTransitionLength</param>
    /// <returns></returns>
    public IEnumerator TransitionFadeIn(float time = 0)
    {
        time = time == 0 ? defaultTransitionLength : time;

        if (!hasTransitionedIn)
        {
            yield return transitionCanvasGroup.DOFade(1, time).WaitForCompletion();
            hasTransitionedIn = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="time">Seconds to complete transition. Default's to GameManager's configured defaultTransitionLength</param>
    /// <returns></returns>
    public IEnumerator TransitionFadeOut(float time = 0)
    {
        time = time == 0 ? defaultTransitionLength : time;
        if (hasTransitionedIn)
        {
            yield return transitionCanvasGroup.DOFade(0, time).WaitForCompletion();
            hasTransitionedIn = false;
        }
    }


    /// <summary>
    /// Expand outward transition start. Second step to complete transition would be TransitionExpandAndCollapseOut
    /// </summary>
    /// <param name="time">Seconds to complete transition. Default's to GameManager's configured defaultTransitionLength</param>
    /// <returns></returns>
    public IEnumerator TransitionExpandAndCollapseIn(float time = 0)
    {
        Vector2 transitionTargetDims = new Vector2(1, 1);
        if (!hasTransitionedIn)
        {
            time = time == 0 ? defaultTransitionLength : time;

            transitionImage.rectTransform.localScale = new Vector2(0, 0);

            yield return transitionImage.rectTransform.DOScale(transitionTargetDims, time).WaitForCompletion();

            hasTransitionedIn = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="time">Seconds to complete transition. Default's to GameManager's configured defaultTransitionLength</param>
    /// <returns></returns>
    public IEnumerator TransitionExpandAndCollapseOut(float time = 0)
    {
        Vector2 transitionTargetDims = new Vector2(0, 0);

        if (hasTransitionedIn)
        {
            time = time == 0 ? defaultTransitionLength : time;

            transitionImage.rectTransform.localScale = new Vector2(1, 1);

            yield return transitionImage.rectTransform.DOScale(transitionTargetDims, time).WaitForCompletion();

            transitionImage.rectTransform.localScale = new Vector2(0, 0);

            hasTransitionedIn = false;
        }
    }

    #endregion
}
