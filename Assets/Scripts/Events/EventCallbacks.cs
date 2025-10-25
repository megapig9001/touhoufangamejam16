using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// To add an event, create a new event class (ex. EventName) as seen at the bottom of this namespace.
/// To add a listener to an event, call EventManager.EventName.AddListener and pass the method you wish to be called
/// when that event is triggered.
/// A method that is used as a listener should take an argument of type EventManager.EventName
/// IMPORTANT: Remember to always call EventManager.EventName.RemoveListener(MethodName)
/// To trigger an event:
/// new EventManager.EventName().InvokeEvent();
/// If you want to set some fields in the event, create a refence to the new event and set the fields before calling InvokeEvent()
/// </summary> 

namespace EventManager
{
    public abstract class Event<T> where T : Event<T>
    {
        public string Description;

        private bool hasBeenInvoked;
        public delegate void EventListener(T info);
        private static event EventListener listeners;

        public static void AddListener(EventListener listener)
        {
            listeners += listener;
        }

        public static void RemoveListener(EventListener listener)
        {
            listeners -= listener;
        }

        public void InvokeEvent()
        {
            if (hasBeenInvoked)
            {
                throw new System.Exception("This event has already been invoked, to prevent infinite loops events can't be invoked again");
            }
            hasBeenInvoked = true;
            if (listeners != null)
            {
                listeners(this as T);
            }
        }
    }

    #region EventTypes
    /// <summary>
    /// A test event class for debugging.
    /// </summary>
    public class DebugEvent : Event<DebugEvent>
    {
        public int n;
        public Vector2 v;
        public GameObject o;
    }

    /// <summary>
    /// Trigger when a player's health changes
    /// Current HP will be set to hpChange.
    /// </summary>
    public class PlayerHealthChangeEvent : Event<PlayerHealthChangeEvent>
    {
        public int newCurrentHealth;
    }

    public class LevelStartEvent : Event<LevelStartEvent>
    {

    }

    public class PlayerDeathEvent : Event<PlayerDeathEvent>
    {

    }

    public class PlayerReachGoalEvent : Event<PlayerReachGoalEvent>
    {

    }
    
    public class LevelRestartEvent : Event<LevelRestartEvent>
    {

    }

    public class LevelCompleteEvent : Event<LevelCompleteEvent>
    {

    }

    #endregion
}
