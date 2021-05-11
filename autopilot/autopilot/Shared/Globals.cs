using autopilot.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace autopilot
{
	class Globals
	{
		public static readonly string VERSION_NUMBER = "v0.0.1";

		public static readonly string USER_DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		public static readonly string MACRO_DIRECTORY = USER_DIRECTORY + @"\autopilot\testmacros\";
		public static readonly string MACRO_EXTENSION = ".apc";

		public static ObservableCollection<MacroFile> MACRO_LIST = new ObservableCollection<MacroFile>();
		public static ObservableCollection<MacroFile> SORTED_FILTERED_MACRO_LIST = new ObservableCollection<MacroFile>();

		public static List<Command> COMMAND_LIST = new List<Command>();
		public static string[] commandCategoryOptions = { "All", "Files", "Programs", "System", "IO", "Networking", "Text", "Math", "Logic" };
		public static readonly string UNBOUND = "[unbound]";

		public static Mutex FILE_ACCESS_MUTEX = new Mutex();

	}
}
