////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine
{
    /// <summary>
    /// Represents a part of the model actor mesh infos collection. Contains information about how to render <see cref="Mesh"/>.
    /// </summary>
    public sealed class MeshInfo
    {
        internal ModelActor _modelActor;
        internal readonly int _index;

        /// <summary>
        /// Gets the parent model actor.
        /// </summary>
        /// <value>
        /// The parent model actor.
        /// </value>
        public ModelActor ParentActor => _modelActor;

        /// <summary>
        /// Gets or sets the material used to render the mesh.
        /// If value if null then model asset mesh default material will be used as a fallback.
        /// </summary>
        /// <value>
        /// The material.
        /// </value>
        public MaterialBase Material
        {
            get => ModelActor.Internal_GetMeshMaterial(_modelActor.unmanagedPtr, _index);
            set => ModelActor.Internal_SetMeshMaterial(_modelActor.unmanagedPtr, _index, Object.GetUnmanagedPtr(value));
        }
        
        /// <summary>
        /// Gets the mesh index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int Index => _index;

        internal MeshInfo(ModelActor model, int index)
        {
            _modelActor = model;
            _index = index;
        }
    }
}
