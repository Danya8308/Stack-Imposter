using System;
using System.Collections.Generic;
using UnityEngine;

public class BotMovePoints : MonoBehaviour
{
    [SerializeField] private BotType _type;

    private Dictionary<int, Transform> _points = new Dictionary<int, Transform>();


    public BotType Type => _type;

    private void Awake()
    {
        foreach (Transform point in GetComponentInChildren<Transform>())
        {
            if (point == transform)
            {
                continue;
            }

            if (int.TryParse(point.name, out int index) == true)
            {
                _points[index] = point;
            }
            else
            {
                throw new Exception($"{point.name} is uncorrect point name");
            }
        }
    }

    public Transform GetPoint(int index)
    {
        if (_points.ContainsKey(index) == false)
        {
            return null;
        }

        return _points[index];
    }

    public void ReplacePoint(Transform newPoint, int index)
    {
        _points[index] = newPoint;
    }
}