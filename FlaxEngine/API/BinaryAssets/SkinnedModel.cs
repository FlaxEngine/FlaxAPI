////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine
{
	public sealed partial class SkinnedModel
	{
		/// <summary>
		/// The asset type content domain.
		/// </summary>
		public const ContentDomain Domain = ContentDomain.Model;

		private MaterialSlot[] _slots;

		/// <summary>
		/// Gets the material slots colelction. Each slot contains information how to render mesh or meshes using it.
		/// </summary>
		public MaterialSlot[] MaterialSlots
		{
			get
			{
				if (_slots == null)
					CacheData();
				return _slots;
			}
			internal set
			{
				// TODO: implement setter and allow to modify the collection
			}
		}

		/// <summary>
		/// Gets the material slot by the name.
		/// </summary>
		/// <param name="name">The slot name.</param>
		/// <returns>The material slot with the given name or null if cannot find it (asset may be not loaded yet).</returns>
		public MaterialSlot GetSlot(string name)
		{
			MaterialSlot result = null;
			var slots = MaterialSlots;
			if (slots != null)
			{
				for (int i = 0; i < slots.Length; i++)
				{
					if (string.Equals(slots[i].Name, name, StringComparison.Ordinal))
					{
						result = slots[i];
						break;
					}
				}
			}

			return result;
		}

		private void CacheData()
		{
			// Ask unmanaged world for amount of material slots
			int slotsCount = Model.Internal_GetSlots(unmanagedPtr);
			if (slotsCount > 0)
			{
				_slots = new MaterialSlot[slotsCount];
				for (int i = 0; i < slotsCount; i++)
					_slots[i] = new MaterialSlot(this, i);
			}
		}

		internal void Internal_OnUnload()
		{
			// Clear cached data
			_slots = null;
		}
	}
}
