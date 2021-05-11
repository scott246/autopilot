using autopilot.Objects;
using autopilot.Utils;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static autopilot.Globals;

namespace autopilot.Views
{
	public partial class EditorView : Window
	{
		private ObservableCollection<Command> editorCommandListItems;
		public EditorView()
		{
			InitializeComponent();

			EditorPanel.Visibility = Visibility.Collapsed;

			MacroViewUtils.LoadMacros();
			MacroListView.ItemsSource = SORTED_FILTERED_MACRO_LIST;
			SortComboBox.ItemsSource = SortFilterUtils.sortOptions;
			SortComboBox.SelectedItem = SortFilterUtils.sortOptions[0];

			COMMAND_LIST = CommandUtils.GetAllActions();
		}

		private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
		{
			new AboutView().ShowDialog();
		}

		private void PreferencesMenuItem_Click(object sender, RoutedEventArgs e)
		{
			new PreferencesView().ShowDialog();
		}

		private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			MacroViewUtils.RefreshMacroList(MacroListView, SortComboBox.SelectedIndex, FilterTextBox.Text);
		}

		private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			MacroViewUtils.RefreshMacroList(MacroListView, SortComboBox.SelectedIndex, FilterTextBox.Text);
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

			MacroViewUtils.RefreshMacroList(MacroListView, SortComboBox.SelectedIndex, FilterTextBox.Text);
		}

		private void DeleteMacroButton_Click(object sender, RoutedEventArgs e)
		{
			MacroFile selectedItem = (MacroFile)MacroListView.SelectedItem;
			if (null != selectedItem && MacroViewUtils.ConfirmDeleteMacro(selectedItem))
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
			MacroViewUtils.RefreshMacroList(MacroListView, SortComboBox.SelectedIndex, FilterTextBox.Text);
		}

		private void MacroListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			string selectionTitle = ((MacroFile)MacroListView.SelectedItem).Title;

			MacroFile readingFile = MacroFileUtils.ReadMacroFile(selectionTitle);

			if (null == readingFile)
			{
				CustomDialog.Display(CustomDialogType.OK, "Read failure", "Failed to read macro file.");
			}
			else
			{
				EditorTitleTextBox.Text = selectionTitle;
				if (null == readingFile.Commands)
				{
					editorCommandListItems = new ObservableCollection<Command>();
				}
				else
				{
					editorCommandListItems = new ObservableCollection<Command>(readingFile.Commands);
				}
				EditorCommandList.ItemsSource = editorCommandListItems;
				BindInputTextBox.Text = (readingFile.Bind != null && readingFile.Bind != "") ? readingFile.Bind : UNBOUND;
			}

			EditorPanel.Visibility = Visibility.Visible;
		}

		private void SaveMacroButton_Click(object sender, RoutedEventArgs e)
		{
			MacroFile file = new MacroFile
			{
				Enabled = true,
				Title = EditorTitleTextBox.Text,
				Bind = BindInputTextBox.Text,
				Commands = EditorCommandList.Items.Cast<Command>().ToList()
			};
			MacroFileUtils.WriteMacroFile(file, true);
			if (!File.Exists(MacroFileUtils.GetFullPathOfMacroFile(file.Title)))
			{
				MACRO_LIST.Add(file);
				SORTED_FILTERED_MACRO_LIST.Add(file);
			}
			SaveMacroButton.Background = (SolidColorBrush)Application.Current.FindResource("ButtonBackgroundColor");
		}

		private void EditBindButton_Click(object sender, RoutedEventArgs e)
		{
			Bind bind = BindEditor.Display();
			BindInputTextBox.Text = BindUtils.ConvertBindToString(bind);
			HighlightSaveButton();
		}

		private void DeleteCommandButton_Click(object sender, RoutedEventArgs e)
		{
			ListBoxItem selectedItem = (ListBoxItem)EditorCommandList.SelectedItem;
			if (selectedItem != null)
			{
				EditorCommandList.Items.Remove(selectedItem);
			}
			HighlightSaveButton();
		}

		private void AddCommandButton_Click(object sender, RoutedEventArgs e)
		{
			Command command;
			if ((command = AddCommand.Display()) != null)
			{
				editorCommandListItems.Add(command);
				EditorCommandList.ItemsSource = editorCommandListItems;
				HighlightSaveButton();
			}
		}

		private void EditorCommandList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void EditCommandButton_Click(object sender, RoutedEventArgs e)
		{
			HighlightSaveButton();
		}

		private void HighlightSaveButton()
		{
			SaveMacroButton.Background = (SolidColorBrush)Application.Current.FindResource("AccentColor");
		}
	}
}
