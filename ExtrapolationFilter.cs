using System.Numerics;
using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;

namespace AntiSmoothingPlugin
{
    [PluginName("Anti-Smoothing Predictor")]
    public class ExtrapolationFilter : IAbsoluteFilter
    {
        [Property("Prediction Multiplier")]
        public float Multiplier { get; set; } = 0.5f;

        private Vector2 _lastPoint;
        private bool _initialized;

        public Vector2 Filter(Vector2 point)
        {
            if (!_initialized)
            {
                _lastPoint = point;
                _initialized = true;
                return point;
            }

            Vector2 velocity = point - _lastPoint;
            Vector2 predicted = point + (velocity * Multiplier);
            _lastPoint = point;

            return predicted;
        }
    }
}