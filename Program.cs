using System.Globalization;
using ExWo.Wallet.Betting;

var random = new SystemRandomProvider();
var gameEngine = new SlotGameEngine(random);

var balance = 0m;

while (true)
{
    Console.Write("Please, submit action:\n");
    var input = Console.ReadLine()?.Trim();

    if (string.IsNullOrEmpty(input))
        continue;

    var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var command = parts[0].ToLowerInvariant();

    if (command == "exit")
    {
        Console.WriteLine("Thanks for playing");
        break;
    }

    if (command == "bet")
    {
        if (parts.Length != 2 || !decimal.TryParse(parts[1], NumberStyles.Number, CultureInfo.InvariantCulture, out var stake))
        {
            Console.WriteLine("Usage: bet <amount>");
            continue;
        }

        if (stake < 1m || stake > 10m)
        {
            Console.WriteLine("Bets must be between $1 and $10.");
            continue;
        }

        var outcome = gameEngine.Play(stake);
        balance = balance - stake + outcome.Payout;

        var message = outcome.Won
            ? $"Congrats - you won ${outcome.Payout}! Your current balance is: ${balance}"
            : $"No luck this time! Your current balance is: ${balance}";

        Console.WriteLine(message);
        continue;
    }

    Console.WriteLine("Unknown command.");
}
