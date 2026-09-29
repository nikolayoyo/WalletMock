namespace ExWo.Wallet.CmdParser;

public interface ICommand { }

public record DepositCommand(decimal Amount) : ICommand;
public record WithdrawCommand(decimal Amount) : ICommand;
public record BetCommand(decimal Stake) : ICommand;
public record ExitCommand : ICommand;
