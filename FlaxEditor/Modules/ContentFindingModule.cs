using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FlaxEditor.Content;
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
            AddQuickAction("Print Hello", () =>
            {
                Debug.Log("Hello");
            });
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
        /// <param name="name"></param>
        /// <param name="action"></param>
        public void AddQuickAction(string name, Action action)
        {
            _quickActions.Add(new QuickAction(){Name = name, Action = action});
        }

        /// <summary>
        /// Remove a quick action by his name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Return true if it succeed, false if there is no QuickAction with this name</returns>
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

        public List<SearchResult> Search(string charsToFind)
        {
            var regex = new Regex(charsToFind.Trim(), RegexOptions.IgnoreCase);
            
            BlockingCollection<SearchResult> matches = new BlockingCollection<SearchResult>();
            
            List<ContentItem> assetsItems = Editor.Instance.ContentDatabase.ProjectContent.Folder.Children;
            List<ContentItem> scriptsItems = Editor.Instance.ContentDatabase.ProjectSource.Folder.Children;


            if (!charsToFind.Trim().Equals(""))
            {
                ProcessItems(regex, assetsItems, matches);
                ProcessItems(regex, scriptsItems, matches);
                
                _quickActions.ForEach((action) =>
                {
                    if(regex.Match(action.Name).Success)
                        matches.Add(new SearchResult(){Name = action.Name, Type = "Quick Action", Item = action});
                });
            }

            return matches.ToList();
        }
        
        private void ProcessItems(Regex regex, List<ContentItem> items, BlockingCollection<SearchResult> matches)
        {
            Match _match;
            foreach (var contentItem in items)
            {
                if (contentItem.IsAsset)
                {
                    _match = regex.Match(contentItem.ShortName);
                    if (_match.Success)
                    {
                        var asset = contentItem as AssetItem;
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
                        matches.Add(new SearchResult() { Name = asset.ShortName, Type = finalName, Item = asset});
                    }
                }
                else if(contentItem.IsFolder)
                {
                    //ThreadPool.QueueUserWorkItem((state) => ProcessItems(regex, ((ContentFolder) contentItem).Children, matches));
                    ProcessItems(regex, ((ContentFolder) contentItem).Children, matches);
                }
                else if (contentItem.GetType().Name.Equals("FileItem"))
                {
                    
                }
                else
                {
                    _match = regex.Match(contentItem.ShortName);
                    if (_match.Success)
                    {
                        string finalName;
                        if (_aliases.ContainsKey(contentItem.GetType().Name))
                        {
                            finalName = _aliases[contentItem.GetType().Name];
                        }
                        else
                        {
                            finalName = contentItem.GetType().Name.Replace("Item", "");
                        }
                        matches.Add(new SearchResult() { Name = contentItem.ShortName, Type = finalName, Item = contentItem});
                    }
                }
            }
        }
        
        private Dictionary<string, string> _aliases = new Dictionary<string, string>()
        {
            {"FlaxEditor.Content.Settings.GameSettings", "Settings"},
            {"FlaxEditor.Content.Settings.GraphicsSettings", "Settings"},
            {"FlaxEditor.Content.Settings.InputSettings", "Settings"},
            {"SceneItem", "Scene"}
        };
    }
}
