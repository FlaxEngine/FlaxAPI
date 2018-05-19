// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.CustomEditors.Editors;
using FlaxEngine;

namespace FlaxEditor.CustomEditors.Dedicated
{
	/// <summary>
	/// Deciated custom editor for <see cref="Actor"/> objects.
	/// </summary>
	/// <seealso cref="FlaxEditor.CustomEditors.Editors.GenericEditor" />
	[CustomEditor(typeof(Actor)), DefaultEditor]
	public class ActorEditor : GenericEditor
	{
		/// <inheritdoc />
		protected override void SpawnProperty(LayoutElementsContainer itemLayout, ValueContainer itemValues, ItemInfo item)
		{
			// Note: we cannot specify actor properties editor types directly because we want to keep editor classes in FlaxEditor assembly
			int order = item.Order?.Order ?? int.MinValue;
			switch (order)
			{
				// Override static flags editor
				case -80:
					item.CustomEditor = new CustomEditorAttribute(typeof(ActorStaticFlagsEditor));
					break;

				// Override layer editor
				case -69:
					item.CustomEditor = new CustomEditorAttribute(typeof(ActorLayerEditor));
					break;

				// Override tag editor
				case -68:
					item.CustomEditor = new CustomEditorAttribute(typeof(ActorTagEditor));
					break;

				// Override position/scale editor
				case -30:
				case -10:
					item.CustomEditor = new CustomEditorAttribute(typeof(ActorTransformEditor.PositionScaleEditor));
					break;

				// Override orientation editor
				case -20:
					item.CustomEditor = new CustomEditorAttribute(typeof(ActorTransformEditor.OrientationEditor));
					break;
			}

			base.SpawnProperty(itemLayout, itemValues, item);
		}

		/// <inheritdoc />
		protected override List<ItemInfo> GetItemsForType(Type type)
		{
			var items = base.GetItemsForType(type);

			// Inject scripts editor
			var scriptsMember = type.GetProperty("Scripts");
			if (scriptsMember != null)
			{
				var item = new ItemInfo(scriptsMember);
				item.CustomEditor = new CustomEditorAttribute(typeof(ScriptsEditor));
				items.Add(item);
			}

			return items;
		}
	}
}
