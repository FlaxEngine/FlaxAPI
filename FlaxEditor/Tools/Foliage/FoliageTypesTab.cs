// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.CustomEditors;
using FlaxEditor.GUI;
using FlaxEngine.GUI;

namespace FlaxEditor.Tools.Foliage
{
    /// <summary>
    /// Foliage types editor tab. Allows to add, remove or modify foliage instance types defined for the current foliage object.
    /// </summary>
    /// <seealso cref="FlaxEditor.GUI.Tab" />
    public class FoliageTypesTab : Tab
    {
        /// <summary>
        /// The object for foliage type settings adjusting via Custom Editor.
        /// </summary>
        private sealed class ProxyObject
        {
            // TODO: expose selected foliage type properties
        }

        private readonly ProxyObject _proxy;
        private readonly CustomEditorPresenter _presenter;

        /// <summary>
        /// The parent foliage types tab.
        /// </summary>
        public readonly FoliageTab FoliageTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="FoliageTypesTab"/> class.
        /// </summary>
        /// <param name="tab">The parent tab.</param>
        public FoliageTypesTab(FoliageTab tab)
        : base("Foliage Types")
        {
            FoliageTypes = tab;
            _proxy = new ProxyObject();

            // Main panel
            var panel = new Panel(ScrollBars.Both)
            {
                DockStyle = DockStyle.Fill,
                Parent = this
            };

            // Options editor
            // TODO: use editor undo for changing brush options
            var editor = new CustomEditorPresenter(null);
            editor.Panel.Parent = panel;
            editor.Select(_proxy);
            _presenter = editor;
        }
    }
}
