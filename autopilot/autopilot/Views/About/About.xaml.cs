using System.Windows;
using System.Diagnostics;


namespace autopilot
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class About : Window
	{
		public About()
		{
			InitializeComponent();
		}

		private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}
	}
}
