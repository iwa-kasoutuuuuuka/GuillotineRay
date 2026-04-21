# Guillotine Ray 🗡️⚡ (ギロチン・レイ) v1.1.1

![Guillotine Ray Logo](docs/logo.png)

## 📦 ダウンロード
- **[GuillotineRay_v1.1.1.zip](https://github.com/iwa-kasoutuuuuuka/GuillotineRay/raw/main/releases/GuillotineRay_v1.1.1.zip)** (Windows 64-bit)

**Guillotine Ray** は、C# と OpenCV を駆使して開発された、高性能かつ精密なスクリーン監視型オートクリッカーです。

---

## 🎯 このアプリでできること（活用例）

画像認識（テンプレートマッチング）を用いることで、従来の座標固定型クリッカーでは不可能だった「特定の状態」になった時だけの操作が可能です。

-   **AI・開発ツールの自動化**: 
    - **Antigravity** などの AI アシストツールで、特定の応答待ちボタンや再試行ボタンが出た瞬間に自動クリックし、作業をノンストップで進める。
-   **ゲーム・エンタメの効率化**:
    - 画面に「READY」や「GO」が出た瞬間の最速クリック。
    - 出現率の低いアイテムやボタンの長期間監視。
-   **ビジネス・ルーチンワーク**:
    - ブラウザの更新ボタンを監視し、特定の変更があった時だけ通知（クリック）する。
    - 処理完了のダイアログが出た瞬間に「OK」を自動で押す。

---

## 🌟 主な特徴 (v1.1.1)

- **超精密クリック**: Win32 API の絶対座標系（0-65535）を用いた最高精度の入力。
- **高視認性 UI**: 黒背景に白文字のハイコントラストデザイン。
- **簡単セットアップ**: 動作に必要なライブラリ (VC++ Redist) を同梱済み。
- **マルチモニター対応**: スケーリングが異なる環境でも座標を正確に計算。
- **緊急停止機能**: F8 キーで即座に動作を中断。

## 📖 使い方
詳細な手順は [取扱説明書 (docs/manual_ja.md)](docs/manual_ja.md) を参照してください。

---

## 🌍 Overview (English)
**Guillotine Ray** is a high-performance screen monitoring and precision auto-clicking tool.

### Use Cases
- **AI Tool Automation**: Automate repetitive clicks in tools like **Antigravity** to keep your workflow fluid.
- **Gaming**: High-speed reaction based on visual cues.
- **Workflow Optimization**: Monitor specific UI states and trigger actions immediately.

## ⚖️ License
This project is licensed under the [MIT License](LICENSE).