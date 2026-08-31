# Anti-Smoothing Predictor for OpenTabletDriver

A custom filter plugin for [OpenTabletDriver (OTD)](https://opentabletdriver.net/) that counteracts heavy hardware smoothing built into certain drawing tablets (such as the XP-Pen G640 Rev B) by predicting pen coordinates ahead of the incoming report, reducing perceived input lag.

Built and tested against **OpenTabletDriver v0.6.7**.

## How It Works
The plugin sits in OTD's filter pipeline and intercepts pen position reports before they reach your output mode. It uses **linear extrapolation**: it tracks the velocity between the last two points and projects the cursor slightly ahead along that vector, scaled by the Prediction Multiplier.

This artificially "reverses" part of the smoothing delay, making the tablet feel snappier and more responsive — useful for fast-paced use cases like osu!.

## Requirements
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- OpenTabletDriver v0.6.6+ (plugin API target: `OpenTabletDriver.Plugin` 0.6.7)

## Installation (Windows)

1. **Compile the plugin:**
   - Open a command prompt in the folder containing the project files.
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
Once enabled, a **Prediction Multiplier** field appears:
- **Low values (e.g. 0.1–0.3):** Subtle reduction in latency.
- **Higher values (e.g. 0.5+):** More aggressive latency reduction, but can introduce jitter or overshoot on fast strokes.

Adjust the value and click **Apply** to test changes in real time. Find the value that balances responsiveness against stability for your tablet and use case.

