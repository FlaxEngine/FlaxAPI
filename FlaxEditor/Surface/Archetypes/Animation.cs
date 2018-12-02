// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.Surface.Elements;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.Archetypes
{
    /// <summary>
    /// Contains archetypes for nodes from the Animation group.
    /// </summary>
    public static partial class Animation
    {
        /// <summary>
        /// Customized <see cref="SurfaceNode"/> for the main animation graph node.
        /// </summary>
        /// <seealso cref="FlaxEditor.Surface.SurfaceNode" />
        public class Output : SurfaceNode
        {
            /// <inheritdoc />
            public Output(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
            }
        }

        /// <summary>
        /// Customized <see cref="SurfaceNode"/> for the animation sampling nodes
        /// </summary>
        /// <seealso cref="FlaxEditor.Surface.SurfaceNode" />
        public class Sample : SurfaceNode
        {
            /// <inheritdoc />
            public Sample(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
            }

            /// <inheritdoc />
            public override void SetValue(int index, object value)
            {
                base.SetValue(index, value);
                UpdateTitle();
            }

            /// <inheritdoc />
            public override void OnSurfaceLoaded()
            {
                base.OnSurfaceLoaded();

                UpdateTitle();
            }

            private void UpdateTitle()
            {
                var asset = Editor.Instance.ContentDatabase.Find((Guid)Values[0]);
                Title = asset?.ShortName ?? "Animation";
                var style = Style.Current;
                Resize(Mathf.Max(230, style.FontLarge.MeasureText(Title).X + 20), 160);
            }
        }

        /// <summary>
        /// Customized <see cref="SurfaceNode"/> for the animation poses blending.
        /// </summary>
        /// <seealso cref="FlaxEditor.Surface.SurfaceNode" />
        public class BlendPose : SurfaceNode
        {
            private readonly List<InputBox> _blendPoses = new List<InputBox>(MaxBlendPoses);
            private Button _addButton;
            private Button _removeButton;

            /// <summary>
            /// The maximum amount of the blend poses to support.
            /// </summary>
            public const int MaxBlendPoses = 8;

            /// <summary>
            /// The index of the first input blend pose box.
            /// </summary>
            public const int FirstBlendPoseBoxIndex = 3;

            /// <summary>
            /// Gets or sets used blend poses count (visible to the user).
            /// </summary>
            public int BlendPosesCount
            {
                get => (int)Values[2];
                set
                {
                    value = Mathf.Clamp(value, 0, MaxBlendPoses);
                    if (value != BlendPosesCount)
                    {
                        SetValue(2, value);
                    }
                }
            }

            /// <inheritdoc />
            public BlendPose(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
            : base(id, context, nodeArch, groupArch)
            {
                // Add buttons for adding/removing blend poses
                _addButton = new Button(70, 94, 20, 20)
                {
                    Text = "+",
                    Parent = this
                };
                _addButton.Clicked += () => BlendPosesCount++;
                _removeButton = new Button(_addButton.Right + 4, _addButton.Y, 20, 20)
                {
                    Text = "-",
                    Parent = this
                };
                _removeButton.Clicked += () => BlendPosesCount--;
            }

            private void UpdateBoxes()
            {
                var posesCount = BlendPosesCount;
                while (_blendPoses.Count > posesCount)
                {
                    var boxIndex = _blendPoses.Count - 1;
                    var box = _blendPoses[boxIndex];
                    _blendPoses.RemoveAt(boxIndex);
                    RemoveElement(box);
                }
                while (_blendPoses.Count < posesCount)
                {
                    var ylevel = 3 + _blendPoses.Count;
                    var arch = new NodeElementArchetype
                    {
                        Type = NodeElementType.Input,
                        Position = new Vector2(
                            FlaxEditor.Surface.Constants.NodeMarginX - FlaxEditor.Surface.Constants.BoxOffsetX,
                            FlaxEditor.Surface.Constants.NodeMarginY + FlaxEditor.Surface.Constants.NodeHeaderSize + ylevel * FlaxEditor.Surface.Constants.LayoutOffsetY),
                        Text = "Pose " + _blendPoses.Count,
                        Single = true,
                        ValueIndex = -1,
                        BoxID = FirstBlendPoseBoxIndex + _blendPoses.Count,
                        ConnectionsType = ConnectionType.Impulse
                    };
                    var box = new InputBox(this, arch);
                    AddElement(box);
                    _blendPoses.Add(box);
                }

                _addButton.Enabled = posesCount < MaxBlendPoses;
                _removeButton.Enabled = posesCount > 0;
            }

            private void UpdateHeight()
            {
                float nodeHeight = 10 + (Mathf.Max(_blendPoses.Count, 1) + 3) * FlaxEditor.Surface.Constants.LayoutOffsetY;
                Height = nodeHeight + FlaxEditor.Surface.Constants.NodeMarginY * 2 + FlaxEditor.Surface.Constants.NodeHeaderSize + FlaxEditor.Surface.Constants.NodeFooterSize;
            }

            /// <inheritdoc />
            public override void OnSurfaceLoaded()
            {
                base.OnSurfaceLoaded();

                // Peek deserialized boxes
                _blendPoses.Clear();
                for (int i = 0; i < Elements.Count; i++)
                {
                    if (Elements[i] is InputBox box && box.Archetype.BoxID >= FirstBlendPoseBoxIndex)
                    {
                        _blendPoses.Add(box);
                    }
                }

                UpdateBoxes();
                UpdateHeight();
            }

            /// <inheritdoc />
            public override void SetValue(int index, object value)
            {
                base.SetValue(index, value);

                // Check if update amount of blend pose inputs
                if (index == 2 && _blendPoses.Count != BlendPosesCount)
                {
                    UpdateBoxes();
                    UpdateHeight();
                }
            }
        }

        /// <summary>
        /// The bone transformation modes.
        /// </summary>
        public enum BoneTransformMode
        {
            /// <summary>
            /// No transformation.
            /// </summary>
            None = 0,

            /// <summary>
            /// Applies the transformation.
            /// </summary>
            Add = 1,

            /// <summary>
            /// Replaces the transformation.
            /// </summary>
            Replace = 2,
        }

        /// <summary>
        /// The animated model root motion mode.
        /// </summary>
        public enum RootMotionMode
        {
            /// <summary>
            /// Don't extract nor apply the root motion.
            /// </summary>
            NoExtraction = 0,

            /// <summary>
            /// Ignore root motion (remove from root node transform).
            /// </summary>
            Ignore = 1,

            /// <summary>
            /// Enable root motion (remove from root node transform and apply to the target).
            /// </summary>
            Enable = 2,
        }

        /// <summary>
        /// The nodes for that group.
        /// </summary>
        public static NodeArchetype[] Nodes =
        {
            new NodeArchetype
            {
                TypeID = 1,
                Create = (id, context, arch, groupArch) => new Output(id, context, arch, groupArch),
                Title = "Animation Output",
                Description = "Main animation graph output node",
                Flags = NodeFlags.AnimGraphOnly | NodeFlags.NoRemove | NodeFlags.NoSpawnViaGUI | NodeFlags.NoCloseButton,
                Size = new Vector2(200, 100),
                DefaultValues = new object[]
                {
                    (int)RootMotionMode.NoExtraction,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Animation", true, ConnectionType.Impulse, 0),
                    NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY, "Root Motion:"),
                    NodeElementArchetype.Factory.ComboBox(80, Surface.Constants.LayoutOffsetY, 100, 0, typeof(RootMotionMode))
                }
            },
            new NodeArchetype
            {
                TypeID = 2,
                Create = (id, context, arch, groupArch) => new Sample(id, context, arch, groupArch),
                Title = "Animation",
                Description = "Animation sampling",
                Flags = NodeFlags.AnimGraphOnly,
                Size = new Vector2(230, 160),
                DefaultValues = new object[]
                {
                    Guid.Empty,
                    1.0f,
                    true,
                    0.0f,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Impulse, 0),
                    NodeElementArchetype.Factory.Output(1, "Normalized Time", ConnectionType.Float, 1),
                    NodeElementArchetype.Factory.Output(2, "Time", ConnectionType.Float, 1),
                    NodeElementArchetype.Factory.Output(3, "Length", ConnectionType.Float, 3),
                    NodeElementArchetype.Factory.Output(4, "Is Playing", ConnectionType.Bool, 4),
                    NodeElementArchetype.Factory.Input(0, "Speed", true, ConnectionType.Float, 5, 1),
                    NodeElementArchetype.Factory.Input(1, "Loop", true, ConnectionType.Bool, 6, 2),
                    NodeElementArchetype.Factory.Input(2, "Start Position", true, ConnectionType.Float, 7, 3),
                    NodeElementArchetype.Factory.Asset(0, Surface.Constants.LayoutOffsetY * 3, 0, ContentDomain.Animation),
                }
            },
            new NodeArchetype
            {
                TypeID = 3,
                Title = "Transform Bone (local space)",
                Description = "Transforms the skeleton bone",
                Flags = NodeFlags.AnimGraphOnly,
                Size = new Vector2(270, 130),
                DefaultValues = new object[]
                {
                    0,
                    (int)BoneTransformMode.Add,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Impulse, 0),
                    NodeElementArchetype.Factory.Input(0, string.Empty, true, ConnectionType.Impulse, 1),
                    NodeElementArchetype.Factory.Input(1, "Translation", true, ConnectionType.Vector3, 2),
                    NodeElementArchetype.Factory.Input(2, "Rotation", true, ConnectionType.Rotation, 3),
                    NodeElementArchetype.Factory.Input(3, "Scale", true, ConnectionType.Vector3, 4),
                    NodeElementArchetype.Factory.SkeletonNodeSelect(40, Surface.Constants.LayoutOffsetY * 4, 120, 0),
                    NodeElementArchetype.Factory.ComboBox(40, Surface.Constants.LayoutOffsetY * 5, 120, 1, typeof(BoneTransformMode)),
                    NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY * 4, "Bone:"),
                    NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY * 5, "Mode:"),
                }
            },
            new NodeArchetype
            {
                TypeID = 4,
                Title = "Transform Bone (global space)",
                Description = "Transforms the skeleton bone",
                Flags = NodeFlags.AnimGraphOnly,
                Size = new Vector2(270, 130),
                DefaultValues = new object[]
                {
                    0,
                    (int)BoneTransformMode.Add,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.ImpulseSecondary, 0),
                    NodeElementArchetype.Factory.Input(0, string.Empty, true, ConnectionType.ImpulseSecondary, 1),
                    NodeElementArchetype.Factory.Input(1, "Translation", true, ConnectionType.Vector3, 2),
                    NodeElementArchetype.Factory.Input(2, "Rotation", true, ConnectionType.Rotation, 3),
                    NodeElementArchetype.Factory.Input(3, "Scale", true, ConnectionType.Vector3, 4),
                    NodeElementArchetype.Factory.SkeletonNodeSelect(40, Surface.Constants.LayoutOffsetY * 4, 120, 0),
                    NodeElementArchetype.Factory.ComboBox(40, Surface.Constants.LayoutOffsetY * 5, 120, 1, typeof(BoneTransformMode)),
                    NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY * 4, "Bone:"),
                    NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY * 5, "Mode:"),
                }
            },
            new NodeArchetype
            {
                TypeID = 5,
                Title = "Local To Global",
                Description = "Transforms the skeleton bones from local into global space",
                Flags = NodeFlags.AnimGraphOnly,
                Size = new Vector2(150, 40),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.ImpulseSecondary, 0),
                    NodeElementArchetype.Factory.Input(0, string.Empty, true, ConnectionType.Impulse, 1),
                }
            },
            new NodeArchetype
            {
                TypeID = 6,
                Title = "Global To Local",
                Description = "Transforms the skeleton bones from global into local space",
                Flags = NodeFlags.AnimGraphOnly,
                Size = new Vector2(150, 40),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Impulse, 0),
                    NodeElementArchetype.Factory.Input(0, string.Empty, true, ConnectionType.ImpulseSecondary, 1),
                }
            },
            new NodeArchetype
            {
                TypeID = 7,
                Title = "Copy Bone",
                Description = "Copies the skeleton bone transformation data (in local space)",
                Flags = NodeFlags.AnimGraphOnly,
                Size = new Vector2(260, 140),
                DefaultValues = new object[]
                {
                    0,
                    1,
                    true,
                    true,
                    true,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Impulse, 0),
                    NodeElementArchetype.Factory.Input(0, string.Empty, true, ConnectionType.Impulse, 1),
                    NodeElementArchetype.Factory.SkeletonNodeSelect(100, Surface.Constants.LayoutOffsetY * 1, 120, 0),
                    NodeElementArchetype.Factory.SkeletonNodeSelect(100, Surface.Constants.LayoutOffsetY * 2, 120, 0),
                    NodeElementArchetype.Factory.Bool(100, Surface.Constants.LayoutOffsetY * 3, 2),
                    NodeElementArchetype.Factory.Bool(100, Surface.Constants.LayoutOffsetY * 4, 3),
                    NodeElementArchetype.Factory.Bool(100, Surface.Constants.LayoutOffsetY * 5, 4),
                    NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY * 1, "Source Bone:"),
                    NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY * 2, "Destination Bone:"),
                    NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY * 3, "Copy Translation:"),
                    NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY * 4, "Copy Rotation:"),
                    NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY * 5, "Copy Scale:"),
                }
            },
            new NodeArchetype
            {
                TypeID = 8,
                Title = "Get Bone Transform",
                Description = "Samples the skeleton bone transformation (in global space)",
                Flags = NodeFlags.AnimGraphOnly,
                Size = new Vector2(250, 40),
                DefaultValues = new object[]
                {
                    0,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, string.Empty, true, ConnectionType.ImpulseSecondary, 0),
                    NodeElementArchetype.Factory.SkeletonNodeSelect(40, Surface.Constants.LayoutOffsetY * 1, 120, 0),
                    NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY * 1, "Bone:"),
                    NodeElementArchetype.Factory.Output(0, "Transform", ConnectionType.Transform, 1),
                }
            },
            new NodeArchetype
            {
                TypeID = 9,
                Title = "Blend",
                Description = "Blend animation poses",
                Flags = NodeFlags.AnimGraphOnly,
                Size = new Vector2(170, 80),
                DefaultValues = new object[]
                {
                    0.0f,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Impulse, 0),
                    NodeElementArchetype.Factory.Input(0, "Pose A", true, ConnectionType.Impulse, 1),
                    NodeElementArchetype.Factory.Input(1, "Pose B", true, ConnectionType.Impulse, 2),
                    NodeElementArchetype.Factory.Input(2, "Alpha", true, ConnectionType.Float, 3, 0),
                }
            },
            new NodeArchetype
            {
                TypeID = 10,
                Title = "Blend Additive",
                Description = "Blend animation poses (with additive mode)",
                Flags = NodeFlags.AnimGraphOnly,
                Size = new Vector2(170, 80),
                DefaultValues = new object[]
                {
                    0.0f,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Impulse, 0),
                    NodeElementArchetype.Factory.Input(0, "Base Pose", true, ConnectionType.Impulse, 1),
                    NodeElementArchetype.Factory.Input(1, "Blend Pose", true, ConnectionType.Impulse, 2),
                    NodeElementArchetype.Factory.Input(2, "Blend Alpha", true, ConnectionType.Float, 3, 0),
                }
            },
            new NodeArchetype
            {
                TypeID = 11,
                Title = "Blend with Mask",
                Description = "Blend animation poses using skeleton mask",
                Flags = NodeFlags.AnimGraphOnly,
                Size = new Vector2(180, 100),
                DefaultValues = new object[]
                {
                    0.0f,
                    Guid.Empty,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, string.Empty, ConnectionType.Impulse, 0),
                    NodeElementArchetype.Factory.Input(0, "Pose A", true, ConnectionType.Impulse, 1),
                    NodeElementArchetype.Factory.Input(1, "Pose B", true, ConnectionType.Impulse, 2),
                    NodeElementArchetype.Factory.Input(2, "Alpha", true, ConnectionType.Float, 3, 0),
                    NodeElementArchetype.Factory.Asset(100, 20, 1, ContentDomain.SkeletonMask),
                }
            },
            new NodeArchetype
            {
                TypeID = 12,
                Create = (id, context, arch, groupArch) => new MultiBlend1D(id, context, arch, groupArch),
                Title = "Multi Blend 1D",
                Description = "Animation blending in 1D",
                Flags = NodeFlags.AnimGraphOnly,
                Size = new Vector2(420, 300),
                DefaultValues = new object[]
                {
                    // Node data
                    new Vector4(0, 100.0f, 0, 0),
                    1.0f,
                    true,
                    0.0f,

                    // Per blend sample data
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                },
                Elements = new[]
                {
                    // Output
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Impulse, 0),

                    // Options
                    NodeElementArchetype.Factory.Input(0, "Speed", true, ConnectionType.Float, 1, 1),
                    NodeElementArchetype.Factory.Input(1, "Loop", true, ConnectionType.Bool, 2, 2),
                    NodeElementArchetype.Factory.Input(2, "Start Position", true, ConnectionType.Float, 3, 3),

                    // Axis X
                    NodeElementArchetype.Factory.Input(4, "X", true, ConnectionType.Float, 4),
                    NodeElementArchetype.Factory.Text(30, 4 * Surface.Constants.LayoutOffsetY, "(min:                   max:                   )"),
                    NodeElementArchetype.Factory.Float(60, 4 * Surface.Constants.LayoutOffsetY, 0, 0),
                    NodeElementArchetype.Factory.Float(145, 4 * Surface.Constants.LayoutOffsetY, 0, 1),
                }
            },
            new NodeArchetype
            {
                TypeID = 13,
                Create = (id, context, arch, groupArch) => new MultiBlend2D(id, context, arch, groupArch),
                Title = "Multi Blend 2D",
                Description = "Animation blending in 2D",
                Flags = NodeFlags.AnimGraphOnly,
                Size = new Vector2(420, 320),
                DefaultValues = new object[]
                {
                    // Node data
                    new Vector4(0, 100.0f, 0, 100.0f),
                    1.0f,
                    true,
                    0.0f,

                    // Per blend sample data
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                    new Vector4(0, 0, 0, 1.0f), Guid.Empty,
                },
                Elements = new[]
                {
                    // Output
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Impulse, 0),

                    // Options
                    NodeElementArchetype.Factory.Input(0, "Speed", true, ConnectionType.Float, 1, 1),
                    NodeElementArchetype.Factory.Input(1, "Loop", true, ConnectionType.Bool, 2, 2),
                    NodeElementArchetype.Factory.Input(2, "Start Position", true, ConnectionType.Float, 3, 3),

                    // Axis X
                    NodeElementArchetype.Factory.Input(4, "X", true, ConnectionType.Float, 4),
                    NodeElementArchetype.Factory.Text(30, 4 * Surface.Constants.LayoutOffsetY, "(min:                   max:                   )"),
                    NodeElementArchetype.Factory.Float(60, 4 * Surface.Constants.LayoutOffsetY, 0, 0),
                    NodeElementArchetype.Factory.Float(145, 4 * Surface.Constants.LayoutOffsetY, 0, 1),

                    // Axis Y
                    NodeElementArchetype.Factory.Input(5, "Y", true, ConnectionType.Float, 5),
                    NodeElementArchetype.Factory.Text(30, 5 * Surface.Constants.LayoutOffsetY, "(min:                   max:                   )"),
                    NodeElementArchetype.Factory.Float(60, 5 * Surface.Constants.LayoutOffsetY, 0, 2),
                    NodeElementArchetype.Factory.Float(145, 5 * Surface.Constants.LayoutOffsetY, 0, 3),
                }
            },
            new NodeArchetype
            {
                TypeID = 14,
                Create = (id, context, arch, groupArch) => new BlendPose(id, context, arch, groupArch),
                Title = "Blend Poses",
                Description = "Select animation pose to pass by index (with blending)",
                Flags = NodeFlags.AnimGraphOnly,
                Size = new Vector2(200, 200),
                DefaultValues = new object[]
                {
                    0,
                    0.2f,
                    8,
                    (int)AlphaBlendMode.HermiteCubic,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Impulse, 0),
                    NodeElementArchetype.Factory.Input(0, "Pose Index", true, ConnectionType.Integer, 1, 0),
                    NodeElementArchetype.Factory.Input(1, "Blend Duration", true, ConnectionType.Float, 2, 1),
                    NodeElementArchetype.Factory.Text(0, Surface.Constants.LayoutOffsetY * 2, "Mode:"),
                    NodeElementArchetype.Factory.ComboBox(40, Surface.Constants.LayoutOffsetY * 2, 100, 3, typeof(AlphaBlendMode)),

                    NodeElementArchetype.Factory.Input(3, "Pose 0", true, ConnectionType.Impulse, 3),
                    NodeElementArchetype.Factory.Input(4, "Pose 1", true, ConnectionType.Impulse, 4),
                    NodeElementArchetype.Factory.Input(5, "Pose 2", true, ConnectionType.Impulse, 5),
                    NodeElementArchetype.Factory.Input(6, "Pose 3", true, ConnectionType.Impulse, 6),
                    NodeElementArchetype.Factory.Input(7, "Pose 4", true, ConnectionType.Impulse, 7),
                    NodeElementArchetype.Factory.Input(8, "Pose 5", true, ConnectionType.Impulse, 8),
                    NodeElementArchetype.Factory.Input(9, "Pose 6", true, ConnectionType.Impulse, 9),
                    NodeElementArchetype.Factory.Input(10, "Pose 7", true, ConnectionType.Impulse, 10),
                }
            },
            new NodeArchetype
            {
                TypeID = 15,
                Title = "Get Root Motion",
                Description = "Gets the computed root motion from the pose",
                Flags = NodeFlags.AnimGraphOnly,
                Size = new Vector2(180, 60),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Translation", ConnectionType.Vector3, 0),
                    NodeElementArchetype.Factory.Output(1, "Rotation", ConnectionType.Rotation, 1),
                    NodeElementArchetype.Factory.Input(0, "Pose", true, ConnectionType.Impulse, 2),
                }
            },
            new NodeArchetype
            {
                TypeID = 16,
                Title = "Set Root Motion",
                Description = "Overrides the root motion of the pose",
                Flags = NodeFlags.AnimGraphOnly,
                Size = new Vector2(180, 60),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Impulse, 0),
                    NodeElementArchetype.Factory.Input(0, "Pose", true, ConnectionType.Impulse, 1),
                    NodeElementArchetype.Factory.Input(1, "Translation", true, ConnectionType.Vector3, 2),
                    NodeElementArchetype.Factory.Input(2, "Rotation", true, ConnectionType.Rotation, 3),
                }
            },
            new NodeArchetype
            {
                TypeID = 17,
                Title = "Add Root Motion",
                Description = "Applies the custom root motion transformation the root motion of the pose",
                Flags = NodeFlags.AnimGraphOnly,
                Size = new Vector2(180, 60),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Impulse, 0),
                    NodeElementArchetype.Factory.Input(0, "Pose", true, ConnectionType.Impulse, 1),
                    NodeElementArchetype.Factory.Input(1, "Translation", true, ConnectionType.Vector3, 2),
                    NodeElementArchetype.Factory.Input(2, "Rotation", true, ConnectionType.Rotation, 3),
                }
            },
            new NodeArchetype
            {
                TypeID = 18,
                Create = (id, context, arch, groupArch) => new StateMachine(id, context, arch, groupArch),
                Title = "State Machine",
                Description = "The animation states machine output node",
                Flags = NodeFlags.AnimGraphOnly,
                Size = new Vector2(270, 100),
                DefaultValues = new object[]
                {
                    "Locomotion",
                    Enumerable.Empty<byte>() as byte[],
                    3,
                    true,
                    true,
                },
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "", ConnectionType.Impulse, 0)
                }
            },
            new NodeArchetype
            {
                TypeID = 19,
                Create = (id, context, arch, groupArch) => new StateMachineEntry(id, context, arch, groupArch),
                Title = "Entry",
                Description = "The animation states machine entry node",
                Flags = NodeFlags.AnimGraphOnly | NodeFlags.NoRemove | NodeFlags.NoSpawnViaGUI | NodeFlags.NoCloseButton,
                Size = new Vector2(100, 0),
                DefaultValues = new object[]
                {
                    -1,
                },
            },
            new NodeArchetype
            {
                TypeID = 20,
                Create = (id, context, arch, groupArch) => new StateMachineState(id, context, arch, groupArch),
                Title = "State",
                Description = "The animation states machine state node",
                Flags = NodeFlags.AnimGraphOnly | NodeFlags.NoSpawnViaGUI,
                Size = new Vector2(100, 0),
                DefaultValues = new object[]
                {
                    "State",
                    Enumerable.Empty<byte>() as byte[],
                    Enumerable.Empty<byte>() as byte[],
                },
            },
            new NodeArchetype
            {
                TypeID = 21,
                Title = "State Output",
                Description = "The animation states machine state output node",
                Flags = NodeFlags.AnimGraphOnly | NodeFlags.NoRemove | NodeFlags.NoSpawnViaGUI | NodeFlags.NoCloseButton,
                Size = new Vector2(120, 30),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Pose", true, ConnectionType.Impulse, 0)
                }
            },
            new NodeArchetype
            {
                TypeID = 22,
                Title = "Rule Output",
                Description = "The animation states machine transition rule output node",
                Flags = NodeFlags.AnimGraphOnly | NodeFlags.NoRemove | NodeFlags.NoSpawnViaGUI | NodeFlags.NoCloseButton,
                Size = new Vector2(150, 30),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Input(0, "Can Start Transition", true, ConnectionType.Bool, 0)
                }
            },
            new NodeArchetype
            {
                TypeID = 23,
                Title = "Transition Source State Anim",
                Description = "The animation state machine transition source state animation data information",
                Flags = NodeFlags.AnimGraphOnly | NodeFlags.NoSpawnViaGUI,
                Size = new Vector2(270, 110),
                Elements = new[]
                {
                    NodeElementArchetype.Factory.Output(0, "Length", ConnectionType.Float, 0),
                    NodeElementArchetype.Factory.Output(1, "Time", ConnectionType.Float, 1),
                    NodeElementArchetype.Factory.Output(2, "Normalized Time", ConnectionType.Float, 2),
                    NodeElementArchetype.Factory.Output(3, "Reaming Time", ConnectionType.Float, 3),
                    NodeElementArchetype.Factory.Output(4, "Reaming Normalized Time", ConnectionType.Float, 4),
                }
            },
        };
    }
}
