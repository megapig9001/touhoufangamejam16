using EventManager;
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// A Manager for the current level. A level manager should exist for each scene (level).
/// </summary> 
/// 
public class LevelManager : MonoBehaviour
{
    [Tooltip("The StoryEvent object that will be used to play an opening when this LevelManager starts up. If one isn't provided, the level just start immediately.")]
    [SerializeField] StoryEvent storyEventToPlayOnStart;

    public string nextLevel;

    [SerializeField] PlayerController player;

    private Coroutine handlingLevelOpening = null;

    private Coroutine handlingLevelClosing = null;

    public Vector2 RespawnPosition { get; set; }

    private void Awake()
    {
        RespawnPosition = player.transform.position;
        player.gameObject.SetActive(false);
    }

    void Start()
    {
        handlingLevelOpening = StartCoroutine(HandleLevelOpening());
    }


    private void OnEnable()
    {
        PlayerDeathEvent.AddListener(HandlePlayerDeathEvent);
        PlayerReachGoalEvent.AddListener(HandlePlayerReachGoalEvent);

    }

    private void OnDisable()
    {
        PlayerDeathEvent.RemoveListener(HandlePlayerDeathEvent);
        PlayerReachGoalEvent.RemoveListener(HandlePlayerReachGoalEvent);
    }

    private void HandlePlayerDeathEvent(PlayerDeathEvent info)
    {
        player.ResetPlayer();

        player.transform.position = RespawnPosition;
    }

    private void HandlePlayerReachGoalEvent(PlayerReachGoalEvent info)
    {
        StartCoroutine(HandleLevelEnding());
    }

    private void StartLevel()
    {
        player.gameObject.SetActive(true);
        new EventManager.LevelStartEvent().InvokeEvent();

    }

    private IEnumerator HandleLevelOpening()
    {
        yield return GameManager.instance.TransitionExpandAndCollapseIn();

        if(storyEventToPlayOnStart != null) 
            yield return GameManager.instance.GoThroughStoryEvent(storyEventToPlayOnStart);

        StartLevel();

        yield return GameManager.instance.TransitionExpandAndCollapseOut();

        handlingLevelOpening = null;
    }

    private IEnumerator HandleLevelEnding()
    {
        yield return GameManager.instance.TransitionExpandAndCollapseIn();

        yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nextLevel);

        handlingLevelClosing = null;
    }
}
