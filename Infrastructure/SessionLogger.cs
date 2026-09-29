namespace ExWo.Wallet.Infrastructure;

public static class SessionLogger
{
    private static string _path = string.Empty;

    public static void Start()
    {
        Directory.CreateDirectory("logs");
        _path = $"logs/session-{DateTime.Now:yyyy-MM-dd-HHmmss}.log";
        File.WriteAllText(_path, $"Session started {DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine}");
    }

    public static void Write(string line) =>
        File.AppendAllText(_path, $"[{DateTime.Now:HH:mm:ss}] {line}{Environment.NewLine}");
}
