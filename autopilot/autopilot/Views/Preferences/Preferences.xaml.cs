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
		}

		private void WarnFileDeleteCheckbox_Checked(object sender, RoutedEventArgs e)
		{
			settingsRef.WarnOnFileDelete = true;
		}

		private void WarnFileDeleteCheckbox_Unchecked(object sender, RoutedEventArgs e)
		{
			settingsRef.WarnOnFileDelete = false;
		}

		private void ApplyButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
