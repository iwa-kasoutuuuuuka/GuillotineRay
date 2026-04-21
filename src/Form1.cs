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
        this.Text = "Guillotine Ray v1.0.9";
        SetupUi();
        
        // 起動時の初期化は Load イベントで行う（ハンドル未作成エラー回避のため）
        this.Load += (s, e) => {
            SetLanguage("JP");
            _hotkey = new GlobalHotkey(this.Handle);
            if (!_hotkey.Success) Log(_currentLang == "JP" ? "エラー: F8キーの登録に失敗しました。" : "Error: Failed to register F8 key.");
        };
        this.FormClosing += (s, e) => { _hotkey?.Dispose(); _matcher.Dispose(); _capture.Dispose(); };
    }

    private void SetupUi()
    {
        int left = 15;
        
        // 言語切り替えボタン
        btnLang = new Button { 
            Text = "JP/EN", 
            Bounds = new Rectangle(210, 5, 55, 25), 
            FlatStyle = FlatStyle.Flat, 
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = Color.FromArgb(60, 60, 60)
        };
        btnLang.FlatAppearance.BorderSize = 1;
        btnLang.FlatAppearance.BorderColor = Color.Gray;
        btnLang.Click += (s, e) => SetLanguage(_currentLang == "JP" ? "EN" : "JP");

        // フォルダ選択
        _labels["folder"] = new Label { Text = "テンプレートフォルダ", Bounds = new Rectangle(left, 12, 180, 18), ForeColor = Color.White };
        string defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templates");
        txtFolder = new TextBox { Bounds = new Rectangle(left, 32, 200, 25), Text = Directory.Exists(defaultPath) ? defaultPath : "" };
        var btnPath = new Button { 
            Text = "...", 
            Bounds = new Rectangle(215, 32, 40, 25), 
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.White,
            BackColor = Color.FromArgb(70, 70, 70)
        };
        btnPath.Click += (s, e) => { using var f = new FolderBrowserDialog(); if (f.ShowDialog() == DialogResult.OK) txtFolder.Text = f.SelectedPath; };

        // しきい値
        _labels["thresh"] = new Label { Text = "しきい値 (0.8 - 0.95)", Bounds = new Rectangle(left, 65, 180, 18), ForeColor = Color.White };
        numThreshold = new NumericUpDown { Bounds = new Rectangle(left, 85, 100, 25), DecimalPlaces = 2, Value = 0.9m, Increment = 0.05m, Minimum = 0.1m, Maximum = 1.0m };

        // 間隔
        _labels["interval"] = new Label { Text = "監視間隔 (ミリ秒)", Bounds = new Rectangle(left, 118, 180, 18), ForeColor = Color.White };
        numInterval = new NumericUpDown { Bounds = new Rectangle(left, 138, 100, 25), Maximum = 60000, Value = 5000, Minimum = 10 };

        // ROI
        _labels["roi"] = new Label { Text = "監視範囲 (X, Y, 幅, 高さ)", Bounds = new Rectangle(left, 172, 240, 18), ForeColor = Color.White };
        for (int i = 0; i < 4; i++) {
            numRoi[i] = new NumericUpDown { Bounds = new Rectangle(left + i * 60, 192, 55, 25), Maximum = 9999 };
        }
        numRoi[2].Value = 300; numRoi[3].Value = 300;

        btnRoi = new Button { 
            Text = "画面から範囲を選択", 
            Bounds = new Rectangle(left, 225, 240, 32), 
            FlatStyle = FlatStyle.Flat, 
            BackColor = Color.FromArgb(80, 80, 80),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9, FontStyle.Bold)
        };
        btnRoi.FlatAppearance.BorderSize = 1;
        btnRoi.FlatAppearance.BorderColor = Color.DimGray;
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

        // 実行ボタン
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

        _labels["folder"].Text = isJp ? "繝・Φ繝励Ξ繝ｼ繝医ヵ繧ｩ繝ｫ繝" : "Template Folder";
        _labels["thresh"].Text = isJp ? "縺励″縺・､ (0.8 - 0.95)" : "Threshold (0.8 - 0.95)";
        _labels["interval"].Text = isJp ? "逶｣隕夜俣髫・(繝溘Μ遘・" : "Interval (ms)";
        _labels["roi"].Text = isJp ? "逶｣隕也ｯ・峇 (X, Y, 蟷・ 鬮倥＆)" : "ROI (X, Y, W, H)";
        btnRoi.Text = isJp ? "逕ｻ髱｢縺九ｉ遽・峇繧帝∈謚・ : "Select ROI on Screen";
        btnStart.Text = isJp ? "逶｣隕夜幕蟋・(START)" : "START";
        btnStop.Text = isJp ? "蛛懈ｭ｢ (STOP) F8" : "STOP (F8)";
        
        Log(isJp ? "險隱槭ｒ譌･譛ｬ隱槭↓險ｭ螳壹＠縺ｾ縺励◆" : "Language set to English");
    }

    private async void Start()
    {
        if (_active) return;
        try { 
            _matcher.Load(txtFolder.Text);
            Log(_currentLang == "JP" ? "繝・Φ繝励Ξ繝ｼ繝医ｒ隱ｭ縺ｿ霎ｼ縺ｿ縺ｾ縺励◆縲・ : "Templates loaded.");
        } catch (Exception ex) { Log(ex.Message); return; }

        _active = true; _cts = new CancellationTokenSource();
        UpdateUiState(true);
        Log(_currentLang == "JP" ? "逶｣隕悶ｒ髢句ｧ九＠縺ｾ縺励◆縲・ : "Monitoring started.");

        try {
            await Task.Run(() => Loop(_cts.Token), _cts.Token);
        } catch (OperationCanceledException) { } catch (Exception ex) { Log(ex.Message); } finally { Stop(); }
    }

    private void Stop()
    {
        if (!_active) return;
        _cts?.Cancel(); _active = false;
        UpdateUiState(false);
        Log(_currentLang == "JP" ? "逶｣隕悶ｒ蛛懈ｭ｢縺励∪縺励◆縲・ : "Monitoring stopped.");
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
                Log(_currentLang == "JP" ? $"讀懷・: {res.Name} (荳閾ｴ蠎ｦ: {res.Score:F3})" : $"Hit: {res.Name} ({res.Score:F3})");
                _matcher.SaveDebugImage(bmp, res, roi);
                await InputController.ClickAtAsync(res.Center.X, res.Center.Y);
                await Task.Delay(50, ct); 
            } else if (res.Score == -100) {
                Log(_currentLang == "JP" ? $"隴ｦ蜻・ {res.Name} 縺檎屮隕也ｯ・峇繧医ｊ螟ｧ縺阪＞縺ｧ縺吶・ : $"Warning: {res.Name} is larger than ROI.");
                await Task.Delay(interval, ct);
            } else {
                await Task.Delay(interval, ct);
            }
            await Task.Yield();
        }
    }

    private void Log(string msg) {
        Action action = () => {
            var item = new ListViewItem(DateTime.Now.ToString("HH:mm:ss"));
            item.SubItems.Add(msg); lstLog.Items.Insert(0, item);
            if (lstLog.Items.Count > 100) lstLog.Items.RemoveAt(100);
        };

        if (InvokeRequired && IsHandleCreated) {
            this.Invoke(action);
        } else {
            action();
        }
    }

    protected override void WndProc(ref Message m) {
        _hotkey?.ProcessMessage(m.Msg, m.WParam);
        if (m.Msg == 0x0312 && _active) Stop(); // F8縺ｧ蛛懈ｭ｢
        base.WndProc(ref m);
    }
}
