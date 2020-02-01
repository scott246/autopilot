using autopilot.Views.Dialogs;
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
        public static TreeView BindFolderTreeViewRef;

        public MainWindow()
        {
            InitializeComponent();
        }

        public static void LoadBindFolderTree()
        {
            BindFolderTreeViewRef.Items.Clear();
            string bindDirectory = MainWindowUtils.bindDirectory;
            try
            {
                Directory.CreateDirectory(bindDirectory);
            }
            catch (Exception)
            {
                CustomDialog.Display(CustomDialogType.OK, "Fatal Error", "Error creating bind directory.", null);
                Application.Current.Shutdown();
            }
            TreeViewItem item = new TreeViewItem
            {
                Header = bindDirectory.Substring(bindDirectory.LastIndexOf('\\') + 1),
                Tag = bindDirectory + '\\',
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.White)
            };
            BindFolderTreeViewRef.Items.Add(item);
            MainWindowUtils.PopulateTreeView(item, bindDirectory);
            item.IsExpanded = true;
        }

        private void BindFolderTreeViewLoaded(object sender, RoutedEventArgs e)
        {
            BindFolderTreeViewRef = BindFolderTreeView;
            LoadBindFolderTree();
        }

        private void AboutMenuItemClicked(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }

        private void CollapseClicked(object sender, RoutedEventArgs e)
        {
            MainWindowUtils.ExpandAllBindTreeElements(false, (TreeViewItem)BindFolderTreeView.Items.GetItemAt(0));
        }

        private void ExpandClicked(object sender, RoutedEventArgs e)
        {
            MainWindowUtils.ExpandAllBindTreeElements(true, (TreeViewItem)BindFolderTreeView.Items.GetItemAt(0));
        }

        private void ToggleClicked(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)BindFolderTreeView.SelectedItem;
            selectedItem.SetActive(!selectedItem.IsActive());
        }

        private void AddBindButtonClicked(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)BindFolderTreeView.SelectedItem;
            if (null == selectedItem)
                selectedItem = (TreeViewItem)BindFolderTreeView.Items.GetItemAt(0);
            if (File.GetAttributes((string)selectedItem.Tag).HasFlag(FileAttributes.Directory))
            {
                CustomDialogResponse response = CustomDialog.Display(CustomDialogType.OKCancel, "New Bind", "New bind name (no extension):", textboxContent: "untitled");
                if (response.ButtonResponse != CustomDialogButtonResponse.Cancel) 
                    MainWindowUtils.CreateBind(selectedItem, response.TextboxResponse);
            }
        }

        private void AddFolderButtonClicked(object sender, RoutedEventArgs e)
        {
            CustomDialogResponse response = CustomDialog.Display(CustomDialogType.OKCancel, "New Folder", "New folder name:", textboxContent: "untitled");
            if (response.ButtonResponse != CustomDialogButtonResponse.Cancel)
            {
                TreeViewItem selectedItem = (TreeViewItem)BindFolderTreeView.SelectedItem;
                if (null == selectedItem)
                    selectedItem = (TreeViewItem)BindFolderTreeView.Items.GetItemAt(0);
                MainWindowUtils.CreateFolder(selectedItem, response.TextboxResponse);
            }
        }

        private void DeleteBindButtonClicked(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)BindFolderTreeView.SelectedItem;
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
                    CustomDialog.Display(CustomDialogType.OK, "Insufficient Access", "Could not remove bind. Try running Autopilot as administrator.", null);
                }
                catch (Exception)
                {
                    CustomDialog.Display(CustomDialogType.OK, "Bind Delete Error", "Could not remove bind.", null);
                }
            }
        }
    }
}
