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
            //BackgroundColor = Color.Red;

            Button b1 = new Button(10, 10)
            {
                Text = "Click me!",
                Parent = this
            };
            b1.Clicked += () => { Debug.Log("Pressed button #122"); };

            Button b2 = new Button(10, b1.Bottom + 4)
            {
                Text = "Click me!",
                Parent = this,
                Visible = false,
            };
            b2.Clicked += () => { Debug.Log("Pressed button #2");};


            TextBox t1 = new TextBox(false, 10, b2.Bottom + 4);
            t1.Text = "This is a text box";
            t1.Parent = this;

            TextBox t2 = new TextBox(false, 10, t1.Bottom + 4);
            t2.Height = 100;
            t2.Text = "This is a text box\nWithout multiline support";
            t2.Parent = this;

            TextBox t3 = new TextBox(true, 10, t2.Bottom + 4);
            t3.Height = 100;
            t3.Text = "This is a text box\nWith multiline support\nAnother line";
            t3.Parent = this;

            TextBox t4 = new TextBox(true, 10, t3.Bottom + 4);
            t4.Height = 100;
            t4.Text = "This is a text box  r rehjbekjhcvnzsek jbjh bmi ba lkjah mjhhbe wemfbl ewf nwefj bnf \nWith multiline support sf rddnytxes ddrs b dr dbfr  edrxdg eg srg rtdh rg srg \nAnother line sdf  ffsjbn idfjv dfjhv bdv jdfv dfj dfj bdszkjd nlL\nscsr dg bdrse \nfrg ewef\na fgsczea rszsvcfwa\nccsrzsecxzcesv\nefcesrvcw";
            t4.Parent = this;
        }
    }
}
