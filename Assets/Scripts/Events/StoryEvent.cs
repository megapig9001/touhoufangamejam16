using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "StoryEvent", menuName = "Scriptable Objects/StoryEvent")]
[Serializable]
public class StoryEvent : ScriptableObject
{
    [field: SerializeField] public StoryEventStep[] storyEventSteps { get; private set; }
}


[Serializable]
public struct StoryEventStep
{
    [SerializeField] private string stepName;
    [field: SerializeField] public Sprite eventImage { get; private set; }
    [field: SerializeField] [TextArea] public string eventText { get; private set; }
}