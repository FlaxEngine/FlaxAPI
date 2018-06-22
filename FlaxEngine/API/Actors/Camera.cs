// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public sealed partial class Camera
    {
        /// <summary>
        /// Projects the point from 3D world-space to the camera screen-space (in screen pixels for default viewport calculated from <see cref="Viewport"/>).
        /// </summary>
        /// <param name="worldSpaceLocation">The input world-space location (XYZ in world).</param>
        /// <param name="screenSpaceLocation">The output screen-space location (XY in screen pixels).</param>
        public void ProjectPoint(ref Vector3 worldSpaceLocation, out Vector2 screenSpaceLocation)
        {
            var viewport = Viewport;
            Matrix v, p, vp;
            GetMatrices(out v, out p, ref viewport);
            Matrix.Multiply(ref v, ref p, out vp);
            Vector3 clipSpaceLocation;
            Vector3.Transform(ref worldSpaceLocation, ref vp, out clipSpaceLocation);
            viewport.Project(ref worldSpaceLocation, ref vp, out clipSpaceLocation);
            screenSpaceLocation = new Vector2(clipSpaceLocation);
        }

        /// <summary>
        /// Converts the mouse location (in screen-space) to 3D ray.
        /// </summary>
        /// <param name="location">The mouse location (screen-space).</param>
        /// <returns>The mouse ray (world-space).</returns>
        public Ray ConvertMouseToRay(Vector2 location)
        {
            // Gather camera properties
            var viewport = Viewport;
            Matrix v, p, ivp;
            GetMatrices(out v, out p, ref viewport);
            Matrix.Multiply(ref v, ref p, out ivp);
            ivp.Invert();

            // Create near and far points
            Vector3 nearPoint = new Vector3(location, 0.0f);
            Vector3 farPoint = new Vector3(location, 1.0f);
            viewport.Unproject(ref nearPoint, ref ivp, out nearPoint);
            viewport.Unproject(ref farPoint, ref ivp, out farPoint);

            // Create direction vector
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            return new Ray(nearPoint, direction);
        }

        /// <summary>
        /// Converts the mouse location (in screen-space) to 3D ray.
        /// </summary>
        /// <param name="location">The mouse location (screen-space).</param>
        /// <param name="ray">The mouse ray (world-space).</param>
        public void ConvertMouseToRay(ref Vector2 location, out Ray ray)
        {
            // Gather camera properties
            var viewport = Viewport;
            Matrix v, p, ivp;
            GetMatrices(out v, out p, ref viewport);
            Matrix.Multiply(ref v, ref p, out ivp);
            ivp.Invert();

            // Create near and far points
            Vector3 nearPoint = new Vector3(location, 0.0f);
            Vector3 farPoint = new Vector3(location, 1.0f);
            viewport.Unproject(ref nearPoint, ref ivp, out nearPoint);
            viewport.Unproject(ref farPoint, ref ivp, out farPoint);

            // Create direction vector
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            ray = new Ray(nearPoint, direction);
        }

        /// <summary>
        /// Converts the mouse location (in screen-space) to 3D ray.
        /// </summary>
        /// <param name="location">The mouse location (screen-space).</param>
        /// <param name="ray">The mouse ray (world-space).</param>
        /// <param name="viewport">The custom viewport used for the camera projection matrices calculations.</param>
        public void ConvertMouseToRay(ref Vector2 location, out Ray ray, ref Viewport viewport)
        {
            // Gather camera properties
            Matrix v, p, ivp;
            GetMatrices(out v, out p, ref viewport);
            Matrix.Multiply(ref v, ref p, out ivp);
            ivp.Invert();

            // Create near and far points
            Vector3 nearPoint = new Vector3(location, 0.0f);
            Vector3 farPoint = new Vector3(location, 1.0f);
            viewport.Unproject(ref nearPoint, ref ivp, out nearPoint);
            viewport.Unproject(ref farPoint, ref ivp, out farPoint);

            // Create direction vector
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            ray = new Ray(nearPoint, direction);
        }

        /// <summary>
        /// Calculates the view and the projection matrices for the camera. Support using custom viewport.
        /// </summary>
        /// <param name="view">The result camera view matrix.</param>
        /// <param name="projection">The result camera projection matrix.</param>
        public void GetMatrices(out Matrix view, out Matrix projection)
        {
            Viewport emptyViewport = new Viewport(0, 0, 0, 0);
            Internal_GetMatrices(unmanagedPtr, out view, out projection, ref emptyViewport);
        }

        /// <summary>
        /// Calculates the view and the projection matrices for the camera. Support using custom viewport.
        /// </summary>
        /// <param name="view">The result camera view matrix.</param>
        /// <param name="projection">The result camera projection matrix.</param>
        /// <param name="customViewport">The custom output viewport.</param>
        public void GetMatrices(out Matrix view, out Matrix projection, ref Viewport customViewport)
        {
            Internal_GetMatrices(unmanagedPtr, out view, out projection, ref customViewport);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name} ({GetType().Name})";
        }

        // Hacky internal call to get proper camera preview model intersection (works only in editor)
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_IntersectsItselfEditor(IntPtr obj, ref Ray ray, out float distance);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool Internal_GetMatrices(IntPtr obj, out Matrix view, out Matrix projection, ref Viewport customViewport);
    }
}
