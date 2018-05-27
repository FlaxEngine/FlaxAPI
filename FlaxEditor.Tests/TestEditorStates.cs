// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEditor.Scripting;
using FlaxEditor.States;
using NUnit.Framework;

namespace FlaxEditor.Tests
{
    [TestFixture]
    public class TestEditorStates
    {
        /*[Test]
        public void TestScriptsCompiledBeforeInit()
        {
            Editor editor = new Editor();
            try
            {
                Scripting.ScriptsBuilder.CompilationsCount = 2;
                editor.Init();

                // Mock scripts compilation finish

            }
            finally
            {
                editor.Exit();
            }
        }*/

        [Test]
        [Ignore("TODO: finish building api for unit tests")]
        public void TestStatesChain()
        {
            Editor editor = new Editor();

            editor.Init(true);
            editor.EnsureState<LoadingState>();

            // Mock scripts compilation finish
            editor.Update();
            Scripting.ScriptsBuilder.Internal_OnEvent(ScriptsBuilder.EventType.CompileEndGood);
            editor.Update();

            editor.EnsureState<EditingSceneState>();
            editor.Exit();
            editor.EnsureState<ClosingState>();
        }
    }
}
