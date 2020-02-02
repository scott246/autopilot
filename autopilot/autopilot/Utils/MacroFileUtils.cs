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
	public class MacroFileUtils
	{
		public static bool WriteMacroFile(string path, MacroFile macroFile, bool canOverwrite)
		{
			if (File.Exists(path) && !canOverwrite)
				return false;
			try
			{
				IFormatter formatter = new BinaryFormatter();
				Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
				formatter.Serialize(stream, macroFile);
				stream.Close();
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public static MacroFile CreateMacroFileItem(string title = "", string desc = "", bool enabled = false, string bind = "", string code = "")
		{
			MacroFile macroFile = new MacroFile
			{
				Title = title,
				Description = desc,
				Enabled = enabled,
				Bind = bind,
				Code = code
			};
			return macroFile;
		}

		public static MacroFile ReadMacroFile(string path)
		{
			MacroFile macroFile;
			try
			{
				IFormatter formatter = new BinaryFormatter();
				Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
				macroFile = (MacroFile)formatter.Deserialize(stream);
				stream.Close();
			}
			catch (Exception)
			{
				return null;
			}
			return macroFile;
		}
	}
}
