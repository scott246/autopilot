using autopilot.Utils;
using autopilot.Views.Dialogs;
using System;
using System.IO;
using System.Windows;
using static autopilot.AppVariables;

namespace autopilot
{
    static class EditorUtils
    {
        public static void LoadMacros(string filterText)
        {
            MACRO_LIST.Clear();
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
                if (filterText == "" || MacroFileUtils.GetFileNameWithNoMacroExtension(item).Contains(filterText))
                {
                    MacroFile file = MacroFileUtils.ReadMacroFile(item);
                    MACRO_LIST.Add(file);
                }
            }
        }

        public static bool ConfirmDeleteMacro(MacroFile itemToDelete)
        {
            CustomDialogResponse confirmResult;
            if (Properties.Settings.Default.WarnOnFileDelete == false) return true;
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
    }
}
