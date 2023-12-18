namespace Content.Shared._Park.EndOfRoundStats.CuffedTime; // Handcuffs are shared.

public sealed class CuffedTimeStatEvent : EntityEventArgs
{
    public TimeSpan Duration;

    public CuffedTimeStatEvent(TimeSpan duration)
    {
        Duration = duration;
    }
}
