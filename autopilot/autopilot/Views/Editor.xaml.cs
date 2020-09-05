using autopilot.Utils;
using autopilot.Views;
using autopilot.Views.Dialogs;
using autopilot.Views.Preferences;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static autopilot.Globals;

namespace autopilot
{
	public partial class Editor : Window
	{
		public Editor()
		{
			InitializeComponent();
			COMMAND_LIST = CommandUtils.GetAllActions();
			EditorUtils.LoadMacros();
			MacroListView.ItemsSource = SORTED_FILTERED_MACRO_LIST;
			EditorPanel.Visibility = Visibility.Hidden;
			SortComboBox.ItemsSource = SortFilterUtils.sortOptions;
			SortComboBox.SelectedItem = SortFilterUtils.sortOptions[0];
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
			EditorPanel.Visibility = Visibility.Hidden;
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
			}
		}

		private void EditorEnabledCheckbox_Checked(object sender, RoutedEventArgs e)
		{
			MacroFile file = (MacroFile)MacroListView.SelectedItem;
			if (file == null)
			{
				return;
			}

			file.Enabled = true;
		}

		private void EditorEnabledCheckbox_Unchecked(object sender, RoutedEventArgs e)
		{
			MacroFile file = (MacroFile)MacroListView.SelectedItem;
			if (file == null)
			{
				return;
			}

			file.Enabled = false;
		}

		private void SaveMacroButton_Click(object sender, RoutedEventArgs e)
		{
			MacroFile file = new MacroFile
			{
				Enabled = (bool)EditorEnabledCheckbox.IsChecked,
				Title = EditorTitleTextBox.Text,
				Description = EditorDescriptionTextBox.Text,
				Bind = EditorBindLabel.Text,
				Commands = EditorCommandList.Items.Cast<Command>().ToList()
			};
			MacroFileUtils.WriteMacroFile(file, true);
			if (!File.Exists(MacroFileUtils.GetFullPathOfMacroFile(file.Title)))
			{
				MACRO_LIST.Add(file);
				SORTED_FILTERED_MACRO_LIST.Add(file);
			}
			EditorUtils.RefreshMacroList(MacroListView, SortComboBox.SelectedIndex, FilterTextBox.Text);
		}

		private void EditBindButton_Click(object sender, RoutedEventArgs e)
		{
			// open bind combination editor window
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

		private void DeleteCommandButton_Click(object sender, RoutedEventArgs e)
		{
			EditorCommandList.Items.Remove(EditorCommandList.SelectedItem);
		}

		private void AddCommandButton_Click(object sender, RoutedEventArgs e)
		{
			Command command;
			if ((command = AddCommand.Display()) != null)
			{
				ListBoxItem item = new ListBoxItem
				{
					Content = command.Name
				};
				EditorCommandList.Items.Add(item);
			}
		}

		private void EditorCommandList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}
	}
}
