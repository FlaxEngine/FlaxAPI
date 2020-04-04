// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System.Linq;
using FlaxEngine;

namespace FlaxEditor.CustomEditors.Dedicated
{
    /// <summary>
    /// Custom editor for <see cref="ParticleEffect"/>.
    /// </summary>
    /// <seealso cref="ActorEditor" />
    [CustomEditor(typeof(ParticleEffect)), DefaultEditor]
    public class ParticleEffectEditor : ActorEditor
    {
        private bool _isValid;

        private bool IsValid
        {
            get
            {
                // All selected particle effects use the same system
                var effect = (ParticleEffect)Values[0];
                var system = effect.ParticleSystem;
                return system != null && Values.TrueForAll(x => (x as ParticleEffect)?.ParticleSystem == system);
            }
        }

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            base.Initialize(layout);

            _isValid = IsValid;
            if (!_isValid)
                return;

            // Show all effect parameters grouped by the emitter track name
            var effect = (ParticleEffect)Values[0];
            var groups = layout.Group("Parameters");
            groups.Panel.Open(false);
            var parameters = effect.Parameters;
            var parametersGroups = parameters.GroupBy(x => x.EmitterIndex);
            foreach (var parametersGroup in parametersGroups)
            {
                var trackName = parametersGroup.First().TrackName;
                var group = groups.Group(trackName);
                group.Panel.Open(false);

                foreach (var parameter in parametersGroup)
                {
                    if (!parameter.IsPublic)
                        continue;

                    var parameterName = parameter.Name;
                    var value = parameter.Value;
                    var valueType = Utilities.Utils.GetGraphParameterValueType(parameter.ParamType);

                    // Parameter value accessor
                    var valueContainer = new CustomValueContainer(
                                                                  valueType,
                                                                  value,
                                                                  (instance, index) => ((ParticleEffect)Values[index]).GetParameterValue(trackName, parameterName),
                                                                  (instance, index, o) => ((ParticleEffect)Values[index]).SetParameterValue(trackName, parameterName, o)
                                                                 );
                    for (int index = 1; index < Values.Count; index++)
                        valueContainer.Add(((ParticleEffect)Values[index]).GetParameterValue(trackName, parameterName));

                    // Default value
                    valueContainer.SetDefaultValue(parameter.DefaultValue);

                    // Prefab value
                    if (Values.HasReferenceValue)
                    {
                        var referenceEffect = (ParticleEffect)Values.ReferenceValue;
                        var referenceParameter = referenceEffect.GetParameter(trackName, parameterName);
                        if (referenceParameter != null)
                        {
                            valueContainer.SetReferenceValue(referenceParameter.Value);
                        }
                    }

                    group.Property(parameter.Name, valueContainer);
                }
            }
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            if (_isValid != IsValid)
                RebuildLayout();

            base.Refresh();
        }
    }
}
