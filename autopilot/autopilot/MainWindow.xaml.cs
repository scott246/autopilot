using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace autopilot
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        private readonly string bindDirectory = @"E:\Code\autopilot\testbinds";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddDirectoryItemsToTreeView(TreeViewItem parentItem, string path)
        {
            foreach (string dir in Directory.EnumerateDirectories(path))
            {
                TreeViewItem item = new TreeViewItem
                {
                    Header = dir.Substring(dir.LastIndexOf('\\') + 1),
                    Tag = dir,
                    FontWeight = FontWeights.Bold
                };

                parentItem.Items.Add(item);

                AddDirectoryItemsToTreeView(item, dir);
            }

            foreach (string file in Directory.EnumerateFiles(path))
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

        private void BindFolderTreeViewLoaded(object sender, RoutedEventArgs e)
        {
            Directory.CreateDirectory(bindDirectory);

            foreach (string dir in Directory.EnumerateDirectories(bindDirectory))
            {
                TreeViewItem item = new TreeViewItem
                {
                    Header = dir.Substring(dir.LastIndexOf('\\') + 1),
                    Tag = dir,
                    FontWeight = FontWeights.Bold
                };

                bindFolderTreeView.Items.Add(item);
                
                AddDirectoryItemsToTreeView(item, dir);
            }

            foreach (string file in Directory.EnumerateFiles(bindDirectory))
            {
                TreeViewItem item = new TreeViewItem
                {
                    Header = file.Substring(file.LastIndexOf('\\') + 1),
                    Tag = file,
                    FontWeight = FontWeights.Normal
                };

                bindFolderTreeView.Items.Add(item);
            }
        }
	}
}
