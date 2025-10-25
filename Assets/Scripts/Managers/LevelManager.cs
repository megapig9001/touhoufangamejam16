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

    private Coroutine handlingLevelEnding = null;

    private Coroutine handlingLevelRestart = null;

    public Vector2 RespawnPosition { get; set; }

    public GameObject dialogueSong;

    public GameObject levelSong;

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
        //Fail safe in case a transition is already occurring
        if (handlingLevelOpening != null || handlingLevelEnding != null || handlingLevelRestart != null)
            return;

        handlingLevelRestart = StartCoroutine(HandleLevelRestart());
    }

    private void HandlePlayerReachGoalEvent(PlayerReachGoalEvent info)
    {
        //Fail safe in case a transition is already occurring
        if (handlingLevelOpening != null || handlingLevelEnding != null || handlingLevelRestart != null)
            return;
        handlingLevelEnding = StartCoroutine(HandleLevelEnding());
    }

    private void StartLevel()
    {
        dialogueSong.SetActive(false);
        levelSong.SetActive(true);
        player.gameObject.SetActive(true);
        new LevelStartEvent().InvokeEvent();

    }

    private IEnumerator HandleLevelOpening()
    {
        yield return GameManager.instance.TransitionExpandAndCollapseIn();

        if (storyEventToPlayOnStart != null)
        {
            dialogueSong.SetActive(true);
            yield return GameManager.instance.GoThroughStoryEvent(storyEventToPlayOnStart);

        }

        StartLevel();

        yield return GameManager.instance.TransitionExpandAndCollapseOut();

        handlingLevelOpening = null;
    }

    private IEnumerator HandleLevelEnding()
    {
        yield return GameManager.instance.TransitionExpandAndCollapseIn();

        yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nextLevel);

        handlingLevelEnding = null;
    }

    private IEnumerator HandleLevelRestart()
    {
        player.transform.localScale = Vector2.zero;
        player.SetPlayerControllersActive(false);

        yield return new WaitForSeconds(0.2f);
        yield return GameManager.instance.TransitionExpandAndCollapseIn();

        player.transform.localScale = Vector2.one;
        player.transform.position = RespawnPosition;
        player.ResetPlayer();

        new LevelRestartEvent().InvokeEvent();

        yield return new WaitForSeconds(0.5f);
        yield return GameManager.instance.TransitionExpandAndCollapseOut();

        player.SetPlayerControllersActive(true);

        new LevelStartEvent().InvokeEvent();

        handlingLevelRestart = null;
    }
}
