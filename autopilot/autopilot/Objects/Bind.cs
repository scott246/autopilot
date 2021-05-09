using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace autopilot.Objects
{
	public class Bind
	{
		public Bind(List<Key> keys)
		{
			Keys = keys;
		}
		public List<Key> Keys { get; set; }
	}

	public class BindUtils
	{
		public static string ConvertBindToString(Bind bind)
		{
			string bindString = "";
			if (bind == null || bind.Keys == null)
			{
				return Globals.UNBOUND;
			}
			foreach (Key key in bind.Keys)
			{
				if (bindString == "")
				{
					bindString = key.ToString();
				}
				else
				{
					bindString += " + " + key.ToString();
				}
			}
			return bindString;
		}
	}
}
