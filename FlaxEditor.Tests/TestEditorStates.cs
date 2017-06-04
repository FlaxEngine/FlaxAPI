using FlaxEditor.States;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlaxEditor.Tests
{
    [TestClass]
    public class TestEditorStates
    {
        /*[TestMethod]
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

        [TestMethod]
        public void TestStatesChain()
        {
            Editor editor = new Editor();

            editor.Init();
            editor.EnsureState<LoadingState>();

            // Mock scripts compilation finish
            Scripting.ScriptsBuilder.Internal_OnCompilationEnd(true);

            editor.EnsureState<EditingSceneState>();
            editor.Exit();
            editor.EnsureState<ClosingState>();
        }
    }
}
