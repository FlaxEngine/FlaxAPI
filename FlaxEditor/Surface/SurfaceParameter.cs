// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Represents parameter in the Surface.
    /// </summary>
    public class SurfaceParameter
    {
        /// <summary>
        /// The default prefix for drag data used for <see cref="FlaxEditor.Surface.SurfaceParameter"/>.
        /// </summary>
        public const string DragPrefix = "SURFPARAM!?";

        /// <summary>
        /// Parameter type
        /// </summary>
        public ParameterType Type;

        /// <summary>
        /// Parameter unique ID
        /// </summary>
        public Guid ID;

        /// <summary>
        /// Parameter name
        /// </summary>
        public string Name;

        /// <summary>
        /// True if is exposed outside
        /// </summary>
        public bool IsPublic;

        /// <summary>
        /// True if cannot edit value
        /// </summary>
        public bool IsStatic;

        /// <summary>
        /// True if can see via UI
        /// </summary>
        public bool IsUIVisible;

        /// <summary>
        /// True if can edit via UI
        /// </summary>
        public bool IsUIEditable;

        /// <summary>
        /// Parameter value
        /// </summary>
        public object Value;

        /// <summary>
        /// The metadata.
        /// </summary>
        [NoSerialize, HideInEditor]
        public readonly SurfaceMeta Meta = new SurfaceMeta();

        /// <summary>
        /// Creates the new parameter of the given type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The created parameter.</returns>
        public static SurfaceParameter Create(ParameterType type)
        {
            // Create new parameter with default values
            var param = new SurfaceParameter
            {
                ID = Guid.NewGuid(),
                IsPublic = true,
                IsStatic = false,
                IsUIEditable = true,
                IsUIVisible = true,
                Name = "New parameter",
                Type = type
            };

            // Initialize default value in a proper way
            switch (type)
            {
            case ParameterType.Bool:
                param.Value = false;
                break;
            case ParameterType.Integer:
                param.Value = 0;
                break;
            case ParameterType.Float:
                param.Value = 0.0f;
                break;
            case ParameterType.Vector2:
                param.Value = Vector2.Zero;
                break;
            case ParameterType.Vector3:
                param.Value = Vector3.Zero;
                break;
            case ParameterType.Vector4:
                param.Value = Vector4.Zero;
                break;
            case ParameterType.Color:
                param.Value = Color.White;
                break;
            case ParameterType.Matrix:
                param.Value = Matrix.Identity;
                break;
            case ParameterType.String:
                param.Value = string.Empty;
                break;
            case ParameterType.Box:
                param.Value = BoundingBox.Zero;
                break;
            case ParameterType.Rectangle:
                param.Value = Rectangle.Empty;
                break;
            case ParameterType.Rotation:
                param.Value = Quaternion.Identity;
                break;
            case ParameterType.Transform:
                param.Value = Transform.Identity;
                break;
            case ParameterType.SceneTexture:
                param.Value = 0;
                break;
            case ParameterType.Asset:
            case ParameterType.Actor:
            case ParameterType.CubeTexture:
            case ParameterType.Texture:
            case ParameterType.NormalMap:
            case ParameterType.GPUTexture:
            case ParameterType.GPUTextureArray:
            case ParameterType.GPUTextureCube:
            case ParameterType.GPUTextureVolume:
                param.Value = Guid.Empty;
                break;
            default: throw new IndexOutOfRangeException();
            }

            return param;
        }
    }
}
