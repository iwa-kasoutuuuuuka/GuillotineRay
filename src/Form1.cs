using System.Drawing.Imaging;

namespace GuillotineRay;

public partial class Form1 : Form
{
    private readonly ImageMatcher _matcher = new();
    private readonly ScreenCapture _capture = new();
    private GlobalHotkey? _hotkey;
    private CancellationTokenSource? _cts;
    private bool _active = false;
    private string _currentLang = "JP";
    private readonly Dictionary<string, Label> _labels = new();

    public Form1()
    {
        InitializeComponent();
        this.Text = "Guillotine Ray v1.0.3";
        SetupUi();
        SetLanguage("JP");
        this.Load += (s, e) => {
            _hotkey = new GlobalHotkey(this.Handle);
            if (!_hotkey.Success) Log(_currentLang == "JP" ? "エラー: F8キーの登録に失敗しました。" : "Error: Failed to register F8 key.");
        };
        this.FormClosing += (s, e) => { _hotkey?.Dispose(); _matcher.Dispose(); _capture.Dispose(); };
    }

    private void SetupUi()
    {
        int left = 15;
        
        // 言語切り替えボタン
        btnLang = new Button { Text = "JP/EN", Bounds = new Rectangle(210, 5, 50, 22), FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 8) };
        btnLang.Click += (s, e) => SetLanguage(_currentLang == "JP" ? "EN" : "JP");

        // フォルダ選択
        _labels["folder"] = new Label { Text = "テンプレートフォルダ", Bounds = new Rectangle(left, 12, 180, 18), ForeColor = Color.LightGray };
        string defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templates");
        txtFolder = new TextBox { Bounds = new Rectangle(left, 32, 200, 25), Text = Directory.Exists(defaultPath) ? defaultPath : "" };
        var btnPath = new Button { Text = "...", Bounds = new Rectangle(215, 32, 40, 25), FlatStyle = FlatStyle.Flat };
        btnPath.Click += (s, e) => { using var f = new FolderBrowserDialog(); if (f.ShowDialog() == DialogResult.OK) txtFolder.Text = f.SelectedPath; };

        // しきい値
        _labels["thresh"] = new Label { Text = "しきい値 (0.8 - 0.95)", Bounds = new Rectangle(left, 65, 180, 18), ForeColor = Color.LightGray };
        numThreshold = new NumericUpDown { Bounds = new Rectangle(left, 85, 100, 25), DecimalPlaces = 2, Value = 0.9m, Increment = 0.05m, Minimum = 0.1m, Maximum = 1.0m };

        // 間隔
        _labels["interval"] = new Label { Text = "監視間隔 (ミリ秒)", Bounds = new Rectangle(left, 118, 180, 18), ForeColor = Color.LightGray };
        numInterval = new NumericUpDown { Bounds = new Rectangle(left, 138, 100, 25), Maximum = 60000, Value = 5000, Minimum = 10 };

        // ROI
        _labels["roi"] = new Label { Text = "監視範囲 (X, Y, 幅, 高さ)", Bounds = new Rectangle(left, 172, 240, 18), ForeColor = Color.LightGray };
        string[] roiTags = { "X", "Y", "W", "H" };
        for (int i = 0; i < 4; i++) {
            numRoi[i] = new NumericUpDown { Bounds = new Rectangle(left + i * 60, 192, 55, 25), Maximum = 9999 };
        }
        numRoi[2].Value = 300; numRoi[3].Value = 300;

        btnRoi = new Button { Text = "画面から範囲を選択", Bounds = new Rectangle(left, 225, 240, 32), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(60, 60, 60) };
        btnRoi.Click += (s, e) => {
            this.Opacity = 0; using var sel = new SelectionForm();
            if (sel.ShowDialog() == DialogResult.OK) {
                numRoi[0].Value = sel.SelectedRect.X; numRoi[1].Value = sel.SelectedRect.Y;
                numRoi[2].Value = sel.SelectedRect.Width; numRoi[3].Value = sel.SelectedRect.Height;
            }
            this.Opacity = 1;
        };

        // ログ
        lstLog = new ListView { Bounds = new Rectangle(275, 40, 500, 395), View = View.Details, BackColor = Color.Black, ForeColor = Color.Lime, BorderStyle = BorderStyle.None, FullRowSelect = true };
        lstLog.Columns.Add("Time", 70); lstLog.Columns.Add("Log", 420);

        // 操作ボタン
        btnStart = new Button { Text = "START", Bounds = new Rectangle(left, 400, 115, 40), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 122, 204), Font = new Font("Segoe UI", 10, FontStyle.Bold) };
        btnStop = new Button { Text = "STOP (F8)", Bounds = new Rectangle(left + 125, 400, 115, 40), FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(204, 0, 0), Enabled = false, Font = new Font("Segoe UI", 10, FontStyle.Bold) };

        btnStart.Click += (s, e) => Start();
        btnStop.Click += (s, e) => Stop();

        this.Controls.AddRange(new Control[] { btnLang, txtFolder, btnPath, numThreshold, numInterval, btnRoi, lstLog, btnStart, btnStop });
        foreach (var lbl in _labels.Values) this.Controls.Add(lbl);
        this.Controls.AddRange(numRoi);
    }

    private void SetLanguage(string lang)
    {
        _currentLang = lang;
        bool isJp = lang == "JP";

        _labels["folder"].Text = isJp ? "テンプレートフォルダ" : "Template Folder";
        _labels["thresh"].Text = isJp ? "しきい値 (0.8 - 0.95)" : "Threshold (0.8 - 0.95)";
        _labels["interval"].Text = isJp ? "監視間隔 (ミリ秒)" : "Interval (ms)";
        _labels["roi"].Text = isJp ? "監視範囲 (X, Y, 幅, 高さ)" : "ROI (X, Y, W, H)";
        btnRoi.Text = isJp ? "画面から範囲を選択" : "Select ROI on Screen";
        btnStart.Text = isJp ? "監視開始 (START)" : "START";
        btnStop.Text = isJp ? "停止 (STOP) F8" : "STOP (F8)";
        
        Log(isJp ? $"言語を日本語に設定しました" : $"Language set to English");
    }

    private async void Start()
    {
        if (_active) return;
        try { 
            _matcher.Load(txtFolder.Text);
            Log(_currentLang == "JP" ? "テンプレートを読み込みました。" : "Templates loaded.");
        } catch (Exception ex) { Log(ex.Message); return; }

        _active = true; _cts = new CancellationTokenSource();
        UpdateUiState(true);
        Log(_currentLang == "JP" ? "監視を開始しました。" : "Monitoring started.");

        try {
            await Task.Run(() => Loop(_cts.Token), _cts.Token);
        } catch (OperationCanceledException) { } catch (Exception ex) { Log(ex.Message); } finally { Stop(); }
    }

    private void Stop()
    {
        if (!_active) return;
        _cts?.Cancel(); _active = false;
        UpdateUiState(false);
        Log(_currentLang == "JP" ? "監視を停止しました。" : "Monitoring stopped.");
    }

    private void UpdateUiState(bool running)
    {
        if (InvokeRequired) { BeginInvoke(new Action(() => UpdateUiState(running))); return; }
        btnStart.Enabled = !running;
        btnStop.Enabled = running;
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
                Log(_currentLang == "JP" ? $"検出: {res.Name} (一致度: {res.Score:F3})" : $"Hit: {res.Name} ({res.Score:F3})");
                _matcher.SaveDebugImage(bmp, res, roi);
                await InputController.ClickAtAsync(res.Center.X, res.Center.Y);
                await Task.Delay(50, ct); 
            } else if (res.Score == -100) {
                Log(_currentLang == "JP" ? $"警告: {res.Name} が監視範囲より大きいです。" : $"Warning: {res.Name} is larger than ROI.");
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
