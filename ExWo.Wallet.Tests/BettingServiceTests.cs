using ExWo.Wallet.Application;
using ExWo.Wallet.Betting;
using ExWo.Wallet.Domain;

namespace ExWo.Wallet.Tests;

public class BettingServiceTests
{
    private PlayerWallet Wallet(decimal seed = 100m)
    {
        var w = new PlayerWallet();
        new WalletService(w).Deposit(seed);
        return w;
    }

    private BettingService Service(PlayerWallet wallet, params double[] rolls) =>
        new(wallet, new SlotGameEngine(new FakeRandomProvider(rolls)));

    [Theory]
    [InlineData(0.99)]
    [InlineData(0)]
    public void PlaceBet_BelowMinimum_Fails(decimal stake)
    {
        var result = Service(Wallet()).PlaceBet(stake);
        Assert.False(result.IsSuccess);
    }

    [Theory]
    [InlineData(10.01)]
    [InlineData(100)]
    public void PlaceBet_AboveMaximum_Fails(decimal stake)
    {
        var result = Service(Wallet()).PlaceBet(stake);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void PlaceBet_InsufficientFunds_Fails()
    {
        var wallet = Wallet(seed: 2m);
        var result = Service(wallet).PlaceBet(5m);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void PlaceBet_Loss_ReducesBalanceByStake()
    {
        var wallet = Wallet(100m);
        var result = Service(wallet, 0.3).PlaceBet(10m);

        Assert.True(result.IsSuccess);
        Assert.False(result.Value.Won);
        Assert.Equal(0m, result.Value.Payout);
        Assert.Equal(90m, result.Value.NewBalance);
    }

    [Fact]
    public void PlaceBet_LowWin_UpdatesBalanceCorrectly()
    {
        var wallet = Wallet(100m);
        var result = Service(wallet, 0.7, 0.5).PlaceBet(10m);

        Assert.True(result.IsSuccess);
        Assert.True(result.Value.Won);
        Assert.Equal(15m, result.Value.Payout);
        Assert.Equal(105m, result.Value.NewBalance);
    }

    [Fact]
    public void PlaceBet_HighWin_UpdatesBalanceCorrectly()
    {
        var wallet = Wallet(100m);
        var result = Service(wallet, 0.95, 0.5).PlaceBet(10m);

        Assert.True(result.IsSuccess);
        Assert.True(result.Value.Won);
        Assert.Equal(60m, result.Value.Payout);
        Assert.Equal(150m, result.Value.NewBalance);
    }
}
