using System;
using UnityEngine;

public class Vector3StepperByCurve : Vector3Stepper
{
    private AnimationCurve _curve;


    public Vector3StepperByCurve(AnimationCurve curve, Func<Vector3> getLocalTarget) 
        : base(getLocalTarget)
    {
        _curve = curve;
    }

    protected override float GetValueByStep(float step)
    {
        return _curve.Evaluate(step);
    }
}