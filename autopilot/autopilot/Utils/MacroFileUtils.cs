using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace autopilot.Utils
{
	public class MacroFileUtils
	{
		public static bool WriteMacroFile(MacroFile macroFile, bool canOverwrite)
		{
			bool success = false;
			Console.WriteLine("Writing macro file {0}", macroFile.Title);
			string macroFileTitle = macroFile.Title;
			string macroFilePath = GetFullPathOfMacroFile(macroFileTitle);
			macroFile.LastModifiedDateTime = DateTime.Now;
			if (File.Exists(macroFilePath) && !canOverwrite)
			{
				Console.WriteLine("Macro file {0} already exists", macroFileTitle);
				return false;
			}
			try
			{
				Globals.FILE_ACCESS_MUTEX.WaitOne();
				BinaryFormatter formatter = new BinaryFormatter();
				Stream stream = new FileStream(macroFilePath, FileMode.Create, FileAccess.Write);
				formatter.Serialize(stream, macroFile);
				stream.Flush();
				stream.Dispose();
				success = true;
				Console.WriteLine("Successfully wrote macro file {0}", macroFileTitle);
				DebugOutput.DumpMacroInfo(macroFile);
			}
			catch (Exception e)
			{
				Console.WriteLine("Failed to write macro file {0}", macroFileTitle);
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
			}
			finally
			{
				Globals.FILE_ACCESS_MUTEX.ReleaseMutex();
			}
			return success;
		}

		public static MacroFile ReadMacroFile(string title)
		{
			MacroFile macroFile;
			try
			{
				Globals.FILE_ACCESS_MUTEX.WaitOne();
				BinaryFormatter formatter = new BinaryFormatter();
				Stream stream = new FileStream(GetFullPathOfMacroFile(title), FileMode.Open, FileAccess.Read);
				macroFile = (MacroFile)formatter.Deserialize(stream);
				stream.Flush();
				stream.Dispose();
				Console.WriteLine("Successfully read macro file {0}", title);
				DebugOutput.DumpMacroInfo(macroFile);
			}
			catch (Exception e)
			{
				Console.WriteLine("Failed to read macro file {0}", title);
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
				return null;
			}
			finally
			{
				Globals.FILE_ACCESS_MUTEX.ReleaseMutex();
			}
			return macroFile;
		}

		public static void DeleteMacroFile(string title)
		{
			Globals.MACRO_LIST.Remove(GetFileByTitle(title));
			Globals.SORTED_FILTERED_MACRO_LIST.Remove(GetFileByTitle(title));
			File.Delete(GetFullPathOfMacroFile(title));
		}

		public static bool CreateMacro(string title)
		{
			MacroFile file = new MacroFile
			{
				Title = GetFileName(title, false),
				Enabled = true,
				CreatedDateTime = DateTime.Now,
				LastModifiedDateTime = DateTime.Now
			};

			if (WriteMacroFile(file, false))
			{
				Globals.MACRO_LIST.Add(file);
				Globals.SORTED_FILTERED_MACRO_LIST.Add(file);
				return true;
			}
			return false;
		}

		public static string GetFileExtension(string fileName)
		{
			return "." + fileName.Split('.')[fileName.Split('.').Length - 1];
		}

		public static string GetFileName(string fileName, bool showMacroExtension)
		{
			if (fileName.EndsWith(Globals.MACRO_EXTENSION) && !showMacroExtension)
			{
				fileName = fileName.Substring(0, fileName.Length - Globals.MACRO_EXTENSION.Length);
			}

			if (!fileName.EndsWith(Globals.MACRO_EXTENSION) && showMacroExtension)
			{
				fileName += Globals.MACRO_EXTENSION;
			}

			return fileName;
		}

		public static string GetFullPathOfMacroFile(string title)
		{
			return Path.Combine(Globals.MACRO_DIRECTORY, GetFileName(title, true));
		}

		public static MacroFile GetFileByTitle(string title)
		{
			foreach (MacroFile file in Globals.MACRO_LIST)
			{
				if (file.Title == title)
				{
					return file;
				}
			}
			return null;
		}
	}
}
