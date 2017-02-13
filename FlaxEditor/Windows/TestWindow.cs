////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Windows
{
    public sealed class TestWindow : Window
    {
        protected override void Initialize()
        {
            BackgroundColor = Color.Red;
        }

        public override bool OnKeyDown(KeyCode key)
        {
            Debug.Log("Key down: " + key);

            return base.OnKeyDown(key);
        }

        public override void OnKeyUp(KeyCode key)
        {
            Debug.Log("Key up: " + key);

            base.OnKeyDown(key);
        }
    }
}
