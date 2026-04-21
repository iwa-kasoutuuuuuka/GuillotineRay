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

    public static async Task ClickAtAsync(int x, int y)
    {
        // 画面解像度に基づいた正規化座標 (0-65535) への変換
        int virtualWidth = GetSystemMetrics(78); // SM_CXVIRTUALSCREEN
        int virtualHeight = GetSystemMetrics(79); // SM_CYVIRTUALSCREEN
        int virtualLeft = GetSystemMetrics(76); // SM_XVIRTUALSCREEN
        int virtualTop = GetSystemMetrics(77); // SM_YVIRTUALSCREEN

        int dx = (x - virtualLeft) * 65535 / (virtualWidth - 1);
        int dy = (y - virtualTop) * 65535 / (virtualHeight - 1);

        INPUT moveDown = new INPUT { type = 0 };
        moveDown.mi = new MOUSEINPUT { 
            dx = dx, dy = dy, 
            dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_VIRTUALDESK 
        };

        INPUT moveUp = new INPUT { type = 0 };
        moveUp.mi = new MOUSEINPUT { 
            dx = dx, dy = dy, 
            dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTUP | MOUSEEVENTF_VIRTUALDESK 
        };

        SendInput(1, new INPUT[] { moveDown }, Marshal.SizeOf(typeof(INPUT)));
        await Task.Delay(100);
        SendInput(1, new INPUT[] { moveUp }, Marshal.SizeOf(typeof(INPUT)));
    }

    private const uint MOUSEEVENTF_MOVE = 0x0001;
    private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const uint MOUSEEVENTF_LEFTUP = 0x0004;
    private const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
    private const uint MOUSEEVENTF_VIRTUALDESK = 0x4000;

    [DllImport("user32.dll")]
    private static extern int GetSystemMetrics(int nIndex);
}
