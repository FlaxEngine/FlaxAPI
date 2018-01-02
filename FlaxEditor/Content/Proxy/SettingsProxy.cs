////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEditor.Content.Create;
using FlaxEditor.Content.Settings;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Content proxy for json settings assets (e.g <see cref="GameSettings"/> or <see cref="TimeSettings"/>).
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.JsonAssetProxy" />
    public sealed class SettingsProxy<T> : JsonAssetProxy where T : SettingsBase
    {
        /// <inheritdoc />
        public override string Name => "Settings";
        //public override string Name { get; } = CustomEditors.CustomEditorsUtil.GetPropertyNameUI(typeof(T).Name);
        
        /// <inheritdoc />
        public override bool CanCreate(ContentFolder targetLocation)
        {
            // Use proxy only for GameSettings for creating
            if (typeof(T) != typeof(GameSettings))
                return false;

            return targetLocation.CanHaveAssets;
        }

        /// <inheritdoc />
        public override void Create(string outputPath)
        {
            Editor.Instance.ContentImporting.Create(new SettingsCreateEntry(outputPath));
        }

        /// <inheritdoc />
        public override string TypeName { get; } = typeof(T).FullName;
    }
}
