# Guillotine Ray - 製品仕様書 (Technical Specification)

## 1. ソフトウェア概要
**Guillotine Ray** は、C# .NET 8 と OpenCV を統合した、高精度・低負荷な Windows 用スクリーン監視型オートクリッカーです。

## 2. システムアーキテクチャ
本ソフトウェアは以下の主要コンポーネントで構成されています。

| クラス名 | 役割 | 技術詳細 |
| :--- | :--- | :--- |
| `Form1` | メインUI / 制御ループ | `Task.Run` による非同期監視、F8 ホットキー監視 |
| `ImageMatcher` | 画像認識エンジン | OpenCV `CCoeffNormed` アルゴリズム、前回ヒットキャッシュ |
| `ScreenCapture` | 画面取得 | `GDI+` による ROI（関心領域）限定キャプチャ、メモリ管理 |
| `SelectionForm` | 範囲選択インターフェース | 全画面（VirtualScreen）オーバーレイ、ドラッグ追従 |
| `InputController` | 入力シミュレーション | Win32 `SendInput` による低レベルマウス制御 |
| `GlobalHotkey` | 強制停止機能 | Win32 `RegisterHotKey` によるシステム全体でのF8検知 |

## 3. 技術的特徴
- **マルチモニター対応**: 仮想デスクトップ全体の座標系をサポート。
- **高DPI対応**: `PerMonitorV2` モードにより、拡大設定 (125%, 150%) 下でも座標ずれが発生しません。
- **管理者権限要求**: `app.manifest` により、特権が必要なアプリケーションへの入力送信を保証。
- **最適化**: キャプチャ範囲を限定し、OpenCV 処理をグレースケール化することで極めて低い CPU 使用率を実現。

## 4. 開発環境
- **言語**: C# 12.0
- **フレームワーク**: .NET 8.0 Windows (WinForms)
- **依存ライブラリ**: OpenCvSharp4 (v4.10.0)
- **プラットフォーム**: Windows 10 / 11 (x64)
