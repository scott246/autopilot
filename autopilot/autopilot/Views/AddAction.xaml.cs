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
		public AddAction()
		{
			InitializeComponent();
			foreach (autopilot.Utils.Action action in Globals.ACTION_LIST)
			{
				ListBoxItem item = new ListBoxItem
				{
					Content = action.Name
				};
				ActionList.Items.Add(item);
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
			autopilot.Utils.Action selectedAction = Array.Find(Globals.ACTION_LIST.ToArray(), x => item.Content == x.Name);
			ActionDesc.Text = selectedAction.Description;
		}
	}
}
