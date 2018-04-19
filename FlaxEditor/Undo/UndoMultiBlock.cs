// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace FlaxEditor
{
	/// <summary>
	/// Helper class to record undo operations in a block with <c>using</c> keyword. Records changes for one or more objects.
	/// </summary>
	/// <example>
	/// using(new UndoMultiBlock(undo, objs, "Rename objects"))
	/// {
	///		foreach(var e in objs)
	///			e.Name = "super name";
	/// }
	/// </example>
	/// <seealso cref="System.IDisposable" />
	public class UndoMultiBlock : IDisposable
	{
		private readonly object[] _snapshotUndoInternal;
		private readonly Undo _undo;

		/// <summary>
		/// Creates new undo object for recording actions with using pattern.
		/// </summary>
		/// <param name="undo">The undo/redo object.</param>
		/// <param name="snapshotInstances">Instances of objects to record.</param>
		/// <param name="actionString">Name of action to be displayed in undo stack.</param>
		public UndoMultiBlock(Undo undo, IEnumerable<object> snapshotInstances, string actionString)
		{
			_snapshotUndoInternal = snapshotInstances.ToArray();
			_undo = undo;
			_undo.RecordMultiBegin(_snapshotUndoInternal, actionString);
		}

		/// <inheritdoc />
		public void Dispose()
		{
			_undo.RecordMultiEnd(_snapshotUndoInternal);
		}
	}
}
