# Linux (non-OTD) path — planned

For Linux users who talk to the tablet via the kernel `hidraw`/DIGImend
stack directly, without running OpenTabletDriver.

This isn't started yet. It depends on the outcome of the reverse-engineering
work in [`../research`](../research) — once the smoothing model (e.g. EMA
with a fitted alpha) is known from the OTD-based captures, the same inverse
math can be reimplemented here as a small userspace filter sitting between
`hidraw` and `uinput`, without needing OTD at all.

Not needed if you already use OpenTabletDriver on Linux — the plugin in
[`../otd-plugin`](../otd-plugin) works there as-is, since OTD itself is
cross-platform.
