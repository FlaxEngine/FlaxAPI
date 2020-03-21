// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The graph parameters types.
    /// </summary>
    [Tooltip("The graph parameters types.")]
    public enum GraphParamType
    {
        /// <summary>
        /// The boolean value.
        /// </summary>
        [Tooltip("The boolean value.")]
        Bool = 0,

        /// <summary>
        /// The integer value.
        /// </summary>
        [Tooltip("The integer value.")]
        Integer = 1,

        /// <summary>
        /// The floating point value (single precision).
        /// </summary>
        [Tooltip("The floating point value (single precision).")]
        Float = 2,

        /// <summary>
        /// The Vector2 structure.
        /// </summary>
        [Tooltip("The Vector2 structure.")]
        Vector2 = 3,

        /// <summary>
        /// The Vector3 structure.
        /// </summary>
        [Tooltip("The Vector3 structure.")]
        Vector3 = 4,

        /// <summary>
        /// The Vector4 structure.
        /// </summary>
        [Tooltip("The Vector4 structure.")]
        Vector4 = 5,

        /// <summary>
        /// The Color structure (RGBA, normalized, 32bit per channel).
        /// </summary>
        [Tooltip("The Color structure (RGBA, normalized, 32bit per channel).")]
        Color = 6,

        /// <summary>
        /// The texture reference.
        /// </summary>
        [Tooltip("The texture reference.")]
        Texture = 7,

        /// <summary>
        /// The normal map reference.
        /// </summary>
        [Tooltip("The normal map reference.")]
        NormalMap = 8,

        /// <summary>
        /// The text (Unicode, UTF-16 encoding).
        /// </summary>
        [Tooltip("The text (Unicode, UTF-16 encoding).")]
        String = 9,

        /// <summary>
        /// The bounding box structure.
        /// </summary>
        [Tooltip("The bounding box structure.")]
        Box = 10,

        /// <summary>
        /// The quaternion structure.
        /// </summary>
        [Tooltip("The quaternion structure.")]
        Rotation = 11,

        /// <summary>
        /// The transform structure (translation, rotation and scale).
        /// </summary>
        [Tooltip("The transform structure (translation, rotation and scale).")]
        Transform = 12,

        /// <summary>
        /// The asset reference.
        /// </summary>
        [Tooltip("The asset reference.")]
        Asset = 13,

        /// <summary>
        /// The actor reference.
        /// </summary>
        [Tooltip("The actor reference.")]
        Actor = 14,

        /// <summary>
        /// The rectangle structure.
        /// </summary>
        [Tooltip("The rectangle structure.")]
        Rectangle = 15,

        /// <summary>
        /// The cube texture.
        /// </summary>
        [Tooltip("The cube texture.")]
        CubeTexture = 16,

        /// <summary>
        /// The scene texture id.
        /// </summary>
        [Tooltip("The scene texture id.")]
        SceneTexture = 17,

        /// <summary>
        /// The GPU texture (created from code).
        /// </summary>
        [Tooltip("The GPU texture (created from code).")]
        GPUTexture = 18,

        /// <summary>
        /// The matrix.
        /// </summary>
        [Tooltip("The matrix.")]
        Matrix = 19,

        /// <summary>
        /// The GPU texture array (created from code).
        /// </summary>
        [Tooltip("The GPU texture array (created from code).")]
        GPUTextureArray = 20,

        /// <summary>
        /// The GPU volume texture (created from code).
        /// </summary>
        [Tooltip("The GPU volume texture (created from code).")]
        GPUTextureVolume = 21,

        /// <summary>
        /// The GPU cube texture (created from code).
        /// </summary>
        [Tooltip("The GPU cube texture (created from code).")]
        GPUTextureCube = 22,

        /// <summary>
        /// The RGBA channel selection mask.
        /// </summary>
        [Tooltip("The RGBA channel selection mask.")]
        ChannelMask = 23,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// The channel mask modes.
    /// </summary>
    [Tooltip("The channel mask modes.")]
    public enum ChannelMask
    {
        /// <summary>
        /// The red channel.
        /// </summary>
        [Tooltip("The red channel.")]
        Red = 0,

        /// <summary>
        /// The green channel.
        /// </summary>
        [Tooltip("The green channel.")]
        Green = 1,

        /// <summary>
        /// The blue channel.
        /// </summary>
        [Tooltip("The blue channel.")]
        Blue = 2,

        /// <summary>
        /// The alpha channel.
        /// </summary>
        [Tooltip("The alpha channel.")]
        Alpha = 3,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Represents a parameter in the Graph.
    /// </summary>
    [Tooltip("Represents a parameter in the Graph.")]
    public unsafe partial class GraphParameter : FlaxEngine.Object
    {
        /// <inheritdoc />
        protected GraphParameter() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="GraphParameter"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public new static GraphParameter New()
        {
            return Internal_Create(typeof(GraphParameter)) as GraphParameter;
        }

        /// <summary>
        /// Parameter type
        /// </summary>
        [Tooltip("Parameter type")]
        public GraphParamType Type
        {
            get { return Internal_GetType(unmanagedPtr); }
            set { Internal_SetType(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern GraphParamType Internal_GetType(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetType(IntPtr obj, GraphParamType value);

        /// <summary>
        /// Parameter unique ID
        /// </summary>
        [Tooltip("Parameter unique ID")]
        public Guid ID
        {
            get { Internal_GetID(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetID(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetID(IntPtr obj, out Guid resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetID(IntPtr obj, ref Guid value);

        /// <summary>
        /// Parameter name
        /// </summary>
        [Tooltip("Parameter name")]
        public string Name
        {
            get { return Internal_GetName(unmanagedPtr); }
            set { Internal_SetName(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetName(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetName(IntPtr obj, string value);

        /// <summary>
        /// True if is exposed outside
        /// </summary>
        [Tooltip("True if is exposed outside")]
        public bool IsPublic
        {
            get { return Internal_GetIsPublic(unmanagedPtr); }
            set { Internal_SetIsPublic(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetIsPublic(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetIsPublic(IntPtr obj, bool value);

        /// <summary>
        /// Gets or sets the value of the parameter.
        /// </summary>
        [Tooltip("The value of the parameter.")]
        public object Value
        {
            get { return Internal_GetValue(unmanagedPtr); }
            set { Internal_SetValue(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern object Internal_GetValue(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetValue(IntPtr obj, object value);
    }
}
