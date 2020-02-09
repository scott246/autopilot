using System.Windows;

namespace autopilot.Views.Preferences
{
	public partial class Preferences : Window
	{
		private static readonly Properties.Settings settingsRef = Properties.Settings.Default;

		public Preferences()
		{
			InitializeComponent();
			WarnFileDeleteCheckbox.IsChecked = settingsRef.WarnOnFileDelete;
			WarnFolderDeleteCheckbox.IsChecked = settingsRef.WarnOnFolderDelete;
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

		private void ApplyButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
