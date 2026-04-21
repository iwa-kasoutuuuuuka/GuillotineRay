# Guillotine Ray 裡・鞘圍

![Guillotine Ray Logo](docs/logo.png)

## 逃 Download / 繝繧ｦ繝ｳ繝ｭ繝ｼ繝・
- **[GuillotineRay_v1.0.2.zip](https://github.com/iwa-kasoutuuuuuka/GuillotineRay/raw/main/releases/GuillotineRay_v1.0.2.zip)** (Windows 64-bit)

**Guillotine Ray** is a high-performance, precision-engineered screen monitoring and auto-clicking tool built with C# and OpenCV. Designed for stability and speed, it excels in complex automation tasks with DPI-aware accuracy and minimal CPU overhead.

[闍ｱ隱槭・隗｣隱ｬ縺ｯ莉･荳九↓縺ゅｊ縺ｾ縺兢(#English)


## ・・ 譌･譛ｬ隱樊ｦりｦ・

**Guillotine Ray** 縺ｯ縲，# 縺ｨ OpenCV 繧帝ｧ・ｽｿ縺励※髢狗匱縺輔ｌ縺溘・ｫ俶ｧ閭ｽ縺九▽邊ｾ蟇・↑繧ｹ繧ｯ繝ｪ繝ｼ繝ｳ逶｣隕悶・繧ｪ繝ｼ繝医け繝ｪ繝・き繝ｼ縺ｧ縺吶ょｮ牙ｮ壽ｧ縺ｨ騾溷ｺｦ繧定ｿｽ豎ゅ＠縲・ｫ魯PI迺ｰ蠅・ｸ九〒繧ゅヴ繧ｯ繧ｻ繝ｫ蜊倅ｽ阪・豁｣遒ｺ縺ｪ蜍穂ｽ懊ｒ謠蝉ｾ帙＠縺ｾ縺吶・
**Guillotine Ray** 繧ｮ繝ｭ繝√Φ繝ｻ繝ｬ繧､縺ｯ縲ヽUN縺ｨ縺帰llow縺ｨ縺九・繝懊ち繝ｳ繧定・蜍輔け繝ｪ繝・け縺吶ｋ縺縺代・繧｢繝励Μ縺ｧ縺吶・
繝舌げ縺後≠縺｣縺ｦ螟ｱ謨励＠縺ｦ繧よｳ｣縺上↑・・ｼ∽ｽ懆・・荳蛻・ｿ晁ｨｼ縺励↑縺・・

### 検 迚ｹ蠕ｴ
- **OpenCV 繝・Φ繝励Ξ繝ｼ繝医・繝・メ繝ｳ繧ｰ**: `CCoeffNormed` 縺ｫ繧医ｋ鬮倡ｲｾ蠎ｦ讀懷・縲・
- **菴手ｲ闕ｷ繧ｭ繝｣繝励メ繝｣**: 髢｢蠢・伜沺・・OI・峨・縺ｿ繧偵せ繧ｭ繝｣繝ｳ縺励，PU豸郁ｲｻ繧呈怙蟆丞喧縲・
- **繝槭Ν繝√Δ繝九ち繝ｼ蟇ｾ蠢・*: Windows 縺ｮ繧ｹ繧ｱ繝ｼ繝ｪ繝ｳ繧ｰ險ｭ螳壹ｒ閠・・縺励◆豁｣遒ｺ縺ｪ蠎ｧ讓呵ｨ育ｮ励・
- **邂｡逅・・ｨｩ髯仙ｯｾ蠢・*: `SendInput` 繧堤｢ｺ螳溘↓蜍穂ｽ懊＆縺帙ｋ縺溘ａ縺ｮ繝槭ル繝輔ぉ繧ｹ繝亥ｮ溯｣・・
- **繝・ヰ繝・げ繝｢繝ｼ繝・*: 繝偵ャ繝域凾縺ｮ繧ｹ繧ｯ繝ｪ繝ｼ繝ｳ繧ｷ繝ｧ繝・ヨ菫晏ｭ倥→繝舌え繝ｳ繝・ぅ繝ｳ繧ｰ繝懊ャ繧ｯ繧ｹ陦ｨ遉ｺ縲・

### 肌 髢狗匱迺ｰ蠅・
- C# / .NET 8 / Windows Forms
- OpenCvSharp4

## 笞厄ｸ・License
This project is licensed under the [MIT License](LICENSE).





---
<a name="English"></a>
## 噫 Key Features

- **OpenCV Pattern Matching**: High-accuracy detection using `CCoeffNormed` algorithm.
- **Ultra-Low Latency**: Performance optimized with grayscale preprocessing and ROI-limited capture.
- **DPI Aware**: Fully compatible with multi-monitor setups and various scaling factors (100%, 125%, 150%, etc.).
- **Win32 SendInput**: Uses low-level input simulation to bypass common detection methods.
- **Emergency Stop**: Global hotkey (F8) to instantly halt operations.
- **Premium UI**: Modern dark-themed interface with real-time logging and debug visualization.

## 屏・・Architecture

- **.NET 8 / WinForms**: Modern runtime for Windows desktop engineering.
- **OpenCvSharp4**: Powerful computer vision wrapper.
- **Admin-Ready**: Includes application manifest to request required privileges.

## 当 How to Use

1. **Build**: Clone and build using Visual Studio or `dotnet build`.
2. **Templates**: Place target images (.png/.jpg) in a folder.
3. **ROI Selection**: Click "Select ROI" to drag-and-select your monitoring area.
4. **Execution**: Set your threshold (0.8 - 0.95) and interval, then hit "START".

---
