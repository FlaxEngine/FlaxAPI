////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine
{
	public sealed partial class AnimatedModel
	{
		/// <summary>
		/// Describes the animation graph updates frequency for the animated model.
		/// </summary>
		public enum AnimationUpdateMode
		{
			/// <summary>
			/// The automatic updates will be used (based on platform capabilities, distance to the player, etc.).
			/// </summary>
			Auto = 0,
			
			/// <summary>
			/// Animation will be updated every game update.
			/// </summary>
			EveryUpdate = 1,

			/// <summary>
			/// Animation will be updated every second game update.
			/// </summary>
			EverySecondUpdate = 2,

			/// <summary>
			/// Animation will be updated every fourth game update.
			/// </summary>
			EveryFourthUpdate = 3,

			/// <summary>
			/// Animation can be updated manually by the user scripts. Use <see cref="AnimatedModel.UpdateAnimation"/> method.
			/// </summary>
			Manual = 4,

			/// <summary>
			/// Animation won't be updated at all.
			/// </summary>
			Never = 5,
		}

		private ModelEntryInfo[] _entries;

		/// <summary>
		/// Gets the skinned model entries collection. Each <see cref="ModelEntryInfo"/> contains data how to render meshes using this entry (material, shadows casting, etc.).
		/// </summary>
		/// <remarks>
		/// It's null if the <see cref="SkinnedModel"/> property is null or asset is not loaded yet.
		/// </remarks>
		[Serialize]
		[EditorOrder(100), EditorDisplay("Entries", EditorDisplayAttribute.InlineStyle)]
		[MemberCollection(CanReorderItems = false, NotNullItems = true, ReadOnly = true)]
		public ModelEntryInfo[] Entries
		{
			get
			{
				// Check if has cached data
				if (_entries != null)
					return _entries;

				// Cache data
				var model = SkinnedModel;
				if (model && model.IsLoaded)
				{
					var meshesCount = model.MaterialSlots.Length;
					_entries = new ModelEntryInfo[meshesCount];
					for (int i = 0; i < meshesCount; i++)
					{
						_entries[i] = new ModelEntryInfo(this, i);
					}
				}

				return _entries;
			}
			internal set
			{
				// Used by the serialization system

				_entries = value;

				EntriesChanged?.Invoke(this);
			}
		}

		/// <summary>
		/// Occurs when entries collection gets changed.
		/// It's called on <see cref="AnimatedModel"/> skinned model changed or when model asset gets reloaded, etc.
		/// </summary>
		public event Action<AnimatedModel> EntriesChanged;

		internal void Internal_OnSkinnedModelChanged()
		{
			// Clear cached data
			_entries = null;

			EntriesChanged?.Invoke(this);
		}
	}
}
