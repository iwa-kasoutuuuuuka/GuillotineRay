using System.Drawing;
using System.Windows.Forms;

namespace GuillotineRay;

public class SelectionForm : Form
{
    private Point _start;
    private Rectangle _rect;
    private bool _isDrawing = false;

    public Rectangle SelectedRect => _rect;

    public SelectionForm()
    {
        this.FormBorderStyle = FormBorderStyle.None;
        this.StartPosition = FormStartPosition.Manual;
        this.Bounds = SystemInformation.VirtualScreen; // 全モニターをカバー
        this.BackColor = Color.Black;
        this.Opacity = 0.5;
        this.Cursor = Cursors.Cross;
        this.TopMost = true;
        this.DoubleBuffered = true;
        this.ShowInTaskbar = false;

        this.MouseDown += (s, e) => { _isDrawing = true; _start = e.Location; };
        this.MouseMove += (s, e) => {
            if (_isDrawing) {
                _rect = new Rectangle(Math.Min(_start.X, e.X), Math.Min(_start.Y, e.Y), Math.Abs(_start.X - e.X), Math.Abs(_start.Y - e.Y));
                this.Invalidate();
            }
        };
        this.MouseUp += (s, e) => { 
            if (_rect.Width > 0 && _rect.Height > 0) {
                // スクリーン座標に変換 (VirtualScreen のオフセットを考慮)
                var screenPoint = this.PointToScreen(_rect.Location);
                _rect.Location = screenPoint;
                this.DialogResult = DialogResult.OK; 
            }
            this.Close(); 
        };
        this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) { _rect = Rectangle.Empty; this.Close(); } };
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (_rect != Rectangle.Empty)
        {
            // ローカル座標での描画に変換
            var localRect = this.RectangleToClient(_rect);
            using var pen = new Pen(Color.Cyan, 2);
            using var brush = new SolidBrush(Color.FromArgb(50, Color.Cyan));
            e.Graphics.DrawRectangle(pen, localRect);
            e.Graphics.FillRectangle(brush, localRect);
        }
    }
}
