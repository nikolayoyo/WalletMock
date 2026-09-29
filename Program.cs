using ExWo.Wallet.Application;
using ExWo.Wallet.Betting;
using ExWo.Wallet.Domain;
using ExWo.Wallet.CmdParser;

var wallet = new PlayerWallet();
var walletService = new WalletService(wallet);
var bettingService = new BettingService(wallet, new SlotGameEngine(new SystemRandomProvider()));

while (true)
{
    Console.Write("Please, submit action:\n");
    var input = Console.ReadLine()?.Trim() ?? string.Empty;

    if (string.IsNullOrEmpty(input))
        continue;

    var parsed = CommandParser.Parse(input);
    if (!parsed.IsSuccess)
    {
        Console.WriteLine($"{parsed.Error}\n");
        continue;
    }

    switch (parsed.Value)
    {
        case ExitCommand:
            Console.WriteLine("Thank you for playing! Hope to see you again soon.");
            return;

        case DepositCommand cmd:
        {
            var result = walletService.Deposit(cmd.Amount);
            Console.WriteLine(result.IsSuccess
                ? $"Your deposit of ${cmd.Amount} was successful. Your current balance is: ${result.Value}\n"
                : $"{result.Error}\n");
            break;
        }

        case WithdrawCommand cmd:
        {
            var result = walletService.Withdraw(cmd.Amount);
            Console.WriteLine(result.IsSuccess
                ? $"Your withdrawal of ${cmd.Amount} was successful. Your current balance is: ${result.Value}\n"
                : $"{result.Error}\n");
            break;
        }

        case BetCommand cmd:
        {
            var result = bettingService.PlaceBet(cmd.Stake);
            if (!result.IsSuccess)
            {
                Console.WriteLine($"{result.Error}\n");
                break;
            }
            Console.WriteLine(result.Value.Won
                ? $"Congrats - you won ${result.Value.Payout}! Your current balance is: ${result.Value.NewBalance}\n"
                : $"No luck this time! Your current balance is: ${result.Value.NewBalance}\n");
            break;
        }
    }
}
