using autopilot.Utils;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace autopilot.Views
{
	/// <summary>
	/// Interaction logic for AddStep.xaml
	/// </summary>
	public partial class CommandItemEditor : Window
	{
		private string filterText = "";

		private ListBoxItem selectedItem = null;
		private ListBoxItem returnedItem = null;

		public CommandItemEditor()
		{
			InitializeComponent();
			DisplayFilteredItems();
		}

		public CommandItemEditor(Command command)
		{
			InitializeComponent();
			DisplayFilteredItems();
			for (int i = 0; i < CommandList.Items.Count; i++)
			{
				if ((string)((ListBoxItem)CommandList.Items.GetItemAt(i)).Content == command.Title)
				{
					CommandList.SelectedIndex = i;
					selectedItem = (ListBoxItem)CommandList.SelectedItem;
					selectedItem.Tag = command;
				}
			}
			for (int i = 0; i < command.Arguments.Count; i++)
			{
				switch (i)
				{
					case 0:
						Argument1Label.Content = command.Arguments[0].Key;
						Argument1Input.Text = command.Arguments[0].Value;
						break;
					case 1:
						Argument2Label.Content = command.Arguments[1].Key;
						Argument2Input.Text = command.Arguments[1].Value;
						break;
					case 2:
						Argument3Label.Content = command.Arguments[2].Key;
						Argument3Input.Text = command.Arguments[2].Value;
						break;
					case 3:
						Argument4Label.Content = command.Arguments[3].Key;
						Argument4Input.Text = command.Arguments[3].Value;
						break;
					case 4:
						Argument5Label.Content = command.Arguments[4].Key;
						Argument5Input.Text = command.Arguments[4].Value;
						break;
					default:
						break;
				}
			}
		}

		public static Command Display()
		{
			CommandItemEditor editor = new CommandItemEditor();
			editor.ShowDialog();
			return (null == editor.returnedItem || null == editor.returnedItem.Tag) ? null : (Command)editor.returnedItem.Tag;
		}

		public static Command Display(Command command)
		{
			CommandItemEditor editor = new CommandItemEditor(command);
			editor.ShowDialog();
			return (null == editor.returnedItem || null == editor.returnedItem.Tag) ? null : (Command)editor.returnedItem.Tag;
		}

		private void DisplayFilteredItems()
		{
			CommandList.Items.Clear();
			foreach (Command command in Globals.COMMAND_LIST)
			{
				if (filterText.Equals("") || command.Title.Contains(filterText))
				{
					CommandList.Items.Add(EditorPanelUtils.ConvertCommandToListBoxItem(command, false));
				}
			}
			CommandDescription.Text = "";
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			returnedItem = null;
			Close();
		}

		private void Done_Click(object sender, RoutedEventArgs e)
		{
			for (int i = 0; i < ((Command)selectedItem.Tag).Arguments.Count; i++)
			{
				switch (i)
				{
					case 0:
						((Command)selectedItem.Tag).Arguments[0] = 
							new KeyValuePair<string, string>(Argument1Label.Content.ToString(), Argument1Input.Text);
						break;
					case 1:
						((Command)selectedItem.Tag).Arguments[1] = 
							new KeyValuePair<string, string>(Argument2Label.Content.ToString(), Argument2Input.Text);
						break;
					case 2:
						((Command)selectedItem.Tag).Arguments[2] =
							new KeyValuePair<string, string>(Argument3Label.Content.ToString(), Argument3Input.Text);
						break;
					case 3:
						((Command)selectedItem.Tag).Arguments[3] =
							new KeyValuePair<string, string>(Argument4Label.Content.ToString(), Argument4Input.Text);
						break;
					case 4:
						((Command)selectedItem.Tag).Arguments[4] =
							new KeyValuePair<string, string>(Argument5Label.Content.ToString(), Argument5Input.Text);
						break;
					default:
						break;
				}
			}
			returnedItem = selectedItem;
			Close();
		}

		private void CommandList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (CommandList.SelectedItem != null)
			{
				selectedItem = (ListBoxItem)CommandList.SelectedItem;
				CommandDescription.Text = ((Command)selectedItem.Tag).Description;
				PopulateArgumentPanel(((Command)selectedItem.Tag).Arguments);
			}
			else
			{
				HideAllArgumentFields();
			}
		}

		private void PopulateArgumentPanel(List<KeyValuePair<string, string>> arguments)
		{
			HideAllArgumentFields();
			for (int i = 0; i < arguments.Count; i++)
			{
				switch (i)
				{
					case 0:
						Argument1Label.Visibility = Visibility.Visible;
						Argument1Label.Content = arguments[0].Key;
						Argument1Input.Visibility = Visibility.Visible;
						Argument1Input.Text = arguments[0].Value;
						break;
					case 1:
						Argument2Label.Visibility = Visibility.Visible;
						Argument2Label.Content = arguments[1].Key;
						Argument2Input.Visibility = Visibility.Visible;
						Argument2Input.Text = arguments[1].Value;
						break;
					case 2:
						Argument3Label.Visibility = Visibility.Visible;
						Argument3Label.Content = arguments[2].Key;
						Argument3Input.Visibility = Visibility.Visible;
						Argument3Input.Text = arguments[2].Value;
						break;
					case 3:
						Argument4Label.Visibility = Visibility.Visible;
						Argument4Label.Content = arguments[3].Key;
						Argument4Input.Visibility = Visibility.Visible;
						Argument4Input.Text = arguments[3].Value;
						break;
					case 4:
						Argument5Label.Visibility = Visibility.Visible;
						Argument5Label.Content = arguments[4].Key;
						Argument5Input.Visibility = Visibility.Visible;
						Argument5Input.Text = arguments[4].Value;
						break;
					default:
						break;
				}
			}
		}

		private void HideAllArgumentFields()
		{
			Argument1Label.Visibility = Visibility.Hidden;
			Argument1Input.Visibility = Visibility.Hidden;
			Argument2Label.Visibility = Visibility.Hidden;
			Argument2Input.Visibility = Visibility.Hidden;
			Argument3Label.Visibility = Visibility.Hidden;
			Argument3Input.Visibility = Visibility.Hidden;
			Argument4Label.Visibility = Visibility.Hidden;
			Argument4Input.Visibility = Visibility.Hidden;
			Argument5Label.Visibility = Visibility.Hidden;
			Argument5Input.Visibility = Visibility.Hidden;
		}

		private void CommandFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			filterText = CommandFilterTextBox.Text;
			DisplayFilteredItems();
		}
	}
}
