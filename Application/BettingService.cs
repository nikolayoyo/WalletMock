using ExWo.Wallet.Betting;
using ExWo.Wallet.Domain;

namespace ExWo.Wallet.Application;

public class BettingService
{
    private readonly PlayerWallet _wallet;
    private readonly SlotGameEngine _gameEngine;

    public BettingService(PlayerWallet wallet, SlotGameEngine gameEngine)
    {
        _wallet = wallet;
        _gameEngine = gameEngine;
    }

    public Result<BetResult> PlaceBet(decimal stake)
    {
        if (stake < 1m || stake > 10m)
            return Result<BetResult>.Fail("Bets must be between $1 and $10.");

        if (_wallet.Balance < stake)
            return Result<BetResult>.Fail("Insufficient funds.");

        var outcome = _gameEngine.Play(stake);

        _wallet.Apply(new LedgerEntry(TransactionType.BetDebit, -stake, DateTime.UtcNow));
        _wallet.Apply(new LedgerEntry(TransactionType.BetCredit, outcome.Payout, DateTime.UtcNow));

        return Result<BetResult>.Ok(new BetResult(outcome.Won, outcome.Payout, _wallet.Balance));
    }
}
