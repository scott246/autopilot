using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace autopilot.Utils
{
	[Serializable]
	public class Command
	{
		public Command(string name, string description, string category)
		{
			Name = name;
			Description = description;
			Category = category;
		}
		public string Name { get; set; }                    // title of action
		public string Description { get; set; }             // action description
		public string Category { get; set; }                // type of action
	}

	public class CommandUtils
	{
		public static List<Command> GetAllActions()
		{
			List<Command> retrievedCommands = new List<Command>();
			string xmlCommandsPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Resources\\CommandsLibrary.xml";
			foreach (XElement command in XElement.Load(xmlCommandsPath).Elements("command"))
			{
				retrievedCommands.Add(new Command((string)command.Element("name"), (string)command.Element("description"), (string)command.Element("category")));
			}
			return retrievedCommands;
		}
	}
}
