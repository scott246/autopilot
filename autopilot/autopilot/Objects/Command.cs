using System;
using System.Collections.Generic;

namespace autopilot.Utils
{
	[Serializable]
	public class Command
	{
		//TODO: make commands actually do stuff
		public Command(string title, List<KeyValuePair<string, string>> arguments, string description)
		{
			Title = title;
			Arguments = arguments;
			Description = description;
		}
		public string Title { get; set; }
		public List<KeyValuePair<string, string>> Arguments { get; set; }
		public string Description { get; set; }
	}
}
