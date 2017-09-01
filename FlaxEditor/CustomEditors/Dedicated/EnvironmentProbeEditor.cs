////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.CustomEditors.Editors;
using FlaxEngine;

namespace FlaxEditor.CustomEditors.Dedicated
{
    /// <summary>
    /// Custom editor for <see cref="EnvironmentProbe"/>.
    /// </summary>
    /// <seealso cref="FlaxEditor.CustomEditors.Editors.GenericEditor" />
    [CustomEditor(typeof(EnvironmentProbe))]
    public sealed class EnvironmentProbeEditor : GenericEditor
    {
        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            base.Initialize(layout);

            if (Values.HasDiffrentTypes == false)
            {
                // Add 'Bake' button
                layout.Space(10);
                var button = layout.Button("Bake");
                button.Button.Clicked += BakeButtonClicked;
            }
        }

        private void BakeButtonClicked()
        {
            for (int i = 0; i < Values.Count; i++)
            {
                if (Values[i] is EnvironmentProbe envProbe)
                {
                    envProbe.Bake();
                    Editor.Instance.Scene.MarkSceneEdited(envProbe.Scene);
                }
            }
        }
    }
}
