using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
///
/// </summary> 
public class TimerController : MonoBehaviour
{
    TextMeshProUGUI timeCounter;

    private TimeSpan timePlaying;
    private bool timerRunning = false;

    private float elapsedTime;

	/// <summary>
    /// Start is called before the first frame update
	/// </summary> 
    void Start()
    {
        timeCounter = GetComponent<TextMeshProUGUI>();
        timeCounter.text = "00:00.00";
    }


    public void StartTimer()
    {
        timerRunning = true;
        elapsedTime = 0f;
    }

    public void PauseTimer()
    {
        timerRunning = false;
    }

    public void StopAndResetTimer()
    {
        timerRunning = false;
        elapsedTime = 0f;
    }

	/// <summary>
    /// Update is called once per frame
	/// </summary> 
    void Update()
    {
        if(timerRunning)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingString = timePlaying.ToString("mm':'ss'.'ff");
            timeCounter.text = timePlayingString;
        }
    }
}
