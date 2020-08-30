using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using static autopilot.Globals;

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
				FILE_ACCESS_MUTEX.WaitOne();
				BinaryFormatter formatter = new BinaryFormatter();
				Stream stream = new FileStream(macroFilePath, FileMode.Create, FileAccess.Write);
				formatter.Serialize(stream, macroFile);
				stream.Flush();
				stream.Dispose();
				success = true;
				Console.WriteLine("Successfully wrote macro file {0}", macroFileTitle);
			}
			catch (Exception e)
			{
				Console.WriteLine("Failed to write macro file {0}", macroFileTitle);
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
			}
			finally
			{
				FILE_ACCESS_MUTEX.ReleaseMutex();
			}
			return success;
		}

		public static MacroFile ReadMacroFile(string title)
		{
			MacroFile macroFile;
			try
			{
				FILE_ACCESS_MUTEX.WaitOne();
				BinaryFormatter formatter = new BinaryFormatter();
				Stream stream = new FileStream(GetFullPathOfMacroFile(title), FileMode.Open, FileAccess.Read);
				macroFile = (MacroFile)formatter.Deserialize(stream);
				stream.Flush();
				stream.Dispose();
				Console.WriteLine("Successfully read macro file {0}", title);
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
				FILE_ACCESS_MUTEX.ReleaseMutex();
			}
			return macroFile;
		}

		public static void DeleteMacroFile(string title)
		{
			MACRO_LIST.Remove(GetFileByTitle(title));
			SORTED_FILTERED_MACRO_LIST.Remove(GetFileByTitle(title));
			File.Delete(GetFullPathOfMacroFile(title));
		}

		public static bool CreateMacro(string title)
		{
			MacroFile file = new MacroFile
			{
				Title = GetFileNameWithNoMacroExtension(title),
				Enabled = true,
				CreatedDateTime = DateTime.Now,
				LastModifiedDateTime = DateTime.Now
			};

			if (WriteMacroFile(file, false))
			{
				MACRO_LIST.Add(file);
				SORTED_FILTERED_MACRO_LIST.Add(file);
				return true;
			}
			return false;
		}

		public static string GetExtensionFromFile(string fileName)
		{
			return "." + fileName.Split('.')[fileName.Split('.').Length - 1];
		}

		public static string GetFileNameWithNoMacroExtension(string fileName)
		{
			if (fileName.EndsWith(MACRO_EXTENSION))
			{
				fileName = fileName.Substring(0, (fileName.Length - MACRO_EXTENSION.Length));
			}

			return fileName;
		}

		public static string GetFileNameWithMacroExtension(string fileName)
		{
			if (!fileName.EndsWith(MACRO_EXTENSION))
			{
				fileName += MACRO_EXTENSION;
			}

			return fileName;
		}

		public static string GetFullPathOfMacroFile(string title)
		{
			return Path.Combine(MACRO_DIRECTORY, GetFileNameWithMacroExtension(title));
		}

		public static MacroFile GetFileByTitle(string title)
		{
			foreach (MacroFile file in MACRO_LIST)
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
