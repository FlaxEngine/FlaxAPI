////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
	public static partial class Audio
	{
		internal static int devicesVersion;
		internal static AudioDevice[] devices;

		/// <summary>
		/// The audio devices collection changed event.
		/// </summary>
		public static event Action DevicesChanged;

		/// <summary>
		/// Gets the audio devices collection detected by the engine.
		/// </summary>
		public static AudioDevice[] Devices
		{
			get
			{
				if (devices == null)
				{
					int count = Internal_GetDevicesCount();
					devices = new AudioDevice[count];
					for (int i = 0; i < count; i++)
						devices[i] = new AudioDevice(i, devicesVersion);
				}

				return devices;
			}
		}

		/// <summary>
		/// Gets the active audio device.
		/// </summary>
		public static AudioDevice ActiveDevice
		{
			get { return Devices[Internal_GetActiveDeviceIndex()]; }
		}

		internal static void Internal_DevicesChanged()
		{
			devicesVersion++;
			devices = null;
			DevicesChanged?.Invoke();
		}

		#region Internal Calls

#if !UNIT_TEST_COMPILANT
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int Internal_GetDevicesCount();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int Internal_GetActiveDeviceIndex();
#endif

		#endregion
	}
}
