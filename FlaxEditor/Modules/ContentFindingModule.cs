using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using FlaxEditor.Content;
using FlaxEditor.SceneGraph;
using FlaxEditor.SceneGraph.Actors;
using FlaxEditor.Surface.ContextMenu;
using FlaxEngine;

namespace FlaxEditor.Modules
{
    internal struct QuickAction
    {
        public string Name;
        public Action Action;
    }

    public struct SearchResult
    {
        public string Name;
        public string Type;
        public object Item;
    }


    /// <summary>
    /// The content finding module.
    /// </summary>
    public class ContentFindingModule : EditorModule
    {
        /// <summary>
        /// The content finding context menu.
        /// </summary>
        public ContentFinder Finder { get; private set; }

        private List<QuickAction> _quickActions = new List<QuickAction>();

        public ContentFindingModule(Editor editor) : base(editor)
        {
        }

        /// <inheritdoc />
        public override void OnInit()
        {
            base.OnInit();
            Finder = new ContentFinder();
        }

        /// <summary>
        /// Add <paramref name="action"/> to quick action list.
        /// </summary>
        /// <param name="name">Action's name</param>
        /// <param name="action">The actual action</param>
        public void AddQuickAction(string name, Action action)
        {
            _quickActions.Add(new QuickAction() { Name = name, Action = action });
        }

        /// <summary>
        /// Remove a quick action by his name
        /// </summary>
        /// <param name="name">Action's name</param>
        /// <returns>Return true when it succeed, false if there is no QuickAction with this name</returns>
        public bool RemoveQuickAction(string name)
        {
            foreach (var action in _quickActions)
            {
                if (action.Name.Equals(name))
                {
                    _quickActions.Remove(action);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Search any assets/scripts/quick actions that match the provided type and name.
        /// </summary>
        /// <param name="charsToFind">Two pattern can be used, the first one will just take a string without ':' and will only match names.
        /// The second looks like this "name:type", it will match name and type. Experimental : You can use regular expressions, might break if you are using ':' character.</param>
        /// <returns></returns>
        public List<SearchResult> Search(string charsToFind)
        {
            string type = ".*";
            string name = charsToFind.Trim();

            if (charsToFind.Contains(':'))
            {
                var args = charsToFind.Split(':');
                type = ".*"+args[1].Trim()+".*";
                name = ".*"+args[0].Trim()+".*";
            }
            
            if (name.Equals(""))
                name = ".*";

            Regex typeRegex = new Regex(type, RegexOptions.IgnoreCase);
            Regex nameRegex = new Regex(name, RegexOptions.IgnoreCase);

            BlockingCollection<SearchResult> matches = new BlockingCollection<SearchResult>();

            List<ContentItem> assetsItems = Editor.Instance.ContentDatabase.ProjectContent.Folder.Children;
            List<ContentItem> scriptsItems = Editor.Instance.ContentDatabase.ProjectSource.Folder.Children;

            ProcessItems(nameRegex, typeRegex, assetsItems, matches);
            ProcessItems(nameRegex, typeRegex, scriptsItems, matches);
            ProcessSceneNodes(nameRegex, typeRegex, Editor.Instance.Scene.Root, matches);
            
            _quickActions.ForEach((action) =>
            {
                if (nameRegex.Match(action.Name).Success && typeRegex.Match("Quick Action").Success)
                    matches.Add(new SearchResult() { Name = action.Name, Type = "Quick Action", Item = action });
            });

            return matches.ToList();
        }

        private void ProcessSceneNodes(Regex nameRegex, Regex typeRegex, SceneGraphNode root, BlockingCollection<SearchResult> matches)
        {
            root.ChildNodes.ForEach((node) =>
            {
                if (typeRegex.Match(node.GetType().Name).Success && nameRegex.Match(node.Name).Success)
                {
                    string finalName = node.GetType().Name;
                    if (_aliases.ContainsKey(node.GetType().Name))
                    {
                        finalName = _aliases[node.GetType().Name];
                    }
                    
                    matches.Add(new SearchResult() { Name = node.Name, Type = finalName, Item = node});
                }

                if (node.ChildNodes.Count != 0)
                { 
                    node.ChildNodes.ForEach((n) =>
                    {
                        ProcessSceneNodes(nameRegex, typeRegex, n, matches);
                    });
                }
            });
        }
        
        private void ProcessItems(Regex nameRegex, Regex typeRegex, List<ContentItem> items, BlockingCollection<SearchResult> matches)
        {
            foreach (var contentItem in items)
            {
                if (contentItem.IsAsset)
                {
                    if (nameRegex.Match(contentItem.ShortName).Success)
                    {
                        var asset = contentItem as AssetItem;
                        
                        if (!typeRegex.Match(asset.TypeName).Success)
                            continue;
                        
                        string finalName;

                        if (_aliases.ContainsKey(asset.TypeName))
                        {
                            finalName = _aliases[asset.TypeName];
                        }
                        else
                        {
                            var splits = asset.TypeName.Split('.');
                            finalName = splits[splits.Length - 1];
                        }
                        matches.Add(new SearchResult() { Name = asset.ShortName, Type = finalName, Item = asset });
                    }
                }
                else if (contentItem.IsFolder)
                {
                    ProcessItems(nameRegex, typeRegex, ((ContentFolder)contentItem).Children, matches);
                }
                else if (contentItem.GetType().Name.Equals("FileItem"))
                {
                }
                else
                {
                    if (nameRegex.Match(contentItem.ShortName).Success && typeRegex.Match(contentItem.GetType().Name).Success)
                    {
                        string finalName = contentItem.GetType().Name.Replace("Item", "");
                        
                        matches.Add(new SearchResult() { Name = contentItem.ShortName, Type = finalName, Item = contentItem });
                    }
                }
            }
        }

        internal void Open(object o)
        {
            switch (o)
            {
            case ContentItem contentItem: Editor.Instance.ContentEditing.Open(contentItem);
                break;
            case QuickAction quickAction: quickAction.Action();
                break;
            case SceneGraphNode sceneNode: Editor.Instance.SceneEditing.Select((Actor)sceneNode.EditableObject);
                Editor.Instance.Windows.EditWin.ShowSelectedActors();
                break;
            }
        }
        
        private Dictionary<string, string> _aliases = new Dictionary<string, string>()
        {
            // Assets
            { "FlaxEditor.Content.Settings.AudioSettings", "Settings" },
            { "FlaxEditor.Content.Settings.BuildSettings", "Settings" },
            { "FlaxEditor.Content.Settings.GameSettings", "Settings" },
            { "FlaxEditor.Content.Settings.GraphicsSettings", "Settings" },
            { "FlaxEditor.Content.Settings.InputSettings", "Settings" },
            { "FlaxEditor.Content.Settings.LayersAndTagsSettings", "Settings" },
            { "FlaxEditor.Content.Settings.NavigationSettings", "Settings" },
            { "FlaxEditor.Content.Settings.PhysicsSettings", "Settings" },
            { "FlaxEditor.Content.Settings.TimeSettings", "Settings" },
            { "FlaxEditor.Content.Settings.UWPPlatformSettings", "Settings" },
            { "FlaxEditor.Content.Settings.WindowsPlatformSettings", "Settings" },
            
            // Scene nodes
            {typeof(AnimatedModelNode).Name, "Animated Model"},
            {typeof(AudioListenerNode).Name, "Audio Listener"},
            {typeof(AudioSourceNode).Name, "Audio Source"},
            {typeof(BoneSocketNode).Name, "Bone Socket"},
            {typeof(BoxBrushNode).Name, "Box Brush"},
            {typeof(CameraNode).Name, "Camera"},
            {typeof(ColliderNode).Name, "Collider"},
            {typeof(DecalNode).Name, "Decal"},
            {typeof(DirectionalLightNode).Name, "Directional Light"},
            {typeof(EnvironmentProbeNode).Name, "Environment Probe"},
            {typeof(ExponentialHeightFogNode).Name, "Height Fog"},
            {typeof(FoliageNode).Name, "Foliage"},
            {typeof(NavLinkNode).Name, "NavLink"},
            {typeof(NavMeshBoundsVolumeNode).Name, "Nav Mesh Bounds"},
            {typeof(ParticleEffectNode).Name, "Particle Effect"},
            {typeof(PointLightNode).Name, "Point Light"},
            {typeof(PostFxVolumeNode).Name, "Post Fx Volume"},
            {typeof(SceneNode).Name, "Scene Root"},
            {typeof(SkyboxNode).Name, "Skybox"},
            {typeof(SkyLightNode).Name, "Sky Light"},
            {typeof(SkyNode).Name, "Sky"},
            {typeof(SpotLightNode).Name, "Spot Light"},
            {typeof(StaticModelNode).Name, "Static Model"},
            {typeof(TerrainNode).Name, "Terrain"},
            {typeof(TextRenderNode).Name, "Text Renderer"},
            
        };
    }
}
