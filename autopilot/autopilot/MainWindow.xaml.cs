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
        private readonly string bindExtension = ".ap1";

        public MainWindow()
        {
            InitializeComponent();
        }

        private string GetExtension(string fileName)
        {
            return "." + fileName.Split('.')[fileName.Split('.').Length - 1];
        }

        private void PopulateTreeView(TreeViewItem parentItem, string path)
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

        private void BindFolderTreeViewLoaded(object sender, RoutedEventArgs e)
        {
            Directory.CreateDirectory(bindDirectory);
            TreeViewItem item = new TreeViewItem
            {
                Header = bindDirectory.Substring(bindDirectory.LastIndexOf('\\') + 1),
                Tag = bindDirectory,
                FontWeight = FontWeights.Bold
            };
            bindFolderTreeView.Items.Add(item);
            PopulateTreeView(item, bindDirectory);
            item.IsExpanded = true;
        }
	}
}
