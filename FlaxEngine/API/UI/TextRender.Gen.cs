// This code was auto-generated. Do not modify it.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Text rendering object.
    /// </summary>
    [Tooltip("Text rendering object.")]
    public unsafe partial class TextRender : Actor
    {
        /// <inheritdoc />
        protected TextRender() : base()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="TextRender"/> object.
        /// </summary>
        /// <returns>The created object.</returns>
        public static TextRender New()
        {
            return Internal_Create(typeof(TextRender)) as TextRender;
        }

        /// <summary>
        /// The material used for the text rendering. It must contain texture parameter named Font used to sample font texture.
        /// </summary>
        [EditorOrder(20), DefaultValue(null), AssetReference(true), EditorDisplay("Text")]
        [Tooltip("The material used for the text rendering. It must contain texture parameter named Font used to sample font texture.")]
        public MaterialBase Material
        {
            get { return Internal_GetMaterial(unmanagedPtr); }
            set { Internal_SetMaterial(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern MaterialBase Internal_GetMaterial(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetMaterial(IntPtr obj, IntPtr value);

        /// <summary>
        /// The font asset used as a text characters source.
        /// </summary>
        [EditorOrder(30), DefaultValue(null), AssetReference(true), EditorDisplay("Text")]
        [Tooltip("The font asset used as a text characters source.")]
        public FontAsset Font
        {
            get { return Internal_GetFont(unmanagedPtr); }
            set { Internal_SetFont(unmanagedPtr, FlaxEngine.Object.GetUnmanagedPtr(value)); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern FontAsset Internal_GetFont(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetFont(IntPtr obj, IntPtr value);

        /// <summary>
        /// The draw passes to use for rendering this object.
        /// </summary>
        [EditorOrder(75), DefaultValue(DrawPass.Default), EditorDisplay("Text")]
        [Tooltip("The draw passes to use for rendering this object.")]
        public DrawPass DrawModes
        {
            get { return Internal_GetDrawModes(unmanagedPtr); }
            set { Internal_SetDrawModes(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern DrawPass Internal_GetDrawModes(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetDrawModes(IntPtr obj, DrawPass value);

        /// <summary>
        /// The shadows casting mode by this visual element.
        /// </summary>
        [EditorOrder(80), DefaultValue(ShadowsCastingMode.All), EditorDisplay("Text")]
        [Tooltip("The shadows casting mode by this visual element.")]
        public ShadowsCastingMode ShadowsMode
        {
            get { return Internal_GetShadowsMode(unmanagedPtr); }
            set { Internal_SetShadowsMode(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ShadowsCastingMode Internal_GetShadowsMode(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetShadowsMode(IntPtr obj, ShadowsCastingMode value);

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        [EditorOrder(0), DefaultValue(""), MultilineText, EditorDisplay("Text")]
        [Tooltip("The text.")]
        public string Text
        {
            get { return Internal_GetText(unmanagedPtr); }
            set { Internal_SetText(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetText(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetText(IntPtr obj, string value);

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        [EditorOrder(10), DefaultValue(typeof(Color), "1,1,1,1"), EditorDisplay("Text")]
        [Tooltip("The color of the text.")]
        public Color Color
        {
            get { Internal_GetColor(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetColor(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetColor(IntPtr obj, out Color resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetColor(IntPtr obj, ref Color value);

        /// <summary>
        /// Gets or sets the font characters size.
        /// </summary>
        [EditorOrder(40), DefaultValue(32), Limit(1, 1000), EditorDisplay("Text")]
        [Tooltip("The font characters size.")]
        public int FontSize
        {
            get { return Internal_GetFontSize(unmanagedPtr); }
            set { Internal_SetFontSize(unmanagedPtr, value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetFontSize(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetFontSize(IntPtr obj, int value);

        /// <summary>
        /// Gets or sets the layout options. Layout is defined in local space of the object (on XY plane).
        /// </summary>
        [EditorOrder(100), EditorDisplay("Text")]
        [Tooltip("The layout options. Layout is defined in local space of the object (on XY plane).")]
        public TextLayoutOptions LayoutOptions
        {
            get { Internal_GetLayoutOptions(unmanagedPtr, out var resultAsRef); return resultAsRef; }
            set { Internal_SetLayoutOptions(unmanagedPtr, ref value); }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLayoutOptions(IntPtr obj, out TextLayoutOptions resultAsRef);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_SetLayoutOptions(IntPtr obj, ref TextLayoutOptions value);

        /// <summary>
        /// Gets the axis=aligned bounding box of the text vertices in the local-space of the actor.
        /// </summary>
        [Tooltip("The axis=aligned bounding box of the text vertices in the local-space of the actor.")]
        public BoundingBox LocalBox
        {
            get { Internal_GetLocalBox(unmanagedPtr, out var resultAsRef); return resultAsRef; }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetLocalBox(IntPtr obj, out BoundingBox resultAsRef);

        /// <summary>
        /// Updates the text vertex buffer layout and cached data if its dirty.
        /// </summary>
        public void UpdateLayout()
        {
            Internal_UpdateLayout(unmanagedPtr);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_UpdateLayout(IntPtr obj);
    }
}
