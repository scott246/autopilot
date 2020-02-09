﻿using autopilot.Utils;
using System;
using System.Threading;

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

		public static Mutex FILE_ACCESS_MUTEX = new Mutex();

	}
}
