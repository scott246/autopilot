using autopilot.Utils;
using autopilot.Views.Dialogs;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace autopilot
{
    static class MainWindowUtils
	{
		public static string GetExtension(string fileName)
		{
			return "." + fileName.Split('.')[fileName.Split('.').Length - 1];
		}

        public static string GetFileNameWithNoMacroExtension(string fileName)
        {
            if (fileName.EndsWith(AppVariables.MACRO_EXTENSION))
                fileName = fileName.Substring(0, (fileName.Length - AppVariables.MACRO_EXTENSION.Length));
            return fileName;
        }

        public static string GetFileNameWithMacroExtension(string fileName)
        {
            if (!fileName.EndsWith(AppVariables.MACRO_EXTENSION))
                fileName += AppVariables.MACRO_EXTENSION;
            return fileName;
        }

        public static string GetFileNameWithMacroExtensionFromPath(string path)
        {
            if (!path.EndsWith(AppVariables.MACRO_EXTENSION))
                path += AppVariables.MACRO_EXTENSION;
            return path.Substring(path.LastIndexOf('\\') + 1);
        }

        public static string GetFileNameWithNoMacroExtensionFromPath(string path)
        {
            if (path.EndsWith(AppVariables.MACRO_EXTENSION))
                path = path.Substring(0, (path.Length - AppVariables.MACRO_EXTENSION.Length));
            return path.Substring(path.LastIndexOf('\\') + 1);
        }

        public static string GetParentFolderNameFromPath(string path)
        {
            return path.Substring(path.LastIndexOf('\\') + 1);
        }

        public static MacroFile GetMacroFileFromParentByTitle(MacroFile parent, string title)
        {
            foreach (MacroFile file in parent.Children)
            {
                if (file.Title == title) return file;
            }
            return null;
        }

        public static MacroFile GetMacroFileFromPath(string path)
        {
            // trim trailing '\\' character(s)
            while (path.ToCharArray()[path.Length - 1] == '\\')
            {
                path = path.Substring(0, path.Length - 2);
            }

            string[] pathElements = path.Split('\\');
            MacroFile latestFile = null;
            MacroFile parentOfLatestFile = AppVariables.MACRO_FILE_TREE_ROOT;
            foreach (string pathElement in pathElements)
            {
                MacroFile fileElement = GetMacroFileFromParentByTitle(parentOfLatestFile, pathElement);
                if (fileElement != null)
                {
                    latestFile = fileElement;
                }
            }
            return latestFile;
        }

        public static void DeleteMacroFile(string path)
        {
            AppVariables.MACRO_FILE_TREE.Remove(GetMacroFileFromPath(path));
        }

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
                if (AppVariables.MACRO_EXTENSION.Equals(GetExtension(item)))
                {
                    MacroFile file = MacroFileUtils.ReadMacroFile(item);
                    parentFile.Children.Add(file);
                }
            }
        }

        public static void ExpandAllMacroTreeElements(bool expand, MacroFile root)
        {
            /*foreach (MacroFile dir in root.Children)
            {
                //dir.IsExpanded()
                (MacroFile)MacroFolderTreeView.Items.GetItemAt(0);
                ExpandAllMacroTreeElements(expand, dir);
            }*/
        }

        public static void CreateMacro(MacroFile parent, string fullMacroPath)
        {
            string fullMacroName = GetFileNameWithMacroExtensionFromPath(fullMacroPath);
            MacroFile file = new MacroFile
            {
                Directory = false,
                Title = fullMacroName,
                Path = fullMacroPath,
                Enabled = true,
                Foreground = AppVariables.EnabledTreeColor,
                FontStyle = AppVariables.EnabledFontStyle,
                FontWeight = AppVariables.FileFontWeight
            };

            parent.Children.Add(file);

            MacroFileUtils.WriteMacroFile(file, true);
            MainWindow.LoadMacroFolderTree();
        }

        public static void CreateFolder(MacroFile parent, string fullFolderPath)
        {
            Directory.CreateDirectory(fullFolderPath);
            MacroFile folder = new MacroFile
            {
                Directory = true,
                Title = GetFileNameWithNoMacroExtensionFromPath(fullFolderPath),
                Path = fullFolderPath,
                FontWeight = AppVariables.FolderFontWeight,
                Foreground = AppVariables.EnabledTreeColor,
                FontStyle = AppVariables.EnabledFontStyle,
                Children = new MacroFileCollection()
            };

            parent.Children.Add(folder);

            MainWindow.LoadMacroFolderTree();
        }

        public static bool ConfirmDeleteMacro(MacroFile itemToDelete)
        {
            if (itemToDelete.Path == AppVariables.MACRO_DIRECTORY)
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
