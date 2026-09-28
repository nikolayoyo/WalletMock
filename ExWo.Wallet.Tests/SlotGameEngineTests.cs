using ExWo.Wallet.Betting;

namespace ExWo.Wallet.Tests;

public class SlotGameEngineTests
{
    [Fact]
    public void Play_LossBand_ReturnsNoPayout()
    {
        var engine = new SlotGameEngine(new FakeRandomProvider(0.49));
        var outcome = engine.Play(10m);

        Assert.False(outcome.Won);
        Assert.Equal(0m, outcome.Payout);
    }

    [Fact]
    public void Play_LowWinBand_ReturnsPayoutBetweenX1AndX2()
    {
        var engine = new SlotGameEngine(new FakeRandomProvider(0.7, 0.99));
        var outcome = engine.Play(10m);

        Assert.True(outcome.Won);
        Assert.True(outcome.Payout >= 10m && outcome.Payout < 20m);
    }

    [Fact]
    public void Play_HighWinBand_ReturnsPayoutBetweenX2AndX10()
    {
        var engine = new SlotGameEngine(new FakeRandomProvider(0.95, 0.5));
        var outcome = engine.Play(10m);

        Assert.True(outcome.Won);
        Assert.True(outcome.Payout >= 20m && outcome.Payout <= 100m);
    }

    [Fact]
    public void Play_Distribution_MatchesExpectedBands()
    {
        var engine = new SlotGameEngine(new SystemRandomProvider());
        const int rounds = 100_000;
        int losses = 0, lowWins = 0, highWins = 0;

        for (int i = 0; i < rounds; i++)
        {
            var outcome = engine.Play(1m);
            if (!outcome.Won) losses++;
            else if (outcome.Payout < 2m) lowWins++;
            else highWins++;
        }

        Assert.InRange(losses / (double)rounds, 0.47, 0.53);
        Assert.InRange(lowWins / (double)rounds, 0.37, 0.43);
        Assert.InRange(highWins / (double)rounds, 0.07, 0.13);
    }
}
