using ExWo.Wallet.Application;
using ExWo.Wallet.Domain;

namespace ExWo.Wallet.Tests;

public class WalletTests
{
    private readonly PlayerWallet _wallet = new();
    private readonly WalletService _service;

    public WalletTests() => _service = new WalletService(_wallet);

    [Fact]
    public void Balance_StartsAtZero() => Assert.Equal(0m, _wallet.Balance);

    [Fact]
    public void Deposit_IncreasesBalance()
    {
        var result = _service.Deposit(50m);

        Assert.True(result.IsSuccess);
        Assert.Equal(50m, result.Value);
    }

    [Fact]
    public void Deposit_NegativeAmount_Fails()
    {
        var result = _service.Deposit(-10m);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Withdraw_DecreasesBalance()
    {
        _service.Deposit(100m);
        var result = _service.Withdraw(40m);

        Assert.True(result.IsSuccess);
        Assert.Equal(60m, result.Value);
    }

    [Fact]
    public void Withdraw_InsufficientFunds_Fails()
    {
        _service.Deposit(10m);
        var result = _service.Withdraw(20m);

        Assert.False(result.IsSuccess);
        Assert.Equal(10m, _wallet.Balance);
    }

    [Fact]
    public void Withdraw_NegativeAmount_Fails()
    {
        var result = _service.Withdraw(-5m);

        Assert.False(result.IsSuccess);
    }
}
