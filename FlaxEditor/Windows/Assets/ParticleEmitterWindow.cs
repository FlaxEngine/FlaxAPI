// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Xml;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.GUI;
using FlaxEditor.GUI;
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
    /// Note: it uses ClonedAssetEditorWindowBase which is creating cloned asset to edit/preview.
    /// </summary>
    /// <seealso cref="ParticleEmitter" />
    /// <seealso cref="FlaxEditor.Windows.Assets.AssetEditorWindow" />
    /// <seealso cref="FlaxEditor.Surface.IVisjectSurfaceOwner" />
    public sealed class ParticleEmitterWindow : ClonedAssetEditorWindowBase<ParticleEmitter>, IVisjectSurfaceOwner
    {
        /// <summary>
        /// The properties proxy object.
        /// </summary>
        private sealed class PropertiesProxy
        {
            [EditorOrder(1000), EditorDisplay("Parameters"), CustomEditor(typeof(ParametersEditor))]
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
                        var pGuidType = false;
                        Type pType;
                        object[] attributes = null;
                        switch (p.Type)
                        {
                        case ParameterType.CubeTexture:
                            pType = typeof(CubeTexture);
                            pGuidType = true;
                            break;
                        case ParameterType.Texture:
                        case ParameterType.NormalMap:
                            pType = typeof(Texture);
                            pGuidType = true;
                            break;
                        case ParameterType.RenderTarget:
                        case ParameterType.RenderTargetArray:
                        case ParameterType.RenderTargetCube:
                        case ParameterType.RenderTargetVolume:
                            pType = typeof(RenderTarget);
                            pGuidType = true;
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
                                var win = (ParticleEmitterWindow)instance;

                                // Visject surface parameters are only value type objects so convert value if need to (eg. instead of texture ref write texture id)
                                var surfaceParam = value;
                                if (pGuidType)
                                    surfaceParam = (value as FlaxEngine.Object)?.ID ?? Guid.Empty;

                                win.Surface.Parameters[pIndex].Value = surfaceParam;
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
                    var win = Values[0] as ParticleEmitterWindow;
                    var particleEmitter = win?.Asset;
                    if (particleEmitter == null || !particleEmitter.IsLoaded)
                        return;

                    var param = SurfaceParameter.Create(type);
                    if (type == ParameterType.NormalMap)
                    {
                        // Use default normal map texture (don't load asset here, just lookup registry for id at path)
                        string typeName;
                        Guid id;
                        FlaxEngine.Content.GetAssetInfo(StringUtils.CombinePaths(Globals.EngineFolder, "Textures/NormalTexture.flax"), out typeName, out id);
                        param.Value = id;
                    }
                    win.Surface.Parameters.Add(param);
                    win.Surface.OnParamCreated(param);
                    RebuildLayout();
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
                    var newName = renamePopup.Text;

                    var win = (ParticleEmitterWindow)Values[0];
                    var param = win.Surface.Parameters[index];
                    param.Name = newName;
                    win.Surface.OnParamRenamed(param);
                    RebuildLayout();
                }

                /// <summary>
                /// Removes the parameter.
                /// </summary>
                /// <param name="index">The index.</param>
                private void DeleteParameter(int index)
                {
                    var win = (ParticleEmitterWindow)Values[0];
                    var param = win.Surface.Parameters[index];
                    win.Surface.Parameters.RemoveAt(index);
                    win.Surface.OnParamDeleted(param);
                    RebuildLayout();
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

        private readonly SplitPanel _split1;
        private readonly SplitPanel _split2;
        private readonly ParticleEmitterPreview _preview;
        private readonly ParticleEmitterSurface _surface;
        private readonly CustomEditorPresenter _propertiesEditor;

        private readonly ToolStripButton _saveButton;
        private readonly PropertiesProxy _properties;
        private bool _isWaitingForSurfaceLoad;
        private bool _tmpParticleEmitterIsDirty;

        /// <summary>
        /// Gets the Particle Emitter surface.
        /// </summary>
        public ParticleEmitterSurface Surface => _surface;

        /// <inheritdoc />
        public ParticleEmitterWindow(Editor editor, AssetItem item)
        : base(editor, item)
        {
            // Split Panel 1
            _split1 = new SplitPanel(Orientation.Horizontal, ScrollBars.None, ScrollBars.None)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.7f,
                Parent = this
            };

            // Split Panel 2
            _split2 = new SplitPanel(Orientation.Vertical, ScrollBars.None, ScrollBars.Vertical)
            {
                DockStyle = DockStyle.Fill,
                SplitterValue = 0.4f,
                Parent = _split1.Panel2
            };

            // ParticleEmitter preview
            _preview = new ParticleEmitterPreview(true)
            {
                PlaySimulation = true,
                Parent = _split2.Panel1
            };

            // ParticleEmitter properties editor
            var propertiesEditor = new CustomEditorPresenter(null);
            propertiesEditor.Panel.Parent = _split2.Panel2;
            _properties = new PropertiesProxy();
            propertiesEditor.Select(_properties);
            propertiesEditor.Modified += OnParticleEmitterPropertyEdited;
            _propertiesEditor = propertiesEditor;

            // Surface
            _surface = new ParticleEmitterSurface(this, Save)
            {
                Parent = _split1.Panel1,
                Enabled = false
            };

            // Toolstrip
            _saveButton = (ToolStripButton)_toolstrip.AddButton(Editor.Icons.Save32, Save).LinkTooltip("Save");
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(editor.Icons.PageScale32, _surface.ShowWholeGraph).LinkTooltip("Show whole graph");
            _toolstrip.AddSeparator();
            _toolstrip.AddButton(editor.Icons.BracketsSlash32, () => ShowSourceCode(_asset)).LinkTooltip("Show generated shader source code");
            _toolstrip.AddButton(editor.Icons.Docs32, () => Application.StartProcess(Utilities.Constants.DocsUrl + "manual/particles/index.html")).LinkTooltip("See documentation to learn more");
        }

        private void OnParticleEmitterPropertyEdited()
        {
            _surface.MarkAsEdited();
        }

        /// <summary>
        /// Shows the ParticleEmitter source code window.
        /// </summary>
        /// <param name="particleEmitter">The ParticleEmitter asset.</param>
        public static void ShowSourceCode(ParticleEmitter particleEmitter)
        {
            var source = Editor.GetParticleEmitterShaderSourceCode(particleEmitter);
            if (string.IsNullOrEmpty(source))
            {
                MessageBox.Show("No generated shader source code.", "No source.");
                return;
            }

            CreateWindowSettings settings = CreateWindowSettings.Default;
            settings.ActivateWhenFirstShown = true;
            settings.AllowMaximize = false;
            settings.AllowMinimize = false;
            settings.HasSizingFrame = false;
            settings.StartPosition = WindowStartPosition.CenterScreen;
            settings.Size = new Vector2(500, 600);
            settings.Title = "Particle Emitter GPU Simulation Source";
            var dialog = Window.Create(settings);

            var copyButton = new Button(4, 4, 100);
            copyButton.Text = "Copy";
            copyButton.Clicked += () => Application.ClipboardText = source;
            copyButton.Parent = dialog.GUI;

            var sourceTextBox = new TextBox(true, 2, copyButton.Bottom + 4, settings.Size.X - 4);
            sourceTextBox.Height = settings.Size.Y - sourceTextBox.Top - 2;
            sourceTextBox.Text = source;
            sourceTextBox.Parent = dialog.GUI;

            dialog.Show();
            dialog.Focus();
        }

        /// <summary>
        /// Refreshes temporary asset to see changes live when editing the surface.
        /// </summary>
        /// <returns>True if cannot refresh it, otherwise false.</returns>
        public bool RefreshTempAsset()
        {
            // Early check
            if (_asset == null || _isWaitingForSurfaceLoad)
                return true;

            // Check if surface has been edited
            if (_surface.IsEdited)
            {
                _surface.Save();
            }

            return false;
        }

        /// <inheritdoc />
        public override void Save()
        {
            // Check if don't need to push any new changes to the original asset
            if (!IsEdited)
                return;

            // Just in case refresh data
            if (RefreshTempAsset())
            {
                // Error
                return;
            }

            // Copy shader cache from the temporary Particle Emitter (will skip compilation on Reload - faster)
            Guid dstId = _item.ID;
            Guid srcId = _asset.ID;
            Editor.Internal_CopyCache(ref dstId, ref srcId);

            // Update original Particle Emitter so user can see changes in the scene
            if (SaveToOriginal())
            {
                // Error
                return;
            }

            // Clear flag
            ClearEditedFlag();

            // Update
            OnSurfaceEditedChanged();
            _item.RefreshThumbnail();
        }

        /// <inheritdoc />
        protected override void UpdateToolstrip()
        {
            _saveButton.Enabled = IsEdited;

            base.UpdateToolstrip();
        }

        /// <inheritdoc />
        protected override void UnlinkItem()
        {
            _properties.OnClean();
            _preview.Emitter = null;
            _isWaitingForSurfaceLoad = false;

            base.UnlinkItem();
        }

        /// <inheritdoc />
        protected override void OnAssetLinked()
        {
            _preview.Emitter = _asset;
            _isWaitingForSurfaceLoad = true;

            base.OnAssetLinked();
        }

        /// <inheritdoc />
        public string SurfaceName => "Particle Emitter";

        /// <inheritdoc />
        public byte[] SurfaceData
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
        public void OnContextCreated(VisjectSurfaceContext context)
        {
        }

        /// <inheritdoc />
        public void OnSurfaceEditedChanged()
        {
            if (_surface.IsEdited)
                MarkAsEdited();
        }

        /// <inheritdoc />
        public void OnSurfaceGraphEdited()
        {
            // Mark as dirty
            _tmpParticleEmitterIsDirty = true;
        }

        /// <inheritdoc />
        public void OnSurfaceClose()
        {
            Close();
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // Check if temporary asset need to be updated
            if (_tmpParticleEmitterIsDirty)
            {
                // Clear flag
                _tmpParticleEmitterIsDirty = false;

                // Update
                RefreshTempAsset();
            }

            // Check if need to load surface
            if (_isWaitingForSurfaceLoad && _asset.IsLoaded)
            {
                // Clear flag
                _isWaitingForSurfaceLoad = false;

                // Init asset properties and parameters proxy
                _properties.OnLoad(this);

                // Load surface data from the asset
                byte[] data = _asset.LoadSurface(true);
                if (data == null)
                {
                    // Error
                    Editor.LogError("Failed to load Particle Emitter surface data.");
                    Close();
                    return;
                }

                // Load surface graph
                if (_surface.Load(data))
                {
                    // Error
                    Editor.LogError("Failed to load Particle Emitter surface.");
                    Close();
                    return;
                }

                // Setup
                _surface.Enabled = true;
                _propertiesEditor.BuildLayout();
                ClearEditedFlag();
            }
        }

        /// <inheritdoc />
        public override bool UseLayoutData => true;

        /// <inheritdoc />
        public override void OnLayoutSerialize(XmlWriter writer)
        {
            writer.WriteAttributeString("Split1", _split1.SplitterValue.ToString());
            writer.WriteAttributeString("Split2", _split2.SplitterValue.ToString());
        }

        /// <inheritdoc />
        public override void OnLayoutDeserialize(XmlElement node)
        {
            float value1;

            if (float.TryParse(node.GetAttribute("Split1"), out value1))
                _split1.SplitterValue = value1;
            if (float.TryParse(node.GetAttribute("Split2"), out value1))
                _split2.SplitterValue = value1;
        }

        /// <inheritdoc />
        public override void OnLayoutDeserialize()
        {
            _split1.SplitterValue = 0.7f;
            _split2.SplitterValue = 0.4f;
        }
    }
}
