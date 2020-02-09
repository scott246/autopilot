using autopilot.Utils;
using autopilot.Views.Dialogs;
using autopilot.Views.Preferences;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using static autopilot.AppVariables;

namespace autopilot
{
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : Window
    {
        private static string filterText = "";
        private static TreeView macroFolderTreeViewRef;

        public Editor()
        {
            InitializeComponent();
            DataContext = MACRO_FILE_TREE;
            EditorPanel.Visibility = Visibility.Hidden;
            LoadMacroFolderTree();
            macroFolderTreeViewRef = MacroFolderTreeView;
            
        }

        public static void LoadMacroFolderTree()
        {
            MACRO_FILE_TREE.Clear();
            string macroDirectory = MACRO_DIRECTORY;
            try
            {
                Directory.CreateDirectory(macroDirectory);
            }
            catch (Exception)
            {
                CustomDialog.Display(CustomDialogType.OK, "Fatal Error", "Error creating macro directory.");
                Application.Current.Shutdown();
            }
            MacroFile file = MacroFileUtils.ReadMacroFile(MACRO_DIRECTORY);
            MACRO_FILE_TREE_ROOT = file;
            MACRO_FILE_TREE.Add(file);
            EditorUtils.PopulateTreeView(file, macroDirectory, filterText);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }

        private void Preferences_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            new Preferences().ShowDialog();
        }

        private void AddMacroButton_Click(object sender, RoutedEventArgs e)
        {
            MacroFile selectedItem = (MacroFile)MacroFolderTreeView.SelectedItem;
            if (null == selectedItem)
                selectedItem = (MacroFile)MacroFolderTreeView.Items.GetItemAt(0);
            if (selectedItem.Directory)
            {
                CustomDialogResponse response = CustomDialog.Display(CustomDialogType.OKCancel, "New Macro", "Name this macro.", textboxContent: "");
                if (response.ButtonResponse != CustomDialogButtonResponse.Cancel && response.TextboxResponse.Trim() != "")
                    if (!MacroFileUtils.CreateMacro(selectedItem, selectedItem.Path + '\\' + MacroFileUtils.GetFileNameWithMacroExtension(response.TextboxResponse)))
                        CustomDialog.Display(CustomDialogType.OK, "Macro create error", "There is already a macro with this name in the folder.");
            }
        }

        private void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            MacroFile selectedItem = (MacroFile)MacroFolderTreeView.SelectedItem;
            if (null == selectedItem)
                selectedItem = (MacroFile)MacroFolderTreeView.Items.GetItemAt(0);
            if (File.GetAttributes(selectedItem.Path).HasFlag(FileAttributes.Directory))
            {
                CustomDialogResponse response = CustomDialog.Display(CustomDialogType.OKCancel, "New Folder", "Name this folder.", textboxContent: "");
                if (response.ButtonResponse != CustomDialogButtonResponse.Cancel && response.TextboxResponse.Trim() != "")
                {
                    if (!MacroFileUtils.CreateFolder(selectedItem, selectedItem.Path + '\\' + response.TextboxResponse))
                        CustomDialog.Display(CustomDialogType.OK, "Folder create error", "This directory already exists.");
                }
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            MacroFile selectedItem = (MacroFile)MacroFolderTreeView.SelectedItem;
            selectedItem.Enabled = !selectedItem.Enabled;
            EditorEnabledCheckbox.IsChecked = selectedItem.Enabled;
        }

        private void DeleteMacroButton_Click(object sender, RoutedEventArgs e)
        {
            MacroFile selectedItem = (MacroFile)MacroFolderTreeView.SelectedItem;
            if (null != selectedItem && EditorUtils.ConfirmDeleteMacro(selectedItem))
            {
                try
                {
                    MacroFileUtils.DeleteMacroFile(selectedItem.Path);
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

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = FilterTextBox.Text;
        }

        private void RefreshTreeButton_Click(object sender, RoutedEventArgs e)
        {
            macroFolderTreeViewRef.DataContext = null;
            macroFolderTreeViewRef.DataContext = MACRO_FILE_TREE;
        }

        private void MacroFolderTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MacroFile file = (MacroFile)MacroFolderTreeView.SelectedItem;

            if (null == file || file.Directory)
                EditorPanel.Visibility = Visibility.Hidden;
            else
            {
                string itemHeader = MacroFileUtils.GetFileNameWithNoMacroExtension(file.Title);
                EditorPanel.Visibility = Visibility.Visible;
                EditorTitleTextBox.Text = itemHeader;
                EditorEnabledCheckbox.IsChecked = file.Enabled;
            }
        }

        private void EditorEnabledCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            MacroFile file = (MacroFile)MacroFolderTreeView.SelectedItem;
            if (file == null) return;
            file.Enabled = true;
            MacroFileUtils.WriteMacroFile(file, true);
        }

        private void EditorEnabledCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            MacroFile file = (MacroFile)MacroFolderTreeView.SelectedItem;
            if (file == null) return;
            file.Enabled = false;
            MacroFileUtils.WriteMacroFile(file, true);
        }

        private void EditorTitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MacroFile file = (MacroFile)MacroFolderTreeView.SelectedItem;
            string editorTitleText = EditorTitleTextBox.Text;
            string editorTitleTextNoExtension = MacroFileUtils.GetFileNameWithNoMacroExtension(editorTitleText);
            if (file == null) return;
            string treeItemHeaderNoExtension = MacroFileUtils.GetFileNameWithNoMacroExtension(file.Title);
            if (editorTitleTextNoExtension != treeItemHeaderNoExtension)
            {
                file.Title = MacroFileUtils.GetFileNameWithMacroExtension(editorTitleText);
                LoadMacroFolderTree();
            }
        }

        private void EditorDescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //save description
        }

        private void EditMacroButton_Click(object sender, RoutedEventArgs e)
        {
            //open macro combination editor window
        }

        private void EditorCode_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
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
