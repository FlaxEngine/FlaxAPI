using FlaxEditor.Surface.ContextMenu;

namespace FlaxEditor.Modules
{
    /// <summary>
    /// The content finding module.
    /// </summary>
    public class ContentFindingModule : EditorModule
    {
        /// <summary>
        /// The content finding context menu.
        /// </summary>
        public ContentFinder Finder { get; private set; }
        
        public ContentFindingModule(Editor editor) : base(editor)
        {
            
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            base.OnInit();
            Finder = new ContentFinder();
        }
    }
}
