////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows.Profiler
{
    /// <summary>
    /// Draws simple chart.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    internal class SingleChart : Control
    {
        private const float TitleHeight = 20;
        private readonly List<float> _samples;
        private string _sample;
        private int _maxSamples;

        /// <summary>
        /// Gets or sets the chart title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The handler funtion to format sample value for label text.
        /// </summary>
        public Func<float, string> FormatSample = (v) => v.ToString();

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleChart"/> class.
        /// </summary>
        /// <param name="maxSamples">The maximum samples to collect.</param>
        public SingleChart(int maxSamples = 60 * 5)
            : base(0, 0, 100, 60 + TitleHeight)
        {
            _maxSamples = maxSamples;
            _samples = new List<float>(maxSamples);
            _sample = string.Empty;
        }

        /// <summary>
        /// Clears all the samples.
        /// </summary>
        public void Clear()
        {
            _samples.Clear();
            _sample = string.Empty;
        }

        /// <summary>
        /// Adds the sample value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void AddSample(float value)
        {
            if (_samples.Count == _maxSamples)
            {
                _samples.RemoveAt(0);
            }

            _samples.Add(value);
            _sample = FormatSample(value);
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            var style = Style.Current;
            float chartHeight = Height - TitleHeight;
            float pointsOffset = 4.0f;

            // Draw chart
            if (_samples.Count > 0)
            {
                int samplesInViewCount = Math.Min((int)(Width / pointsOffset), _samples.Count);
                float maxValue = _samples[0];
                for (int i = 1; i < samplesInViewCount; i++)
                {
                    maxValue = Mathf.Max(maxValue, _samples[i]);
                }

                var chartRect = new Rectangle(0, 0, Width, chartHeight);
                Render2D.PushClip(ref chartRect);
                Color chartColor = style.BackgroundSelected;
                Vector2 chartRoot = chartRect.BottomRight;
                float samplesRange = maxValue * 1.1f;
                float samplesCoeff = -chartHeight / samplesRange;
                Vector2 posPrev = chartRoot + new Vector2(0, _samples.Last() * samplesCoeff);
                float posX = 0;

                for (int i = _samples.Count - 1; i >= 0; i--)
                {
                    float sample = _samples[i];
                    Vector2 pos = chartRoot + new Vector2(posX, sample * samplesCoeff);
                    Render2D.DrawLine(posPrev, pos, chartColor);
                    posPrev = pos;
                    posX -= pointsOffset;
                }

                Render2D.PopClip();
            }

            // Draw title
            var headerRect = new Rectangle(0, chartHeight, Width, TitleHeight);
            var headerTextRect = new Rectangle(2, chartHeight, Width - 4, TitleHeight);
            Render2D.FillRectangle(headerRect, style.BackgroundNormal);
            Render2D.DrawText(style.FontMedium, Title, headerTextRect, Color.White * 0.8f, TextAlignment.Near, TextAlignment.Center);
            Render2D.DrawText(style.FontMedium, _sample, headerTextRect, Color.White, TextAlignment.Far, TextAlignment.Center);
        }
    }
}
