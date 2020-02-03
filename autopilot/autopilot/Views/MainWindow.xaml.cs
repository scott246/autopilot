using autopilot.Utils;
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
        public MainWindow()
        {
            InitializeComponent();
            DataContext = AppVariables.MACRO_FILE_TREE;
        }

        public static void LoadMacroFolderTree()
        {
            AppVariables.MACRO_FILE_TREE.Clear();
            string macroDirectory = AppVariables.MACRO_DIRECTORY;
            try
            {
                Directory.CreateDirectory(macroDirectory);
            }
            catch (Exception)
            {
                CustomDialog.Display(CustomDialogType.OK, "Fatal Error", "Error creating macro directory.");
                Application.Current.Shutdown();
            }
            MacroFile file = MacroFileUtils.ReadMacroFile(AppVariables.MACRO_DIRECTORY);
            AppVariables.MACRO_FILE_TREE_ROOT = file;
            AppVariables.MACRO_FILE_TREE.Add(file);
            Console.WriteLine(AppVariables.MACRO_DIRECTORY);
            Console.WriteLine(AppVariables.MACRO_FILE_TREE);
            Console.WriteLine(AppVariables.MACRO_FILE_TREE_ROOT);
            MainWindowUtils.PopulateTreeView(file, macroDirectory);
        }

        private void MacroFolderTreeViewLoaded(object sender, RoutedEventArgs e)
        {
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
            //MainWindowUtils.ExpandAllMacroTreeElements(false, (MacroFile)MacroFolderTreeView.Items.GetItemAt(0));
        }

        private void ExpandClicked(object sender, RoutedEventArgs e)
        {
            //MainWindowUtils.ExpandAllMacroTreeElements(true, (MacroFile)MacroFolderTreeView.Items.GetItemAt(0));
        }

        private void ToggleClicked(object sender, RoutedEventArgs e)
        {
            MacroFile selectedItem = (MacroFile)MacroFolderTreeView.SelectedItem;
            selectedItem.Enabled = !selectedItem.Enabled;
            if (selectedItem.Enabled)
            {
                selectedItem.Foreground = AppVariables.EnabledTreeColor;
                selectedItem.FontStyle = AppVariables.EnabledFontStyle;
            }
            else
            {
                selectedItem.Foreground = AppVariables.DisabledTreeColor;
                selectedItem.FontStyle = AppVariables.DisabledFontStyle;
            }
            EditorEnabledCheckbox.IsChecked = selectedItem.Enabled;
        }

        private void AddMacroButtonClicked(object sender, RoutedEventArgs e)
        {
            MacroFile selectedItem = (MacroFile)MacroFolderTreeView.SelectedItem;
            if (null == selectedItem)
                selectedItem = (MacroFile)MacroFolderTreeView.Items.GetItemAt(0);
            if (File.GetAttributes(selectedItem.Path).HasFlag(FileAttributes.Directory))
            {
                CustomDialogResponse response = CustomDialog.Display(CustomDialogType.OKCancel, "New Macro", "New macro name (no extension):", textboxContent: "");
                if (response.ButtonResponse != CustomDialogButtonResponse.Cancel && response.TextboxResponse.Trim() != "")  
                    MainWindowUtils.CreateMacro(selectedItem, response.TextboxResponse);
            }
        }

        private void AddFolderButtonClicked(object sender, RoutedEventArgs e)
        {
            MacroFile selectedItem = (MacroFile)MacroFolderTreeView.SelectedItem;
            if (File.GetAttributes(selectedItem.Path).HasFlag(FileAttributes.Directory))
            {
                CustomDialogResponse response = CustomDialog.Display(CustomDialogType.OKCancel, "New Folder", "New folder name:", textboxContent: "");
                if (response.ButtonResponse != CustomDialogButtonResponse.Cancel && response.TextboxResponse.Trim() != "")
                {
                    if (null == selectedItem)
                        selectedItem = (MacroFile)MacroFolderTreeView.Items.GetItemAt(0);
                    MainWindowUtils.CreateFolder(selectedItem, response.TextboxResponse);
                }
            }
        }

        private void DeleteMacroButtonClicked(object sender, RoutedEventArgs e)
        {
            MacroFile selectedItem = (MacroFile)MacroFolderTreeView.SelectedItem;
            if (null != selectedItem && MainWindowUtils.ConfirmDeleteMacro(selectedItem))
            {
                try
                {
                    File.Delete(selectedItem.Path);
                    MainWindowUtils.DeleteMacroFile(selectedItem.Path);
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
            MacroFile file = (MacroFile)MacroFolderTreeView.SelectedItem;
            string fileLocation = file.Path;
            if (null == file || file.Children.Count == 0 || Directory.Exists(fileLocation))
                EditorPanel.Visibility = Visibility.Hidden;
            else
            {
                string itemHeader = MainWindowUtils.GetFileNameWithNoMacroExtension(file.Title);
                EditorPanel.Visibility = Visibility.Visible;
                EditorTitleTextBox.Text = itemHeader;
                EditorEnabledCheckbox.IsChecked = file.Enabled;
            }
        }

        private void EnabledCheckboxChecked(object sender, RoutedEventArgs e)
        {
            MacroFile file = (MacroFile)MacroFolderTreeView.SelectedItem;
            if (file == null) return;
            file.Enabled = true;
        }

        private void EnabledCheckboxUnchecked(object sender, RoutedEventArgs e)
        {
            MacroFile file = (MacroFile)MacroFolderTreeView.SelectedItem;
            if (file == null) return;
            file.Enabled = false;
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
            string editorTitleTextNoExtension = MainWindowUtils.GetFileNameWithNoMacroExtension(editorTitleText);
            if (treeItem == null) return;
            string treeItemHeader = treeItem.Header.ToString();
            string treeItemHeaderNoExtension = MainWindowUtils.GetFileNameWithNoMacroExtension(treeItemHeader);
            if (editorTitleTextNoExtension != treeItemHeaderNoExtension)
            {
                treeItem.Header = MainWindowUtils.GetFileNameWithMacroExtension(editorTitleText);
                LoadMacroFolderTree();
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
