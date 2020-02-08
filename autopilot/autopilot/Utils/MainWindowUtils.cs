using autopilot.Utils;
using autopilot.Views.Dialogs;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static autopilot.AppVariables;

namespace autopilot
{
    static class MainWindowUtils
	{
        public static void PopulateTreeView(MacroFile parentFile, string path)
        {
            foreach (string dir in Directory.EnumerateDirectories(path))
            {
                MacroFile file = MacroFileUtils.ReadMacroFile(dir);
                parentFile.Children.Add(file);

                PopulateTreeView(file, dir);
            }

            foreach (string item in Directory.EnumerateFiles(path))
            {
                if (MACRO_EXTENSION.Equals(MacroFileUtils.GetExtension(item)))
                {
                    MacroFile file = MacroFileUtils.ReadMacroFile(item);
                    parentFile.Children.Add(file);
                }
            }
        }

        public static void ExpandAllMacroTreeElements(bool expand, TreeViewItem root)
        {
            foreach (TreeViewItem dir in root.Items)
            {
                dir.IsExpanded = expand;
                ExpandAllMacroTreeElements(expand, dir);
            }
        }

        public static bool ConfirmDeleteMacro(MacroFile itemToDelete)
        {
            if (itemToDelete.Path == MACRO_DIRECTORY)
            {
                return false;
            }

            CustomDialogResponse confirmResult;
            if (!itemToDelete.Children.Equals(null))
            {
                if (Properties.Settings.Default.WarnOnFolderDelete == false) 
                    return true;
                confirmResult = CustomDialog.Display(CustomDialogType.YesNo, "Warning", "Are you sure you want to delete the entire '" + itemToDelete.Title + "' folder? This cannot be undone.", "Do not show again");
                if (confirmResult.CheckboxResponse == true)
                {
                    Properties.Settings.Default.WarnOnFolderDelete = false;
                    Properties.Settings.Default.Save();
                }
            }
            else
            {
                if (Properties.Settings.Default.WarnOnFileDelete == false) 
                    return true;
                confirmResult = CustomDialog.Display(CustomDialogType.YesNo, "Warning", "Are you sure you want to delete '" + itemToDelete.Title + "'? This cannot be undone.", "Do not show again");
                if (confirmResult.CheckboxResponse == true)
                {
                    Properties.Settings.Default.WarnOnFileDelete = false;
                    Properties.Settings.Default.Save();
                }
            }
            if (confirmResult.ButtonResponse == CustomDialogButtonResponse.Yes)
            {
                return true;
            }
            return false;
        }
    }
}
