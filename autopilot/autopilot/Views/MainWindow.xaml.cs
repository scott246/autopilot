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
        public static TreeView MacroFolderTreeViewRef;

        public MainWindow()
        {
            InitializeComponent();
        }

        public static void LoadMacroFolderTree()
        {
            MacroFolderTreeViewRef.Items.Clear();
            string macroDirectory = AppVariables.macroDirectory;
            try
            {
                Directory.CreateDirectory(macroDirectory);
            }
            catch (Exception)
            {
                CustomDialog.Display(CustomDialogType.OK, "Fatal Error", "Error creating macro directory.");
                Application.Current.Shutdown();
            }
            TreeViewItem item = new TreeViewItem
            {
                Header = macroDirectory.Substring(macroDirectory.LastIndexOf('\\') + 1),
                Tag = macroDirectory + '\\',
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.White)
            };
            MacroFolderTreeViewRef.Items.Add(item);
            MainWindowUtils.PopulateTreeView(item, macroDirectory);
            item.IsExpanded = true;
        }

        public static void RefreshMacroFolderTree()
        {
            MacroFolderTreeViewRef.Items.Refresh();
        }

        private void MacroFolderTreeViewLoaded(object sender, RoutedEventArgs e)
        {
            MacroFolderTreeViewRef = MacroFolderTreeView;
            EditorPanel.Visibility = Visibility.Hidden;
            LoadMacroFolderTree();
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
            MainWindowUtils.ExpandAllMacroTreeElements(false, (TreeViewItem)MacroFolderTreeView.Items.GetItemAt(0));
        }

        private void ExpandClicked(object sender, RoutedEventArgs e)
        {
            MainWindowUtils.ExpandAllMacroTreeElements(true, (TreeViewItem)MacroFolderTreeView.Items.GetItemAt(0));
        }

        private void ToggleClicked(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)MacroFolderTreeView.SelectedItem;
            selectedItem.SetActive(!selectedItem.IsActive());
            EditorEnabledCheckbox.IsChecked = selectedItem.IsActive();
        }

        private void AddMacroButtonClicked(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)MacroFolderTreeView.SelectedItem;
            if (null == selectedItem)
                selectedItem = (TreeViewItem)MacroFolderTreeView.Items.GetItemAt(0);
            if (File.GetAttributes((string)selectedItem.Tag).HasFlag(FileAttributes.Directory))
            {
                CustomDialogResponse response = CustomDialog.Display(CustomDialogType.OKCancel, "New Macro", "New macro name (no extension):", textboxContent: "untitled");
                if (response.ButtonResponse != CustomDialogButtonResponse.Cancel) 
                    MainWindowUtils.CreateMacro(selectedItem, response.TextboxResponse);
            }
        }

        private void AddFolderButtonClicked(object sender, RoutedEventArgs e)
        {
            CustomDialogResponse response = CustomDialog.Display(CustomDialogType.OKCancel, "New Folder", "New folder name:", textboxContent: "untitled");
            if (response.ButtonResponse != CustomDialogButtonResponse.Cancel)
            {
                TreeViewItem selectedItem = (TreeViewItem)MacroFolderTreeView.SelectedItem;
                if (null == selectedItem)
                    selectedItem = (TreeViewItem)MacroFolderTreeView.Items.GetItemAt(0);
                MainWindowUtils.CreateFolder(selectedItem, response.TextboxResponse);
            }
        }

        private void DeleteMacroButtonClicked(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)MacroFolderTreeView.SelectedItem;
            if (null != selectedItem && MainWindowUtils.ConfirmDeleteMacro(selectedItem))
            {
                try
                {
                    File.Delete((string)selectedItem.Tag);
                    TreeViewItem parent = (TreeViewItem)selectedItem.Parent;
                    parent.Items.Remove(selectedItem);
                }
                catch (UnauthorizedAccessException)
                {
                    CustomDialog.Display(CustomDialogType.OK, "Insufficient Access", "Could not remove macro. Try running Autopilot as administrator.");
                }
                catch (Exception)
                {
                    CustomDialog.Display(CustomDialogType.OK, "Macro Delete Error", "Could not remove macro.");
                }
            }
        }

        private void SelectedMacroTreeItemChanged(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)MacroFolderTreeView.SelectedItem;
            if (null == item || item.HasItems)
                EditorPanel.Visibility = Visibility.Hidden;
            else
            {
                string itemHeader = MainWindowUtils.TrimAutopilotMacroExtension(item.Header.ToString());
                EditorPanel.Visibility = Visibility.Visible;
                EditorTitleTextBox.Text = itemHeader;
                EditorEnabledCheckbox.IsChecked = item.IsActive();
            }
        }

        private void EnabledCheckboxChecked(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)MacroFolderTreeView.SelectedItem;
            if (item == null) return;
            item.SetActive(true);
        }

        private void EnabledCheckboxUnchecked(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)MacroFolderTreeView.SelectedItem;
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

        private void EditorTitleTextChanged(object sender, TextChangedEventArgs e)
        {
            TreeViewItem treeItem = (TreeViewItem)MacroFolderTreeView.SelectedItem;
            string editorTitleText = EditorTitleTextBox.Text;
            string editorTitleTextNoExtension = MainWindowUtils.TrimAutopilotMacroExtension(editorTitleText);
            if (treeItem == null) return;
            string treeItemHeader = treeItem.Header.ToString();
            string treeItemHeaderNoExtension = MainWindowUtils.TrimAutopilotMacroExtension(treeItemHeader);
            if (editorTitleTextNoExtension != treeItemHeaderNoExtension)
            {
                treeItem.Header = editorTitleTextNoExtension + AppVariables.macroExtension;
                RefreshMacroFolderTree();
            }
        }

        private void EditorDescriptionTextChanged(object sender, TextChangedEventArgs e)
        {
            //save the description
        }

        private void EditMacroButtonClicked(object sender, RoutedEventArgs e)
        {
            //open macro editor window
        }
    }
}
