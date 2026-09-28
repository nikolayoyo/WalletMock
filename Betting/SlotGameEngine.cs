namespace ExWo.Wallet.Betting;

public class SlotGameEngine
{
    private readonly IRandomProvider _random;

    public SlotGameEngine(IRandomProvider random)
    {
        _random = random;
    }

    public BetOutcome Play(decimal stake)
    {
        var roll = _random.NextDouble();

        if (roll < 0.5)
            return new BetOutcome(false, 0m, 0m);

        var multiplier = roll < 0.9
            ? 1 + _random.NextDouble()
            : 2 + _random.NextDouble() * 8;

        var payout = Math.Round(stake * (decimal)multiplier, 2);
        return new BetOutcome(true, (decimal)multiplier, payout);
    }
}
