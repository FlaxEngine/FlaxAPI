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
        public void TestStatesChain()
        {
            Editor editor = new Editor();

            editor.Init();
            editor.EnsureState<LoadingState>();
            
            // Mock scripts compilation finish
            editor.Update();
            Scripting.ScriptsBuilder.Internal_OnCompilationEnd(true);
            editor.Update();

            editor.EnsureState<EditingSceneState>();
            editor.Exit();
            editor.EnsureState<ClosingState>();
        }
    }
}
