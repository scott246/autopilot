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
			MacroViewUtils.LoadMacros();
			MacroListView.ItemsSource = SORTED_FILTERED_MACRO_LIST;
			SortComboBox.ItemsSource = SortFilterUtils.sortOptions;
			SortComboBox.SelectedItem = SortFilterUtils.sortOptions[0];
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

		private void EditMacroButton_Click(object sender, RoutedEventArgs e)
		{
			string selectionTitle = ((MacroFile)MacroListView.SelectedItem).Title;
			new EditorView(selectionTitle).Show();
		}

		private void MacroListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void MacroListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			string selectionTitle = ((MacroFile)MacroListView.SelectedItem).Title;
			new EditorView(selectionTitle).Show();
		}

	}
}
