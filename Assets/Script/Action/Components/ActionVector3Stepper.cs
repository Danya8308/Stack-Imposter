using UnityEngine;

public class ActionVector3Stepper<TContext> : ActionComponent<TContext>
    where TContext : IShiftable
{
    private Vector3Stepper[] _steppers;


    public ActionVector3Stepper(params Vector3Stepper[] steppers)
    {
        _steppers = steppers;
    }

    protected override void AcceptEvent(ActionBase<TContext> skill)
    {
        foreach (var stepper in _steppers)
        {
            skill.OnLaunch += stepper.Launch;
            skill.OnStop += stepper.Reset;

            skill.OnUpdate += Next;
        }
    }

    protected override void ResetEvent(ActionBase<TContext> skill)
    {
        foreach (var stepper in _steppers)
        {
            skill.OnLaunch -= stepper.Launch;
            skill.OnStop -= stepper.Reset;

            skill.OnUpdate -= Next;
        }
    }

    private void Next()
    {
        Vector3 direction = Vector3.zero;

        foreach (var stepper in _steppers)
        {
            stepper.Next(Skill.ProcessValue);

            direction += stepper.DirectionToNextPoint;
        }

        Context.Shift(direction);
    }
}