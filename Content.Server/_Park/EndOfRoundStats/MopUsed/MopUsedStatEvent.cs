using Content.Shared.FixedPoint;
using Robust.Shared.GameObjects;

namespace Content.Server._Park.EndOfRoundStats.MopUsed;

public sealed class MopUsedStatEvent : EntityEventArgs
{
    public EntityUid Mopper;
    public FixedPoint2 AmountMopped;

    public MopUsedStatEvent(EntityUid mopper, FixedPoint2 amountMopped)
    {
        Mopper = mopper;
        AmountMopped = amountMopped;
    }
}
