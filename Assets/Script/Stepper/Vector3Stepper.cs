using System;
using UnityEngine;

public class Vector3Stepper
{
    private float? _lastValue = null;
    private Vector3 _target;

    private Func<Vector3> _getLocalTarget;


    public Vector3 DirectionToNextPoint { get; private set; }

    public Vector3Stepper(Func<Vector3> getLocalTarget)
    {
        _getLocalTarget = getLocalTarget;
    }

    public void Launch()
    {
        _target = _getLocalTarget();
    }

    public void Next(float nextStep)
    {
        float valueByStep = GetValueByStep(nextStep);

        if (_lastValue is null)
        {
            DirectionToNextPoint = GetPoint(valueByStep);
        }
        else
        {
            DirectionToNextPoint = GetPoint(valueByStep) - GetPoint((float)_lastValue);
        }

        _lastValue = valueByStep;
    }

    public void Reset()
    {
        _lastValue = null;

        _target = Vector3.zero;
        DirectionToNextPoint = Vector3.zero;
    }

    protected virtual float GetValueByStep(float step)
    {
        return step;
    }

    private Vector3 GetPoint(float step)
    {
        return Vector3.Lerp(Vector3.zero, _target, step);
    }
}