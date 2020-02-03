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
		public static readonly string MACRO_DIRECTORY = @"C:\Users\ndsco\AppData\Local\autopilot\testmacros\";
		public static readonly string MACRO_EXTENSION = ".apscr";
		public static MacroFileCollection MACRO_FILE_TREE = new MacroFileCollection();
		public static MacroFile MACRO_FILE_TREE_ROOT;

		public static Color EnabledTreeColor = Color.FromRgb(204, 204, 204);
		public static FontStyle EnabledFontStyle = FontStyles.Normal;
		public static Color DisabledTreeColor = Color.FromRgb(255, 0, 0);
		public static FontStyle DisabledFontStyle = FontStyles.Italic;

		public static FontWeight FolderFontWeight = FontWeights.Bold;
		public static FontWeight FileFontWeight = FontWeights.Normal;
	}
}
