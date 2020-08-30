using autopilot.Utils;
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
		private string filterCategory = "All";

		private static ListBoxItem selectedItem = null;

		public AddCommand()
		{
			InitializeComponent();
			CommandCategoryFilterComboBox.ItemsSource = Globals.commandCategoryOptions;
			DisplayFilteredItems();
		}

		public static Command Display()
		{
			AddCommand a = new AddCommand();
			a.ShowDialog();
			return selectedItem.Tag == null ? null : (Command)selectedItem.Tag;
		}

		private void DisplayFilteredItems()
		{
			CommandList.Items.Clear();
			foreach (Command command in Globals.COMMAND_LIST)
			{
				if ((filterText.Equals("") || command.Name.Contains(filterText)) &&
					(filterCategory.Equals("All") || filterCategory.Equals(command.Category)))
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

		private void CommandCategoryFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			filterCategory = (string)CommandCategoryFilterComboBox.SelectedItem;
			DisplayFilteredItems();
		}
	}
}
