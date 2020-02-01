using autopilot.Views.Dialogs;
using autopilot.Views.Preferences;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace autopilot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
	{
        public static TreeView BindFolderTreeViewRef;

        public MainWindow()
        {
            InitializeComponent();
        }

        public static void LoadBindFolderTree()
        {
            BindFolderTreeViewRef.Items.Clear();
            string bindDirectory = AppVariables.bindDirectory;
            try
            {
                Directory.CreateDirectory(bindDirectory);
            }
            catch (Exception)
            {
                CustomDialog.Display(CustomDialogType.OK, "Fatal Error", "Error creating bind directory.", null);
                Application.Current.Shutdown();
            }
            TreeViewItem item = new TreeViewItem
            {
                Header = bindDirectory.Substring(bindDirectory.LastIndexOf('\\') + 1),
                Tag = bindDirectory + '\\',
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.White)
            };
            BindFolderTreeViewRef.Items.Add(item);
            MainWindowUtils.PopulateTreeView(item, bindDirectory);
            item.IsExpanded = true;
        }

        private void BindFolderTreeViewLoaded(object sender, RoutedEventArgs e)
        {
            BindFolderTreeViewRef = BindFolderTreeView;
            EditorPanel.Visibility = Visibility.Hidden;
            LoadBindFolderTree();
        }

        private void AboutMenuItemClicked(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }

        private void PreferencesMenuItemClicked(object sender, RoutedEventArgs e)
        {
            new Preferences().ShowDialog();
        }

        private void CollapseClicked(object sender, RoutedEventArgs e)
        {
            MainWindowUtils.ExpandAllBindTreeElements(false, (TreeViewItem)BindFolderTreeView.Items.GetItemAt(0));
        }

        private void ExpandClicked(object sender, RoutedEventArgs e)
        {
            MainWindowUtils.ExpandAllBindTreeElements(true, (TreeViewItem)BindFolderTreeView.Items.GetItemAt(0));
        }

        private void ToggleClicked(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)BindFolderTreeView.SelectedItem;
            selectedItem.SetActive(!selectedItem.IsActive());
            EditorEnabledCheckbox.IsChecked = selectedItem.IsActive();
        }

        private void AddBindButtonClicked(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)BindFolderTreeView.SelectedItem;
            if (null == selectedItem)
                selectedItem = (TreeViewItem)BindFolderTreeView.Items.GetItemAt(0);
            if (File.GetAttributes((string)selectedItem.Tag).HasFlag(FileAttributes.Directory))
            {
                CustomDialogResponse response = CustomDialog.Display(CustomDialogType.OKCancel, "New Bind", "New bind name (no extension):", textboxContent: "untitled");
                if (response.ButtonResponse != CustomDialogButtonResponse.Cancel) 
                    MainWindowUtils.CreateBind(selectedItem, response.TextboxResponse);
            }
        }

        private void AddFolderButtonClicked(object sender, RoutedEventArgs e)
        {
            CustomDialogResponse response = CustomDialog.Display(CustomDialogType.OKCancel, "New Folder", "New folder name:", textboxContent: "untitled");
            if (response.ButtonResponse != CustomDialogButtonResponse.Cancel)
            {
                TreeViewItem selectedItem = (TreeViewItem)BindFolderTreeView.SelectedItem;
                if (null == selectedItem)
                    selectedItem = (TreeViewItem)BindFolderTreeView.Items.GetItemAt(0);
                MainWindowUtils.CreateFolder(selectedItem, response.TextboxResponse);
            }
        }

        private void DeleteBindButtonClicked(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)BindFolderTreeView.SelectedItem;
            if (null != selectedItem && MainWindowUtils.ConfirmDeleteBind(selectedItem))
            {
                try
                {
                    File.Delete((string)selectedItem.Tag);
                    TreeViewItem parent = (TreeViewItem)selectedItem.Parent;
                    parent.Items.Remove(selectedItem);
                }
                catch (UnauthorizedAccessException)
                {
                    CustomDialog.Display(CustomDialogType.OK, "Insufficient Access", "Could not remove bind. Try running Autopilot as administrator.", null);
                }
                catch (Exception)
                {
                    CustomDialog.Display(CustomDialogType.OK, "Bind Delete Error", "Could not remove bind.", null);
                }
            }
        }

        private void SelectedBindTreeItemChanged(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)BindFolderTreeView.SelectedItem;
            if (item.Equals(null) || item.HasItems)
                EditorPanel.Visibility = Visibility.Hidden;
            else
            {
                EditorPanel.Visibility = Visibility.Visible;
                EditorTitleTextBox.Text = (string)item.Header;
                EditorEnabledCheckbox.IsChecked = item.IsActive();
            }
        }

        private void EnabledCheckboxChecked(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)BindFolderTreeView.SelectedItem;
            if (item == null) return;
            item.SetActive(true);
        }

        private void EnabledCheckboxUnchecked(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)BindFolderTreeView.SelectedItem;
            if (item == null) return;
            item.SetActive(false);
        }

        private void EditorCodePreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            EditorLineNumbers.Text = "";
            int lines = EditorCode.Text.Split('\n').Length;
            for (int i = 0; i < lines; i++)
            {
                EditorLineNumbers.Text += ((i + 1).ToString() + '\n');
            }
            int currentLine = EditorCode.GetLineIndexFromCharacterIndex(EditorCode.CaretIndex);
            EditorLineNumbers.ScrollToLine(currentLine);
        }
    }
}
