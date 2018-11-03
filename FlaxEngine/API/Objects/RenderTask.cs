// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine.Rendering
{
    public partial class RenderTask
    {
        /// <summary>
        /// Disposes render task data and child components (output and buffers).
        /// </summary>
        public virtual void Dispose()
        {
            // Disable it
            Enabled = false;
        }

        /// <summary>
        /// Computes the model Level of Detail index to use during rendering in the current view.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="bounds">The object bounds (transformed model instance bounds).</param>
        /// <returns>The LOD.</returns>
        public int ComputeModelLOD(Model model, ref BoundingSphere bounds)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            return Internal_ComputeModelLOD(model.unmanagedPtr, ref bounds, unmanagedPtr);
        }

        internal virtual bool Internal_Begin(out IntPtr outputPtr)
        {
            outputPtr = IntPtr.Zero;
            return true;
        }

        internal virtual void Internal_Render(GPUContext context)
        {
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DrawCall
        {
            public enum Types : byte
            {
                Mesh,
                TerrainChunk,
            }

            public StaticFlags Flags;
            public int LodIndex;
            public Int2 Index0;
            public Int2 Index1;
            public Types Type;
            public byte Padding0;
            public short Padding1;
            public IntPtr Object;
            public IntPtr Material;
            public Matrix World;
        }

        internal virtual DrawCall[] Internal_Draw()
        {
            return null;
        }

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_ComputeModelLOD(IntPtr modelObj, ref BoundingSphere bounds, IntPtr taskObj);
#endif
    }
}
