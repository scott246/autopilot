﻿using autopilot.Objects;
using autopilot.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static autopilot.Globals;

namespace autopilot.Views
{
	public partial class EditorView : Window
	{
		private ObservableCollection<ListBoxItem> editorCommandListItems;

		public EditorView()
		{
			InitializeComponent();

			EditorPanel.Visibility = Visibility.Collapsed;

			MacroPanelUtils.LoadMacros();
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
				if (!MacroFileUtils.CreateMacro(MacroFileUtils.GetFileName(response.TextboxResponse, true)))
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
				string selectionTitle = ((MacroFile)MacroListView.SelectedItem).Title;
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
							editorCommandListItems.Add(EditorPanelUtils.ConvertCommandToListBoxItem(c));
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
				commandList.Add((Command)item.Tag);
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
			MacroPanelUtils.RefreshMacroList(MacroListView, SortComboBox.SelectedIndex, FilterTextBox.Text);
		}

		private void EditBindButton_Click(object sender, RoutedEventArgs e)
		{
			Bind bind = BindEditor.Display();
			BindInputTextBox.Text = BindUtils.ConvertBindToString(bind);
			HighlightSaveButton();
		}

		private void DeleteCommandButton_Click(object sender, RoutedEventArgs e)
		{
			if (null != EditorCommandList.SelectedItem)
			{
				ListBoxItem selectedItem = (ListBoxItem)EditorCommandList.SelectedItem;
				if (null != selectedItem && EditorPanelUtils.ConfirmDeleteCommand(selectedItem))
				{
					editorCommandListItems.Remove(selectedItem);
					EditorCommandList.ItemsSource = editorCommandListItems;
				}
			}
			HighlightSaveButton();
		}

		private void AddCommandButton_Click(object sender, RoutedEventArgs e)
		{
			Command command;
			if ((command = CommandItemEditor.Display()) != null)
			{
				editorCommandListItems.Add(EditorPanelUtils.ConvertCommandToListBoxItem(command));
				EditorCommandList.ItemsSource = editorCommandListItems;

				HighlightSaveButton();
			}
		}

		private void EditCommandButton_Click(object sender, RoutedEventArgs e)
		{
			Command newCommand = CommandItemEditor.Display((Command)((ListBoxItem)EditorCommandList.SelectedItem).Tag);
			if (null != newCommand && null != newCommand.Arguments)
			{
				ListBoxItem item = EditorPanelUtils.ConvertCommandToListBoxItem(newCommand);

				((ListBoxItem)EditorCommandList.SelectedItem).Tag = item.Tag;
				((ListBoxItem)EditorCommandList.SelectedItem).Content = item.Content;
			}
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
