namespace GuillotineRay;

public class Config
{
    public string TemplatePath { get; set; } = "";
    public double Threshold { get; set; } = 0.9;
    public int IntervalMs { get; set; } = 5000;
    
    public int RoiX { get; set; } = 0;
    public int RoiY { get; set; } = 0;
    public int RoiW { get; set; } = 300;
    public int RoiH { get; set; } = 300;

    public bool DebugMode { get; set; } = true;
}
