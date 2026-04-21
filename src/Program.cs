using System;
using System.Windows.Forms;

namespace GuillotineRay;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        
        // グローバルな例外ハンドラを追加
        Application.ThreadException += (s, e) => MessageBox.Show($"Thread Error: {e.Exception.Message}\n{e.Exception.StackTrace}", "Error");
        AppDomain.CurrentDomain.UnhandledException += (s, e) => MessageBox.Show($"Fatal Error: {e.ExceptionObject}", "Fatal Error");

        try
        {
            Application.Run(new Form1());
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Startup Error: {ex.Message}\n{ex.StackTrace}", "Critical Error");
        }
    }
}
