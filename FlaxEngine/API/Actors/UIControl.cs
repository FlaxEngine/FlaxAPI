// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Globalization;
using System.IO;
using System.Text;
using FlaxEngine.GUI;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FlaxEngine
{
    public sealed partial class UIControl
    {
        private Control _control;

        /// <summary>
        /// Gets or sets the GUI control used by this actor.
        /// </summary>
        /// <remarks>
        /// When changing the control, the previous one is disposed. Use <see cref="UnlinkControl"/> to manage it on your own.
        /// </remarks>
        [EditorDisplay("Control", EditorDisplayAttribute.InlineStyle), CustomEditorAlias("FlaxEditor.CustomEditors.Dedicated.UIControlControlEditor"), EditorOrder(50)]
        public Control Control
        {
            get => _control;
            set
            {
                if (_control == value)
                    return;

                // Cleanup previous
                if (_control != null)
                {
                    _control.OnLocationChanged -= OnControlLocationChanged;
                    _control.Dispose();
                }

                // Set value
                _control = value;

                // Link the new one (events and parent)
                if (_control != null)
                {
                    var containerControl = _control as ContainerControl;
                    if (containerControl != null)
                        containerControl.UnlockChildrenRecursive();

                    _control.Parent = GetParent();
                    _control.IndexInParent = OrderInParent;
                    _control.Location = new Vector2(LocalPosition);
                    // TODO: sync control order in parent with actor order in parent (think about specialcases like Panel with scroll bars used as internal controls)
                    _control.OnLocationChanged += OnControlLocationChanged;

                    if (containerControl != null && IsActiveInHierarchy)
                    {
                        var children = ChildCount;
                        for (int i = 0; i < children; i++)
                        {
                            var child = GetChild(i) as UIControl;
                            if (child != null && child.IsActiveInHierarchy && child.HasControl)
                            {
                                child.Control.Parent = containerControl;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this actor has control.
        /// </summary>
        public bool HasControl => _control != null;

        /// <summary>
        /// Gets the control object cased to the given type.
        /// </summary>
        /// <typeparam name="T">The type of the control.</typeparam>
        /// <returns>The control object.</returns>
        public T Get<T>() where T : Control
        {
            return (T)_control;
        }

        /// <summary>
        /// Creates a new UIControl with the control of the given type and links it to this control as a child.
        /// </summary>
        /// <remarks>
        /// The current actor has to have a valid container control.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <returns>The created UIControl that contains a new control of the given type.</returns>
        public UIControl AddChild<T>() where T : Control
        {
            if (!(_control is ContainerControl))
                throw new InvalidOperationException("To add child to the control it has to be ContainerControl.");

            var child = New();
            AddChild(child);
            child.Control = (Control)Activator.CreateInstance(typeof(T));
            return child;
        }

        /// <summary>
        /// Unlinks the control from the actor without disposing it or modyfing.
        /// </summary>
        public void UnlinkControl()
        {
            if (_control != null)
            {
                _control.OnLocationChanged -= OnControlLocationChanged;
                _control = null;
            }
        }

        private void OnControlLocationChanged(Control control)
        {
            LocalPosition = new Vector3(control.Location, LocalPosition.Z);
        }

        private ContainerControl GetParent()
        {
            // Don't link disabled actors
            if (!IsActiveInHierarchy)
                return null;

            var parent = Parent;
            if (parent is UIControl uiControl && uiControl.Control is ContainerControl uiContainerControl)
                return uiContainerControl;
            if (parent is UICanvas uiCanvas)
                return uiCanvas.GUI;
            return null;
        }

        internal string Serialize(out string controlType)
        {
            if (_control == null)
            {
                controlType = null;
                return null;
            }

            var type = _control.GetType();

            JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(Json.JsonSerializer.Settings);
            jsonSerializer.Formatting = Formatting.Indented;

            StringBuilder sb = new StringBuilder(1024);
            StringWriter sw = new StringWriter(sb, CultureInfo.InvariantCulture);
            using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
            {
                // Prepare writer settings
                jsonWriter.IndentChar = '\t';
                jsonWriter.Indentation = 1;
                jsonWriter.Formatting = jsonSerializer.Formatting;
                jsonWriter.DateFormatHandling = jsonSerializer.DateFormatHandling;
                jsonWriter.DateTimeZoneHandling = jsonSerializer.DateTimeZoneHandling;
                jsonWriter.FloatFormatHandling = jsonSerializer.FloatFormatHandling;
                jsonWriter.StringEscapeHandling = jsonSerializer.StringEscapeHandling;
                jsonWriter.Culture = jsonSerializer.Culture;
                jsonWriter.DateFormatString = jsonSerializer.DateFormatString;

                JsonSerializerInternalWriter serializerWriter = new JsonSerializerInternalWriter(jsonSerializer);

                serializerWriter.Serialize(jsonWriter, _control, type);
            }

            controlType = type.FullName;
            return sw.ToString();
        }

        internal void Deserialize(string json, Type controlType)
        {
            Control = (Control)Activator.CreateInstance(controlType);

            if (_control != null)
            {
                Json.JsonSerializer.Deserialize(_control, json);
            }
        }

        internal void ParentChanged()
        {
            if (_control != null)
            {
                _control.Parent = GetParent();
                _control.IndexInParent = OrderInParent;
            }
        }

        internal void TransformChanged()
        {
            if (_control != null)
                _control.Location = new Vector2(LocalPosition);
        }

        internal void ActiveInTreeChanged()
        {
            if (_control != null)
            {
                // Link or unlink control (won't modify Enable/Visible state)
                _control.Parent = GetParent();
                _control.IndexInParent = OrderInParent;
            }
        }

        internal void OrderInParentChanged()
        {
            if (_control != null)
                _control.IndexInParent = OrderInParent;
        }

        internal void EndPlay()
        {
            if (_control != null)
            {
                _control.Dispose();
                _control = null;
            }
        }
    }
}
