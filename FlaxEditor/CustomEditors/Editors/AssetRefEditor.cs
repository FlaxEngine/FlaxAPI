////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Linq;
using FlaxEditor.Content;
using FlaxEditor.CustomEditors.Elements;
using FlaxEditor.GUI;
using FlaxEngine;

namespace FlaxEditor.CustomEditors.Editors
{
    /// <summary>
    /// Default implementation of the inspector used to edit reference to the <see cref="FlaxEngine.Asset"/>.
    /// </summary>
    [CustomEditor(typeof(Asset)), DefaultEditor]
    public sealed class AssetRefEditor : CustomEditor
    {
        private CustomElement<AssetPicker> element;
        private string typeName;

        /// <inheritdoc />
        public override DisplayStyle Style => DisplayStyle.Inline;

        /// <inheritdoc />
        public override void Initialize(LayoutElementsContainer layout)
        {
            if (!HasDiffrentTypes)
            {
                // TODO: find better way to get content domain from the asset type (mayb util function?)
                var domain = ContentDomain.Other;
                var type = Values.Type != typeof(object) || Values[0] == null ? Values.Type : Values[0].GetType();
                if (type == typeof(Texture) || type == typeof(SpriteAtlas))
                    domain = ContentDomain.Texture;
                else if (type == typeof(CubeTexture))
                    domain = ContentDomain.CubeTexture;
                else if (type == typeof(Material) || type == typeof(MaterialInstance) || type == typeof(MaterialBase))
                    domain = ContentDomain.Material;
                else if (type == typeof(Model))
                    domain = ContentDomain.Model;
                //else if (type == typeof(PrefabAsset))
                //    domain = ContentDomain.Prefab;
                else if (type == typeof(Shader))
                    domain = ContentDomain.Shader;
                else if (type == typeof(FontAsset))
                    domain = ContentDomain.Font;
                else if (type == typeof(IESProfile))
                    domain = ContentDomain.IESProfile;
                //else if (type == typeof(SceneAsset))
                //    domain = ContentDomain.Scene;
                else if (type == typeof(JsonAsset))
                    domain = JsonAsset.Domain;

                typeName = null;
                float height = 48;
                if (Values.Info != null)
                {
                    var attributes = Values.Info.GetCustomAttributes(true);
                    var assetReference = (AssetReferenceAttribute)attributes.FirstOrDefault(x => x is AssetReferenceAttribute);
                    if (assetReference != null)
                    {
                        typeName = assetReference.TypeName;
                        if (assetReference.UseSmallPicker)
                            height = 24;
                    }
                }

                element = layout.Custom<AssetPicker>();
                element.CustomControl.Domain = domain;
                element.CustomControl.Height = height;
                element.CustomControl.SelectedItemChanged += () => SetValue(element.CustomControl.SelectedAsset);
                element.CustomControl.ValidateValue += ValidateValue;
            }
        }

        private bool ValidateValue(AssetItem item)
        {
            // Filter invalid typename
            if (!string.IsNullOrEmpty(typeName) && item != null)
            {
                if (item.TypeName != typeName)
                    return false;
            }
            
            return true;
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            if (!HasDiffrentValues)
            {
                element.CustomControl.SelectedAsset = Values[0] as Asset;
            }
        }
    }
}
