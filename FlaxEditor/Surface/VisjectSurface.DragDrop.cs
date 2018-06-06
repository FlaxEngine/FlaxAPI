// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.IO;
using System.Threading;
using FlaxEditor.Content;
using FlaxEditor.GUI.Drag;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface
{
    public partial class VisjectSurface
    {
        private DragAssets _dragOverItems = new DragAssets();
        private DragSurfaceParameter _dragOverParameter = new DragSurfaceParameter();

        /// <inheritdoc />
        public override DragDropEffect OnDragEnter(ref Vector2 location, DragData data)
        {
            var result = base.OnDragEnter(ref location, data);

            if (result == DragDropEffect.None)
            {
                if (_dragOverItems.OnDragEnter(data, ValidateDragItemFunc))
                {
                    result = _dragOverItems.Effect;
                }
                else if (_dragOverParameter.OnDragEnter(data, ValidateDragParameterFunc))
                {
                    result = _dragOverParameter.Effect;
                }
            }

            return result;
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragMove(ref Vector2 location, DragData data)
        {
            var result = base.OnDragMove(ref location, data);

            if (result == DragDropEffect.None)
            {
                if (_dragOverItems.HasValidDrag)
                {
                    result = _dragOverItems.Effect;
                }
                else if (_dragOverParameter.HasValidDrag)
                {
                    result = _dragOverParameter.Effect;
                }
            }

            return result;
        }

        /// <inheritdoc />
        public override void OnDragLeave()
        {
            _dragOverItems.OnDragLeave();
            _dragOverParameter.OnDragLeave();

            base.OnDragLeave();
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragDrop(ref Vector2 location, DragData data)
        {
            var result = base.OnDragDrop(ref location, data);
            if (result != DragDropEffect.None)
                return result;

            if (_dragOverItems.HasValidDrag)
            {
                result = _dragOverItems.Effect;
                var surfaceLocation = _surface.PointFromParent(location);

                switch (Type)
                {
                case SurfaceType.Material:
                {
                    for (int i = 0; i < _dragOverItems.Objects.Count; i++)
                    {
                        var item = _dragOverItems.Objects[i];
                        SurfaceNode node = null;

                        switch (item.ItemDomain)
                        {
                        case ContentDomain.Texture:
                        {
                            // Check if it's a normal map
                            bool isNormalMap = false;
                            var obj = FlaxEngine.Content.LoadAsync<Texture>(item.ID);
                            if (obj)
                            {
                                Thread.Sleep(50);

                                if (!obj.WaitForLoaded())
                                {
                                    isNormalMap = obj.IsNormalMap;
                                }
                            }

                            node = SpawnNode(5, (ushort)(isNormalMap ? 4 : 1), surfaceLocation, new object[] { item.ID });
                            break;
                        }

                        case ContentDomain.CubeTexture:
                        {
                            node = SpawnNode(5, 3, surfaceLocation, new object[] { item.ID });
                            break;
                        }

                        case ContentDomain.Material:
                        {
                            node = SpawnNode(8, 1, surfaceLocation, new object[] { item.ID });
                            break;
                        }
                        }

                        if (node != null)
                        {
                            surfaceLocation.X += node.Width + 10;
                        }
                    }

                    break;
                }
                case SurfaceType.AnimationGraph:
                {
                    for (int i = 0; i < _dragOverItems.Objects.Count; i++)
                    {
                        var item = _dragOverItems.Objects[i];
                        SurfaceNode node = null;

                        switch (item.ItemDomain)
                        {
                        case ContentDomain.Animation:
                        {
                            node = SpawnNode(9, 2, surfaceLocation, new object[]
                            {
                                item.ID,
                                1.0f,
                                true,
                                0.0f,
                            });
                            break;
                        }
                        case ContentDomain.SkeletonMask:
                        {
                            node = SpawnNode(9, 11, surfaceLocation, new object[]
                            {
                                0.0f,
                                item.ID,
                            });
                            break;
                        }
                        }

                        if (node != null)
                        {
                            surfaceLocation.X += node.Width + 10;
                        }
                    }

                    break;
                }
                }

                _dragOverItems.OnDragDrop();
            }
            else if (_dragOverParameter.HasValidDrag)
            {
                result = _dragOverParameter.Effect;
                var surfaceLocation = _surface.PointFromParent(location);
                var parameter = GetParameter(_dragOverParameter.Parameter);
                if (parameter == null)
                    throw new InvalidDataException();

                var node = SpawnNode(6, 1, surfaceLocation, new object[]
                {
                    parameter.ID
                });

                _dragOverParameter.OnDragDrop();
            }

            return result;
        }

        private bool ValidateDragItemFunc(AssetItem assetItem)
        {
            switch (Type)
            {
            case SurfaceType.Material:
            {
                switch (assetItem.ItemDomain)
                {
                case ContentDomain.Texture:
                case ContentDomain.CubeTexture:
                case ContentDomain.Material: return true;
                }
                break;
            }
            case SurfaceType.AnimationGraph:
            {
                switch (assetItem.ItemDomain)
                {
                case ContentDomain.SkeletonMask:
                case ContentDomain.Animation: return true;
                }
                break;
            }
            }
            return false;
        }

        private bool ValidateDragParameterFunc(string parameterName)
        {
            return GetParameter(parameterName) != null;
        }
    }
}
