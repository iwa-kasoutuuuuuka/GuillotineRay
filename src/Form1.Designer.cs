namespace GuillotineRay;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.btnStart = new Button();
        this.btnStop = new Button();
        this.lstLog = new ListView();
        this.numThreshold = new NumericUpDown();
        this.numInterval = new NumericUpDown();
        // ... (省略箇所は Form1.cs で手動配置)
        this.SuspendLayout();
        
        this.Text = "Guillotine Ray";
        this.Size = new Size(800, 500);
        this.BackColor = Color.FromArgb(30, 30, 30);
        this.ForeColor = Color.White;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        
        this.ResumeLayout(false);
    }

    private Button btnStart;
    private Button btnStop;
    private ListView lstLog;
    private NumericUpDown numThreshold;
    private NumericUpDown numInterval;
    private TextBox txtFolder;
    private NumericUpDown[] numRoi = new NumericUpDown[4];
}
