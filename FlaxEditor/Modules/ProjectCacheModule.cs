// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using FlaxEngine;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// Caching local project data manager. Used to store and manage the information about expanded actor nodes in the scene tree and other local user data used by the editor. Stores data in the project cache directory.
    /// </summary>
    /// <seealso cref="FlaxEditor.Modules.EditorModule" />
    public sealed class ProjectCacheModule : EditorModule
    {
        private readonly string _cachePath;
        private bool _isDirty;
        private DateTime _lastSaveTime;

        private readonly HashSet<Guid> _expandedActors = new HashSet<Guid>();

        /// <summary>
        /// Gets or sets the automatic data save interval.
        /// </summary>
        public TimeSpan AutoSaveInterval { get; set; } = TimeSpan.FromSeconds(3);

        /// <inheritdoc />
        internal ProjectCacheModule(Editor editor)
        : base(editor)
        {
            // After editor options but before the others
            InitOrder = -900;

            _cachePath = StringUtils.CombinePaths(Globals.ProjectCacheFolder, "ProjectCache.dat");
            _isDirty = true;
        }

        /// <summary>
        /// Determines whether actor identified by the given ID is expanded in the scene tree UI.
        /// </summary>
        /// <param name="id">The actor identifier.</param>
        /// <returns><c>true</c> if actor is expanded; otherwise, <c>false</c>.</returns>
        public bool IsExpandedActor(ref Guid id)
        {
            return _expandedActors.Contains(id);
        }

        /// <summary>
        /// Sets the actor expanded cached value.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="isExpanded">if set to <c>true</c> actor will be cached as an expanded, otherwise false.</param>
        public void SetExpandedActor(ref Guid id, bool isExpanded)
        {
            if (isExpanded)
                _expandedActors.Add(id);
            else
                _expandedActors.Remove(id);
            _isDirty = true;
        }

        private void LoadGuarded()
        {
            using (var stream = new FileStream(_cachePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new BinaryReader(stream))
            {
                var version = reader.ReadInt32();

                switch (version)
                {
                case 1:
                {
                    int expandedActorsCount = reader.ReadInt32();
                    _expandedActors.Clear();
                    var bytes16 = new byte[16];
                    for (int i = 0; i < expandedActorsCount; i++)
                    {
                        reader.Read(bytes16, 0, 16);
                        _expandedActors.Add(new Guid(bytes16));
                    }

                    break;
                }
                default:
                    Editor.LogWarning("Unknown editor cache version.");
                    return;
                }
            }
        }

        private void Load()
        {
            if (!File.Exists(_cachePath))
            {
                Editor.LogWarning("Missing editor cache file.");
                return;
            }

            _lastSaveTime = DateTime.UtcNow;

            try
            {
                LoadGuarded();
            }
            catch (Exception ex)
            {
                Editor.LogWarning(ex);
                Editor.LogError("Failed to load editor cache. " + ex.Message);
            }
        }

        private void SaveGuarded()
        {
            using (var stream = new FileStream(_cachePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(1);
                writer.Write(_expandedActors.Count);
                foreach (var e in _expandedActors)
                {
                    writer.Write(e.ToByteArray());
                }
            }
        }

        private void Save()
        {
            if (!_isDirty)
                return;

            _lastSaveTime = DateTime.UtcNow;

            try
            {
                SaveGuarded();

                _isDirty = false;
            }
            catch (Exception ex)
            {
                Editor.LogWarning(ex);
                Editor.LogError("Failed to save editor cache. " + ex.Message);
            }
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            Load();
        }

        /// <inheritdoc />
        public override void OnUpdate()
        {
            var dt = DateTime.UtcNow - _lastSaveTime;
            if (dt >= AutoSaveInterval)
            {
                Save();
            }
        }

        /// <inheritdoc />
        public override void OnExit()
        {
            Save();
        }
    }
}
