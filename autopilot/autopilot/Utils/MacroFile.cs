using System;
using System.Collections.ObjectModel;

namespace autopilot.Utils
{
	[Serializable]
	public class MacroFile
	{
		public string Title { get; set; }           // title of macro
		public string Description { get; set; }     // macro description
		public bool Enabled { get; set; }           // macro active status
		public string Bind { get; set; }            // the combination of keys/actions to trigger the event
		public string Code { get; set; }            // the code for the events that follow the bind
	}
}
