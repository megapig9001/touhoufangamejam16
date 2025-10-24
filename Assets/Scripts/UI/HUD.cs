using UnityEngine;
using System;
using TMPro;
using EventManager;

public class HUD : MonoBehaviour
{
    [SerializeField]
    TimerController timer;

    [SerializeField]
    TextMeshProUGUI playerHealthText;

    private void OnEnable()
    {
        EventManager.LevelStartEvent.AddListener(HandleLevelStartEvent);
        EventManager.PlayerDeathEvent.AddListener(HandlePlayerDeathEvent);
        EventManager.PlayerHealthChangeEvent.AddListener(HandlePlayerHealthChangeEvent);
    }

    private void OnDisable()
    {
        EventManager.LevelStartEvent.RemoveListener(HandleLevelStartEvent);
        EventManager.PlayerDeathEvent.RemoveListener(HandlePlayerDeathEvent);
        EventManager.PlayerHealthChangeEvent.RemoveListener(HandlePlayerHealthChangeEvent);
    }

    private void HandleLevelStartEvent(EventManager.LevelStartEvent info)
    {
        timer.StartTimer();
    }

    private void HandlePlayerHealthChangeEvent(PlayerHealthChangeEvent info)
    {
        playerHealthText.text = info.newCurrentHealth.ToString();
    }

    private void HandlePlayerDeathEvent(EventManager.PlayerDeathEvent info)
    {
        timer.StopAndResetTimer();
        timer.StartTimer();
    }
}
