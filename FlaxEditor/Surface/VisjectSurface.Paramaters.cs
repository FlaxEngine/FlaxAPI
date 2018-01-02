////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace FlaxEditor.Surface
{
    public partial class VisjectSurface
    {
        /// <summary>
        /// The collection of the surface parameters.
        /// </summary>
        public readonly List<SurfaceParameter> Parameters = new List<SurfaceParameter>();

        /// <inheritdoc />
        public void OnParamCreated(SurfaceParameter param)
        {
            for (int i = 0; i < _nodes.Count; i++)
            {
                if (_nodes[i] is IParametersDependantNode node)
                    node.OnParamCreated(param);
            }
            MarkAsEdited();
        }

        /// <inheritdoc />
        public void OnParamRenamed(SurfaceParameter param)
        {
            for (int i = 0; i < _nodes.Count; i++)
            {
                if (_nodes[i] is IParametersDependantNode node)
                    node.OnParamRenamed(param);
            }
            MarkAsEdited();
        }

        /// <inheritdoc />
        public void OnParamDeleted(SurfaceParameter param)
        {
            for (int i = 0; i < _nodes.Count; i++)
            {
                if (_nodes[i] is IParametersDependantNode node)
                    node.OnParamDeleted(param);
            }
            MarkAsEdited();
        }
    }
}
