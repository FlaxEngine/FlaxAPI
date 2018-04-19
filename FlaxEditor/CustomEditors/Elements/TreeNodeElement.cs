// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors.Elements
{
	/// <summary>
	/// The tree structure node element.
	/// </summary>
	/// <seealso cref="FlaxEditor.CustomEditors.LayoutElementsContainer" />
	public class TreeNodeElement : LayoutElementsContainer
	{
		/// <summary>
		/// The tree node control.
		/// </summary>
		public readonly TreeNode TreeNode = new TreeNode(false);

		/// <inheritdoc />
		public override ContainerControl ContainerControl => TreeNode;

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
