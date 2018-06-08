// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    public sealed partial class BoneSocket
    {
        /// <summary>
        /// Gets or sets the name of the skeleton bone that socket is attached to.
        /// For better performance use <see cref="BoneIndex"/> as <see cref="BoneName"/> can introduce stalls and uses string for a lookup.
        /// </summary>
        [EditorOrder(10), EditorDisplay("Bone Socket"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.SkeletonBoneEditor"), Tooltip("The name of the bone to track its transfrom.")]
        public string BoneName
        {
            get
            {
                if (Parent is AnimatedModel animatedModel && animatedModel.SkinnedModel)
                {
                    if (!animatedModel.SkinnedModel.WaitForLoaded())
                    {
                        var boneIndex = BoneIndex;
                        var skeleton = animatedModel.SkinnedModel.Skeleton;
                        if (skeleton != null && boneIndex < skeleton.Length)
                            return skeleton[boneIndex].Name;
                    }
                }

                return string.Empty;
            }
            set
            {
                if (Parent is AnimatedModel animatedModel && animatedModel.SkinnedModel)
                {
                    if (!animatedModel.SkinnedModel.WaitForLoaded())
                    {
                        var skeleton = animatedModel.SkinnedModel.Skeleton;
                        if (skeleton != null)
                        {
                            for (int i = 0; i < skeleton.Length; i++)
                            {
                                if (string.Equals(skeleton[i].Name, value, StringComparison.OrdinalIgnoreCase))
                                {
                                    BoneIndex = i;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
