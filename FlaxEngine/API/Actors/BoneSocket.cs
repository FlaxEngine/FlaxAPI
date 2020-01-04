// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    public sealed partial class BoneSocket
    {
        /// <summary>
        /// Gets or sets the name of the skeleton bone that socket is attached to.
        /// For better performance use <see cref="BoneIndex"/> as <see cref="BoneName"/> can introduce stalls and uses string for a lookup.
        /// </summary>
        [EditorOrder(10), EditorDisplay("Bone Socket"), CustomEditorAlias("FlaxEditor.CustomEditors.Editors.SkeletonBoneEditor"), Tooltip("The name of the bone to track its transform.")]
        public string BoneName
        {
            get
            {
                if (Parent is AnimatedModel animatedModel && animatedModel.SkinnedModel)
                {
                    if (!animatedModel.SkinnedModel.WaitForLoaded())
                    {
                        var boneIndex = BoneIndex;
                        var nodes = animatedModel.SkinnedModel.Nodes;
                        var bones = animatedModel.SkinnedModel.Bones;
                        if (nodes != null && bones != null && boneIndex < bones.Length)
                            return nodes[bones[boneIndex].NodeIndex].Name;
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
                        var nodes = animatedModel.SkinnedModel.Nodes;
                        var bones = animatedModel.SkinnedModel.Bones;
                        if (nodes != null && bones != null)
                        {
                            for (int i = 0; i < bones.Length; i++)
                            {
                                if (string.Equals(nodes[bones[i].NodeIndex].Name, value, StringComparison.OrdinalIgnoreCase))
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
