using autopilot.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace autopilot
{
	class AppVariables
	{
		//TODO: change to something more permanent
		public static readonly string USER_DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		public static readonly string MACRO_DIRECTORY = USER_DIRECTORY + @"\autopilot\testmacros\";
		public static readonly string MACRO_EXTENSION = ".apscr";
		public static ObservableCollection<MacroFile> MACRO_LIST = new ObservableCollection<MacroFile>();

		public static Mutex FILE_ACCESS_MUTEX = new Mutex();

	}
}
