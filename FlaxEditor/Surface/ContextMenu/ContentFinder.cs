using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FlaxEditor.Content;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface.ContextMenu
{

    public struct FinderResult
    {
        public string Name;
        public string TypeName;
        public ContentItem Item;
    }
    
    public class ContentFinder : ContextMenuBase
    {
        private bool _ctrlKey;
        private bool _fKey;
        private Panel _resultPanel;
        private TextBox _searchBox;
        private Match _match;
        private FinderItem _selectedItem;
        
        /// <summary>
        /// Gets or sets the height per item.
        /// </summary>
        public float ItemHeight { get; set; } = 20;
        
        /// <summary>
        /// Gets or sets the logo size.
        /// </summary>
        public float ItemLogoSize { get; set; } = 15;
        
        /// <summary>
        /// Gets or sets the number of item to show.
        /// </summary>
        public int VisibleItemCount { get; set; } = 6;
        
        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public FinderItem SelectedItem {
            get { return _selectedItem; }

            set
            {
                if (value == null && !MatchedItems.Contains(value))
                    return;
                
                if(_selectedItem != null)
                    _selectedItem.BackgroundColor = Color.Transparent;
                
                value.BackgroundColor = Style.Current.BackgroundSelected;
                _selectedItem = value;

                if (MatchedItems.Count > VisibleItemCount)
                {
                    _resultPanel.VScrollBar.SmoothingScale = 0;
                    _resultPanel.ScrollViewTo(_selectedItem);
                    _resultPanel.VScrollBar.SmoothingScale = 1;
                }
            }
        }
        
        /// <summary>
        /// Gets actual matched item list.
        /// </summary>
        public List<FinderItem> MatchedItems { get; } = new List<FinderItem>();
        
        internal bool Hand { get; set; }
        
        private Vector2 firstPos;
        
        public ContentFinder(float width = 280)
        {
            Width = width;
            
            _searchBox = AddChild<TextBox>();
            _searchBox.X = 1;
            _searchBox.Y = 1;
            _searchBox.Width = 278;
            _searchBox.TextChanged += OnTextChanged;

            _resultPanel = AddChild<Panel>();
            _resultPanel.Y = _searchBox.Height + 1;
            _resultPanel.X = 1;
            _resultPanel.Height = Height - (_searchBox.Height + 1 + 1);
            _resultPanel.Width = 278;

            Height = _searchBox.Height+1;
            
            Editor.Instance.Windows.MainWindow.KeyDown += OnGlobalKeyDown;
        }
        
        private void OnTextChanged()
        {
            MatchedItems.Clear();
            
            var text = _searchBox.Text;

            var regex = new Regex(text.Trim(), RegexOptions.IgnoreCase);
            
            BlockingCollection<FinderResult> matches = new BlockingCollection<FinderResult>();
            
            List<ContentItem> assetsItems = Editor.Instance.ContentDatabase.ProjectContent.Folder.Children;
            List<ContentItem> scriptsItems = Editor.Instance.ContentDatabase.ProjectSource.Folder.Children;
            
            if (!text.Trim().Equals(""))
            {
                ProcessItems(regex, assetsItems, matches);
                ProcessItems(regex, scriptsItems, matches);
            }
            
            BuildList(matches.ToList());
        }

        private void BuildList(List<FinderResult> items)
        {
            _resultPanel.DisposeChildren();

            if (items.Count == 0)
            {
                Height = _searchBox.Height+1;
                _resultPanel.ScrollBars = ScrollBars.None;
                RootWindow.Window.ClientSize = new Vector2(RootWindow.Window.ClientSize.X, Height);
                return;
            }

            if (items.Count <= VisibleItemCount)
            {
                Height = 1 + _searchBox.Height + 1 + ItemHeight * items.Count;
                _resultPanel.ScrollBars = ScrollBars.None;
            }
            else
            {
                Height = 1 + _searchBox.Height + 1 + ItemHeight * VisibleItemCount;
                _resultPanel.ScrollBars = ScrollBars.Vertical;
            }

            _resultPanel.Height = ItemHeight * VisibleItemCount;

            int count = 0;
            items.ForEach((item) =>
            {
                var i = new FinderItem(count, item.Name, item.TypeName, item.Item, this);
                MatchedItems.Add(i);
                _resultPanel.AddChild(i);
                i.Build(ItemHeight, ItemLogoSize);
                count++;
            });
            
            RootWindow.Window.ClientSize = new Vector2(RootWindow.Window.ClientSize.X, Height);
            
            PerformLayout();
        }
        
        private void ProcessItems(Regex regex, List<ContentItem> items, BlockingCollection<FinderResult> matches)
        {
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
                        matches.Add(new FinderResult() { Name = asset.ShortName, TypeName = finalName, Item = asset});
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
                        matches.Add(new FinderResult() { Name = contentItem.ShortName, TypeName = finalName, Item = contentItem});
                    }
                }
            }
        }

        public override void Show(Control parent, Vector2 location)
        {
            base.Show(parent, location);
            _searchBox.Text = "";
            _searchBox.Focus();
        }

        public override void Update(float delta)
        {
            Hand = false;
            base.Update(delta);
        }

        public override bool OnKeyDown(Keys key)
        {
            if (key == Keys.ArrowDown)
            {
                if (MatchedItems.Count == 0)
                    return true;
                
                int currentPos;

                if (_selectedItem != null)
                {
                    currentPos = MatchedItems.IndexOf(_selectedItem) + 1;
                    if (currentPos >= MatchedItems.Count)
                        currentPos--;
                }
                else
                    currentPos = 0;

                SelectedItem = MatchedItems[currentPos];
                
                return true;
            }
            else if (key == Keys.ArrowUp)
            {
                if (MatchedItems.Count == 0)
                    return true;
                
                int currentPos;

                if (_selectedItem != null)
                {
                    currentPos = MatchedItems.IndexOf(_selectedItem) - 1;
                    if (currentPos < 0)
                        currentPos = 0;
                }
                else
                    currentPos = 0;

                SelectedItem = MatchedItems[currentPos];
                
                return true;
            }
            else if (key == Keys.Return)
            {
                if(_selectedItem != null)
                {
                    Hide();
                    Editor.Instance.ContentEditing.Open(_selectedItem.Item);
                }

                return true;
            }
            else if (key == Keys.Escape)
            {
                Hide();
                return true;
            }
            else
            {
                return base.OnKeyDown(key);
            }
        }

        private void OnGlobalKeyDown(Keys key)
        {
            if (key == Keys.Control)
            {
                _ctrlKey = true;
            }
            else if (key == Keys.F && _ctrlKey)
            {
                _fKey = true;
            }
            else
            {
                _fKey = false;
                _ctrlKey = false;
            }

            if (_ctrlKey && _fKey)
            {
                firstPos = Editor.Instance.Windows.MainWindow.MousePosition;
                Show(Editor.Instance.Windows.MainWindow.GUI, firstPos);
                _searchBox.Text = "";
                _resultPanel.ScrollViewTo(new Vector2(0,0));
                _fKey = false;
                _ctrlKey = false;
            }
        }

        private Dictionary<string, string> _aliases = new Dictionary<string, string>()
        {
            {"FlaxEditor.Content.Settings.GameSettings", "Settings"},
            {"FlaxEditor.Content.Settings.GraphicsSettings", "Settings"},
            {"FlaxEditor.Content.Settings.InputSettings", "Settings"},
            {"SceneItem", "Scene"}
        };

        public override void Dispose()
        {
            Editor.Instance.Windows.MainWindow.KeyDown -= OnGlobalKeyDown;
            base.Dispose();
        }
    }
}
