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

        public static string TrimAutopilotMacroExtension(string fileName)
        {
            if (fileName.EndsWith(AppVariables.macroExtension))
                fileName = fileName.Substring(0, (fileName.Length - AppVariables.macroExtension.Length));
            return fileName;
        }

        public static void PopulateTreeView(TreeViewItem parentItem, string path)
        {
            foreach (string dir in Directory.EnumerateDirectories(path))
            {
                TreeViewItem item = new TreeViewItem
                {
                    Header = dir.Substring(dir.LastIndexOf('\\') + 1),
                    Tag = dir + @"\",
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Colors.White)
                };

                parentItem.Items.Add(item);

                PopulateTreeView(item, dir);
            }

            foreach (string file in Directory.EnumerateFiles(path))
            {
                if (AppVariables.macroExtension.Equals(GetExtension(file)))
                {
                    TreeViewItem item = new TreeViewItem
                    {
                        Header = file.Substring(file.LastIndexOf('\\') + 1),
                        Tag = file,
                        FontWeight = FontWeights.Normal,
                        Foreground = new SolidColorBrush(Colors.LightGray)
                    };

                    parentItem.Items.Add(item);
                }
            }
        }

        public static void ExpandAllMacroTreeElements(Boolean expand, TreeViewItem root)
        {
            foreach (TreeViewItem dir in root.Items)
            {
                dir.IsExpanded = expand;
                ExpandAllMacroTreeElements(expand, dir);
            }
        }

        public static void CreateMacro(TreeViewItem parent, string name)
        {
            string fullMacroName = name + AppVariables.macroExtension;

            TreeViewItem newMacro = new TreeViewItem
            {
                Header = fullMacroName,
                Tag = parent.Tag + fullMacroName,
                FontWeight = FontWeights.Normal,
                Foreground = new SolidColorBrush(Colors.LightGray)
            };
            parent.Items.Add(newMacro);
            string fileName = parent.Tag + fullMacroName;
            MacroFile macroFile = MacroFileUtils.CreateMacroFileItem(title: name, enabled: true);
            MacroFileUtils.WriteMacroFile(fileName, macroFile, true);
            MainWindow.RefreshMacroFolderTree();
        }

        public static void CreateFolder(TreeViewItem parent, string name)
        {
            TreeViewItem newFolder = new TreeViewItem
            {
                Header = name,
                Tag = parent.Tag + @"\" + name + @"\",
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.LightGray)
            };
            parent.Items.Add(newFolder);
            Console.WriteLine(parent.Tag + name);
            Directory.CreateDirectory(parent.Tag + name);
            MainWindow.RefreshMacroFolderTree();
        }

        public static bool ConfirmDeleteMacro(TreeViewItem itemToDelete)
        {
            if ((string)itemToDelete.Tag == AppVariables.macroDirectory)
            {
                return false;
            }

            CustomDialogResponse confirmResult;
            if (itemToDelete.HasItems)
            {
                if (Properties.Settings.Default.WarnOnFolderDelete == false) 
                    return true;
                confirmResult = CustomDialog.Display(CustomDialogType.YesNo, "Warning", "Are you sure you want to delete the entire '" + itemToDelete.Header + "' folder? This cannot be undone.", "Do not show again");
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
                confirmResult = CustomDialog.Display(CustomDialogType.YesNo, "Warning", "Are you sure you want to delete '" + itemToDelete.Header + "'? This cannot be undone.", "Do not show again");
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
