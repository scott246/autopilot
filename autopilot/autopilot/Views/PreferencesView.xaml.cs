using System.Windows;

namespace autopilot.Views
{
	public partial class PreferencesView : Window
	{
		private static readonly Properties.Settings settingsRef = Properties.Settings.Default;

		public PreferencesView()
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
