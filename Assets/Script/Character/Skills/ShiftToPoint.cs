using System.Collections.Generic;
using UnityEngine;

public class ShiftToPoint : ActionBase<BotLogic>
{
    [SerializeField] private float _yTarget = 0.28f;
    private float _duration;


    public Transform CurrentPoint => CurrentContext.CurrentPoint;

    public override float Duration => _duration;

    protected override IEnumerable<ActionComponent<BotLogic>> _components => new List<ActionComponent<BotLogic>>()
    {
        new ActionVector3Stepper<BotLogic>(new Vector3Stepper(GetLocalTarget)),
    };

    protected override void BeforeLaunchEvent()
    {
        InitDuration();
    }

    protected override void UpdateEvent()
    {
        if (CurrentContext.CachedCharacter.IsJumping == false)
        {
            Rotate();
        }

        if (CurrentContext.CachedCharacter.OnSurface == false)
        {
            Stop();
        }
    }

    private Vector3 GetDirection()
    {
        if (CurrentPoint == null)
        {
            return CurrentContext.transform.forward;
        }

        Vector3 direction = CurrentPoint.position - CurrentContext.transform.position;
        direction.y = 0f;

        return direction;
    }

    private void Rotate()
    {
        Vector3 direction = GetDirection();

        if (direction != Vector3.zero)
        {
            CurrentContext.transform.forward = direction;
        }
    }

    private void InitDuration()
    {
        _duration = GetDirection().magnitude / CurrentContext.CachedCharacter.CurrentStats.Speed;
    }

    private Vector3 GetLocalTarget()
    {
        Vector3 target = CurrentContext.transform.InverseTransformPoint(CurrentPoint.position);
        target.y = _yTarget;

        return target;
    }
}