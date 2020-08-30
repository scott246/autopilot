using autopilot.Utils;
using autopilot.Views.Dialogs;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using static autopilot.Globals;

namespace autopilot
{
	static class EditorUtils
	{
		public static void LoadMacros()
		{
			MACRO_LIST.Clear();
			SORTED_FILTERED_MACRO_LIST.Clear();
			try
			{
				Directory.CreateDirectory(MACRO_DIRECTORY);
			}
			catch (Exception)
			{
				CustomDialog.Display(CustomDialogType.OK, "Fatal Error", "Error creating macro directory.");
				Application.Current.Shutdown();
			}
			foreach (string item in Directory.EnumerateFiles(MACRO_DIRECTORY))
			{
				MacroFile file = MacroFileUtils.ReadMacroFile(item);
				MACRO_LIST.Add(file);
				SORTED_FILTERED_MACRO_LIST.Add(file);
			}
		}

		public static void RefreshMacroList(ListBox list, int sortAlgo, string filterText)
		{
			MACRO_LIST.Clear();
			SORTED_FILTERED_MACRO_LIST.Clear();
			foreach (string item in Directory.EnumerateFiles(MACRO_DIRECTORY))
			{
				MacroFile file = MacroFileUtils.ReadMacroFile(item);
				MACRO_LIST.Add(file);
				SORTED_FILTERED_MACRO_LIST.Add(file);
			}
			SortFilterUtils.SortFilterMacroList(sortAlgo, filterText);
			list.InvalidateArrange();
			list.UpdateLayout();
		}

		public static bool ConfirmDeleteMacro(MacroFile itemToDelete)
		{
			CustomDialogResponse confirmResult;
			if (Properties.Settings.Default.WarnOnFileDelete == false)
			{
				return true;
			}

			confirmResult = CustomDialog.Display(CustomDialogType.YesNo, "Warning", "Are you sure you want to delete the '" + itemToDelete.Title + "' file? This cannot be undone.", "Do not show again");
			if (confirmResult.CheckboxResponse == true)
			{
				Properties.Settings.Default.WarnOnFileDelete = false;
				Properties.Settings.Default.Save();
			}
			if (confirmResult.ButtonResponse == CustomDialogButtonResponse.Yes)
			{
				return true;
			}
			return false;
		}

		public static void RefreshCommandList(ListBox list)
		{
			list.InvalidateArrange();
			list.UpdateLayout();
		}
	}
}
