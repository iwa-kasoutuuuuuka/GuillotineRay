namespace GuillotineRay;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        // DPI 意識 (PerMonitorV2) はプロジェクト設定および manifest でも定義済み
        Application.Run(new Form1());
    }
}
