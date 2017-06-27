////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEngine.Rendering;

namespace FlaxEditor.Viewport
{
    public class MainGizmoEditorViewport : EditorViewport
    {
        public MainGizmoEditorViewport()
            : base(RenderTask.Create<SceneRenderTask>(), true)
        {
        }
    }
}
