using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace autopilot.Utils
{
	[Serializable]
	public class MacroFile
	{
		public bool Directory { get; set; }			// is file a directory
		public string Title { get; set; }			// title of macro
		public string Description { get; set; }		// macro description
		public bool Enabled { get; set; }			// macro active status
		public string Bind { get; set; }			// the combination of keys/actions to trigger the event
		public string Code { get; set; }			// the code for the events that follow the bind
		public string Path { get; set; }			// location on disk
		public MacroFileCollection Children { get; set; }
	}

	[Serializable]
	public class MacroFileCollection : ObservableCollection<MacroFile>
	{

	}
}
