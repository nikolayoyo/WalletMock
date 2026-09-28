using ExWo.Wallet.Betting;

namespace ExWo.Wallet.Tests;

internal class FakeRandomProvider : IRandomProvider
{
    private readonly Queue<double> _values;

    public FakeRandomProvider(params double[] values) => _values = new Queue<double>(values);

    public double NextDouble() => _values.Dequeue();
}
