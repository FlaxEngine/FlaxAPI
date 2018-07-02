// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine.Rendering;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Implementation of the GUI control that renders a material in the user-interface. Use materials with <see cref="MaterialDomain.GUI"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    public class GUIMaterialControl : Control
    {
        private MaterialBase _material;

        /// <summary>
        /// Gets or sets the material.
        /// </summary>
        [EditorOrder(0), Tooltip("The material to use for GUI control area rendering. It must be GUI domain.")]
        public MaterialBase Material
        {
            get => _material;
            set => _material = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GUIMaterialControl"/> class.
        /// </summary>
        public GUIMaterialControl()
        : base(0, 0, 64, 64)
        {
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            if (_material && _material.IsLoaded && _material.IsGUI)
            {
                Render2D.DrawMaterial(_material, new Rectangle(Vector2.Zero, Size));
            }
        }
    }
}
