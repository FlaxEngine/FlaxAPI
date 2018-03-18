////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEditor.Surface.Elements;
using FlaxEngine;

namespace FlaxEditor.Surface
{
    public partial class VisjectSurface
    {
        private static bool CanCast(ConnectionType oB, ConnectionType iB)
        {
            return (oB != ConnectionType.Impulse && oB != ConnectionType.Object) &&
                   (iB != ConnectionType.Impulse && iB != ConnectionType.Object) &&
                   (Mathf.IsPowerOfTwo((int)oB) && Mathf.IsPowerOfTwo((int)iB));
        }

        private static bool CanConnectBoxes(Box start, Box end)
        {
            // Disable for the same box
            if (start == end)
            {
                // Cannot
                return false;
            }

            // Check if boxes are connected
            bool areConnected = start.AreConnected(end);

            // Check if boxes are diffrent or (one of them is disabled and both are disconnected)
            if (end.IsOutput == start.IsOutput || !((end.Enabled && start.Enabled) || areConnected))
            {
                // Cannot
                return false;
            }

            // Cache Input and Output box (since connection may be made in a diffrent way)
            InputBox iB;
            OutputBox oB;
            if (start.IsOutput)
            {
                iB = (InputBox)end;
                oB = (OutputBox)start;
            }
            else
            {
                iB = (InputBox)start;
                oB = (OutputBox)end;
            }

            // Validate connection type (also check if any of boxes parent can manage that connections types)
            if (!iB.CanUseType(oB.CurrentType))
            {
                if (!CanCast(oB.CurrentType, iB.CurrentType))
                {
                    // Cannot
                    return false;
                }
            }

            // Can
            return true;
        }

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
					// Scripts need casting node usage
	                return false;
                }
            }
            return result;
        }

        internal void OnMosueOverBox(Box box)
        {
            _lastBoxUnderMouse = box;
        }

        /// <summary>
        /// Begins connecting boxes action.
        /// </summary>
        /// <param name="box">The start box.</param>
        public void ConnectingStart(Box box)
        {
            if (box != null && box != _startBox)
            {
                _startBox = box;
                StartMouseCapture();
            }
        }

        /// <summary>
        /// Ends connecting boxes action.
        /// </summary>
        /// <param name="end">The end box.</param>
        public void ConnectingEnd(Box end)
        {
            // Ensure that there was a proper start box
            if (_startBox == null)
                return;
            
            Box start = _startBox;
            _startBox = null;

            // Check if boxes are diffrent and end box is specified
            if (start == end || end == null)
                return;
            
            // Check if boxes are connected
            bool areConnected = start.AreConnected(end);

            // Check if boxes are diffrent or (one of them is disabled and both are disconnected)
            if (end.IsOutput == start.IsOutput || !((end.Enabled && start.Enabled) || areConnected))
            {
                // Back
                return;
            }

            // Check if they are already connected
            if (areConnected)
            {
                // Break link
                start.BreakConnection(end);

                // Mark as edited
                MarkAsEdited();

                // Back
                return;
            }

            // Cache Input and Output box (since connection may be made in a diffrent way)
            InputBox iB;
            OutputBox oB;
            if (start.IsOutput)
            {
                iB = (InputBox)end;
                oB = (OutputBox)start;
            }
            else
            {
                iB = (InputBox)start;
                oB = (OutputBox)end;
            }

            // Validate connection type (also check if any of boxes parent can manage that connections types)
            bool useCaster = false;
            if (!iB.CanUseType(oB.CurrentType))
            {
                if (CanCast(oB.CurrentType, iB.CurrentType))
                    useCaster = true;
                else
                    return;
            }

            // Connect boxes
            if (useCaster)
            {
                // Connect via Caster
                //AddCaster(oB, iB);
                throw new NotImplementedException("AddCaster(..) function");
            }
            else
            {
                // Connect directly
                iB.CreateConnection(oB);
            }

            // Mark as edited
            MarkAsEdited();
        }
    }
}
