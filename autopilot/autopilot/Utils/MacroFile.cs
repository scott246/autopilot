using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace autopilot.Utils
{
	[Serializable]
	public class MacroFile
	{
		public string Title;
		public string Description;
		public bool Enabled;
		public string Bind; //the combination of keys/actions to trigger the event
		public string Code; //the code for the events that follow the bind
	}
}
