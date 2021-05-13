using autopilot.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	}
}
