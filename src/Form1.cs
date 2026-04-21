using System.Drawing.Imaging;

namespace GuillotineRay;

public partial class Form1 : Form
{
    private readonly ImageMatcher _matcher = new();
    private readonly ScreenCapture _capture = new();
    private GlobalHotkey? _hotkey;
    private CancellationTokenSource? _cts;
    private bool _active = false;

    public Form1()
    {
        InitializeComponent();
        SetupUi();
        this.Load += (s, e) => _hotkey = new GlobalHotkey(this.Handle);
    }

    private void SetupUi()
    {
        // フォルダ選択
        string defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templates");
        txtFolder = new TextBox { Bounds = new Rectangle(10, 30, 200, 25), Text = Directory.Exists(defaultPath) ? defaultPath : "" };
        var btnPath = new Button { Text = "...", Bounds = new Rectangle(215, 30, 40, 25) };
        btnPath.Click += (s, e) => { using var f = new FolderBrowserDialog(); if (f.ShowDialog() == DialogResult.OK) txtFolder.Text = f.SelectedPath; };

        // 設定値
        numThreshold = new NumericUpDown { Bounds = new Rectangle(10, 80, 100, 25), DecimalPlaces = 2, Value = 0.9m, Increment = 0.05m };
        numInterval = new NumericUpDown { Bounds = new Rectangle(10, 130, 100, 25), Maximum = 60000, Value = 5000 };

        // ROI
        for (int i = 0; i < 4; i++) numRoi[i] = new NumericUpDown { Bounds = new Rectangle(10 + i * 55, 180, 50, 25), Maximum = 9999 };
        numRoi[2].Value = 300; numRoi[3].Value = 300;

        var btnRoi = new Button { Text = "Set ROI", Bounds = new Rectangle(10, 210, 245, 30) };
        btnRoi.Click += (s, e) => {
            this.Opacity = 0; using var sel = new SelectionForm();
            if (sel.ShowDialog() == DialogResult.OK) {
                numRoi[0].Value = sel.SelectedRect.X; numRoi[1].Value = sel.SelectedRect.Y;
                numRoi[2].Value = sel.SelectedRect.Width; numRoi[3].Value = sel.SelectedRect.Height;
            }
            this.Opacity = 1;
        };

        // ログ
        lstLog = new ListView { Bounds = new Rectangle(270, 30, 500, 400), View = View.Details, BackColor = Color.Black, ForeColor = Color.Green };
        lstLog.Columns.Add("Time", 80); lstLog.Columns.Add("Log", 400);

        // ボタン
        btnStart = new Button { Text = "START", Bounds = new Rectangle(10, 400, 120, 40), BackColor = Color.DarkGreen };
        btnStop = new Button { Text = "STOP", Bounds = new Rectangle(135, 400, 120, 40), BackColor = Color.DarkRed, Enabled = false };

        btnStart.Click += (s, e) => Start();
        btnStop.Click += (s, e) => Stop();

        this.Controls.AddRange(new Control[] { txtFolder, btnPath, numThreshold, numInterval, btnRoi, lstLog, btnStart, btnStop });
        this.Controls.AddRange(numRoi);
    }

    private async void Start()
    {
        if (_active) return;
        try { _matcher.Load(txtFolder.Text); } catch (Exception ex) { Log(ex.Message); return; }

        _active = true; _cts = new CancellationTokenSource();
        btnStart.Enabled = false; btnStop.Enabled = true;
        Log("Monitoring started.");

        try {
            await Task.Run(() => Loop(_cts.Token), _cts.Token);
        } catch (OperationCanceledException) { } catch (Exception ex) { Log(ex.Message); } finally { Stop(); }
    }

    private void Stop()
    {
        if (!_active) return;
        _cts?.Cancel(); _active = false;
        btnStart.Enabled = true; btnStop.Enabled = false;
        Log("Monitoring stopped.");
    }

    private async Task Loop(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            Rectangle roi = new(); double threshold = 0; int interval = 0;
            this.Invoke(() => {
                roi = new Rectangle((int)numRoi[0].Value, (int)numRoi[1].Value, (int)numRoi[2].Value, (int)numRoi[3].Value);
                threshold = (double)numThreshold.Value;
                interval = (int)numInterval.Value;
            });

            using var bmp = _capture.CaptureArea(roi);
            var res = _matcher.FindBest(bmp, threshold, roi);

            if (res.Found) {
                Log($"Hit: {res.Name} ({res.Score:F3})");
                _matcher.SaveDebugImage(bmp, res, roi);
                await InputController.ClickAtAsync(res.Center.X, res.Center.Y);
                await Task.Delay(50, ct); 
            } else if (res.Score == -100) {
                Log($"Warning: {res.Name} is larger than ROI.");
                await Task.Delay(interval, ct);
            } else {
                await Task.Delay(interval, ct);
            }
            await Task.Yield();
        }
    }

    private void Log(string msg) {
        this.Invoke(() => {
            var item = new ListViewItem(DateTime.Now.ToString("HH:mm:ss"));
            item.SubItems.Add(msg); lstLog.Items.Insert(0, item);
            if (lstLog.Items.Count > 100) lstLog.Items.RemoveAt(100);
        });
    }

    protected override void WndProc(ref Message m) {
        _hotkey?.ProcessMessage(m.Msg, m.WParam);
        if (m.Msg == 0x0312 && _active) Stop(); // F8で停止
        base.WndProc(ref m);
    }
}
