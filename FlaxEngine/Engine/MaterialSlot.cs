////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// Single material slot desribies how to render meshes using it.
    /// </summary>
    public sealed class MaterialSlot
    {
        internal Model _model;
        internal readonly int _index;

        /// <summary>
        /// Gets the parent model asset.
        /// </summary>
        public Model ParentModel => _model;

        /// <summary>
        /// Gets the index of the slot (in the parnet model).
        /// </summary>
        public int SlotIndex => _index;

        /// <summary>
        /// Gets or sets the material.
        /// </summary>
        [EditorOrder(30), Tooltip("Material asset used to by this slot"), EditorDisplay(null, EditorDisplayAttribute.InlineStyle)]
        public MaterialBase Material
        {
            get => Internal_GetMaterial(_model.unmanagedPtr, _index);
            set => Internal_SetMaterial(_model.unmanagedPtr, _index, Object.GetUnmanagedPtr(value));
        }
        
        /// <summary>
        /// Gets or sets the shadows casting mode by the meshes using this slot.
        /// </summary>
        [EditorOrder(20), Tooltip("Shadows casting mode by the meshes using this slot"), EditorDisplay(Name = "Shadows")]
        public ShadowsCastingMode ShadowsMode
        {
            get => Internal_GetShadowsMode(_model.unmanagedPtr, _index);
            set => Internal_SetShadowsMode(_model.unmanagedPtr, _index, value);
        }

        /// <summary>
        /// Gets or sets the slot name. It doesn't have to be unique.
        /// </summary>
        [EditorOrder(10), Tooltip("Material slot name")]
        public string Name
        {
            get => Internal_GetName(_model.unmanagedPtr, _index);
            set => Internal_SetName(_model.unmanagedPtr, _index, value);
        }

        internal MaterialSlot(Model model, int index)
        {
            _model = model;
            _index = index;
        }

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MaterialBase Internal_GetMaterial(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMaterial(IntPtr obj, int index, IntPtr value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ShadowsCastingMode Internal_GetShadowsMode(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetShadowsMode(IntPtr obj, int index, ShadowsCastingMode value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetName(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetName(IntPtr obj, int index, string value);
#endif
    }
}
