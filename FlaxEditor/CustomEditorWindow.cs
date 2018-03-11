////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.CustomEditors;
using FlaxEditor.Scripting;
using FlaxEditor.Windows;
using FlaxEngine.GUI;
using FlaxEngine.GUI.Docking;

namespace FlaxEditor
{
	/// <summary>
	/// Base class for custom editor window that can create custom GUI layout and expose various functionalities to the user.
	/// </summary>
	/// <seealso cref="FlaxEditor.CustomEditors.CustomEditor" />
	public abstract class CustomEditorWindow : CustomEditor
	{
		private class Win : EditorWindow
		{
			private readonly CustomEditorPresenter _presenter;
			private CustomEditorWindow _customEditor;

			public Win(CustomEditorWindow customEditor)
				: base(Editor.Instance, false, ScrollBars.Vertical)
			{
				Title = customEditor.GetType().Name;
				_customEditor = customEditor;

				_presenter = new CustomEditorPresenter(null);
				_presenter.Panel.Parent = this;

				Set(customEditor);
			}

			private void Set(CustomEditorWindow value)
			{
				_customEditor = value;
				_presenter.Select(value);
				_presenter.OverrideEditor = value;
			}

			/// <inheritdoc />
			protected override void OnShow()
			{
				base.OnShow();

				_presenter.BuildLayout();
			}

			/// <inheritdoc />
			protected override void OnClose()
			{
				Set(null);

				base.OnClose();
			}
		}

		private readonly Win _win;

		/// <summary>
		/// Gets the editor window.
		/// </summary>
		public EditorWindow Window => _win;

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomEditorWindow"/> class.
		/// </summary>
		protected CustomEditorWindow()
		{
			_win = new Win(this);
			ScriptsBuilder.ScriptsReloadBegin += OnScriptsReloadBegin;
		}

		/// <summary>
		/// Finalizes an instance of the <see cref="CustomEditorWindow"/> class.
		/// </summary>
		~CustomEditorWindow()
		{
			ScriptsBuilder.ScriptsReloadBegin -= OnScriptsReloadBegin;
		}

		private void OnScriptsReloadBegin()
		{
			// TODO: support restoring custom editor windows on code reload (deselect obj and restore it later)
			Window.Close();
		}

		/// <summary>
		/// Shows the window.
		/// </summary>
		/// <param name="state">Initial window state.</param>
		public void Show(DockState state = DockState.Float)
		{
			_win.Show(state);
		}
	}
}
