using autopilot.Objects;
using autopilot.Utils;
using System;
using System.Collections.Generic;
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
		//TODO: break this functionality into manageable (<100 sloc) bits
		private ObservableCollection<ListBoxItem> macroListItems;
		private ObservableCollection<ListBoxItem> editorCommandListItems;

		public EditorView()
		{
			InitializeComponent();

			EditorPanel.Visibility = Visibility.Collapsed;

			MacroPanelUtils.LoadMacros();
			macroListItems = new ObservableCollection<ListBoxItem>();
			foreach (MacroFile macroFile in SORTED_FILTERED_MACRO_LIST)
			{
				string bind = (macroFile.Bind != null && macroFile.Bind != "") ? macroFile.Bind : UNBOUND;
				macroListItems.Add(new ListBoxItem
				{
					Content = macroFile.Title + " (" + bind + ")"
				});
			}
			MacroListView.ItemsSource = macroListItems;
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
			MacroPanelUtils.RefreshMacroList(MacroListView, SortComboBox.SelectedIndex, FilterTextBox.Text);
		}

		private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			MacroPanelUtils.RefreshMacroList(MacroListView, SortComboBox.SelectedIndex, FilterTextBox.Text);
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

			MacroPanelUtils.RefreshMacroList(MacroListView, SortComboBox.SelectedIndex, FilterTextBox.Text);
		}

		private void DeleteMacroButton_Click(object sender, RoutedEventArgs e)
		{
			if (null != MacroListView.SelectedItem)
			{
				MacroFile selectedItem = (MacroFile)MacroListView.SelectedItem;
				if (null != selectedItem && MacroPanelUtils.ConfirmDeleteMacro(selectedItem))
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
				MacroPanelUtils.RefreshMacroList(MacroListView, SortComboBox.SelectedIndex, FilterTextBox.Text);
			}
		}

		private void MacroListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (null == MacroListView.SelectedItem)
			{
				EditorPanel.Visibility = Visibility.Collapsed;
			}
			else
			{
				string displayedListBoxTitle = ((ListBoxItem)MacroListView.SelectedItem).Content.ToString();
				string selectionTitle = displayedListBoxTitle.Substring(0, displayedListBoxTitle.LastIndexOf(" ")); // ((MacroFile)MacroListView.SelectedItem).Title;

				MacroFile readingFile = MacroFileUtils.ReadMacroFile(selectionTitle);

				if (null == readingFile)
				{
					CustomDialog.Display(CustomDialogType.OK, "Read failure", "Failed to read macro file.");
				}
				else
				{
					EditorTitleTextBox.Text = selectionTitle;
					EnabledCheckbox.IsChecked = readingFile.Enabled;
					editorCommandListItems = new ObservableCollection<ListBoxItem>();
					if (null != readingFile.Commands)
					{
						foreach (Command c in readingFile.Commands)
						{
							editorCommandListItems.Add(new ListBoxItem
							{
								Content = c.Title + " (" + c.Arguments + ")"
							});
						}
					}
					EditorCommandList.ItemsSource = editorCommandListItems;
					BindInputTextBox.Text = (readingFile.Bind != null && readingFile.Bind != "") ? readingFile.Bind : UNBOUND;

					EditorPanel.Visibility = Visibility.Visible;
				}

			}
		}

		private void SaveMacroButton_Click(object sender, RoutedEventArgs e)
		{
			List<Command> commandList = new List<Command>();
			foreach (ListBoxItem item in editorCommandListItems)
			{
				//TODO: convert listboxitem to command with appropriate arguments and description
				commandList.Add(new Command(item.Content.ToString(), null, null));
			}
			MacroFile file = new MacroFile
			{
				Enabled = (bool)EnabledCheckbox.IsChecked,
				Title = EditorTitleTextBox.Text,
				Bind = BindInputTextBox.Text,
				Commands = commandList
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
			//TODO: warn on step delete with checkbox to opt out
			ListBoxItem selectedItem = (ListBoxItem)EditorCommandList.SelectedItem;
			if (selectedItem != null)
			{
				editorCommandListItems.Remove(selectedItem);
				EditorCommandList.ItemsSource = editorCommandListItems;
			}
			HighlightSaveButton();
		}

		private void AddCommandButton_Click(object sender, RoutedEventArgs e)
		{
			Command command;
			if ((command = CommandItemEditor.Display()) != null)
			{
				editorCommandListItems.Add(new ListBoxItem
				{
					Content = command.Title
				});
				EditorCommandList.ItemsSource = editorCommandListItems;
				HighlightSaveButton();
			}
		}

		private void EditCommandButton_Click(object sender, RoutedEventArgs e)
		{
			//TODO: add edit command functionality (AddCommand (now CommandItemEditor) but with current settings set?)
			HighlightSaveButton();
		}

		private void EnabledCheckbox_Click(object sender, RoutedEventArgs e)
		{
			//TODO: make enabled checkbox autosave enabled status
			//TODO: make enabled checkbox instantly turn text red in macro panel if disabled, otherwise turn white
			HighlightSaveButton();
		}

		private void HighlightSaveButton()
		{
			SaveMacroButton.Background = (SolidColorBrush)Application.Current.FindResource("AccentColor");
		}
	}
}
