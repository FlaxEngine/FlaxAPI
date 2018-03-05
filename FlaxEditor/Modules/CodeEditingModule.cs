////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using FlaxEditor.Content;
using FlaxEditor.Scripting;

namespace FlaxEditor.Modules
{
	/// <summary>
	/// Opening/editing script source file and project.
	/// </summary>
	/// <seealso cref="FlaxEditor.Modules.EditorModule" />
	public sealed class CodeEditingModule : EditorModule
	{
		private readonly List<ScriptItem> _scripts = new List<ScriptItem>();
		private bool _hasValidScripts;

		internal CodeEditingModule(Editor editor)
			: base(editor)
		{
		}

		/// <summary>
		/// Opens the file.
		/// </summary>
		/// <param name="path">The source file path.</param>
		public void OpenFile(string path)
		{
			ScriptsBuilder.OpenFile(path);
		}

		/// <inheritdoc />
		public override void OnInit()
		{
			Editor.ContentDatabase.ItemAdded += ContentDatabaseOnItemAdded;
			Editor.ContentDatabase.ItemRemoved += ContentDatabaseOnItemRemoved;
		}

		/// <inheritdoc />
		public override void OnExit()
		{
			Editor.ContentDatabase.ItemAdded -= ContentDatabaseOnItemAdded;
			Editor.ContentDatabase.ItemRemoved -= ContentDatabaseOnItemRemoved;
		}

		private void ContentDatabaseOnItemAdded(ContentItem contentItem)
		{
			if (_hasValidScripts && contentItem is ScriptItem script)
				_scripts.Add(script);
		}

		private void ContentDatabaseOnItemRemoved(ContentItem contentItem)
		{
			if (contentItem is ScriptItem script)
				_scripts.Remove(script);
		}

		private void FindScripts(ContentFolder folder)
		{
			for (int i = 0; i < folder.Children.Count; i++)
			{
				if (folder.Children[i] is ContentFolder subFolder)
					FindScripts(subFolder);
				else if (folder.Children[i] is ScriptItem script)
					_scripts.Add(script);
			}
		}

		/// <summary>
		/// Gets the scripts from the project.
		/// </summary>
		/// <returns>The scripts collection (readonly).</returns>
		public List<ScriptItem> GetScripts()
		{
			if (!_hasValidScripts)
			{
				_scripts.Clear();
				_hasValidScripts = true;

				FindScripts(Editor.ContentDatabase.ProjectSource.Folder);
			}

			return _scripts;
		}
	}
}
