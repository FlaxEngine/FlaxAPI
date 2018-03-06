////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Drag
{
	/// <summary>
	/// Helper class for handling <see cref="Script"/> instance drag and drop.
	/// </summary>
	/// <seealso cref="Script" />
	public sealed class DragScripts : DragHelper<Script>
	{
		/// <summary>
		/// The default prefix for drag data used for <see cref="Script"/>.
		/// </summary>
		public const string DragPrefix = "SCRIPT!?";

		/// <inheritdoc />
		protected override void GetherObjects(DragDataText data, Func<Script, bool> validateFunc)
		{
			var items = ParseData(data);
			for (int i = 0; i < items.Length; i++)
			{
				if (validateFunc(items[i]))
					Objects.Add(items[i]);
			}
		}

		/// <summary>
		/// Tries to parse the drag data to extract <see cref="Script"/> collection.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns>Gathered objects or empty array if cannot get any valid.</returns>
		public static Script[] ParseData(DragDataText data)
		{
			if (data.Text.StartsWith(DragPrefix))
			{
				// Remove prefix and parse splited names
				var ids = data.Text.Remove(0, DragPrefix.Length).Split('\n');
				var results = new List<Script>(ids.Length);
				for (int i = 0; i < ids.Length; i++)
				{
					// Find element
					Guid id;
					if (Guid.TryParse(ids[i], out id))
					{
						var obj = FlaxEngine.Object.Find<Script>(ref id);

						// Check it
						if (obj != null)
							results.Add(obj);
					}
				}

				return results.ToArray();
			}

			return new Script[0];
		}

		/// <summary>
		/// Tries to parse the drag data to validate if it has valid scripts darg.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns>True if drag data has valid scripts, otherwise false.</returns>
		public static bool IsValidData(DragDataText data)
		{
			if (data.Text.StartsWith(DragPrefix))
			{
				// Remove prefix and parse splited names
				var ids = data.Text.Remove(0, DragPrefix.Length).Split('\n');
				for (int i = 0; i < ids.Length; i++)
				{
					// Find element
					Guid id;
					if (Guid.TryParse(ids[i], out id))
					{
						var obj = FlaxEngine.Object.Find<Script>(ref id);

						// Check it
						if (obj != null)
							return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Gets the drag data.
		/// </summary>
		/// <param name="script">The script.</param>
		/// <returns>The data.</returns>
		public static DragDataText GetDragData(Script script)
		{
			if (script == null)
				throw new ArgumentNullException();

			return new DragDataText(DragPrefix + script.ID.ToString("N"));
		}

		/// <summary>
		/// Gets the drag data.
		/// </summary>
		/// <param name="items">The items.</param>
		/// <returns>The data.</returns>
		public static DragDataText GetDragData(IEnumerable<Script> items)
		{
			if (items == null)
				throw new ArgumentNullException();

			string text = DragPrefix;
			foreach (var item in items)
				text += item.ID.ToString("N") + '\n';

			return new DragDataText(text);
		}
	}
}
