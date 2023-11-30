public interface IStoppable
{
    public bool Active { get; }

    void Stop();
}