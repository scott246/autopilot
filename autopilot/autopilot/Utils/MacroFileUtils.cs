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
		public static bool WriteMacroFile(MacroFile macroFile, bool canOverwrite)
		{
			string macroFilePath = macroFile.Path;
			if (macroFile.Directory == true) Directory.CreateDirectory(macroFilePath);
			if (File.Exists(macroFilePath) && !canOverwrite)
				return false;
			try
			{
				IFormatter formatter = new BinaryFormatter();
				Stream stream = new FileStream(macroFilePath, FileMode.Create, FileAccess.Write);
				formatter.Serialize(stream, macroFile);
				stream.Close();
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public static MacroFile ReadMacroFile(string path)
		{
			MacroFile macroFile;
			if (Directory.Exists(path))
			{
				macroFile = new MacroFile
				{
					Directory = true,
					Path = path,
					Title = MainWindowUtils.GetFileNameWithNoMacroExtension(path),
					FontStyle = AppVariables.EnabledFontStyle,
					FontWeight = AppVariables.FolderFontWeight,
					Children = new MacroFileCollection()
				};
				return macroFile;
			}
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
