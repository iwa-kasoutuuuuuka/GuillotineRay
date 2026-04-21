# Guillotine Ray 🗡️⚡ (ギロチン・レイ)

![Guillotine Ray Logo](docs/logo.png)

## 📦 ダウンロード
- **[GuillotineRay_v1.0.9.zip](https://github.com/iwa-kasoutuuuuuka/GuillotineRay/raw/main/releases/GuillotineRay_v1.0.9.zip)** (Windows 64-bit)

**Guillotine Ray** は、C# と OpenCV を駆使して開発された、高性能かつ精密なスクリーン監視型オートクリッカーです。安定性と速度を追求し、高DPI環境下でもピクセル単位の正確な動作を提供します。

---

## 🌟 主な特徴 (v1.0.9)

- **超精密クリックロジック**: Win32 API の絶対座標系（0-65535）を用いた高い同期性と正確性。
- **高視認性 UI**: 黒背景に白文字のハイコントラストデザインを採用し、操作ミスを防止。
- **簡単セットアップ**: OpenCV の動作に必要なライブラリ（VC++ 再頒布可能パッケージ）を同梱。
- **マルチモニター対応**: Windows のスケーリング設定を考慮した正確な座標計算。
- **低負荷キャプチャ**: 関心領域（ROI）のみをスキャンし、CPU消費を最小化。
- **緊急停止機能**: グローバルホットキー（F8）により、いつでも即座に動作を停止可能。

## 📖 使い方
詳細な手順は [取扱説明書 (docs/manual_ja.md)](docs/manual_ja.md) を参照してください。

---

## 🌍 English Overview

**Guillotine Ray** is a professional-grade, high-performance screen monitoring and precision auto-clicking tool built with C# and OpenCV.

### Key Features
- **Ultra-High Precision**: New normalized coordinate system for sub-pixel accuracy.
- **Improved UI Visibility**: Enhanced contrast dark-mode interface.
- **One-Click Stability**: Integrated VC++ Redistributable installer included.
- **DPI & Multi-Monitor Aware**: Fully compatible with PerMonitorV2 scaling.

## ⚖️ License
This project is licensed under the [MIT License](LICENSE).