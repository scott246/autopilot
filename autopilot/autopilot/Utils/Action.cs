using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace autopilot.Utils
{
	[Serializable]
	public class Action
	{
		public Action(string name, string description, string category)
		{
			Name = name;
			Description = description;
			Category = category;
		}
		public string Name { get; set; }                    // title of action
		public string Description { get; set; }             // action description
		public string Category { get; set; }				// type of action
	}

	public class ActionUtils
	{
		public static List<Action> GetAllActions()
		{
			List<Action> retrievedActions = new List<Action>();
			string xmlActionsPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Resources\\ActionsLibrary.xml";
			foreach (XElement action in XElement.Load(xmlActionsPath).Elements("action"))
			{
				retrievedActions.Add(new Action((string)action.Element("name"), (string)action.Element("description"), (string)action.Element("category")));
			}
			return retrievedActions;
		}
	}
}
