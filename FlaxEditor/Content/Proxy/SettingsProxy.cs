// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.Content.Create;
using FlaxEditor.Content.Settings;

namespace FlaxEditor.Content
{
    /// <summary>
    /// Content proxy for json settings assets (e.g <see cref="GameSettings"/> or <see cref="TimeSettings"/>).
    /// </summary>
    /// <seealso cref="FlaxEditor.Content.JsonAssetProxy" />
    public sealed class SettingsProxy<TSettings> : JsonAssetProxy where TSettings : SettingsBase
    {
        /// <inheritdoc />
        public override string Name => "Settings";
        //public override string Name { get; } = CustomEditors.CustomEditorsUtil.GetPropertyNameUI(typeof(T).Name);

        /// <inheritdoc />
        public override bool CanCreate(ContentFolder targetLocation)
        {
            // Use proxy only for GameSettings for creating
            if (typeof(TSettings) != typeof(GameSettings))
                return false;

            return targetLocation.CanHaveAssets;
        }

        /// <inheritdoc />
        public override void Create(string outputPath, object arg)
        {
            Editor.Instance.ContentImporting.Create(new SettingsCreateEntry(outputPath));
        }

        /// <inheritdoc />
        public override bool IsProxyFor<T>()
        {
            return typeof(T) == typeof(TSettings);
        }

        /// <inheritdoc />
        public override string TypeName { get; } = typeof(TSettings).FullName;
    }
}
