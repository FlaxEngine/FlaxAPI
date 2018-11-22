// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;
using Object = FlaxEngine.Object;

namespace FlaxEditor.Actions
{
    /// <summary>
    /// Implementation of <see cref="IUndoAction"/> used to add/remove <see cref="Script"/> from the <see cref="Actor"/>.
    /// </summary>
    /// <seealso cref="IUndoAction" />
    public sealed class AddRemoveScript : IUndoAction
    {
        private bool _isAdd;
        private Script _script;
        private Guid _scriptId;
        private Guid _prefabId;
        private Guid _prefabObjectId;
        private Type _scriptType;
        private string _scriptData;
        private Guid _parentId;
        private int _orderInParent;
        private bool _enabled;

        internal AddRemoveScript(bool isAdd, Script script)
        {
            _isAdd = isAdd;
            _script = script;
            _scriptId = script.ID;
            _scriptType = script.GetType();
            _prefabId = script.PrefabID;
            _prefabObjectId = script.PrefabObjectID;
            _scriptData = FlaxEngine.Json.JsonSerializer.Serialize(script);
            _parentId = script.Actor.ID;
            _orderInParent = script.OrderInParent;
            _enabled = script.Enabled;
        }

        internal AddRemoveScript(bool isAdd, Actor parentActor, Type scriptType)
        {
            _isAdd = isAdd;
            _script = null;
            _scriptId = Guid.NewGuid();
            _scriptType = scriptType;
            _scriptData = null;
            _parentId = parentActor.ID;
            _orderInParent = -1;
            _enabled = true;
        }

        /// <summary>
        /// Creates a new added script undo action.
        /// </summary>
        /// <param name="script">The new script.</param>
        /// <returns>The action.</returns>
        public static AddRemoveScript Added(Script script)
        {
            if (script == null)
                throw new ArgumentNullException(nameof(script));
            return new AddRemoveScript(true, script);
        }

        /// <summary>
        /// Creates a new add script undo action.
        /// </summary>
        /// <param name="parentActor">The parent actor.</param>
        /// <param name="scriptType">The script type.</param>
        /// <returns>The action.</returns>
        public static AddRemoveScript Add(Actor parentActor, Type scriptType)
        {
            if (parentActor == null)
                throw new ArgumentNullException(nameof(parentActor));
            if (scriptType == null)
                throw new ArgumentNullException(nameof(scriptType));
            return new AddRemoveScript(true, parentActor, scriptType);
        }

        /// <summary>
        /// Creates a new remove script undo action.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns>The action.</returns>
        public static AddRemoveScript Remove(Script script)
        {
            if (script == null)
                throw new ArgumentNullException(nameof(script));
            return new AddRemoveScript(false, script);
        }

        /// <inheritdoc />
        public string ActionString => _isAdd ? "Add script" : "Remove script";

        /// <inheritdoc />
        public void Do()
        {
            if (_isAdd)
                DoAdd();
            else
                DoRemove();
        }

        /// <inheritdoc />
        public void Undo()
        {
            if (_isAdd)
                DoRemove();
            else
                DoAdd();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _script = null;
            _scriptType = null;
        }

        private void DoRemove()
        {
            // Remove script (it could be removed by sth else, just check it)
            if (_script)
            {
                if (_script.Actor)
                    Editor.Instance.Scene.MarkSceneEdited(_script.Scene);
                Object.Destroy(ref _script);
            }
        }

        private void DoAdd()
        {
            // Restore script
            var parentActor = Object.Find<Actor>(ref _parentId);
            if (parentActor == null)
            {
                // Error
                Editor.LogWarning("Missing parent actor.");
                return;
            }
            _script = (Script)Object.New(_scriptType);
            if (_script == null)
            {
                // Error
                Editor.LogWarning("Cannot create script of type " + _scriptType);
                return;
            }
            Object.Internal_ChangeID(_script.unmanagedPtr, ref _scriptId);
            if (_scriptData != null)
                FlaxEngine.Json.JsonSerializer.Deserialize(_script, _scriptData);
            _script.Enabled = _enabled;
            parentActor.AddScript(_script);
            if (_orderInParent != -1)
                _script.OrderInParent = _orderInParent;
            if (_prefabObjectId != Guid.Empty)
                Script.Internal_LinkPrefab(_script.unmanagedPtr, ref _prefabId, ref _prefabObjectId);
            Editor.Instance.Scene.MarkSceneEdited(parentActor.Scene);
        }
    }
}
