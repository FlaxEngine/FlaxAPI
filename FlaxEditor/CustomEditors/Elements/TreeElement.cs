// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Elements
{
	/// <summary>
	/// The tree structure element.
	/// </summary>
	/// <seealso cref="FlaxEditor.CustomEditors.LayoutElementsContainer" />
	public class TreeElement : LayoutElementsContainer
	{
		/// <summary>
		/// The tree control.
		/// </summary>
		public readonly Tree TreeControl = new Tree(false);

		/// <inheritdoc />
		public override ContainerControl ContainerControl => TreeControl;

		/// <summary>
		/// Adds new tree node element.
		/// </summary>
		/// <param name="text">The node name (title text).</param>
		/// <returns>The created element.</returns>
		public TreeNodeElement Node(string text)
		{
			TreeNodeElement element = new TreeNodeElement();
			element.TreeNode.Text = text;
			OnAddElement(element);
			return element;
		}
	}
}
