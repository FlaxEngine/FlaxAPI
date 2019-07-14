// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.GUI;
using FlaxEditor.GUI;
using FlaxEditor.GUI.ContextMenu;
using FlaxEditor.GUI.Drag;
using FlaxEditor.Surface;
using FlaxEditor.Viewport.Previews;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;

// ReSharper disable MemberCanBePrivate.Local

namespace FlaxEditor.Windows.Assets
{
    /// <summary>
    /// Particle Emitter window allows to view and edit <see cref="ParticleEmitter"/> asset.
    /// </summary>
    /// <seealso cref="ParticleEmitter" />
    /// <seealso cref="ParticleEmitterSurface" />
    /// <seealso cref="ParticleEmitterPreview" />
    public sealed class ParticleEmitterWindow : VisjectSurfaceWindow<ParticleEmitter, ParticleEmitterSurface, ParticleEmitterPreview>
    {
        /// <summary>
        /// The properties proxy object.
        /// </summary>
        private sealed class PropertiesProxy
        {
            [EditorOrder(1000), EditorDisplay("Parameters"), CustomEditor(typeof(ParametersEditor)), NoSerialize]
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public ParticleEmitterWindow ParticleEmitterWinRef { get; set; }

            /// <summary>
            /// Custom editor for editing Particle Emitter parameters collection.
            /// </summary>
            /// <seealso cref="FlaxEditor.CustomEditors.CustomEditor" />
            public class ParametersEditor : CustomEditor
            {
                private static readonly object[] DefaultAttributes = { new LimitAttribute(float.MinValue, float.MaxValue, 0.1f) };

                private enum NewParameterType
                {
                    Bool = (int)ParameterType.Bool,
                    Integer = (int)ParameterType.Integer,
                    Float = (int)ParameterType.Float,
                    Vector2 = (int)ParameterType.Vector2,
                    Vector3 = (int)ParameterType.Vector3,
                    Vector4 = (int)ParameterType.Vector4,
                    Color = (int)ParameterType.Color,
                    Texture = (int)ParameterType.Texture,
                    RenderTarget = (int)ParameterType.RenderTarget,
                    RenderTargetArray = (int)ParameterType.RenderTargetArray,
                    RenderTargetVolume = (int)ParameterType.RenderTargetVolume,
                    Matrix = (int)ParameterType.Matrix,
                }

                /// <inheritdoc />
                public override DisplayStyle Style => DisplayStyle.InlineIntoParent;

                /// <inheritdoc />
                public override void Initialize(LayoutElementsContainer layout)
                {
                    var particleEmitterWin = Values[0] as ParticleEmitterWindow;
                    var particleEmitter = particleEmitterWin?.Asset;
                    if (particleEmitter == null || !particleEmitter.IsLoaded)
                    {
                        layout.Label("Loading...");
                        return;
                    }
                    var parameters = particleEmitterWin.Surface.Parameters;

                    for (int i = 0; i < parameters.Count; i++)
                    {
                        var p = parameters[i];
                        if (!p.IsPublic)
                            continue;

                        var pIndex = i;
                        var pValue = p.Value;
                        Type pType;
                        object[] attributes = null;
                        switch (p.Type)
                        {
                        case ParameterType.CubeTexture:
                            pType = typeof(CubeTexture);
                            break;
                        case ParameterType.Texture:
                        case ParameterType.NormalMap:
                            pType = typeof(Texture);
                            break;
                        case ParameterType.RenderTarget:
                        case ParameterType.RenderTargetArray:
                        case ParameterType.RenderTargetCube:
                        case ParameterType.RenderTargetVolume:
                            pType = typeof(RenderTarget);
                            break;
                        default:
                            pType = p.Value.GetType();
                            // TODO: support custom attributes with defined value range for parameter (min, max)
                            attributes = DefaultAttributes;
                            break;
                        }

                        var propertyValue = new CustomValueContainer(
                            pType,
                            pValue,
                            (instance, index) =>
                            {
                                var win = (ParticleEmitterWindow)instance;
                                return win.Surface.Parameters[pIndex].Value;
                            },
                            (instance, index, value) =>
                            {
                                // Set surface parameter
                                var win = (ParticleEmitterWindow)instance;
                                var action = new EditParamAction
                                {
                                    Window = win,
                                    Index = pIndex,
                                    Before = win.Surface.Parameters[pIndex].Value,
                                    After = value,
                                };
                                win.Surface.Undo.AddAction(action);
                                action.Do();
                            },
                            attributes
                        );

                        var propertyLabel = new DragablePropertyNameLabel(p.Name);
                        propertyLabel.Tag = pIndex;
                        propertyLabel.MouseLeftDoubleClick += (label, location) => StartParameterRenaming(pIndex, label);
                        propertyLabel.MouseRightClick += (label, location) => ShowParameterMenu(pIndex, label, ref location);
                        propertyLabel.Drag = DragParameter;
                        var property = layout.AddPropertyItem(propertyLabel);
                        property.Object(propertyValue);
                    }

                    if (parameters.Count > 0)
                        layout.Space(10);
                    else
                        layout.Label("No parameters");

                    // Parameters creating
                    var paramType = layout.Enum(typeof(NewParameterType));
                    paramType.Value = (int)NewParameterType.Float;
                    var newParam = layout.Button("Add parameter");
                    newParam.Button.Clicked += () => AddParameter((ParameterType)paramType.Value);
                }

                private DragData DragParameter(DragablePropertyNameLabel label)
                {
                    var win = (ParticleEmitterWindow)Values[0];
                    var parameter = win.Surface.Parameters[(int)label.Tag];
                    return DragNames.GetDragData(SurfaceParameter.DragPrefix, parameter.Name);
                }

                /// <summary>
                /// Shows the parameter context menu.
                /// </summary>
                /// <param name="index">The index.</param>
                /// <param name="label">The label control.</param>
                /// <param name="targetLocation">The target location.</param>
                private void ShowParameterMenu(int index, Control label, ref Vector2 targetLocation)
                {
                    var contextMenu = new ContextMenu();
                    contextMenu.AddButton("Rename", () => StartParameterRenaming(index, label));
                    contextMenu.AddButton("Delete", () => DeleteParameter(index));
                    contextMenu.Show(label, targetLocation);
                }

                /// <summary>
                /// Adds the parameter.
                /// </summary>
                /// <param name="type">The type.</param>
                private void AddParameter(ParameterType type)
                {
                    var window = Values[0] as ParticleEmitterWindow;
                    var particleEmitter = window?.Asset;
                    if (particleEmitter == null || !particleEmitter.IsLoaded)
                        return;

                    var action = new AddRemoveParamAction
                    {
                        Window = window,
                        IsAdd = true,
                        Name = "New parameter",
                        Type = type,
                        Index = window.Surface.Parameters.Count,
                    };
                    window.Surface.Undo.AddAction(action);
                    action.Do();
                }

                /// <summary>
                /// Starts renaming parameter.
                /// </summary>
                /// <param name="index">The index.</param>
                /// <param name="label">The label control.</param>
                private void StartParameterRenaming(int index, Control label)
                {
                    var win = (ParticleEmitterWindow)Values[0];
                    var parameter = win.Surface.Parameters[index];
                    var dialog = RenamePopup.Show(label, new Rectangle(0, 0, label.Width - 2, label.Height), parameter.Name, false);
                    dialog.Tag = index;
                    dialog.Renamed += OnParameterRenamed;
                }

                private void OnParameterRenamed(RenamePopup renamePopup)
                {
                    var index = (int)renamePopup.Tag;
                    var win = (ParticleEmitterWindow)Values[0];

                    var action = new RenameParamAction
                    {
                        Window = win,
                        Index = index,
                        Before = win.Surface.Parameters[index].Name,
                        After = renamePopup.Text,
                    };
                    win.Surface.Undo.AddAction(action);
                    action.Do();
                }

                /// <summary>
                /// Removes the parameter.
                /// </summary>
                /// <param name="index">The index.</param>
                private void DeleteParameter(int index)
                {
                    var win = (ParticleEmitterWindow)Values[0];

                    var action = new AddRemoveParamAction
                    {
                        Window = win,
                        IsAdd = false,
                        Index = index,
                    };
                    win.Surface.Undo.AddAction(action);
                    action.Do();
                }
            }

            /// <summary>
            /// Gathers parameters from the specified ParticleEmitter.
            /// </summary>
            /// <param name="particleEmitterWin">The ParticleEmitter window.</param>
            public void OnLoad(ParticleEmitterWindow particleEmitterWin)
            {
                // Link
                ParticleEmitterWinRef = particleEmitterWin;
            }

            /// <summary>
            /// Clears temporary data.
            /// </summary>
            public void OnClean()
            {
                // Unlink
                ParticleEmitterWinRef = null;
            }
        }

        private readonly PropertiesProxy _properties;

        /// <inheritdoc />
        public ParticleEmitterWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            // Asset preview
            _preview = new ParticleEmitterPreview(true)
            {
                PlaySimulation = true,
                Parent = _split2.Panel1
            };

            // Asset properties proxy
            _properties = new PropertiesProxy();
            _propertiesEditor.Select(_properties);

            // Surface
            _surface = new ParticleEmitterSurface(this, Save, _undo)
            {
                Parent = _split1.Panel1,
                Enabled = false
            };

            // Toolstrip
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(editor.Icons.BracketsSlash32, () => ShowSourceCode(_asset)).LinkTooltip("Show generated shader source code");
            _toolstrip.AddButton(editor.Icons.Docs32, () => Application.StartProcess(Utilities.Constants.DocsUrl + "manual/particles/index.html")).LinkTooltip("See documentation to learn more");
        }

        /// <summary>
        /// Shows the ParticleEmitter source code window.
        /// </summary>
        /// <param name="particleEmitter">The ParticleEmitter asset.</param>
        public static void ShowSourceCode(ParticleEmitter particleEmitter)
        {
            var source = Editor.GetParticleEmitterShaderSourceCode(particleEmitter);
            Utilities.Utils.ShowSourceCode(source, "Particle Emitter GPU Simulation Source");
        }

        /// <inheritdoc />
        protected override void OnParamRenameUndo(RenameParamAction action)
        {
            base.OnParamRenameUndo(action);

            _refreshPropertiesOnLoad = true;
        }

        /// <inheritdoc />
        protected override void OnParamAddUndo(AddRemoveParamAction action)
        {
            base.OnParamAddUndo(action);

            _refreshPropertiesOnLoad = true;
        }

        /// <inheritdoc />
        protected override void OnParamRemoveUndo(AddRemoveParamAction action)
        {
            base.OnParamRemoveUndo(action);

            _refreshPropertiesOnLoad = true;
        }

        /// <inheritdoc />
        protected override void OnParamEditUndo(EditParamAction action, object value)
        {
            base.OnParamEditUndo(action, value);

            _refreshPropertiesOnLoad = true;
        }

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            _properties.OnClean();
            _preview.Emitter = null;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            _preview.Emitter = _asset;

            base.OnAssetLinked();
        }

        /// <inheritdoc />
        public override string SurfaceName => "Particle Emitter";

        /// <inheritdoc />
        public override byte[] SurfaceData
        {
            get => _asset.LoadSurface(true);
            set
            {
                // Save data to the temporary asset
                if (_asset.SaveSurface(value))
                {
                    // Error
                    _surface.MarkAsEdited();
                    Editor.LogError("Failed to save Particle Emitter surface data");
                }
                _preview.PreviewActor.ResetSimulation();
            }
        }

        /// <inheritdoc />
        protected override bool LoadSurface()
        {
            // Init asset properties and parameters proxy
            _properties.OnLoad(this);

            // Load surface data from the asset
            byte[] data = _asset.LoadSurface(true);
            if (data == null)
            {
                // Error
                Editor.LogError("Failed to load Particle Emitter surface data.");
                return true;
            }

            // Load surface graph
            if (_surface.Load(data))
            {
                // Error
                Editor.LogError("Failed to load Particle Emitter surface.");
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        protected override bool SaveSurface()
        {
            _surface.Save();
            return false;
        }

        /// <inheritdoc />
        protected override bool SaveToOriginal()
        {
            // Copy shader cache from the temporary Particle Emitter (will skip compilation on Reload - faster)
            Guid dstId = _item.ID;
            Guid srcId = _asset.ID;
            Editor.Internal_CopyCache(ref dstId, ref srcId);

            return base.SaveToOriginal();
        }
    }
}
