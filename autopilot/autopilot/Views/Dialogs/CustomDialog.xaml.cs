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

namespace autopilot.Views.Dialogs
{
	public partial class CustomDialog : Window
	{
		private CustomDialogButtonResponse buttonResponse = CustomDialogButtonResponse.None;
		private bool checkboxChecked = false;
		private readonly CustomDialogType type;

		public CustomDialog(CustomDialogType cdt, string title, string dialogContent, string checkboxContent)
		{
			InitializeComponent();
			type = cdt;
			Title = title;
			Message.Content = dialogContent;
			if (cdt == CustomDialogType.OK) InitControls("OK", null, null, checkboxContent);
			else if (cdt == CustomDialogType.OKCancel) InitControls("OK", "Cancel", null, checkboxContent);
			else if (cdt == CustomDialogType.YesNo) InitControls("Yes", "No", null, checkboxContent);
			else if (cdt == CustomDialogType.YesNoCancel) InitControls("Yes", "No", "Cancel", checkboxContent);
			else
			{
				type = CustomDialogType.OK;
				InitControls("OK", null, null, null);
				Title = "Error";
				Message.Content = "An error occurred while initializing this dialog box.";
			}
		}

		public CustomDialogResponse DisplayCustomDialog()
		{
			ShowDialog();
			return new CustomDialogResponse
			{
				ButtonResponse = buttonResponse,
				CheckboxResponse = checkboxChecked
			};
		}

		private void InitControls(string button1Content, string button2Content, string button3Content, string checkboxContent)
		{
			Button1.Content = button1Content;
			if (button2Content == null) Button2.Visibility = Visibility.Hidden;
			else Button2.Content = button2Content;
			if (button3Content == null) Button3.Visibility = Visibility.Hidden;
			else Button3.Content = button3Content;
			if (checkboxContent == null) Checkbox.Visibility = Visibility.Hidden;
			else Checkbox.Content = checkboxContent;
		}

		public CustomDialogButtonResponse GetDialogResponse()
		{
			return buttonResponse;
		}

		public bool GetCheckboxState()
		{
			return checkboxChecked;
		}

		private void Button1Clicked(object sender, RoutedEventArgs e)
		{
			if (type == CustomDialogType.OK || type == CustomDialogType.OKCancel)
				buttonResponse = CustomDialogButtonResponse.OK;
			else if (type == CustomDialogType.YesNo || type == CustomDialogType.YesNoCancel)
				buttonResponse = CustomDialogButtonResponse.Yes;
			Close();
		}

		private void Button2Clicked(object sender, RoutedEventArgs e)
		{
			if (type == CustomDialogType.OKCancel)
				buttonResponse = CustomDialogButtonResponse.Cancel;
			else if (type == CustomDialogType.YesNo || type == CustomDialogType.YesNoCancel)
				buttonResponse = CustomDialogButtonResponse.No;
			Close();
		}

		private void Button3Clicked(object sender, RoutedEventArgs e)
		{
			if (type == CustomDialogType.YesNoCancel)
				buttonResponse = CustomDialogButtonResponse.Cancel;
			Close();
		}

		private void CheckboxClicked(object sender, RoutedEventArgs e)
		{
			checkboxChecked = (bool)Checkbox.IsChecked;
			Close();
		}

	}

	public class CustomDialogResponse
	{
		public CustomDialogButtonResponse ButtonResponse { get; set; }
		public bool CheckboxResponse { get; set; }
	}

	public enum CustomDialogButtonResponse : int
	{
		None = -1,
		Cancel = 0,
		OK = 1,
		No = 2,
		Yes = 3
	}

	public enum CustomDialogType : int
	{
		OK = 0,
		OKCancel = 1,
		YesNo = 2,
		YesNoCancel = 3
	}
}
