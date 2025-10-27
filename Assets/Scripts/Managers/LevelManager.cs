using EventManager;
using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

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
        handlingLevelEnding = StartCoroutine(HandleLevelEnding(info));
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

        if (storyEventToPlayOnStart != null && !GameManager.instance.EnteredCurrentLevelFromLevelSelectMenu)
        {
            dialogueSong.SetActive(true);
            yield return GameManager.instance.GoThroughStoryEvent(storyEventToPlayOnStart);
        }

        StartLevel();

        yield return GameManager.instance.TransitionExpandAndCollapseOut();

        handlingLevelOpening = null;
    }

    private IEnumerator HandleLevelEnding(PlayerReachGoalEvent eventInfo)
    {
        //Play win animation
        Rigidbody2D rigidbody2D = player.GetComponent<Rigidbody2D>();
        rigidbody2D.simulated = false;

        var moving = player.transform.DOMove(eventInfo.goalPosition, 0.2f);

        yield return new WaitForSeconds(0.1f);

        var spinning = player.transform.DORotate(Vector3.forward*360*4, 4f, RotateMode.FastBeyond360);

        yield return new WaitForSeconds(1.6f);

        yield return GameManager.instance.TransitionExpandAndCollapseIn();

        spinning.Kill();
        moving.Kill();

        player.gameObject.SetActive(false);
        rigidbody2D.simulated = true;
        player.transform.rotation = Quaternion.identity;
        //end win animation

        if (GameManager.instance.EnteredCurrentLevelFromLevelSelectMenu)
        {
            //This event is used to trigger the "level clear" menu to appear, displaying final time and Retry/Quit options
            //Currently, should only display when a level is selected from the Level Select menu
            
            new LevelCompleteEvent().InvokeEvent();
        }
        else
        {
            yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nextLevel);
        }

        handlingLevelEnding = null;
    }

    private IEnumerator HandleLevelRestart()
    {
        player.gameObject.SetActive(true);
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
