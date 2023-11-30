using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class Jump : ActionBase<Character>
{
    [Space(5f)]

    [SerializeField] private AnimationCurve _yCurve;


    protected override IEnumerable<ActionComponent<Character>> _components => new List<ActionComponent<Character>>()
    {
        new ActionVector3Stepper<Character>(
            new Vector3StepperByCurve(_yCurve, () => Vector3.up * CurrentContext.CurrentStats.JumpHeight),
            new Vector3Stepper(() => Vector3.forward * CurrentContext.CurrentStats.JumpLength))
    };

    public override float Duration => CurrentContext.CurrentStats.JumpHeight / CurrentContext.CurrentStats.JumpSpeed;
}