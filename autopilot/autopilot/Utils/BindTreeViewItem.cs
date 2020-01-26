using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
				t.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
				t.FontStyle = FontStyles.Normal;
			}
			else
			{
				t.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
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
