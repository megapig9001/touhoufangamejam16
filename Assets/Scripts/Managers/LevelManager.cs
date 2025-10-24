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
    [SerializeField] PlayerController player;

    public Vector2 RespawnPosition { get; set; }

    private void Awake()
    {
        RespawnPosition = player.transform.position;

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
        PlayerController controller = player.GetComponent<PlayerController>();
        controller.ResetPlayer();

        controller.transform.position = RespawnPosition;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        new EventManager.LevelStartEvent().InvokeEvent();
    }

    //private IEnumerator HandlePlayerDeath()
    //{

    //}
}
