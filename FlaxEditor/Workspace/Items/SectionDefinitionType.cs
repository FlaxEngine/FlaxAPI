namespace FlaxEditor.Workspace.Items
{
    /// <summary>
    /// Sln files in current revision "Microsoft Visual Studio Solution File, Format Version 12.00" Have only 2 sections. Project and Global thus usage of this simple enum for easier definition
    /// </summary>
    public enum SectionDefinitionType
    {
        /// <summary>
        /// Project seciton type of the *.sln file
        /// </summary>
        Project,
        /// <summary>
        /// Global seciton type of the *.sln file
        /// </summary>
        Global
    }
}
