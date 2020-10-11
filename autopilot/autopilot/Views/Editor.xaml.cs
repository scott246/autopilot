using autopilot.Utils;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static autopilot.Globals;

namespace autopilot.Views
{
	public partial class Editor : Window
	{
		public Editor(string title)
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
				EditorBindLabel.Text = (readingFile.Bind != null && readingFile.Bind != "") ? readingFile.Bind : "[unbound]";
			}
		}

		private void SaveMacroButton_Click(object sender, RoutedEventArgs e)
		{
			MacroFile file = new MacroFile
			{
				Enabled = true,
				Title = EditorTitleTextBox.Text,
				Bind = EditorBindLabel.Text,
				Commands = EditorCommandList.Items.Cast<Command>().ToList()
			};
			MacroFileUtils.WriteMacroFile(file, true);
			if (!File.Exists(MacroFileUtils.GetFullPathOfMacroFile(file.Title)))
			{
				MACRO_LIST.Add(file);
				SORTED_FILTERED_MACRO_LIST.Add(file);
			}
			
		}

		private void EditBindButton_Click(object sender, RoutedEventArgs e)
		{
			// open bind combination editor window
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
				EditorCommandList.Items.Add(new ListBoxItem
				{
					Content = command.Name
				});
			}
		}

		private void EditorCommandList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void EditCommandButton_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
