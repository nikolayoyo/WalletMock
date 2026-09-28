namespace ExWo.Wallet.Domain;

public record LedgerEntry(TransactionType Type, decimal Amount, DateTime Timestamp);
