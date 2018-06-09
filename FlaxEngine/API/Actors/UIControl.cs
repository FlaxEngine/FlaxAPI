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
        /// When changing the control, the previous one is disposed. Use <see cref="UnlinkControl"/> to manage it by myself.
        /// </remarks>
        [EditorDisplay("Control", EditorDisplayAttribute.InlineStyle), CustomEditorAlias("FlaxEditor.CustomEditors.Dedicated.UIControlControlEditor"), EditorOrder(50)]
        public Control Control
        {
            get => _control;
            set
            {
                if (_control != null)
                {
                    _control.OnLocationChanged -= OnControlLocationChanged;
                    _control.Dispose();
                }

                _control = value;

                if (_control != null)
                {
                    _control.Parent = GetParent();
                    _control.Location = new Vector2(LocalPosition);
                    // TODO: sync control order in parent with actor order in parent
                    _control.OnLocationChanged += OnControlLocationChanged;
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
            Json.JsonSerializer.Deserialize(Control, json);
        }

        internal void ParentChanged()
        {
            if (_control != null)
                _control.Parent = GetParent();
        }

        internal void TransformChanged()
        {
            if (_control != null)
                _control.Location = new Vector2(LocalPosition);
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
