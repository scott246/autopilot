using System.Windows;

namespace autopilot.Views.Dialogs
{
	public partial class CustomDialog : Window
	{
		private static CustomDialogButtonResponse buttonResponse = CustomDialogButtonResponse.None;
		private static bool checkboxChecked = false;
		private static string textboxResponse = null;
		private static CustomDialogType type;

		public CustomDialog()
		{
			InitializeComponent();
			Button1.Focus();
		}

		public static CustomDialogResponse Display(CustomDialogType cdt, string title, string dialogContent, string checkboxContent = null, string textboxContent = null)
		{
			CustomDialog dialog = new CustomDialog();
			type = cdt;
			dialog.Title = title;
			dialog.Message.Text = dialogContent;
			if (cdt == CustomDialogType.OK) InitControls(dialog, "OK", null, null, checkboxContent, textboxContent);
			else if (cdt == CustomDialogType.OKCancel) InitControls(dialog, "OK", "Cancel", null, checkboxContent, textboxContent);
			else if (cdt == CustomDialogType.YesNo) InitControls(dialog, "Yes", "No", null, checkboxContent, textboxContent);
			else if (cdt == CustomDialogType.YesNoCancel) InitControls(dialog, "Yes", "No", "Cancel", checkboxContent, textboxContent);
			else
			{
				type = CustomDialogType.OK;
				InitControls(dialog, "OK", null, null, null, null);
				dialog.Title = "Error";
				dialog.Message.Text = "An error occurred while initializing this dialog box.";
			}
			dialog.ShowDialog();
			return new CustomDialogResponse
			{
				ButtonResponse = buttonResponse,
				CheckboxResponse = checkboxChecked,
				TextboxResponse = textboxResponse
			};
		}

		private static void InitControls(CustomDialog dialog, string button1Content, string button2Content, string button3Content, string checkboxContent, string textboxContent)
		{
			dialog.Button1.Content = button1Content;
			if (button2Content == null) dialog.Button2.Visibility = Visibility.Hidden;
			else dialog.Button2.Content = button2Content;
			if (button3Content == null) dialog.Button3.Visibility = Visibility.Hidden;
			else dialog.Button3.Content = button3Content;
			if (checkboxContent == null) dialog.Checkbox.Visibility = Visibility.Hidden;
			else dialog.Checkbox.Content = checkboxContent;
			if (textboxContent == null) dialog.Textbox.Visibility = Visibility.Hidden;
			else dialog.Textbox.Text = textboxContent;
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
		}

		private void TextboxTextChanged(object sender, RoutedEventArgs e)
		{
			textboxResponse = Textbox.Text;
		}
	}

	public class CustomDialogResponse
	{
		public CustomDialogButtonResponse ButtonResponse { get; set; }
		public bool CheckboxResponse { get; set; }
		public string TextboxResponse { get; set; }
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
