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

namespace autopilot.Views.Preferences
{
	public partial class Preferences : Window
	{
		private static Properties.Settings settingsRef = Properties.Settings.Default;

		public Preferences()
		{
			InitializeComponent();
			WarnFileDeleteCheckbox.IsChecked = settingsRef.WarnOnFileDelete;
			WarnFolderDeleteCheckbox.IsChecked = settingsRef.WarnOnFolderDelete;
		}

		private void ApplyButtonClicked(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void WarnFileDeleteCheckbox_Checked(object sender, RoutedEventArgs e)
		{
			settingsRef.WarnOnFileDelete = true;
		}

		private void WarnFileDeleteCheckbox_Unchecked(object sender, RoutedEventArgs e)
		{
			settingsRef.WarnOnFileDelete = false;
		}

		private void WarnFolderDeleteCheckbox_Checked(object sender, RoutedEventArgs e)
		{
			settingsRef.WarnOnFolderDelete = true;
		}

		private void WarnFolderDeleteCheckbox_Unchecked(object sender, RoutedEventArgs e)
		{
			settingsRef.WarnOnFolderDelete = false;
		}
	}
}
