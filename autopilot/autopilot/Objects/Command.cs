﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Xml.Linq;

namespace autopilot.Utils
{
	[Serializable]
	public class Command
	{
		//TODO: make commands actually do stuff
		public Command(string title, List<string> arguments, string description)
		{
			Title = title;
			Arguments = arguments;
			Description = description;
		}
		public string Title { get; set; }
		public List<string> Arguments { get; set; }
		public string Description { get; set; }
	}

	public class CommandUtils
	{
		public static List<Command> GetAllActions()
		{
			List<Command> retrievedCommands = new List<Command>();
			string xmlCommandsPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Resources\\CommandsLibrary.xml";
			foreach (XElement command in XElement.Load(xmlCommandsPath).Elements("command"))
			{
				List<string> arguments = new List<string>();
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
