// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// The debug shapes rendering service. Not available in final game. For use only in the editor.
    /// </summary>
    [Tooltip("The debug shapes rendering service. Not available in final game. For use only in the editor.")]
    public static unsafe partial class DebugDraw
    {
        /// <summary>
        /// Draws the collected debug shapes to the output.
        /// </summary>
        /// <param name="renderContext">The rendering context.</param>
        /// <param name="target">The rendering output surface handle.</param>
        /// <param name="depthBuffer">The custom depth texture used for depth test. Can be MSAA. Must match target surface size.</param>
        /// <param name="enableDepthTest">True if perform manual depth test with scene depth buffer when rendering the primitives. Uses custom shader and the scene depth buffer.</param>
        public static void Draw(ref RenderContext renderContext, GPUTextureView target = null, GPUTextureView depthBuffer = null, bool enableDepthTest = false)
        {
            Internal_Draw(ref renderContext, FlaxEngine.Object.GetUnmanagedPtr(target), FlaxEngine.Object.GetUnmanagedPtr(depthBuffer), enableDepthTest);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_Draw(ref RenderContext renderContext, IntPtr target, IntPtr depthBuffer, bool enableDepthTest);

        /// <summary>
        /// Draws the debug shapes for the given collection of selected actors and other scene actors debug shapes.
        /// </summary>
        /// <param name="selectedActors">The list of actors to draw.</param>
        /// <param name="selectedActorsCount">The size of the list of actors.</param>
        public static void DrawActors(IntPtr selectedActors, int selectedActorsCount)
        {
            Internal_DrawActors(selectedActors, selectedActorsCount);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawActors(IntPtr selectedActors, int selectedActorsCount);

        /// <summary>
        /// Draws the line.
        /// </summary>
        /// <param name="start">The start point.</param>
        /// <param name="end">The end point.</param>
        /// <param name="color">The color.</param>
        /// <param name="duration">The duration (in seconds). Use 0 to draw it only once.</param>
        /// <param name="depthTest">If set to <c>true</c> depth test will be performed, otherwise depth will be ignored.</param>
        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.0f, bool depthTest = true)
        {
            Internal_DrawLine(ref start, ref end, ref color, duration, depthTest);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawLine(ref Vector3 start, ref Vector3 end, ref Color color, float duration, bool depthTest);

        /// <summary>
        /// Draws the lines. Line positions are located one after another (e.g. l0.start, l0.end, l1.start, l1.end,...).
        /// </summary>
        /// <param name="lines">The list of vertices for lines (must have multiple of 2 elements).</param>
        /// <param name="transform">The custom matrix used to transform all line vertices.</param>
        /// <param name="color">The color.</param>
        /// <param name="duration">The duration (in seconds). Use 0 to draw it only once.</param>
        /// <param name="depthTest">If set to <c>true</c> depth test will be performed, otherwise depth will be ignored.</param>
        public static void DrawLines(Vector3[] lines, Matrix transform, Color color, float duration = 0.0f, bool depthTest = true)
        {
            Internal_DrawLines(lines, ref transform, ref color, duration, depthTest);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawLines(Vector3[] lines, ref Matrix transform, ref Color color, float duration, bool depthTest);

        /// <summary>
        /// Draws the circle.
        /// </summary>
        /// <param name="position">The center position.</param>
        /// <param name="normal">The normal vector direction.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="color">The color.</param>
        /// <param name="duration">The duration (in seconds). Use 0 to draw it only once.</param>
        /// <param name="depthTest">If set to <c>true</c> depth test will be performed, otherwise depth will be ignored.</param>
        public static void DrawCircle(Vector3 position, Vector3 normal, float radius, Color color, float duration = 0.0f, bool depthTest = true)
        {
            Internal_DrawCircle(ref position, ref normal, radius, ref color, duration, depthTest);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawCircle(ref Vector3 position, ref Vector3 normal, float radius, ref Color color, float duration, bool depthTest);

        /// <summary>
        /// Draws the wireframe triangle.
        /// </summary>
        /// <param name="v0">The first triangle vertex.</param>
        /// <param name="v1">The second triangle vertex.</param>
        /// <param name="v2">The third triangle vertex.</param>
        /// <param name="color">The color.</param>
        /// <param name="duration">The duration (in seconds). Use 0 to draw it only once.</param>
        /// <param name="depthTest">If set to <c>true</c> depth test will be performed, otherwise depth will be ignored.</param>
        public static void DrawWireTriangle(Vector3 v0, Vector3 v1, Vector3 v2, Color color, float duration = 0.0f, bool depthTest = true)
        {
            Internal_DrawWireTriangle(ref v0, ref v1, ref v2, ref color, duration, depthTest);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawWireTriangle(ref Vector3 v0, ref Vector3 v1, ref Vector3 v2, ref Color color, float duration, bool depthTest);

        /// <summary>
        /// Draws the triangle.
        /// </summary>
        /// <param name="v0">The first triangle vertex.</param>
        /// <param name="v1">The second triangle vertex.</param>
        /// <param name="v2">The third triangle vertex.</param>
        /// <param name="color">The color.</param>
        /// <param name="duration">The duration (in seconds). Use 0 to draw it only once.</param>
        /// <param name="depthTest">If set to <c>true</c> depth test will be performed, otherwise depth will be ignored.</param>
        public static void DrawTriangle(Vector3 v0, Vector3 v1, Vector3 v2, Color color, float duration = 0.0f, bool depthTest = true)
        {
            Internal_DrawTriangle(ref v0, ref v1, ref v2, ref color, duration, depthTest);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawTriangle(ref Vector3 v0, ref Vector3 v1, ref Vector3 v2, ref Color color, float duration, bool depthTest);

        /// <summary>
        /// Draws the triangles.
        /// </summary>
        /// <param name="vertices">The triangle vertices list (must have multiple of 3 elements).</param>
        /// <param name="color">The color.</param>
        /// <param name="duration">The duration (in seconds). Use 0 to draw it only once.</param>
        /// <param name="depthTest">If set to <c>true</c> depth test will be performed, otherwise depth will be ignored.</param>
        public static void DrawTriangles(Vector3[] vertices, Color color, float duration = 0.0f, bool depthTest = true)
        {
            Internal_DrawTriangles(vertices, ref color, duration, depthTest);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawTriangles(Vector3[] vertices, ref Color color, float duration, bool depthTest);

        /// <summary>
        /// Draws the triangles using the given index buffer.
        /// </summary>
        /// <param name="vertices">The triangle vertices list.</param>
        /// <param name="indices">The triangle indices list (must have multiple of 3 elements).</param>
        /// <param name="color">The color.</param>
        /// <param name="duration">The duration (in seconds). Use 0 to draw it only once.</param>
        /// <param name="depthTest">If set to <c>true</c> depth test will be performed, otherwise depth will be ignored.</param>
        public static void DrawTriangles(Vector3[] vertices, int[] indices, Color color, float duration = 0.0f, bool depthTest = true)
        {
            Internal_DrawTriangles1(vertices, indices, ref color, duration, depthTest);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawTriangles1(Vector3[] vertices, int[] indices, ref Color color, float duration, bool depthTest);

        /// <summary>
        /// Draws the wireframe box.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <param name="color">The color.</param>
        /// <param name="duration">The duration (in seconds). Use 0 to draw it only once.</param>
        /// <param name="depthTest">If set to <c>true</c> depth test will be performed, otherwise depth will be ignored.</param>
        public static void DrawWireBox(BoundingBox box, Color color, float duration = 0.0f, bool depthTest = true)
        {
            Internal_DrawWireBox(ref box, ref color, duration, depthTest);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawWireBox(ref BoundingBox box, ref Color color, float duration, bool depthTest);

        /// <summary>
        /// Draws the wireframe frustum.
        /// </summary>
        /// <param name="frustum">The frustum.</param>
        /// <param name="color">The color.</param>
        /// <param name="duration">The duration (in seconds). Use 0 to draw it only once.</param>
        /// <param name="depthTest">If set to <c>true</c> depth test will be performed, otherwise depth will be ignored.</param>
        public static void DrawWireFrustum(BoundingFrustum frustum, Color color, float duration = 0.0f, bool depthTest = true)
        {
            Internal_DrawWireFrustum(ref frustum, ref color, duration, depthTest);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawWireFrustum(ref BoundingFrustum frustum, ref Color color, float duration, bool depthTest);

        /// <summary>
        /// Draws the wireframe box.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <param name="color">The color.</param>
        /// <param name="duration">The duration (in seconds). Use 0 to draw it only once.</param>
        /// <param name="depthTest">If set to <c>true</c> depth test will be performed, otherwise depth will be ignored.</param>
        public static void DrawWireBox(OrientedBoundingBox box, Color color, float duration = 0.0f, bool depthTest = true)
        {
            Internal_DrawWireBox1(ref box, ref color, duration, depthTest);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawWireBox1(ref OrientedBoundingBox box, ref Color color, float duration, bool depthTest);

        /// <summary>
        /// Draws the wireframe sphere.
        /// </summary>
        /// <param name="sphere">The sphere.</param>
        /// <param name="color">The color.</param>
        /// <param name="duration">The duration (in seconds). Use 0 to draw it only once.</param>
        /// <param name="depthTest">If set to <c>true</c> depth test will be performed, otherwise depth will be ignored.</param>
        public static void DrawWireSphere(BoundingSphere sphere, Color color, float duration = 0.0f, bool depthTest = true)
        {
            Internal_DrawWireSphere(ref sphere, ref color, duration, depthTest);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawWireSphere(ref BoundingSphere sphere, ref Color color, float duration, bool depthTest);

        /// <summary>
        /// Draws the sphere.
        /// </summary>
        /// <param name="sphere">The sphere.</param>
        /// <param name="color">The color.</param>
        /// <param name="duration">The duration (in seconds). Use 0 to draw it only once.</param>
        /// <param name="depthTest">If set to <c>true</c> depth test will be performed, otherwise depth will be ignored.</param>
        public static void DrawSphere(BoundingSphere sphere, Color color, float duration = 0.0f, bool depthTest = true)
        {
            Internal_DrawSphere(ref sphere, ref color, duration, depthTest);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawSphere(ref BoundingSphere sphere, ref Color color, float duration, bool depthTest);

        /// <summary>
        /// Draws the wireframe tube.
        /// </summary>
        /// <param name="position">The center position.</param>
        /// <param name="orientation">The orientation.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="length">The length.</param>
        /// <param name="color">The color.</param>
        /// <param name="duration">The duration (in seconds). Use 0 to draw it only once.</param>
        /// <param name="depthTest">If set to <c>true</c> depth test will be performed, otherwise depth will be ignored.</param>
        public static void DrawWireTube(Vector3 position, Quaternion orientation, float radius, float length, Color color, float duration = 0.0f, bool depthTest = true)
        {
            Internal_DrawWireTube(ref position, ref orientation, radius, length, ref color, duration, depthTest);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawWireTube(ref Vector3 position, ref Quaternion orientation, float radius, float length, ref Color color, float duration, bool depthTest);

        /// <summary>
        /// Draws the wireframe cylinder.
        /// </summary>
        /// <param name="position">The center position.</param>
        /// <param name="orientation">The orientation.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="height">The height.</param>
        /// <param name="color">The color.</param>
        /// <param name="duration">The duration (in seconds). Use 0 to draw it only once.</param>
        /// <param name="depthTest">If set to <c>true</c> depth test will be performed, otherwise depth will be ignored.</param>
        public static void DrawWireCylinder(Vector3 position, Quaternion orientation, float radius, float height, Color color, float duration = 0.0f, bool depthTest = true)
        {
            Internal_DrawWireCylinder(ref position, ref orientation, radius, height, ref color, duration, depthTest);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawWireCylinder(ref Vector3 position, ref Quaternion orientation, float radius, float height, ref Color color, float duration, bool depthTest);

        /// <summary>
        /// Draws the wireframe arrow.
        /// </summary>
        /// <param name="position">The arrow origin position.</param>
        /// <param name="orientation">The orientation (defines the arrow direction).</param>
        /// <param name="scale">The arrow scale (used to adjust the arrow size).</param>
        /// <param name="color">The color.</param>
        /// <param name="duration">The duration (in seconds). Use 0 to draw it only once.</param>
        /// <param name="depthTest">If set to <c>true</c> depth test will be performed, otherwise depth will be ignored.</param>
        public static void DrawWireArrow(Vector3 position, Quaternion orientation, float scale, Color color, float duration = 0.0f, bool depthTest = true)
        {
            Internal_DrawWireArrow(ref position, ref orientation, scale, ref color, duration, depthTest);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawWireArrow(ref Vector3 position, ref Quaternion orientation, float scale, ref Color color, float duration, bool depthTest);

        /// <summary>
        /// Draws the box.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <param name="color">The color.</param>
        /// <param name="duration">The duration (in seconds). Use 0 to draw it only once.</param>
        /// <param name="depthTest">If set to <c>true</c> depth test will be performed, otherwise depth will be ignored.</param>
        public static void DrawBox(BoundingBox box, Color color, float duration = 0.0f, bool depthTest = true)
        {
            Internal_DrawBox(ref box, ref color, duration, depthTest);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawBox(ref BoundingBox box, ref Color color, float duration, bool depthTest);

        /// <summary>
        /// Draws the box.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <param name="color">The color.</param>
        /// <param name="duration">The duration (in seconds). Use 0 to draw it only once.</param>
        /// <param name="depthTest">If set to <c>true</c> depth test will be performed, otherwise depth will be ignored.</param>
        public static void DrawBox(OrientedBoundingBox box, Color color, float duration = 0.0f, bool depthTest = true)
        {
            Internal_DrawBox1(ref box, ref color, duration, depthTest);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_DrawBox1(ref OrientedBoundingBox box, ref Color color, float duration, bool depthTest);
    }
}
