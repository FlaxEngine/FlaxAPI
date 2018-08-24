// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    public sealed partial class AnimatedModel
    {
        /// <summary>
        /// Contains the snapshot of the animated model pose.
        /// </summary>
        public struct Pose
        {
            /// <summary>
            /// The per-bone final transformations in actor world-space.
            /// </summary>
            public Matrix[] Bones;

            /// <summary>
            /// Gets the position of the bone in the actor local space.
            /// </summary>
            /// <param name="index">The bone index.</param>
            /// <returns>The bone position.</returns>
            public Vector3 GetBonePosition(int index)
            {
                return Bones[index].TranslationVector;
            }
        }

        /// <summary>
        /// Describes the animation graph updates frequency for the animated model.
        /// </summary>
        public enum AnimationUpdateMode
        {
            /// <summary>
            /// The automatic updates will be used (based on platform capabilities, distance to the player, etc.).
            /// </summary>
            Auto = 0,

            /// <summary>
            /// Animation will be updated every game update.
            /// </summary>
            EveryUpdate = 1,

            /// <summary>
            /// Animation will be updated every second game update.
            /// </summary>
            EverySecondUpdate = 2,

            /// <summary>
            /// Animation will be updated every fourth game update.
            /// </summary>
            EveryFourthUpdate = 3,

            /// <summary>
            /// Animation can be updated manually by the user scripts. Use <see cref="AnimatedModel.UpdateAnimation"/> method.
            /// </summary>
            Manual = 4,

            /// <summary>
            /// Animation won't be updated at all.
            /// </summary>
            Never = 5,
        }

        private ModelEntryInfo[] _entries;
        private AnimationGraphParameter[] _parameters;

        /// <summary>
        /// Helper value used to keep parameters collection in sync with actual backend data.
        /// </summary>
        internal int _parametersHash;

        /// <summary>
        /// Gets or sets the animation graph parameters collection.
        /// </summary>
        /// <remarks>
        /// It's null or empty if the <see cref="AnimationGraph"/> property is null or asset is not loaded yet.
        /// It's highly recommended to use <see cref="GetParam(int)"/> and cache the returned object to improve your game logic performance.
        /// </remarks>
        [NoSerialize]
        [HideInEditor] // TODO: maybe show parameters in play-mode ?? so it would be easier to debug/develop them
        public AnimationGraphParameter[] Parameters
        {
            get
            {
                // Check if has cached value
                if (_parameters != null)
                    return _parameters;

                // Get next hash #hashtag
                _parametersHash++;

                // Get parameters metadata from the backend
                var parameters = Internal_CacheParameters(unmanagedPtr);
                if (parameters != null && parameters.Length > 0)
                {
                    _parameters = new AnimationGraphParameter[parameters.Length];
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var p = parameters[i];

                        // Packed:
                        // Bits 0-7: Type
                        // Bit 8: IsPublic
                        var type = (AnimationGraphParameterType)(p & 0b1111);
                        var isPublic = (p & 0b10000) != 0;

                        _parameters[i] = new AnimationGraphParameter(_parametersHash, this, i, type, isPublic);
                    }
                }
                else
                {
                    // No parameters at all
                    _parameters = new AnimationGraphParameter[0];
                }

                return _parameters;
            }
        }

        /// <summary>
        /// Gets the skinned model entries collection. Each <see cref="ModelEntryInfo"/> contains data how to render meshes using this entry (material, shadows casting, etc.).
        /// </summary>
        /// <remarks>
        /// It's null if the <see cref="SkinnedModel"/> property is null or asset is not loaded yet.
        /// </remarks>
        [Serialize]
        [EditorOrder(100), EditorDisplay("Entries", EditorDisplayAttribute.InlineStyle)]
        [MemberCollection(CanReorderItems = false, NotNullItems = true, ReadOnly = true)]
        public ModelEntryInfo[] Entries
        {
            get
            {
                // Check if has cached data
                if (_entries != null)
                    return _entries;

                // Cache data
                var model = SkinnedModel;
                if (model && model.IsLoaded)
                {
                    var meshesCount = model.MaterialSlotsCount;
                    _entries = new ModelEntryInfo[meshesCount];
                    for (int i = 0; i < meshesCount; i++)
                    {
                        _entries[i] = new ModelEntryInfo(this, i);
                    }
                }

                return _entries;
            }
            internal set
            {
                // Used by the serialization system

                _entries = value;

                EntriesChanged?.Invoke(this);
            }
        }

        /// <summary>
        /// Gets the parameter by index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The material parameter.</returns>
        public AnimationGraphParameter GetParam(int index)
        {
            return Parameters[index];
        }

        /// <summary>
        /// Gets the parameter by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The animation graph parameter.</returns>
        public AnimationGraphParameter GetParam(string name)
        {
            var parameters = Parameters;
            var index = Internal_GetParamIndexByName(unmanagedPtr, name);
            return index >= 0 && index < parameters.Length ? parameters[index] : null;
        }

        /// <summary>
        /// Gets the parameter by ID.
        /// </summary>
        /// <param name="paramId">The id.</param>
        /// <returns>The animation graph parameter.</returns>
        public AnimationGraphParameter GetParam(Guid paramId)
        {
            var parameters = Parameters;
            var index = Internal_GetParamIndexById(unmanagedPtr, ref paramId);
            return index >= 0 && index < parameters.Length ? parameters[index] : null;
        }

        /// <summary>
        /// Occurs when entries collection gets changed.
        /// It's called on <see cref="AnimatedModel"/> skinned model changed or when model asset gets reloaded, etc.
        /// </summary>
        public event Action<AnimatedModel> EntriesChanged;

        /// <summary>
        /// Occurs when animation graph parameters collection gets changed.
        /// It's called on <see cref="AnimatedModel"/> animation graph asset changed or when graph asset gets reloaded, etc.
        /// </summary>
        public event Action<AnimatedModel> ParametersChanged;

        internal void Internal_OnSkinnedModelChanged()
        {
            // Clear cached data
            _entries = null;

            EntriesChanged?.Invoke(this);
        }

        internal void Internal_OnAnimationGraphChanged()
        {
            // Clear cached data
            _parametersHash++;
            _parameters = null;

            ParametersChanged?.Invoke(this);
        }

        /// <summary>
        /// Gets the current animated skeleton pose. Will allocate the bone transformation array memory or reuse the cached one.
        /// </summary>
        /// <param name="pose">The output pose.</param>
        public void GetCurrentPose(ref Pose pose)
        {
            Internal_GetCurrentPose(unmanagedPtr, ref pose);
        }

        #region Internal Calls

#if !UNIT_TEST_COMPILANT
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void Internal_GetCurrentPose(IntPtr obj, ref Pose pose);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern ulong[] Internal_CacheParameters(IntPtr obj);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetParamName(IntPtr obj, int index);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_SetParamValue(IntPtr obj, int index, IntPtr ptr);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern string Internal_GetParamValue(IntPtr obj, int index, IntPtr ptr);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetParamIndexByName(IntPtr obj, string name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int Internal_GetParamIndexById(IntPtr obj, ref Guid id);
#endif

        #endregion
    }
}
