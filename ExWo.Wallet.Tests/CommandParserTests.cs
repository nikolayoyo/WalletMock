using ExWo.Wallet.CmdParser;

namespace ExWo.Wallet.Tests;

public class CommandParserTests
{
    [Fact]
    public void Parse_Exit_ReturnsExitCommand()
    {
        var result = CommandParser.Parse("exit");
        Assert.True(result.IsSuccess);
        Assert.IsType<ExitCommand>(result.Value);
    }

    [Theory]
    [InlineData("deposit 50", 50)]
    [InlineData("deposit 10.50", 10.50)]
    public void Parse_Deposit_ReturnsDepositCommand(string input, decimal expected)
    {
        var result = CommandParser.Parse(input);
        Assert.True(result.IsSuccess);
        var cmd = Assert.IsType<DepositCommand>(result.Value);
        Assert.Equal(expected, cmd.Amount);
    }

    [Theory]
    [InlineData("withdraw 20", 20)]
    [InlineData("withdraw 5.99", 5.99)]
    public void Parse_Withdraw_ReturnsWithdrawCommand(string input, decimal expected)
    {
        var result = CommandParser.Parse(input);
        Assert.True(result.IsSuccess);
        var cmd = Assert.IsType<WithdrawCommand>(result.Value);
        Assert.Equal(expected, cmd.Amount);
    }

    [Theory]
    [InlineData("bet 5", 5)]
    [InlineData("bet 1", 1)]
    public void Parse_Bet_ReturnsBetCommand(string input, decimal expected)
    {
        var result = CommandParser.Parse(input);
        Assert.True(result.IsSuccess);
        var cmd = Assert.IsType<BetCommand>(result.Value);
        Assert.Equal(expected, cmd.Stake);
    }

    [Fact]
    public void Parse_UnknownVerb_Fails()
    {
        var result = CommandParser.Parse("dance");
        Assert.False(result.IsSuccess);
    }

    [Theory]
    [InlineData("deposit")]
    [InlineData("deposit abc")]
    [InlineData("withdraw")]
    [InlineData("bet")]
    public void Parse_MissingOrInvalidAmount_Fails(string input)
    {
        var result = CommandParser.Parse(input);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Parse_IsCaseInsensitive()
    {
        var result = CommandParser.Parse("DEPOSIT 10");
        Assert.True(result.IsSuccess);
        Assert.IsType<DepositCommand>(result.Value);
    }
}
