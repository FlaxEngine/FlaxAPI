// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Globalization;
using System.IO;
using System.Text;
using FlaxEngine.GUI;
using Newtonsoft.Json;

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

        internal string Serialize()
        {
            StringBuilder sb = new StringBuilder(256);
            StringWriter sw = new StringWriter(sb, CultureInfo.InvariantCulture);
            using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.IndentChar = '\t';
                jsonWriter.Indentation = 1;

                jsonWriter.WriteStartObject();

                if (_control != null)
                {
                    jsonWriter.WritePropertyName("Control");
                    jsonWriter.WriteValue(_control);
                }

                jsonWriter.WriteEndObject();
            }

            return sw.ToString();
        }

        internal void Deserialize(string json)
        {
            Json.JsonSerializer.Deserialize(this, json);
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
            // TODO: sync control order in parent with actor order in parent
        }
    }
}
