using autopilot.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Documents;

namespace autopilot
{
	class Globals
	{
		public static readonly string USER_DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		public static readonly string MACRO_DIRECTORY = USER_DIRECTORY + @"\autopilot\testmacros\";
		public static readonly string MACRO_EXTENSION = ".apc";

		public static ObservableCollection<MacroFile> MACRO_LIST = new ObservableCollection<MacroFile>();
		public static ObservableCollection<MacroFile> SORTED_FILTERED_MACRO_LIST = new ObservableCollection<MacroFile>();

		public static List<autopilot.Utils.Action> ACTION_LIST = new List<autopilot.Utils.Action>();

		public static Mutex FILE_ACCESS_MUTEX = new Mutex();

	}
}
