using autopilot.Utils;
using System;
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

		public static Command Display()
		{
			CommandItemEditor a = new CommandItemEditor();
			a.ShowDialog();
			return (null == a.returnedItem || null == a.returnedItem.Tag) ? null : (Command)a.returnedItem.Tag;
		}

		private void DisplayFilteredItems()
		{
			CommandList.Items.Clear();
			foreach (Command command in Globals.COMMAND_LIST)
			{
				if (filterText.Equals("") || command.Title.Contains(filterText))
				{
					CommandList.Items.Add(new ListBoxItem
					{
						Content = command.Title,
						Tag = command
					});
				}
			}
			CommandDescription.Text = "";
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			returnedItem = null;
			Close();
		}

		private void Add_Click(object sender, RoutedEventArgs e)
		{
			returnedItem = selectedItem;
			Close();
		}

		private void CommandList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			selectedItem = (ListBoxItem)CommandList.SelectedItem;
			CommandDescription.Text = ((Command)selectedItem.Tag).Description;
			PopulateArgumentPanel(((Command)selectedItem.Tag).Arguments);
		}

		private void PopulateArgumentPanel(List<string> arguments)
		{
			HideAllArgumentFields();
			switch (arguments.Count)
			{
				case 1:
					Argument1Label.Visibility = Visibility.Visible;
					Argument1Label.Content = arguments[0];
					Argument1Input.Visibility = Visibility.Visible;
					break;
				case 2:
					Argument1Label.Visibility = Visibility.Visible;
					Argument1Label.Content = arguments[0];
					Argument1Input.Visibility = Visibility.Visible;
					Argument2Label.Visibility = Visibility.Visible;
					Argument2Label.Content = arguments[1];
					Argument2Input.Visibility = Visibility.Visible;
					break;
				case 3:
					Argument1Label.Visibility = Visibility.Visible;
					Argument1Label.Content = arguments[0];
					Argument1Input.Visibility = Visibility.Visible;
					Argument2Label.Visibility = Visibility.Visible;
					Argument1Label.Content = arguments[1];
					Argument2Input.Visibility = Visibility.Visible;
					Argument3Label.Visibility = Visibility.Visible;
					Argument1Label.Content = arguments[2];
					Argument3Input.Visibility = Visibility.Visible;
					break;
				case 4:
					Argument1Label.Visibility = Visibility.Visible;
					Argument1Label.Content = arguments[0];
					Argument1Input.Visibility = Visibility.Visible;
					Argument2Label.Visibility = Visibility.Visible;
					Argument1Label.Content = arguments[1];
					Argument2Input.Visibility = Visibility.Visible;
					Argument3Label.Visibility = Visibility.Visible;
					Argument1Label.Content = arguments[2];
					Argument3Input.Visibility = Visibility.Visible;
					Argument4Label.Visibility = Visibility.Visible;
					Argument1Label.Content = arguments[3];
					Argument4Input.Visibility = Visibility.Visible;
					break;
				case 5:
					Argument1Label.Visibility = Visibility.Visible;
					Argument1Label.Content = arguments[0];
					Argument1Input.Visibility = Visibility.Visible;
					Argument2Label.Visibility = Visibility.Visible;
					Argument1Label.Content = arguments[1];
					Argument2Input.Visibility = Visibility.Visible;
					Argument3Label.Visibility = Visibility.Visible;
					Argument1Label.Content = arguments[2];
					Argument3Input.Visibility = Visibility.Visible;
					Argument4Label.Visibility = Visibility.Visible;
					Argument1Label.Content = arguments[3];
					Argument4Input.Visibility = Visibility.Visible;
					Argument5Label.Visibility = Visibility.Visible;
					Argument1Label.Content = arguments[4];
					Argument5Input.Visibility = Visibility.Visible;
					break;
				default:
					break;
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
