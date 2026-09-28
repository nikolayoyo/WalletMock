using System.Globalization;
using ExWo.Wallet.Application;
using ExWo.Wallet.Betting;
using ExWo.Wallet.Domain;

var wallet = new PlayerWallet();
var walletService = new WalletService(wallet);
var bettingService = new BettingService(wallet, new SlotGameEngine(new SystemRandomProvider()));

while (true)
{
    Console.Write("Please, submit action:");
    var input = Console.ReadLine()?.Trim();

    if (string.IsNullOrEmpty(input))
        continue;

    var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var command = parts[0].ToLowerInvariant();

    if (command == "exit")
    {
        Console.WriteLine("Thank you for playing! Hope to see you again soon.");
        break;
    }

    if (command == "deposit")
    {
        if (parts.Length != 2 || !decimal.TryParse(parts[1], NumberStyles.Number, CultureInfo.InvariantCulture, out var amount))
        {
            Console.WriteLine("Type: deposit <amount>");
            continue;
        }

        var result = walletService.Deposit(amount);
        if (!result.IsSuccess)
        {
            Console.WriteLine($"{result.Error}");
            continue;
        }

        Console.WriteLine($"Your deposit of ${amount} was successful. Your current balance is: ${result.Value}");
        continue;
    }

    if (command == "withdraw")
    {
        if (parts.Length != 2 || !decimal.TryParse(parts[1], NumberStyles.Number, CultureInfo.InvariantCulture, out var amount))
        {
            Console.WriteLine("Type: withdraw <amount>");
            continue;
        }

        var result = walletService.Withdraw(amount);
        if (!result.IsSuccess)
        {
            Console.WriteLine($"{result.Error}");
            continue;
        }

        Console.WriteLine($"Your withdrawal of ${amount} was successful. Your current balance is: ${result.Value}");
        continue;
    }

    if (command == "bet")
    {
        if (parts.Length != 2 || !decimal.TryParse(parts[1], NumberStyles.Number, CultureInfo.InvariantCulture, out var stake))
        {
            Console.WriteLine("Type: bet <amount>");
            continue;
        }

        var result = bettingService.PlaceBet(stake);
        if (!result.IsSuccess)
        {
            Console.WriteLine($"{result.Error}");
            continue;
        }

        var message = result.Value.Won
            ? $"Congrats - you won ${result.Value.Payout}! Your current balance is: ${result.Value.NewBalance}"
            : $"No luck this time! Your current balance is: ${result.Value.NewBalance}";

        Console.WriteLine(message);
        continue;
    }

    Console.WriteLine("Unknown command.");
}
