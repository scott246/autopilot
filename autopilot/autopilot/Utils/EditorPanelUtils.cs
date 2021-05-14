using autopilot.Views;
using System.Collections.Generic;
using System.Windows.Controls;

namespace autopilot.Utils
{
	class EditorPanelUtils
	{
		public static bool ConfirmDeleteCommand(ListBoxItem itemToDelete)
		{
			CustomDialogResponse confirmResult;
			if (Properties.Settings.Default.WarnOnCommandDelete == false)
			{
				return true;
			}

			confirmResult = CustomDialog.Display(CustomDialogType.YesNo, "Warning", "Are you sure you want to delete '" + itemToDelete.Content.ToString() + "'? This can't be undone.", "Do not show again");
			if (confirmResult.CheckboxResponse == true)
			{
				Properties.Settings.Default.WarnOnCommandDelete = false;
				Properties.Settings.Default.Save();
			}
			if (confirmResult.ButtonResponse == CustomDialogButtonResponse.Yes)
			{
				return true;
			}
			return false;
		}

		public static string ConvertCommandArgumentsToString(Command c)
		{
			string argumentString = "";
			if (c.Arguments != null)
			{
				foreach (KeyValuePair<string, string> kvArg in c.Arguments)
				{
					argumentString += " | " +  kvArg.Key + ": " + kvArg.Value;
				}
			}
			return argumentString;
		}

		public static ListBoxItem ConvertCommandToListBoxItem(Command c, bool showArgumentsInContent = true)
		{
			string content = c.Title;
			if (showArgumentsInContent)
			{
				content += ConvertCommandArgumentsToString(c);
			}
			return new ListBoxItem
			{
				Content = content,
				Tag = c
			};
		}

		public static Command ConvertListBoxItemToCommand(ListBoxItem l)
		{
			return (Command)l.Tag;
		}
	}
}
