using autopilot.Utils;
using autopilot.Views.Dialogs;
using autopilot.Views.Preferences;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using static autopilot.Globals;

namespace autopilot
{
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : Window
    {
        private static string filterText = "";
        private static string[] sortOptions = { "A-Z", "Z-A", "Enabled/Disabled" };

        public Editor()
        {
            InitializeComponent();
            MacroListView.ItemsSource = MACRO_LIST;
            EditorPanel.Visibility = Visibility.Hidden;
            SortComboBox.ItemsSource = sortOptions;
            SortComboBox.SelectedItem = "A-Z";
            EditorUtils.LoadMacros(filterText);
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }

        private void Preferences_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            new Preferences().ShowDialog();
        }

        private void AddMacroButton_Click(object sender, RoutedEventArgs e)
        {
            CustomDialogResponse response = CustomDialog.Display(CustomDialogType.OKCancel, "New Macro", "Name this macro.", textboxContent: "");
            if (response.ButtonResponse != CustomDialogButtonResponse.Cancel && response.TextboxResponse.Trim() != "")
                if (!MacroFileUtils.CreateMacro(MacroFileUtils.GetFileNameWithMacroExtension(response.TextboxResponse)))
                    CustomDialog.Display(CustomDialogType.OK, "Macro create error", "There is already a macro with this name.");
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            MacroFile selectedItem = (MacroFile)MacroListView.SelectedItem;
            if (null == selectedItem) return;
            selectedItem.Enabled = !selectedItem.Enabled;
            EditorEnabledCheckbox.IsChecked = selectedItem.Enabled;
        }

        private void DeleteMacroButton_Click(object sender, RoutedEventArgs e)
        {
            MacroFile selectedItem = (MacroFile)MacroListView.SelectedItem;
            if (null != selectedItem && EditorUtils.ConfirmDeleteMacro(selectedItem))
            {
                try
                {
                    string title = selectedItem.Title;
                    MacroFileUtils.DeleteMacroFile(title);
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
            MacroListView.ItemsSource = null;
            MacroListView.ItemsSource = MACRO_LIST;
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = FilterTextBox.Text;
        }

        private void MacroListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditorPanel.Visibility = Visibility.Hidden;
            if (null == MacroListView.SelectedItem)
                return;

            string fileTitle = ((MacroFile)MacroListView.SelectedItem).Title;
            MacroFile readingFile = MacroFileUtils.ReadMacroFile(fileTitle);

            if (null == readingFile)
                CustomDialog.Display(CustomDialogType.OK, "Read failure", "Failed to read macro file.");
            else
            {
                string itemHeader = MacroFileUtils.GetFileNameWithNoMacroExtension(readingFile.Title);
                EditorTitleTextBox.Text = itemHeader;
                EditorDescriptionTextBox.Text = readingFile.Description;
                EditorEnabledCheckbox.IsChecked = readingFile.Enabled;
                EditorCode.Text = readingFile.Code;
                EditorBindLabel.Text = readingFile.Bind;
                EditorPanel.Visibility = Visibility.Visible;
            }
        }

        private void EditorEnabledCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            MacroFile file = (MacroFile)MacroListView.SelectedItem;
            if (file == null) return;
            file.Enabled = true;
        }

        private void EditorEnabledCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            MacroFile file = (MacroFile)MacroListView.SelectedItem;
            if (file == null) return;
            file.Enabled = false;
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

        private void SaveMacroButton_Click(object sender, RoutedEventArgs e)
        {
            MacroFile file = new MacroFile
            {
                Enabled = (bool)EditorEnabledCheckbox.IsChecked,
                Title = EditorTitleTextBox.Text,
                Description = EditorDescriptionTextBox.Text,
                Bind = EditorBindLabel.Text,
                Code = EditorCode.Text,
            };
            if (File.Exists(MacroFileUtils.GetFullPathOfMacroFile(file.Title)))
            {
                MacroListView.ItemsSource = null;
                MacroListView.ItemsSource = MACRO_LIST;
            }
            else
            {
                MACRO_LIST.Add(file);
            }
            MacroFileUtils.WriteMacroFile(file, true);
        }

        private void TestMacroButton_Click(object sender, RoutedEventArgs e)
        {
            //run the macro
        }

        private void EditBindButton_Click(object sender, RoutedEventArgs e)
        {
            //open bind combination editor window
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
