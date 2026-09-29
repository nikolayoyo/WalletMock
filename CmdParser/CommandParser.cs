using System.Globalization;
using ExWo.Wallet.Domain;

namespace ExWo.Wallet.CmdParser;

public static class CommandParser
{
    public static Result<ICommand> Parse(string input)
    {
        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var verb = parts[0].ToLowerInvariant();

        if (verb == "exit")
            return Result<ICommand>.Ok(new ExitCommand());

        if (parts.Length != 2)
            return Result<ICommand>.Fail($"Usage: {verb} <amount>");

        if (!decimal.TryParse(parts[1], NumberStyles.Number, CultureInfo.InvariantCulture, out var amount))
            return Result<ICommand>.Fail($"'{parts[1]}' is not a valid amount.");

        return verb switch
        {
            "deposit"  => Result<ICommand>.Ok(new DepositCommand(amount)),
            "withdraw" => Result<ICommand>.Ok(new WithdrawCommand(amount)),
            "bet"      => Result<ICommand>.Ok(new BetCommand(amount)),
            _          => Result<ICommand>.Fail($"Unknown command '{verb}'.")
        };
    }
}
