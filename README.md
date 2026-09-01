# Anti-Smoothing Predictor for the XP-Pen G640 Rev B

Solutions for counteracting the heavy hardware smoothing built into the
XP-Pen G640 Rev B (and likely other tablets sharing its firmware), which
adds noticeable input lag.

## Repo layout

- **[`otd-plugin/`](otd-plugin)** — the [OpenTabletDriver (OTD)](https://opentabletdriver.net/) filter plugin. Works on Windows, Linux, and Mac, since OTD itself is cross-platform. This is the solution most people want. Built and tested against **OpenTabletDriver v0.6.7**.
- **[`research/`](research)** — tooling and notes for reverse-engineering the tablet's actual smoothing algorithm (currently the plugin uses a naive linear-extrapolation guess; the goal is to replace it with a real inverse of the firmware's filter).
- **[`linux-hidraw/`](linux-hidraw)** — planned: a non-OTD path for Linux users on the `hidraw`/DIGImend stack. Not started yet; depends on the research findings.

If you just want to use the plugin, see [`otd-plugin/README.md`](otd-plugin/README.md) for details and install steps.

