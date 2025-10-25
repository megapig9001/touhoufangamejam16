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

    [SerializeField] PlayerController player;

    private Coroutine handlingLevelOpening = null;

    public Vector2 RespawnPosition { get; set; }

    private void Awake()
    {
        RespawnPosition = player.transform.position;
        //player.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        PlayerDeathEvent.AddListener(HandlePlayerDeathEvent);
    }

    private void OnDisable()
    {
        PlayerDeathEvent.RemoveListener(HandlePlayerDeathEvent);

    }

    private void HandlePlayerDeathEvent(PlayerDeathEvent info)
    {
        player.ResetPlayer();

        player.transform.position = RespawnPosition;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        handlingLevelOpening = StartCoroutine(HandleLevelOpening());
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

    //private IEnumerator HandlePlayerDeath()
    //{

    //}
}
