// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Surface parameters types.
    /// </summary>
    public enum ParameterType
    {
        /// <summary>
        /// The boolean value.
        /// </summary>
        Bool = 0,

        /// <summary>
        /// The integer value.
        /// </summary>
        Integer = 1,

        /// <summary>
        /// The floating point value.
        /// </summary>
        Float = 2,

        /// <summary>
        /// The Vector2 value.
        /// </summary>
        Vector2 = 3,

        /// <summary>
        /// The Vector2 value.
        /// </summary>
        Vector3 = 4,

        /// <summary>
        /// The Vector2 value.
        /// </summary>
        Vector4 = 5,

        /// <summary>
        /// The Vector2 value.
        /// </summary>
        Color = 6,

        /// <summary>
        /// The texture id.
        /// </summary>
        Texture = 7,

        /// <summary>
        /// The normal map texture id.
        /// </summary>
        NormalMap = 8,

        /// <summary>
        /// The string value.
        /// </summary>
        String = 9,

        /// <summary>
        /// The BoundingBox value.
        /// </summary>
        Box = 10,

        /// <summary>
        /// The Quaternion value.
        /// </summary>
        Rotation = 11,

        /// <summary>
        /// The Transform value.
        /// </summary>
        Transform = 12,

        /// <summary>
        /// The asset id.
        /// </summary>
        Asset = 13,

        /// <summary>
        /// The actor id.
        /// </summary>
        Actor = 14,

        /// <summary>
        /// The Rectangle value.
        /// </summary>
        Rectangle = 15,

        /// <summary>
        /// The cube texture id.
        /// </summary>
        CubeTexture = 16,

        /// <summary>
        /// The scene texture type.
        /// </summary>
        SceneTexture = 17,

        /// <summary>
        /// The render target (created from code).
        /// </summary>
        RenderTarget = 18,

        /// <summary>
        /// The matrix.
        /// </summary>
        Matrix = 19,

        /// <summary>
        /// The render target array (created from code).
        /// </summary>
        RenderTargetArray = 20,

        /// <summary>
        /// The volume render target (created from code).
        /// </summary>
        RenderTargetVolume = 21,

        /// <summary>
        /// The cube render target (created from code).
        /// </summary>
        RenderTargetCube = 22,
    }
}
