////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Surface.Elements;

namespace FlaxEditor.Surface
{
    public partial class VisjectSurface
    {
        /// <summary>
        /// Checks if can use direct conversion from one type to another.
        /// </summary>
        /// <param name="from">Source type.</param>
        /// <param name="to">Target type.</param>
        /// <returns>True if can use direct conversion, otherwise false.</returns>
        public bool CanUseDirectCast(ConnectionType from, ConnectionType to)
        {
            bool result = (from & to) != 0;
            if (!result)
            {
                if (Type == SurfaceType.Material)
                {
                    switch (from)
                    {
                        case ConnectionType.Bool:
                        case ConnectionType.Integer:
                        case ConnectionType.Float:
                        case ConnectionType.Vector2:
                        case ConnectionType.Vector3:
                        case ConnectionType.Vector4:
                            switch (to)
                            {
                                case ConnectionType.Bool:
                                case ConnectionType.Integer:
                                case ConnectionType.Float:
                                case ConnectionType.Vector2:
                                case ConnectionType.Vector3:
                                case ConnectionType.Vector4:
                                    result = true;
                                    break;
                            }
                            break;
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            return result;
        }

        /// <summary>
        /// Begins connecting boxes action.
        /// </summary>
        /// <param name="box">The start box.</param>
        public void ConnectingStart(Box box)
        {
            throw new NotImplementedException("TODO: connecting boxes");
        }
        
        /// <summary>
        /// Ends connecting boxes action.
        /// </summary>
        /// <param name="box">The end box.</param>
        public void ConnectingEnd(Box box)
        {
            throw new NotImplementedException("TODO: connecting boxes");
        }
    }
}
