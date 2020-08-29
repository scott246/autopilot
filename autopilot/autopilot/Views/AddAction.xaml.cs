using autopilot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace autopilot.Views
{
	/// <summary>
	/// Interaction logic for AddAction.xaml
	/// </summary>
	public partial class AddAction : Window
	{
		private string filterText = "";
		private string filterCategory = "All";

		public AddAction()
		{
			InitializeComponent();
			ActionCategoryFilterComboBox.ItemsSource = Globals.actionCategoryOptions;
			DisplayFilteredItems();
		}

		private void DisplayFilteredItems()
		{
			ActionList.Items.Clear();
			foreach (autopilot.Utils.Action action in Globals.ACTION_LIST)
			{
				if ((filterText.Equals("") || action.Name.Contains(filterText)) && 
					(filterCategory.Equals("All") || filterCategory.Equals(action.Category)))
				{
					ActionList.Items.Add(new ListBoxItem
					{
						Content = action.Name,
						Tag = action.Description
					});
				}
			}
			ActionDesc.Text = "";
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void Add_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ActionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListBoxItem item = (ListBoxItem)ActionList.SelectedItem;
			ActionDesc.Text = (string)item.Tag;
		}

		private void ActionFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			filterText = ActionFilterTextBox.Text;
			DisplayFilteredItems();
		}

		private void ActionCategoryFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			filterCategory = (string)ActionCategoryFilterComboBox.SelectedItem;
			DisplayFilteredItems();
		}
	}
}
