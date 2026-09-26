namespace ExWo.Wallet.Betting;

public class SystemRandomProvider : IRandomProvider
{
    private readonly Random _random = new();

    public double NextDouble() => _random.NextDouble();
}
