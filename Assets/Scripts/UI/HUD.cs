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
        EventManager.LevelRestartEvent.AddListener(HandleLevelRestartEvent);
        EventManager.PlayerHealthChangeEvent.AddListener(HandlePlayerHealthChangeEvent);
    }

    private void OnDisable()
    {
        EventManager.LevelStartEvent.RemoveListener(HandleLevelStartEvent);
        EventManager.LevelRestartEvent.RemoveListener(HandleLevelRestartEvent);
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

    private void HandleLevelRestartEvent(EventManager.LevelRestartEvent info)
    {
        timer.StopAndResetTimer();
    }
}
