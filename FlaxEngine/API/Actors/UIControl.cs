// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine.GUI;

namespace FlaxEngine
{
    public sealed partial class UIControl
    {
        private Control _control;

        /// <summary>
        /// Gets or sets the GUI control used by this actor.
        /// </summary>
        /// <remarks>
        /// When changing the control, the previous one is disposed. Use <see cref="UnlinkControl"/> to manage it by myself.
        /// </remarks>
        [EditorDisplay("Control", EditorDisplayAttribute.InlineStyle), EditorOrder(50)]
        public Control Control
        {
            get => _control;
            set
            {
                if (_control != null)
                {
                    _control.Dispose();
                }

                _control = value;

                if (_control != null)
                {
                    // TODO: link the control to the parent
                }
            }
        }

        /// <summary>
        /// Unlinks the control from the actor without disposing it or modyfing.
        /// </summary>
        public void UnlinkControl()
        {
            _control = null;
        }
    }
}
