////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
	/// <summary>
	/// Represents a single audio device.
	/// </summary>
	public sealed class AudioDevice
	{
		/// <summary>
		/// Gets the device name.
		/// </summary>
		public string Name
		{
			get
			{
				if (_version != Audio.devicesVersion)
					throw new AccessViolationException();
				return Internal_GetName(_index);
			}
		}

		private readonly int _index;
		private readonly int _version;

		internal AudioDevice(int index, int version)
		{
			_index = index;
			_version = version;
		}

		#region Internal Calls

#if !UNIT_TEST_COMPILANT
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string Internal_GetName(int index);
#endif

		#endregion
	}
}
