# ExWo Wallet

Console app simulating a player wallet for a slot game — deposit, withdraw, place bets, track balance.

---

## Running the app

```bash
dotnet run
```

Commands:
```
deposit 50
withdraw 20
bet 5
exit
```

All amounts are positive numbers. Bets must be between $1 and $10.

---

## Running tests

```bash
dotnet test ExWo.Wallet.Tests
```

---

## Project structure

```
ExWo.Wallet/
├── Domain/             core types — wallet, ledger, result
├── Application/        use cases — WalletService, BettingService
├── Betting/            slot game engine and RNG abstraction
├── Presentation/       command parsing — turns raw input into typed commands
├── Infrastructure/     session logger
├── logs/               per-session audit logs (gitignored)
└── ExWo.Wallet.Tests/  unit + distribution tests
```

Single project with folder-based layers.

---

## Command parsing

Raw console input goes through `CommandParser`. It returns a typed command (`DepositCommand`, `WithdrawCommand`, `BetCommand`, `ExitCommand`) or a failure result if the input is malformed. `Program.cs` switches on the command type and calls the right service.

---

## Session logs (addition from my side)

Every session writes its own log file under `logs/`:

```
logs/session-2026-09-28-142301.log
```

These local files act as audit logs mocks, which are very important part of the business. 
Kept local and gitignored — useful as a compliance trail during development and demos.

---

## How the wallet works

Balance is never stored directly. Instead, every operation is appended to a `Ledger` and the balance is the sum of all the stored operations.
`WalletService` handles deposits and withdrawals. `BettingService` orchestrates a full bet round: validates stake, checks balance, calls the game engine, then applies debit and credit together so there's never a partially-applied state.

---

## Game rules

- 50% lose
- 40% win up to x2 the stake
- 10% win between x2 and x10 the stake
- New balance = old balance - stake + payout

---