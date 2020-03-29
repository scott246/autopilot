﻿using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace autopilot.Utils
{
	[Serializable]
	public class MacroFile
	{
		public string Title { get; set; }					// title of macro
		public string Description { get; set; }				// macro description
		public bool Enabled { get; set; }					// macro active status
		public string Bind { get; set; }					// the combination of keys/actions to trigger the event
		public ItemCollection Commands { get; set; }        // the code for the events that follow the bind
		public DateTime CreatedDateTime { get; set; }		// the macro creation time
		public DateTime LastModifiedDateTime { get; set; }	// the time the macro was last modified
	}
}
