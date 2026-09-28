namespace ExWo.Wallet.Domain;

public class PlayerWallet
{
    private readonly List<LedgerEntry> _ledger = new();

    public decimal Balance => _ledger.Sum(e => e.Amount);

    internal void Apply(LedgerEntry entry) => _ledger.Add(entry);
}
