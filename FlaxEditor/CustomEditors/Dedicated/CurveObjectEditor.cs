// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using FlaxEditor.GUI;
using FlaxEngine;

namespace FlaxEditor.CustomEditors.Dedicated
{
    /// <summary>
    /// Custom editor for <see cref="Curve{T}"/>.
    /// </summary>
    class CurveObjectEditor<T> : CustomEditor where T : struct
    {
        private bool _isSetting;
        private CurveEditor<T> _curve;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            var item = layout.CustomContainer<CurveEditor<T>>();
            _curve = item.CustomControl;
            _curve.Height = 120.0f;
            _curve.Edited += OnCurveEdited;
        }

        private void OnCurveEdited()
        {
            if (_isSetting)
                return;

            _isSetting = true;
            SetValue(new Curve<T>(_curve.Keyframes));
            _isSetting = false;
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            base.Refresh();

            var value = (Curve<T>)Values[0];
            if (value != null && !_curve.IsUserEditing && !Utils.ArraysEqual(value.Keyframes, _curve.Keyframes))
            {
                _isSetting = true;
                _curve.SetKeyframes(value.Keyframes);
                _isSetting = false;
            }
        }

        /// <inheritdoc />
        protected override void Deinitialize()
        {
            _curve = null;

            base.Deinitialize();
        }
    }

    [CustomEditor(typeof(Curve<int>)), DefaultEditor]
    sealed class IntCurveObjectEditor : CurveObjectEditor<int>
    {
    }

    [CustomEditor(typeof(Curve<float>)), DefaultEditor]
    sealed class FloatCurveObjectEditor : CurveObjectEditor<float>
    {
    }

    [CustomEditor(typeof(Curve<Vector2>)), DefaultEditor]
    sealed class Vector2CurveObjectEditor : CurveObjectEditor<Vector2>
    {
    }

    [CustomEditor(typeof(Curve<Vector3>)), DefaultEditor]
    sealed class Vector3CurveObjectEditor : CurveObjectEditor<Vector3>
    {
    }

    [CustomEditor(typeof(Curve<Vector4>)), DefaultEditor]
    sealed class Vector4CurveObjectEditor : CurveObjectEditor<Vector4>
    {
    }

    [CustomEditor(typeof(Curve<Quaternion>)), DefaultEditor]
    sealed class QuaternionCurveObjectEditor : CurveObjectEditor<Quaternion>
    {
    }

    [CustomEditor(typeof(Curve<Color>)), DefaultEditor]
    sealed class ColorCurveObjectEditor : CurveObjectEditor<Color>
    {
    }
}
