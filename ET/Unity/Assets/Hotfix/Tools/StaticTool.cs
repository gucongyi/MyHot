
using Company.Cfg;

public static class StaticTool
{
    public static Config TabToyDataConfig;
    public static void Dispose()
    {
        TabToyDataConfig = null;
    }
}
