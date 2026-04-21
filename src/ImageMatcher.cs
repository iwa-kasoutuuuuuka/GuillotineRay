using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;

namespace GuillotineRay;

public class MatchResult
{
    public bool Found { get; set; }
    public double Score { get; set; }
    public System.Drawing.Point Center { get; set; }
    public Rect BoundingBox { get; set; }
    public string Name { get; set; } = "";
}

public class ImageMatcher : IDisposable
{
    private readonly Dictionary<string, Mat> _templates = new();
    private string? _lastMatchedKey;

    public void Load(string path)
    {
        DisposeTemplates();
        if (!Directory.Exists(path)) return;

        foreach (var file in Directory.GetFiles(path, "*.*").Where(f => f.EndsWith(".png") || f.EndsWith(".jpg")))
        {
            var mat = new Mat(file, ImreadModes.Grayscale);
            if (!mat.Empty()) _templates[Path.GetFileName(file)] = mat;
        }
    }

    public MatchResult FindBest(Bitmap sceneBmp, double threshold, Rectangle roi)
    {
        using var sceneMat = sceneBmp.ToMat();
        using var sceneGray = new Mat();
        Cv2.CvtColor(sceneMat, sceneGray, ColorConversionCodes.BGR2GRAY);

        MatchResult best = new() { Found = false, Score = -1 };

        // キャッシュ優先探索
        if (_lastMatchedKey != null && _templates.TryGetValue(_lastMatchedKey, out var lastMat))
        {
            best = Match(sceneGray, lastMat, _lastMatchedKey, threshold, roi);
            if (best.Found) return best;
        }

        foreach (var kv in _templates)
        {
            if (kv.Key == _lastMatchedKey) continue;
            var res = Match(sceneGray, kv.Value, kv.Key, threshold, roi);
            if (res.Score > best.Score) best = res;
        }

        if (best.Found) _lastMatchedKey = best.Name;
        return best;
    }

    private MatchResult Match(Mat scene, Mat template, string name, double threshold, Rectangle roi)
    {
        if (template.Width > scene.Width || template.Height > scene.Height)
        {
            // ログ用にスコアに特殊な値を設定
            return new MatchResult { Found = false, Score = -100, Name = name };
        }

        using var result = new Mat();
        Cv2.MatchTemplate(scene, template, result, TemplateMatchModes.CCoeffNormed);
        Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out var maxLoc);

        if (maxVal >= threshold)
        {
            return new MatchResult
            {
                Found = true,
                Score = maxVal,
                Name = name,
                Center = new System.Drawing.Point(roi.X + maxLoc.X + template.Width / 2, roi.Y + maxLoc.Y + template.Height / 2),
                BoundingBox = new Rect(roi.X + maxLoc.X, roi.Y + maxLoc.Y, template.Width, template.Height)
            };
        }
        return new MatchResult { Found = false, Score = maxVal, Name = name };
    }

    public void SaveDebugImage(Bitmap sceneBmp, MatchResult res, Rectangle roi)
    {
        if (!res.Found) return;
        using var debugMat = sceneBmp.ToMat();
        var relativeRect = new Rect(res.BoundingBox.X - roi.X, res.BoundingBox.Y - roi.Y, res.BoundingBox.Width, res.BoundingBox.Height);
        Cv2.Rectangle(debugMat, relativeRect, Scalar.Red, 2);
        
        string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Debug");
        Directory.CreateDirectory(dir);
        debugMat.SaveImage(Path.Combine(dir, $"debug_{DateTime.Now:HHmmss}.png"));
    }

    private void DisposeTemplates()
    {
        foreach (var t in _templates.Values) t.Dispose();
        _templates.Clear();
        _lastMatchedKey = null;
    }

    public void Dispose()
    {
        DisposeTemplates();
        GC.SuppressFinalize(this);
    }
}
