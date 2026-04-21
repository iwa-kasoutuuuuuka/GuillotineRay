namespace GuillotineRay;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
        this.ClientSize = new System.Drawing.Size(790, 460);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.Name = "Form1";
        this.Text = "Guillotine Ray";
        this.ResumeLayout(false);
    }

    private Button btnStart;
    private Button btnStop;
    private ListView lstLog;
    private NumericUpDown numThreshold;
    private NumericUpDown numInterval;
    private TextBox txtFolder;
    private Button btnRoi;
    private Button btnLang;
    private NumericUpDown[] numRoi = new NumericUpDown[4];
}
