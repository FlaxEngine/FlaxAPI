// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.CustomEditors.Dedicated
{
    /// <summary>
    /// Custom editor for <see cref="AnimatedModel"/>.
    /// </summary>
    /// <seealso cref="ActorEditor" />
    [CustomEditor(typeof(AnimatedModel)), DefaultEditor]
    public class AnimatedModelEditor : ActorEditor
    {
        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            base.Initialize(layout);

            // Show instanced parameters to view/edit at runtime
            if (Values.IsSingleObject && Editor.Instance.StateMachine.IsPlayMode)
            {
                var group = layout.Group("Parameters");
                group.Panel.Open(false);
                group.Panel.IndexInParent -= 2;
                var animatedModel = (AnimatedModel)Values[0];
                var parameters = animatedModel.Parameters;
                for (int i = 0; i < parameters.Length; i++)
                {
                    var param = parameters[i];
                    if (!param.IsPublic)
                        continue;

                    var id = param.Identifier;
                    var value = param.Value;
                    var valueType = Utilities.Utils.GetGraphParameterValueType(param.Type);
                    var valueContainer = new CustomValueContainer(
                                                                  valueType,
                                                                  value,
                                                                  (instance, index) => animatedModel.GetParameterValue(id),
                                                                  (instance, index, o) => animatedModel.SetParameterValue(id, o)
                                                                 );
                    group.Property(param.Name, valueContainer);
                }
            }
        }
    }
}
