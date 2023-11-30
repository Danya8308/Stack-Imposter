using System.Collections.Generic;
using UnityEngine;

public class AllBotMovePoints : MonoBehaviour
{
    private Dictionary<BotType, BotMovePoints> _points = new Dictionary<BotType, BotMovePoints>();


    public static AllBotMovePoints Instance { get; private set; }

    protected void Awake()
    {
        Instance = this;

        foreach (var points in GetComponentsInChildren<BotMovePoints>())
        {
            _points[points.Type] = points;
        }
    }

    public Transform GetPoint(BotType type, int index)
    {
        if (_points.ContainsKey(type) == false)
        {
            return null;
        }

        return _points[type].GetPoint(index);
    }
}