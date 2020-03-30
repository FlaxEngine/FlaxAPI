// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System.ComponentModel;
using FlaxEngine;

namespace FlaxEditor.Options
{
    /// <summary>
    /// Input editor options data container.
    /// </summary>
    [CustomEditor(typeof(Editor<InputOptions>))]
    public sealed class InputOptions
    {
        #region Common

        [DefaultValue(typeof(InputBinding), "Ctrl+S")]
        [EditorDisplay("Common"), EditorOrder(100)]
        public InputBinding Save = new InputBinding(KeyboardKeys.S, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "F2")]
        [EditorDisplay("Common"), EditorOrder(110)]
        public InputBinding Rename = new InputBinding(KeyboardKeys.F2);

        [DefaultValue(typeof(InputBinding), "Ctrl+C")]
        [EditorDisplay("Common"), EditorOrder(120)]
        public InputBinding Copy = new InputBinding(KeyboardKeys.C, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "Ctrl+X")]
        [EditorDisplay("Common"), EditorOrder(130)]
        public InputBinding Cut = new InputBinding(KeyboardKeys.X, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "Ctrl+V")]
        [EditorDisplay("Common"), EditorOrder(140)]
        public InputBinding Paste = new InputBinding(KeyboardKeys.V, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "Ctrl+D")]
        [EditorDisplay("Common"), EditorOrder(150)]
        public InputBinding Duplicate = new InputBinding(KeyboardKeys.D, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "Delete")]
        [EditorDisplay("Common"), EditorOrder(160)]
        public InputBinding Delete = new InputBinding(KeyboardKeys.Delete);

        [DefaultValue(typeof(InputBinding), "Ctrl+Z")]
        [EditorDisplay("Common"), EditorOrder(170)]
        public InputBinding Undo = new InputBinding(KeyboardKeys.Z, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "Ctrl+Y")]
        [EditorDisplay("Common"), EditorOrder(180)]
        public InputBinding Redo = new InputBinding(KeyboardKeys.Y, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "Ctrl+A")]
        [EditorDisplay("Common"), EditorOrder(190)]
        public InputBinding SelectAll = new InputBinding(KeyboardKeys.A, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "F")]
        [EditorDisplay("Common"), EditorOrder(200)]
        public InputBinding FocusSelection = new InputBinding(KeyboardKeys.F);

        [DefaultValue(typeof(InputBinding), "Ctrl+F")]
        [EditorDisplay("Common"), EditorOrder(210)]
        public InputBinding Search = new InputBinding(KeyboardKeys.F, KeyboardKeys.Control);

        [DefaultValue(typeof(InputBinding), "Ctrl+O")]
        [EditorDisplay("Common"), EditorOrder(220)]
        public InputBinding ContentFinder = new InputBinding(KeyboardKeys.O, KeyboardKeys.Control);

        #endregion

        #region Scene

        [DefaultValue(typeof(InputBinding), "End")]
        [EditorDisplay("Scene", "Snap To Ground"), EditorOrder(500)]
        public InputBinding SnapToGround = new InputBinding(KeyboardKeys.End);

        [DefaultValue(typeof(InputBinding), "F5")]
        [EditorDisplay("Scene"), EditorOrder(510)]
        public InputBinding Play = new InputBinding(KeyboardKeys.F5);

        [DefaultValue(typeof(InputBinding), "F6")]
        [EditorDisplay("Scene"), EditorOrder(520)]
        public InputBinding Pause = new InputBinding(KeyboardKeys.F6);

        [DefaultValue(typeof(InputBinding), "F11")]
        [EditorDisplay("Scene"), EditorOrder(530)]
        public InputBinding StepFrame = new InputBinding(KeyboardKeys.F11);

        #endregion

        #region Gizmo

        [DefaultValue(typeof(InputBinding), "Alpha1")]
        [EditorDisplay("Gizmo"), EditorOrder(1000)]
        public InputBinding TranslateMode = new InputBinding(KeyboardKeys.Alpha1);

        [DefaultValue(typeof(InputBinding), "Alpha2")]
        [EditorDisplay("Gizmo"), EditorOrder(1010)]
        public InputBinding RotateMode = new InputBinding(KeyboardKeys.Alpha2);

        [DefaultValue(typeof(InputBinding), "Alpha3")]
        [EditorDisplay("Gizmo"), EditorOrder(1020)]
        public InputBinding ScaleMode = new InputBinding(KeyboardKeys.Alpha3);

        #endregion

        #region Viewport

        [DefaultValue(typeof(InputBinding), "W")]
        [EditorDisplay("Viewport"), EditorOrder(1500)]
        public InputBinding Forward = new InputBinding(KeyboardKeys.W);

        [DefaultValue(typeof(InputBinding), "S")]
        [EditorDisplay("Viewport"), EditorOrder(1510)]
        public InputBinding Backward = new InputBinding(KeyboardKeys.S);

        [DefaultValue(typeof(InputBinding), "A")]
        [EditorDisplay("Viewport"), EditorOrder(1520)]
        public InputBinding Left = new InputBinding(KeyboardKeys.A);

        [DefaultValue(typeof(InputBinding), "D")]
        [EditorDisplay("Viewport"), EditorOrder(1530)]
        public InputBinding Right = new InputBinding(KeyboardKeys.D);

        [DefaultValue(typeof(InputBinding), "E")]
        [EditorDisplay("Viewport"), EditorOrder(1540)]
        public InputBinding Up = new InputBinding(KeyboardKeys.E);

        [DefaultValue(typeof(InputBinding), "Q")]
        [EditorDisplay("Viewport"), EditorOrder(1550)]
        public InputBinding Down = new InputBinding(KeyboardKeys.Q);

        #endregion
    }
}
