using System;
using System.Numerics;
using OpenTabletDriver.Plugin;
using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;

namespace AntiSmoothingPlugin
{
    // The tablet's firmware smooths pen positions with what measurements show
    // is an exponential moving average: reported[n] = a*raw[n] + (1-a)*reported[n-1].
    // This filter first inverts that exactly to recover the real raw position,
    // then predicts slightly ahead using the recovered velocity to cancel the
    // resulting lag. See research/notes.md for how "a" was measured.
    [PluginName("Anti-Smoothing Predictor")]
    public class ExtrapolationFilter : IPositionedPipelineElement<IDeviceReport>
    {
        [Property("Smoothing Alpha"), DefaultPropertyValue(0.4f)]
        public float Alpha { get; set; } = 0.4f;

        [Property("Prediction Multiplier")]
        public float Multiplier { get; set; } = 0.5f;

        private Vector2? _lastReported;
        private Vector2? _lastRaw;

        public event Action<IDeviceReport>? Emit;

        public PipelinePosition Position => PipelinePosition.PreTransform;

        public void Consume(IDeviceReport report)
        {
            if (report is ITabletReport tabletReport)
            {
                Vector2 reported = tabletReport.Position;
                float a = Math.Clamp(Alpha, 0.05f, 1f);

                if (_lastReported is Vector2 lastReported && _lastRaw is Vector2 lastRaw)
                {
                    Vector2 raw = (reported - (1 - a) * lastReported) / a;
                    Vector2 velocity = raw - lastRaw;

                    tabletReport.Position = raw + velocity * Multiplier;
                    report = tabletReport;

                    _lastRaw = raw;
                }
                else
                {
                    _lastRaw = reported;
                }

                _lastReported = reported;
            }

            Emit?.Invoke(report);
        }
    }
}
