public abstract class ActionCheckerForLaunch<TContext> : ActionComponent<TContext>
{
    protected sealed override void AcceptEvent(ActionBase<TContext> action)
    {
        action.WhenCheckingLaunchCapability += Check;
    }

    protected sealed override void ResetEvent(ActionBase<TContext> action)
    {
        action.WhenCheckingLaunchCapability -= Check;
    }

    protected abstract bool Check();
}