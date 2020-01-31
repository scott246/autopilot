using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace autopilot
{
	static class MainWindowUtils
	{
		//TODO: change to something more permanent
		public static readonly string bindDirectory = @"D:\Code\autopilot\testbinds\";
        public static readonly string bindExtension = ".ap1";

		public static string GetExtension(string fileName)
		{
			return "." + fileName.Split('.')[fileName.Split('.').Length - 1];
		}

        public static void PopulateTreeView(TreeViewItem parentItem, string path)
        {
            foreach (string dir in Directory.EnumerateDirectories(path))
            {
                TreeViewItem item = new TreeViewItem
                {
                    Header = dir.Substring(dir.LastIndexOf('\\') + 1),
                    Tag = dir + @"\",
                    FontWeight = FontWeights.Bold
                };

                parentItem.Items.Add(item);

                PopulateTreeView(item, dir);
            }

            foreach (string file in Directory.EnumerateFiles(path))
            {
                if (bindExtension.Equals(GetExtension(file)))
                {
                    TreeViewItem item = new TreeViewItem
                    {
                        Header = file.Substring(file.LastIndexOf('\\') + 1),
                        Tag = file,
                        FontWeight = FontWeights.Normal
                    };

                    parentItem.Items.Add(item);
                }
            }
        }

        public static void ExpandAllBindTreeElements(Boolean expand, TreeViewItem root)
        {
            foreach (TreeViewItem dir in root.Items)
            {
                dir.IsExpanded = expand;
                ExpandAllBindTreeElements(expand, dir);
            }
        }

        public static void CreateBind(TreeViewItem parent)
        {
            string newBindName = "untitled" + bindExtension;

            TreeViewItem newBind = new TreeViewItem
            {
                Header = newBindName,
                Tag = parent.Tag + newBindName,
                FontWeight = FontWeights.Normal
            };
            parent.Items.Add(newBind);
            Console.WriteLine(parent.Tag + newBindName);
            File.Create(parent.Tag + newBindName);
        }

        public static bool ConfirmDeleteBind(TreeViewItem itemToDelete)
        {
            if ((string)itemToDelete.Tag == bindDirectory)
            {
                return false;
            }

            MessageBoxResult confirmResult;
            if (itemToDelete.HasItems)
            {
                confirmResult = MessageBox.Show("Are you sure you want to delete the entire '" + itemToDelete.Header + "' folder? This cannot be undone.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            }
            else
            {
                confirmResult = MessageBox.Show("Are you sure you want to delete '" + itemToDelete.Header + "'? This cannot be undone.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            }
            if (confirmResult == MessageBoxResult.Yes)
            {
                return true;
            }
            return false;
        }
    }
}
