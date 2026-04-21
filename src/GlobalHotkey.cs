using System.Runtime.InteropServices;

namespace GuillotineRay;

public class GlobalHotkey : IDisposable
{
    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    private readonly IntPtr _hWnd;
    private const int ID = 0xAA01;
    public event Action? Pressed;
    public bool Success { get; private set; }

    public GlobalHotkey(IntPtr hWnd)
    {
        _hWnd = hWnd;
        Success = RegisterHotKey(_hWnd, ID, 0, 0x77); // F8
    }

    public void ProcessMessage(int msg, IntPtr wParam)
    {
        if (msg == 0x0312 && wParam.ToInt32() == ID) Pressed?.Invoke();
    }

    public void Dispose()
    {
        UnregisterHotKey(_hWnd, ID);
        GC.SuppressFinalize(this);
    }
}
