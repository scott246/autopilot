using System;
using System.Windows;
using System.Windows.Controls;
using autopilot.Utils;
using static autopilot.Globals;

namespace autopilot.Views
{
	/// <summary>
	/// Interaction logic for MacroView.xaml
	/// </summary>
	public partial class MacroView : Window
	{
		public MacroView()
		{
			InitializeComponent();
			EditorUtils.LoadMacros();
			MacroListView.ItemsSource = SORTED_FILTERED_MACRO_LIST;
			SortComboBox.ItemsSource = SortFilterUtils.sortOptions;
			SortComboBox.SelectedItem = SortFilterUtils.sortOptions[0];
			//EditorUtils.RefreshMacroList(MacroListView, SortComboBox.SelectedIndex, FilterTextBox.Text);
		}

		private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			EditorUtils.RefreshMacroList(MacroListView, SortComboBox.SelectedIndex, FilterTextBox.Text);
		}

		private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
		{
			new About().ShowDialog();
		}

		private void PreferencesMenuItem_Click(object sender, RoutedEventArgs e)
		{
			new Preferences().ShowDialog();
		}

		private void AddMacroButton_Click(object sender, RoutedEventArgs e)
		{
			CustomDialogResponse response = CustomDialog.Display(CustomDialogType.OKCancel, "New Macro", "Name this macro.", textboxContent: "");
			if (response.ButtonResponse != CustomDialogButtonResponse.Cancel && response.TextboxResponse.Trim() != "")
			{
				if (!MacroFileUtils.CreateMacro(MacroFileUtils.GetFileNameWithMacroExtension(response.TextboxResponse)))
				{
					CustomDialog.Display(CustomDialogType.OK, "Macro create error", "There is already a macro with this name.");
				}
			}

			EditorUtils.RefreshMacroList(MacroListView, SortComboBox.SelectedIndex, FilterTextBox.Text);
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
			EditorUtils.RefreshMacroList(MacroListView, SortComboBox.SelectedIndex, FilterTextBox.Text);
		}

		private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			EditorUtils.RefreshMacroList(MacroListView, SortComboBox.SelectedIndex, FilterTextBox.Text);
		}

		private void MacroListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			
		}

		private void EditMacroButton_Click(object sender, RoutedEventArgs e)
		{
			/*EditorPanel.Visibility = Visibility.Hidden;
			if (null == MacroListView.SelectedItem)
			{
				return;
			}

			string fileTitle = ((MacroFile)MacroListView.SelectedItem).Title;
			MacroFile readingFile = MacroFileUtils.ReadMacroFile(fileTitle);

			if (null == readingFile)
			{
				CustomDialog.Display(CustomDialogType.OK, "Read failure", "Failed to read macro file.");
			}
			else
			{
				string itemHeader = MacroFileUtils.GetFileNameWithNoMacroExtension(readingFile.Title);
				EditorTitleTextBox.Text = itemHeader;
				EditorDescriptionTextBox.Text = readingFile.Description;
				EditorEnabledCheckbox.IsChecked = readingFile.Enabled;
				EditorCommandList.ItemsSource = readingFile.Commands;
				EditorBindLabel.Text = (readingFile.Bind != null && readingFile.Bind != "") ? readingFile.Bind : "[unbound]";
				EditorPanel.Visibility = Visibility.Visible;
			}*/
		}
	}
}
