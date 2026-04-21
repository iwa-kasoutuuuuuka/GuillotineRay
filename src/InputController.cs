using System.Runtime.InteropServices;

namespace GuillotineRay;

/// <summary>
/// マウス入力をシミュレートするユーティリティ
/// </summary>
public static class InputController
{
    [StructLayout(LayoutKind.Sequential)]
    struct MOUSEINPUT { public int dx; public int dy; public uint mouseData; public uint dwFlags; public uint time; public IntPtr dwExtraInfo; }
    [StructLayout(LayoutKind.Explicit)]
    struct INPUT { [FieldOffset(0)] public uint type; [FieldOffset(8)] public MOUSEINPUT mi; }

    [DllImport("user32.dll")]
    static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int x, int y);

    private const uint INPUT_MOUSE = 0;
    private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const uint MOUSEEVENTF_LEFTUP = 0x0004;

    public static async Task ClickAtAsync(int x, int y)
    {
        SetCursorPos(x, y);

        var down = new INPUT { type = INPUT_MOUSE };
        down.mi.dwFlags = MOUSEEVENTF_LEFTDOWN;

        var up = new INPUT { type = INPUT_MOUSE };
        up.mi.dwFlags = MOUSEEVENTF_LEFTUP;

        SendInput(1, new[] { down }, Marshal.SizeOf(typeof(INPUT)));
        await Task.Delay(100); // 要件: 100ms 待機
        SendInput(1, new[] { up }, Marshal.SizeOf(typeof(INPUT)));
    }
}
