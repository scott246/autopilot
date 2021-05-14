using autopilot.Objects;
using System.Windows.Input;

namespace autopilot.Utils
{
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
