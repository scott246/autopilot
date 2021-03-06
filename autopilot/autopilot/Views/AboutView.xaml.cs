﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace autopilot.Views
{
	public partial class AboutView : Window
	{
		public AboutView()
		{
			InitializeComponent();
			VersionLabel.Content = Globals.VERSION_NUMBER;
			PreviewKeyDown += new KeyEventHandler(HandleEsc);
		}

		private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}

		private void HandleEsc(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				Close();
			}
		}
	}
}
