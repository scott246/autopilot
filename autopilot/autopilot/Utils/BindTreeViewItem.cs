using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace autopilot
{
	static class BindTreeViewItem
	{
		private static bool Active = true;

		public static bool IsActive(this TreeViewItem _)
		{
			return Active;
		}

		public static void SetActive(this TreeViewItem t, bool active)
		{
			if (active)
			{
				t.Foreground = new SolidColorBrush(Colors.LightGray);
				t.FontStyle = FontStyles.Normal;
			}
			else
			{
				t.Foreground = new SolidColorBrush(Colors.Red);
				t.FontStyle = FontStyles.Italic;
			}

			foreach (TreeViewItem item in t.Items) 
			{
				item.SetActive(active);
			}
			Active = active;
		}
	}
}
