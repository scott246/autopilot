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
