////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Runtime.CompilerServices;

namespace FlaxEngine
{
	public static partial class Profiler
	{
		/// <summary>
		/// Beginss profiling a piece of code with a custom label.
		/// </summary>
		/// <param name="name">The name of the event.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void BeginEvent(string name);

		/// <summary>
		/// Ends profiling an event.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void EndEvent();

		/// <summary>
		/// Beginss GPU profiling a piece of code with a custom label.
		/// </summary>
		/// <param name="name">The name of the event.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void BeginEventGPU(string name);

		/// <summary>
		/// Ends GPU profiling an event.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void EndEventGPU();
	}
}
