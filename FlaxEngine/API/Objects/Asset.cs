////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine
{
    /// <summary>
    /// Basic types of the content assets base types
    /// </summary>
    public enum ContentDomain
    {
        Invalid,
        Texture,
        CubeTexture,
        Material,
        Model,
        Prefab,
        Document,
        Other,
        Shader,
        Font,
        IESProfile,
        Scene
    }

    /// <summary>
    /// Assets objects base class.
    /// </summary>
    public partial class Asset
	{
        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Path} ({GetType().Name})";
        }
    }
}
