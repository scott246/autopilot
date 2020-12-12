﻿using autopilot.Utils;
using System.Windows;
using System.Windows.Controls;

namespace autopilot.Views
{
	/// <summary>
	/// Interaction logic for AddStep.xaml
	/// </summary>
	public partial class AddCommand : Window
	{
		private string filterText = "";

		private ListBoxItem selectedItem = null;

		public AddCommand()
		{
			InitializeComponent();
			DisplayFilteredItems();
		}

		public static Command Display()
		{
			AddCommand a = new AddCommand();
			a.ShowDialog();
			return (null == a.selectedItem || null == a.selectedItem.Tag) ? null : (Command)a.selectedItem.Tag;
		}

		private void DisplayFilteredItems()
		{
			CommandList.Items.Clear();
			foreach (Command command in Globals.COMMAND_LIST)
			{
				if (filterText.Equals("") || command.Name.Contains(filterText))
				{
					CommandList.Items.Add(new ListBoxItem
					{
						Content = command.Name,
						Tag = command
					});
				}
			}
			CommandDescription.Text = "";
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			selectedItem = null;
			Close();
		}

		private void Add_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void CommandList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			selectedItem = (ListBoxItem)CommandList.SelectedItem;
			CommandDescription.Text = ((Command)selectedItem.Tag).Description;
		}

		private void CommandFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			filterText = CommandFilterTextBox.Text;
			DisplayFilteredItems();
		}
	}
}
