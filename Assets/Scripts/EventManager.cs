using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public delegate void Action(params object[] args);
    private static Dictionary<Event, Action> eventDictionary = new();
    private static Dictionary<Delegate, Action> delegateDictionary = new(); // mapping from custom delegate to action; used to unsubscribe


    public enum Event
    {
        LoadingStatusChanged,
        ChordPlayed,
        NotePressed, 
        ChordHit,
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    private static Action WrapListener(Delegate func)
    {
        return (object[] args) => func.DynamicInvoke(args); // may be cringe
    }

    public static void StartListening(Event eventName, Delegate listener)
    {
        if (!delegateDictionary.TryGetValue(listener, out var action))
        {
            action = WrapListener(listener);
            delegateDictionary[listener] = action;
        }

        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] += action;
        }
        else
        {
            eventDictionary.Add(eventName, action);
        }
    }

    public static void StopListening(Event eventName, Delegate listener)
    {
        if (!delegateDictionary.TryGetValue(listener, out var action))
        {
            return;
        }

        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] -= action;
        }
    }

    public static void TriggerEvent(Event eventName, params object[] message)
    {
        if (eventDictionary.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent.Invoke(message);
        }
    }
}