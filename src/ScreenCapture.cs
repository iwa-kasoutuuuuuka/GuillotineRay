using System.Drawing;
using System.Drawing.Imaging;

namespace GuillotineRay;

/// <summary>
/// 指定矩形エリアのキャプチャ機能
/// </summary>
public class ScreenCapture : IDisposable
{
    public Bitmap? LastCapture { get; private set; }

    public Bitmap CaptureArea(Rectangle rect)
    {
        LastCapture?.Dispose();
        LastCapture = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
        
        using (var g = Graphics.FromImage(LastCapture))
        {
            g.CopyFromScreen(rect.Location, Point.Empty, rect.Size, CopyPixelOperation.SourceCopy);
        }
        
        return LastCapture;
    }

    public void Dispose()
    {
        LastCapture?.Dispose();
        GC.SuppressFinalize(this);
    }
}
