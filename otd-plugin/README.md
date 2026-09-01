# Anti-Smoothing Predictor (OpenTabletDriver plugin)

A filter plugin for [OpenTabletDriver (OTD)](https://opentabletdriver.net/) that counteracts heavy hardware smoothing built into certain drawing tablets (such as the XP-Pen G640 Rev B) by predicting pen coordinates ahead of the incoming report, reducing perceived input lag.

Built and tested against **OpenTabletDriver v0.6.7**.

## How It Works
The plugin sits in OTD's filter pipeline and intercepts pen position reports before they reach your output mode.

Measurements (see [`../research`](../research)) show the tablet's firmware smooths pen positions with an exponential moving average: `reported[n] = a*raw[n] + (1-a)*reported[n-1]`, with `a` around 0.4. The plugin does two things:

1. **De-smooths**: inverts that formula exactly to reconstruct the real raw pen position from the smoothed reports.
2. **Predicts ahead**: uses the reconstructed velocity to project the position slightly forward, scaled by the Prediction Multiplier, canceling the remaining latency.

This is a real inverse of the measured filter, not a guess, so it should track the tablet's actual behavior more closely than a plain extrapolation would.

## Requirements
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- OpenTabletDriver v0.6.6+ (plugin API target: `OpenTabletDriver.Plugin` 0.6.7)

## Installation (Windows)

1. **Compile the plugin:**
   - Open a command prompt in this folder (`otd-plugin/`).
   - Run:
     ```
     dotnet build -c Release
     ```
2. **Locate the built `.dll`:**
   - Find it at `bin\Release\net8.0\AntiSmoothingPlugin.dll`.
3. **Install to OpenTabletDriver:**
   - In OTD, go to **Settings → Open Plugin Directory** (or press `Win + R`, paste `%localappdata%\OpenTabletDriver\Plugins`, and press Enter).
   - Create a subfolder for the plugin, e.g. `AntiSmoothingPlugin`, and copy `AntiSmoothingPlugin.dll` into it. OTD loads plugins from their own subdirectories, so avoid dropping the raw `.dll` directly into the root `Plugins` folder.
4. **Activate:**
   - Fully restart OpenTabletDriver.
   - Go to the **Filters** tab.
   - **Anti-Smoothing Predictor** will appear in the filter list — click it and check **Enable Anti-Smoothing Predictor**.
   - Click **Apply**.

## Usage
Once enabled, two fields appear:
- **Smoothing Alpha** (default `0.4`): how strong the tablet's own smoothing is. This was measured, not guessed, but it came from a small sample of test strokes on one unit, so treat it as a starting point. If the cursor feels jittery/noisy, try lowering it a bit; if it still feels laggy, try raising it.
- **Prediction Multiplier** (default `0.5`): how far ahead to predict on top of the de-smoothed signal.
  - **Low values (e.g. 0.1–0.3):** Subtle reduction in latency.
  - **Higher values (e.g. 0.5+):** More aggressive latency reduction, but can introduce jitter or overshoot on fast strokes.

Adjust and click **Apply** to test changes in real time. Find the values that balance responsiveness against stability for your tablet and use case.
