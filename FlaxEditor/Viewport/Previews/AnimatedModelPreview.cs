// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.Rendering;
using Object = FlaxEngine.Object;

namespace FlaxEditor.Viewport.Previews
{
    /// <summary>
    /// Animated model asset preview editor viewport.
    /// </summary>
    /// <seealso cref="AssetPreview" />
    public class AnimatedModelPreview : AssetPreview
    {
        private AnimatedModel _previewModel;
        private StaticModel _previewBonesActor;
        private Model _previewBonesModel;
        private AnimatedModel.Pose _previewModelPose;
        private int _previewBonesCounter;
        private List<Vector3> _previewBonesVertex;
        private List<int> _previewBonesIndex;

        /// <summary>
        /// Gets or sets the skinned model asset to preview.
        /// </summary>
        public SkinnedModel SkinnedModel
        {
            get => _previewModel.SkinnedModel;
            set => _previewModel.SkinnedModel = value;
        }

        /// <summary>
        /// Gets the skinned model actor used to preview selected asset.
        /// </summary>
        public AnimatedModel PreviewActor => _previewModel;

        /// <summary>
        /// Gets or sets a value indicating whether play the animation in editor.
        /// </summary>
        public bool PlayAnimation { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether show animated model skeleton bones debug view.
        /// </summary>
        public bool ShowBones { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether scale the model to the normalized bounds.
        /// </summary>
        public bool ScaleToFit { get; set; } = true;

        /// <summary>
        /// Gets or sets the custom mask for the skeleton bones. Bones with false values will be skipped during rendering. Works only if <see cref="ShowBones"/> is set to true and the array matches the attached <see cref="SkinnedModel"/> bones hierarchy.
        /// </summary>
        public bool[] BonesMask { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedModelPreview"/> class.
        /// </summary>
        /// <param name="useWidgets">if set to <c>true</c> use widgets.</param>
        public AnimatedModelPreview(bool useWidgets)
        : base(useWidgets)
        {
            Task.Begin += OnBegin;

            // Setup preview scene
            _previewModel = AnimatedModel.New();
            _previewModel.UseTimeScale = false;
            _previewModel.UpdateWhenOffscreen = true;
            _previewModel.BoundsScale = 1000.0f;
            _previewModel.UpdateMode = AnimatedModel.AnimationUpdateMode.Manual;
            _previewBonesModel = FlaxEngine.Content.CreateVirtualAsset<Model>();
            _previewBonesModel.SetupLODs(1);
            _previewBonesActor = StaticModel.New();
            _previewBonesActor.Model = _previewBonesModel;
            _previewBonesActor.Entries[0].Material = FlaxEngine.Content.LoadAsyncInternal<MaterialBase>(EditorAssets.WiresDebugMaterial);

            // Link actors for rendering
            Task.CustomActors.Add(_previewModel);
            Task.CustomActors.Add(_previewBonesActor);
        }

        private void OnBegin(SceneRenderTask task, GPUContext context)
        {
            // Update preview model scale to fit the preview
            var skinnedModel = SkinnedModel;
            if (skinnedModel && skinnedModel.IsLoaded)
            {
                float targetSize = 30.0f;
                float maxSize = Mathf.Max(0.001f, skinnedModel.Box.Size.MaxValue);
                _previewModel.Scale = new Vector3(targetSize / maxSize);
            }
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // Manually update animation
            if (PlayAnimation)
            {
                _previewModel.UpdateAnimation();
            }

            // Update the bones debug (once every few frames)
            _previewBonesActor.Transform = _previewModel.Transform;
            var updateBonesCount = PlayAnimation || _previewBonesVertex?.Count == 0 ? 1 : 10;
            _previewBonesActor.IsActive = ShowBones;
            if (_previewBonesCounter++ % updateBonesCount == 0 && ShowBones)
            {
                _previewModel.GetCurrentPose(ref _previewModelPose);
                var skeleton = _previewModel.SkinnedModel?.Skeleton;
                if (_previewModelPose.Bones == null || _previewModelPose.Bones.Length == 0 || skeleton == null)
                {
                    _previewBonesActor.IsActive = false;
                }
                else
                {
                    if (_previewBonesVertex == null)
                        _previewBonesVertex = new List<Vector3>(1024 * 2);
                    else
                        _previewBonesVertex.Clear();
                    if (_previewBonesIndex == null)
                        _previewBonesIndex = new List<int>(1024 * 3);
                    else
                        _previewBonesIndex.Clear();

                    // Draw bounding box at the bone locations
                    var bonesMask = BonesMask != null && BonesMask.Length == skeleton.Length ? BonesMask : null;
                    var boneBox = new OrientedBoundingBox(new Vector3(-1.0f), new Vector3(1.0f));
                    for (int i = 0; i < _previewModelPose.Bones.Length; i++)
                    {
                        if (bonesMask != null && !bonesMask[i])
                            continue;

                        var boneTransform = _previewModelPose.Bones[i];
                        boneTransform.Decompose(out var scale, out var _, out var _);
                        boneTransform = Matrix.Invert(Matrix.Scaling(scale)) * boneTransform;

                        // Some inlined code to improve peformance
                        var box = boneBox * boneTransform;
                        //
                        var iStart = _previewBonesVertex.Count;
                        box.GetCorners(_previewBonesVertex);
                        //
                        _previewBonesIndex.Add(iStart + 0);
                        _previewBonesIndex.Add(iStart + 1);
                        _previewBonesIndex.Add(iStart + 0);
                        //
                        _previewBonesIndex.Add(iStart + 0);
                        _previewBonesIndex.Add(iStart + 4);
                        _previewBonesIndex.Add(iStart + 0);
                        //
                        _previewBonesIndex.Add(iStart + 1);
                        _previewBonesIndex.Add(iStart + 2);
                        _previewBonesIndex.Add(iStart + 1);
                        //
                        _previewBonesIndex.Add(iStart + 1);
                        _previewBonesIndex.Add(iStart + 5);
                        _previewBonesIndex.Add(iStart + 1);
                        //
                        _previewBonesIndex.Add(iStart + 2);
                        _previewBonesIndex.Add(iStart + 3);
                        _previewBonesIndex.Add(iStart + 2);
                        //
                        _previewBonesIndex.Add(iStart + 2);
                        _previewBonesIndex.Add(iStart + 6);
                        _previewBonesIndex.Add(iStart + 2);
                        //
                        _previewBonesIndex.Add(iStart + 3);
                        _previewBonesIndex.Add(iStart + 7);
                        _previewBonesIndex.Add(iStart + 3);
                        //
                        _previewBonesIndex.Add(iStart + 4);
                        _previewBonesIndex.Add(iStart + 5);
                        _previewBonesIndex.Add(iStart + 4);
                        //
                        _previewBonesIndex.Add(iStart + 4);
                        _previewBonesIndex.Add(iStart + 7);
                        _previewBonesIndex.Add(iStart + 4);
                        //
                        _previewBonesIndex.Add(iStart + 5);
                        _previewBonesIndex.Add(iStart + 6);
                        _previewBonesIndex.Add(iStart + 5);
                        //
                        _previewBonesIndex.Add(iStart + 6);
                        _previewBonesIndex.Add(iStart + 7);
                        _previewBonesIndex.Add(iStart + 6);
                        //
                    }

                    // Bone bone connections
                    for (int i = 0; i < skeleton.Length; i++)
                    {
                        int parentIndex = skeleton[i].ParentIndex;

                        if (parentIndex != -1)
                        {
                            if (bonesMask != null && (!bonesMask[i] || !bonesMask[parentIndex]))
                                continue;

                            var parentPos = _previewModelPose.GetBonePosition(parentIndex);
                            var bonePos = _previewModelPose.GetBonePosition(i);

                            var iStart = _previewBonesVertex.Count;
                            _previewBonesVertex.Add(parentPos);
                            _previewBonesVertex.Add(bonePos);
                            _previewBonesIndex.Add(iStart + 0);
                            _previewBonesIndex.Add(iStart + 1);
                            _previewBonesIndex.Add(iStart + 0);
                        }
                    }

                    if (_previewBonesIndex.Count > 0)
                        _previewBonesModel.LODs[0].Meshes[0].UpdateMesh(_previewBonesVertex.ToArray(), _previewBonesIndex.ToArray());
                    else
                        _previewBonesActor.IsActive = false;
                }
            }
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Ensure to cleanup created actor objects
            _previewBonesActor.Model = null;
            Object.Destroy(ref _previewModel);
            Object.Destroy(ref _previewBonesActor);
            Object.Destroy(ref _previewBonesModel);
            BonesMask = null;

            base.OnDestroy();
        }
    }
}
