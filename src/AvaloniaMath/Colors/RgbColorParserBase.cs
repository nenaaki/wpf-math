using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using WpfMath.Utils;

namespace WpfMath.Colors
{
    /// <summary>A generic parser class for RGB color.</summary>
    /// <typeparam name="T">Type of component value (e.g. integer or double).</typeparam>
    internal abstract class RgbColorParserBase<T> : FixedComponentCountColorParser where T : struct
    {
        private readonly AlphaChannelMode _alphaChannelMode;

        protected RgbColorParserBase(AlphaChannelMode alphaChannelMode)
            : base(alphaChannelMode == AlphaChannelMode.None ? 3 : 4)
        {
            _alphaChannelMode = alphaChannelMode;
        }

        protected abstract T DefaultAlpha { get; }

        protected abstract T? TryParseComponent(string component);
        protected abstract byte GetByteValue(T val);

        protected override Color? ParseComponents(List<string> components)
        {
            var values = components
                .Select(TryParseComponent)
                .ToArray();
            var index = 0;
            T? alpha = DefaultAlpha;
            if (_alphaChannelMode == AlphaChannelMode.AlphaFirst)
                alpha = values[index++];

            var r = values[index++];
            var g = values[index++];
            var b = values[index++];

            if (_alphaChannelMode == AlphaChannelMode.AlphaLast)
                alpha = values[index];

            return alpha == null || r == null || g == null || b == null
                ? (Color?) null
                : Color.FromArgb(
                    GetByteValue(alpha.Value),
                    GetByteValue(r.Value),
                    GetByteValue(g.Value),
                    GetByteValue(b.Value));
        }
    }
}
