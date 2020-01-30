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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BindFolderTreeViewLoaded(object sender, RoutedEventArgs e)
        {
            string bindDirectory = MainWindowUtils.bindDirectory;
            try
            {
                Directory.CreateDirectory(bindDirectory);
            } catch (Exception)
            {
                MessageBox.Show("Error creating bind directory.", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
            TreeViewItem item = new TreeViewItem
            {
                Header = bindDirectory,
                Tag = bindDirectory,
                FontWeight = FontWeights.Bold
            };
            bindFolderTreeView.Items.Add(item);
            MainWindowUtils.PopulateTreeView(item, bindDirectory);
            item.IsExpanded = true;
        }

        private void AboutMenuItemClicked(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }

        private void CollapseClicked(object sender, RoutedEventArgs e)
        {
            MainWindowUtils.ExpandAllBindTreeElements(false, (TreeViewItem)bindFolderTreeView.Items.GetItemAt(0));
        }

        private void ExpandClicked(object sender, RoutedEventArgs e)
        {
            MainWindowUtils.ExpandAllBindTreeElements(true, (TreeViewItem)bindFolderTreeView.Items.GetItemAt(0));
        }

        private void ToggleClicked(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)bindFolderTreeView.SelectedItem;
            selectedItem.SetActive(!selectedItem.IsActive());
        }

        private void AddBindButtonClicked(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)bindFolderTreeView.SelectedItem;
            if (null == selectedItem)
            {
                selectedItem = (TreeViewItem)bindFolderTreeView.Items.GetItemAt(0);
            }
            if (File.GetAttributes((string)selectedItem.Tag).HasFlag(FileAttributes.Directory))
            {
                MainWindowUtils.CreateBind(selectedItem);
            }
        }

        private void DeleteBindButtonClicked(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)bindFolderTreeView.SelectedItem;
            if (null != selectedItem && MainWindowUtils.ConfirmDeleteBind(selectedItem))
            {
                try
                {
                    File.Delete((string)selectedItem.Tag);
                    TreeViewItem parent = (TreeViewItem)selectedItem.Parent;
                    parent.Items.Remove(selectedItem);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Could not remove bind. Try running Autopilot as administrator.", "Insufficient Access", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not remove bind.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }
    }
}
