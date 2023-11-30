using UnityEngine;

public class ActionSurfaceCondition<TContext> : ActionCheckerForLaunch<TContext>
    where TContext : ICheckingSurface
{
    [SerializeField] private bool _onGround;
    [SerializeField] private bool _inFlight;


    public ActionSurfaceCondition(bool onGround = false, bool inFlight = false)
    {
        _onGround = onGround;
        _inFlight = inFlight;
    }

    protected override bool Check()
    {
        if (_onGround == true && _inFlight == true)
        {
            return true;
        }

        if (Context.OnSurface == true)
        {
            return _onGround;
        }
        else
        {
            return _inFlight;
        }
    }
}