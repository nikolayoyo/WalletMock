using ExWo.Wallet.Domain;

namespace ExWo.Wallet.Application;

public class WalletService
{
    private readonly PlayerWallet _wallet;

    public WalletService(PlayerWallet wallet) => _wallet = wallet;

    public Result<decimal> Deposit(decimal amount)
    {
        if (amount <= 0)
            return Result<decimal>.Fail("Deposit amount must be positive.");

        _wallet.Apply(new LedgerEntry(TransactionType.Deposit, amount, DateTime.UtcNow));
        return Result<decimal>.Ok(_wallet.Balance);
    }

    public Result<decimal> Withdraw(decimal amount)
    {
        if (amount <= 0)
            return Result<decimal>.Fail("Withdrawal amount must be positive.");

        if (_wallet.Balance < amount)
            return Result<decimal>.Fail("Insufficient funds.");

        _wallet.Apply(new LedgerEntry(TransactionType.Withdrawal, -amount, DateTime.UtcNow));
        return Result<decimal>.Ok(_wallet.Balance);
    }
}
