////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.GUI.Common;

namespace FlaxEditor.Windows
{
    public sealed class TestWindow : Window
    {
        protected override void Initialize()
        {
            //BackgroundColor = Color.Red;

            Button b1 = new Button(10, 10);
            b1.Text = "Click me!";
            b1.Parent = this;
            b1.Clicked += () => { Debug.Log("Pressed button #1"); };

            Button b2 = new Button(10, b1.Bottom + 4);
            b2.Text = "Click me!";
            b2.Parent = this;
            b2.Clicked += () => { Debug.Log("Pressed button #2");};
        }
    }
}
