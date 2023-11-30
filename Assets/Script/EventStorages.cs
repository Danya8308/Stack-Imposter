using System;
using System.Collections.Generic;

public class EventStorages<TKey, TValue>
{
    private Dictionary<TKey, List<Action<TValue>>> _events = new Dictionary<TKey, List<Action<TValue>>>();


    public void Call(TKey key, TValue value)
    {
        if (_events.ContainsKey(key) == false)
        {
            return;
        }

        foreach (var action in _events[key])
        {
            action?.Invoke(value);
        }
    }

    public void Subscribe(TKey key, Action<TValue> subscriber)
    {
        if (_events.ContainsKey(key) == false)
        {
            _events[key] = new List<Action<TValue>>();
        }

        _events[key].Add(subscriber);
    }

    public void Unsubscribe(TKey key, Action<TValue> subscriber)
    {
        if (_events.ContainsKey(key) == false)
        {
            return;
        }

        _events[key].Remove(subscriber);
    }
}