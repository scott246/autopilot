using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Xml.Linq;

namespace autopilot.Utils
{
	[Serializable]
	public class Command
	{
		public Command(string name, List<string> arguments, string description)
		{
			Name = name;
			Arguments = arguments;
			Description = description;
		}
		public string Name { get; set; }
		public List<string> Arguments { get; set; }
		public string Description { get; set; }
	}

	public class CommandUtils
	{
		public static List<Command> GetAllActions()
		{
			List<Command> retrievedCommands = new List<Command>();
			List<string> arguments = new List<string>();
			string xmlCommandsPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Resources\\CommandsLibrary.xml";
			foreach (XElement command in XElement.Load(xmlCommandsPath).Elements("command"))
			{
				foreach (string argument in command.Elements("argument"))
				{
					arguments.Add(argument);
				}
				retrievedCommands.Add(new Command((string)command.Element("name"), arguments, (string)command.Element("description")));
			}
			return retrievedCommands;
		}

		public static void RefreshCommandList(ListBox list)
		{
			list.InvalidateArrange();
			list.UpdateLayout();
		}
	}
}
