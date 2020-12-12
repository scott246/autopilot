using autopilot.Objects;
using autopilot.Utils;
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
		public EditorView(string title)
		{
			InitializeComponent();
			COMMAND_LIST = CommandUtils.GetAllActions();

			MacroFile readingFile = MacroFileUtils.ReadMacroFile(title);

			if (null == readingFile)
			{
				CustomDialog.Display(CustomDialogType.OK, "Read failure", "Failed to read macro file.");
				Close();
			}
			else
			{
				string itemHeader = MacroFileUtils.GetFileNameWithNoMacroExtension(readingFile.Title);
				EditorTitleTextBox.Text = itemHeader;
				EditorCommandList.ItemsSource = readingFile.Commands;
				BindInputTextBox.Text = (readingFile.Bind != null && readingFile.Bind != "") ? readingFile.Bind : "[unbound]";
			}
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
				EditorCommandList.Items.Add(new ListBoxItem
				{
					Content = command.Name
				});
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
