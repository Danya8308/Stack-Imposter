public abstract class ActionComponent<TContext>
{
    public ActionBase<TContext> Skill { get; private set; }

    public TContext Context => Skill.CurrentContext;

    public void Accept(ActionBase<TContext> skill)
    {
        Skill = skill;

        AcceptEvent(skill);
    }

    public void Reset(ActionBase<TContext> skill)
    {
        Skill = null;

        ResetEvent(skill);
    }

    protected virtual void AcceptEvent(ActionBase<TContext> skill) { }

    protected virtual void ResetEvent(ActionBase<TContext> skill) { }
}