using autopilot.Properties;
using autopilot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace autopilot
{
	class AppVariables
	{
		//TODO: change to something more permanent
		public static readonly string USER_DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		public static readonly string MACRO_DIRECTORY = @"C:\Users\ndsco\AppData\Local\autopilot\testmacros\";
		public static readonly string MACRO_EXTENSION = ".apscr";
		public static MacroFileCollection MACRO_FILE_TREE = new MacroFileCollection();
		public static MacroFile MACRO_FILE_TREE_ROOT;

		public static Color BACKGROUND_COLOR_1 = Color.FromRgb(51, 51, 51);
		public static Color BACKGROUND_COLOR_2 = Color.FromRgb(102, 102, 102);
		public static Color FOREGROUND_COLOR_1 = Color.FromRgb(255, 255, 255);
		public static Color FOREGROUND_COLOR_2 = Color.FromRgb(204, 204, 204);
		public static Color WARNING_COLOR = Color.FromRgb(255, 0, 0);
		public static Color ACCENT_COLOR = Color.FromRgb(255, 165, 0);
		public static SolidColorBrush ENABLED_TREE_COLOR = new SolidColorBrush(FOREGROUND_COLOR_2);
		public static SolidColorBrush DISABLED_TREE_COLOR = new SolidColorBrush(WARNING_COLOR);
        public static FontStyle ENABLED_FONT_STYLE = FontStyles.Normal;
        public static FontStyle DISABLED_FONT_STYLE = FontStyles.Italic;
        public static FontWeight FOLDER_FONT_WEIGHT = FontWeights.Bold;
        public static FontWeight FILE_FONT_WEIGHT = FontWeights.Normal;

	}
}
