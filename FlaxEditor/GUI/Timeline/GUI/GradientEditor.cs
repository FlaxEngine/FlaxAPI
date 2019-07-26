// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Timeline.GUI
{
    /// <summary>
    /// The color gradient editing control for a timeline media event. Allows to edit the gradients stops to create the linear color animation over time.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class GradientEditor : ContainerControl
    {
        /// <summary>
        /// The gradient stop.
        /// </summary>
        public struct Stop
        {
            /// <summary>
            /// The time position.
            /// </summary>
            public float Time;

            /// <summary>
            /// The value.
            /// </summary>
            public Color Value;
        }

        private List<Stop> _stops = new List<Stop>();

        /// <summary>
        /// Gets or sets the list of gradient stops.
        /// </summary>
        public IList<Stop> Stops
        {
            get => _stops;
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (value.SequenceEqual(_stops))
                    return;

                _stops.Clear();
                _stops.AddRange(value);
                _stops.Sort((a, b) => a.Time > b.Time ? 1 : 0);
            }
        }

        /// <summary>
        /// Occurs when stops collection gets changed (added/removed).
        /// </summary>
        public event Action StopsChanged;

        /// <summary>
        /// Occurs when stops collection gets modified (stop value or time modified).
        /// </summary>
        public event Action Edited;

        /// <summary>
        /// Called when stops collection gets changed (added/removed).
        /// </summary>
        protected virtual void OnStopsChanged()
        {
            StopsChanged?.Invoke();
        }

        /// <summary>
        /// Called when stops collection gets modified (stop value or time modified).
        /// </summary>
        protected virtual void OnEdited()
        {
            Edited?.Invoke();
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            if (IsDisposing)
                return;

            _stops.Clear();
            _stops = null;

            base.Dispose();
        }
    }
}
