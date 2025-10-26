using UnityEngine;
using System;
using TMPro;
using EventManager;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    [SerializeField]
    TimerController timer;

    [SerializeField]
    TextMeshProUGUI playerHealthText;

    [SerializeField]
    private ParticleSystem timeAddPopupEffect;

    private void OnEnable()
    {
        EventManager.LevelStartEvent.AddListener(HandleLevelStartEvent);
        EventManager.LevelRestartEvent.AddListener(HandleLevelRestartEvent);
        EventManager.PlayerHealthChangeEvent.AddListener(HandlePlayerHealthChangeEvent);
        EventManager.PlayerReachGoalEvent.AddListener(HandlePlayerReachGoalEvent);
    }

    private void OnDisable()
    {
        EventManager.LevelStartEvent.RemoveListener(HandleLevelStartEvent);
        EventManager.LevelRestartEvent.RemoveListener(HandleLevelRestartEvent);
        EventManager.PlayerHealthChangeEvent.RemoveListener(HandlePlayerHealthChangeEvent);
        EventManager.PlayerReachGoalEvent.RemoveListener(HandlePlayerReachGoalEvent);
    }

    private void HandleLevelStartEvent(EventManager.LevelStartEvent info)
    {
        timer.StartTimer();
    }

    private void HandlePlayerHealthChangeEvent(PlayerHealthChangeEvent info)
    {
        playerHealthText.text = info.newCurrentHealth.ToString();

        //Increase time upon taking damage
        if(info.healthChangeValue < 0 && info.newCurrentHealth > 0)
        {
            timer.AddToTimer(3);
            timeAddPopupEffect.transform.position = info.playerPosition;
            timeAddPopupEffect.Play();
        }
    }

    private void HandleLevelRestartEvent(EventManager.LevelRestartEvent info)
    {
        timer.StopAndResetTimer();
    }

    private void HandlePlayerReachGoalEvent(PlayerReachGoalEvent info)
    {
        timer.PauseTimer();
        float currentRecordTime = SaveAndLoadMethods.LoadLevelTime(SceneManager.GetActiveScene().name);
        float newTime = timer.elapsedTime;

        //Check if there is a current record
        if(currentRecordTime >= 0)
        {
            //If this new time is worse than the current time, do nothing.
            if(newTime >= currentRecordTime)
            {
                return;
            }
        }

        SaveAndLoadMethods.SaveLevelTime(SceneManager.GetActiveScene().name, newTime);
    }
}
