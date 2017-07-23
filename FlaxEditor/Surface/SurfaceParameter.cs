////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Represents parameter in the Surface.
    /// </summary>
    public class SurfaceParameter
    {
        /// <summary>
        /// Parameter type
        /// </summary>
        public ParameterType Type;

        /// <summary>
        /// Parameter unique ID
        /// </summary>
        public Guid ID;

        /// <summary>
        /// Parameter name
        /// </summary>
        public string Name;

        /// <summary>
        /// True if is exposed outside
        /// </summary>
        public bool IsPublic;

        /// <summary>
        /// True if cannot edit value
        /// </summary>
        public bool IsStatic;

        /// <summary>
        /// True if can see via UI
        /// </summary>
        public bool IsUIVisible;

        /// <summary>
        /// True if can edit via UI
        /// </summary>
        public bool IsUIEditable;

        /// <summary>
        /// List of nodes referencing to that parameter
        /// </summary>
        public readonly List<SurfaceNode> ReferencedBy = new List<SurfaceNode>();

        /// <summary>
        /// Parameter value
        /// </summary>
        public object Value;
        
        /// <summary>
        /// The metadata.
        /// </summary>
        public readonly SurfaceMeta Meta = new SurfaceMeta();
    }
}
