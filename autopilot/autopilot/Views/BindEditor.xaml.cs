using autopilot.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace autopilot.Views
{
	/// <summary>
	/// Interaction logic for BindEditor.xaml
	/// </summary>
	public partial class BindEditor : Window
	{
		public static List<Key> recordedKeys = new List<Key>();
		public static bool recording = false;

		public BindEditor()
		{
			InitializeComponent();
		}

		public static Bind Display()
		{
			BindEditor bindEditor = new BindEditor();
			bindEditor.ShowDialog();
			return (recordedKeys.Count > 0 ? new Bind(recordedKeys) : new Bind(null));
		}

		private void RecordButton_Click(object sender, RoutedEventArgs e)
		{
			recording = !recording;
			if (recording)
			{
				BindInputTextBox.Text = "";
				recordedKeys.Clear();
				RecordButton.Content = "Stop recording";
				RecordButton.Background = new SolidColorBrush(Colors.Red);
			}
			else
			{
				RecordButton.Content = "Re-record";
				RecordButton.Background = FindResource("AccentColor") as SolidColorBrush;
			}
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			recordedKeys.Clear();
			Close();
		}

		private void OKButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (recording)
			{
				recordedKeys.Add(e.Key);
				if (BindInputTextBox.Text == "")
				{
					BindInputTextBox.Text = e.Key.ToString();
				}
				else
				{
					BindInputTextBox.Text += " + " + e.Key.ToString();
				}
			}
		}
	}
}
