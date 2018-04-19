// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
	/// <summary>
	/// Represents part of the skinned model that is made of verticies which can be rendered.
	/// </summary>
	public sealed class SkinnedMesh
	{
		internal SkinnedModel _skinnedModel;
		internal readonly int _index;

		/// <summary>
		/// Gets the parent skinned model asset.
		/// </summary>
		public SkinnedModel ParentSkinnedModel => _skinnedModel;

		/// <summary>
		/// Gets the index of the mesh.
		/// </summary>
		public int MeshIndex => _index;

		/// <summary>
		/// Gets the index of the material slot to use during this mesh rendering.
		/// </summary>
		public int MaterialSlotIndex
		{
			get => Internal_GetMaterialSlotIndex(_skinnedModel.unmanagedPtr, _index);
			set => Internal_SetMaterialSlotIndex(_skinnedModel.unmanagedPtr, _index, value);
		}

		/// <summary>
		/// Gets the material slot used by this mesh during rendering.
		/// </summary>
		public MaterialSlot MaterialSlot => _skinnedModel.MaterialSlots[MaterialSlotIndex];

		/// <summary>
		/// Gets the triangle count.
		/// </summary>
		public int Triangles => Internal_GetTriangleCount(_skinnedModel.unmanagedPtr, _index);

		/// <summary>
		/// Gets the vertex count.
		/// </summary>
		public int Vertices => Internal_GetVertexCount(_skinnedModel.unmanagedPtr, _index);

		internal SkinnedMesh(SkinnedModel model, int index)
		{
			_skinnedModel = model;
			_index = index;
		}

#if !UNIT_TEST_COMPILANT
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int Internal_GetMaterialSlotIndex(IntPtr obj, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Internal_SetMaterialSlotIndex(IntPtr obj, int index, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int Internal_GetTriangleCount(IntPtr obj, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int Internal_GetVertexCount(IntPtr obj, int index);
#endif
	}
}
