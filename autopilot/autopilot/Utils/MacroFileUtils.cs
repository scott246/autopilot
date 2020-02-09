using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using static autopilot.AppVariables;

namespace autopilot.Utils
{
	public class MacroFileUtils
	{
        public static bool WriteMacroFile(MacroFile macroFile, bool canOverwrite)
        {
            bool success = false;
            Console.WriteLine("Writing macro file {0}", macroFile);
            string macroFilePath = macroFile.Path;
            if (macroFile.Directory == true) Directory.CreateDirectory(macroFilePath);
            if (File.Exists(macroFilePath) && !canOverwrite)
            {
                Console.WriteLine("Macro file {0} already exists", macroFilePath);
                return false;
            }
			try
			{
                FILE_ACCESS_MUTEX.WaitOne();
				BinaryFormatter formatter = new BinaryFormatter();
				Stream stream = new FileStream(macroFilePath, FileMode.Create, FileAccess.Write);
				formatter.Serialize(stream, macroFile);
				stream.Flush();
                success = true;
                Console.WriteLine("Successfully wrote macro file {0}", macroFile);
            }
			catch (Exception e)
			{
                Console.WriteLine("Failed to write macro file {0}", macroFile);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
			}
            finally
            {
                FILE_ACCESS_MUTEX.ReleaseMutex();
            }
			return success;
		}

		public static MacroFile ReadMacroFile(string path)
		{
			MacroFile macroFile;
            if (Directory.Exists(path))
            {
                Console.WriteLine("Macro file at path {0} is a directory, writing new macrofile", path);
                macroFile = new MacroFile
                {
                    Directory = true,
                    Path = path,
                    Title = GetFolderNameFromPath(path),
                    Children = new MacroFileCollection()
                };
                return macroFile;
            }
            try
            {
                FILE_ACCESS_MUTEX.WaitOne();
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                macroFile = (MacroFile)formatter.Deserialize(stream);
                stream.Flush();
                Console.WriteLine("Successfully read macro file at {0}", path);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to read macro file at {0}", path);
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

		public static void DeleteMacroFile(string path)
		{
			MACRO_FILE_TREE.Remove(GetMacroFileFromPath(path));
            File.Delete(path);
        }

        public static void CreateMacro(MacroFile parent, string fullMacroPath)
        {
            string fullMacroName = GetFileNameWithMacroExtensionFromPath(fullMacroPath);
            MacroFile file = new MacroFile
            {
                Directory = false,
                Title = fullMacroName,
                Path = fullMacroPath,
                Enabled = true,
                Children = new MacroFileCollection()
            };

            parent.Children.Add(file);

            WriteMacroFile(file, true);
            //MainWindow.LoadMacroFolderTree();
        }

        public static void CreateFolder(MacroFile parent, string fullFolderPath)
        {
            Directory.CreateDirectory(fullFolderPath);
            MacroFile folder = new MacroFile
            {
                Directory = true,
                Title = GetFileNameWithNoMacroExtensionFromPath(fullFolderPath),
                Path = fullFolderPath,
                Children = new MacroFileCollection()
            };

            parent.Children.Add(folder);

            //MainWindow.LoadMacroFolderTree();
        }

        public static string GetExtension(string fileName)
        {
            return "." + fileName.Split('.')[fileName.Split('.').Length - 1];
        }

        public static string GetFileNameWithNoMacroExtension(string fileName)
        {
            if (fileName.EndsWith(MACRO_EXTENSION))
                fileName = fileName.Substring(0, (fileName.Length - MACRO_EXTENSION.Length));
            return fileName;
        }

        public static string GetFileNameWithMacroExtension(string fileName)
        {
            if (!fileName.EndsWith(MACRO_EXTENSION))
                fileName += MACRO_EXTENSION;
            return fileName;
        }

        public static string GetFileNameWithMacroExtensionFromPath(string path)
        {
            if (!path.EndsWith(MACRO_EXTENSION))
                path += MACRO_EXTENSION;
            return path.Substring(path.LastIndexOf('\\') + 1);
        }

        public static string GetFileNameWithNoMacroExtensionFromPath(string path)
        {
            if (path.EndsWith(MACRO_EXTENSION))
                path = path.Substring(0, (path.Length - MACRO_EXTENSION.Length));
            return path.Substring(path.LastIndexOf('\\') + 1);
        }

        public static string GetFolderNameFromPath(string path)
        {
            string[] folders = path.Split('\\');
            int folderDepth = folders.Length - 1;
            while (folderDepth > 0)
            {
                if (folders[folderDepth].EndsWith(MACRO_EXTENSION) || folders[folderDepth] == "")
                {
                    folderDepth--;
                    continue;
                }
                return folders[folderDepth];
            }
            if (path.Length - 1 == path.LastIndexOf('\\'))
            {
                path = path.Substring(0, path.LastIndexOf('\\'));
                return path.Substring(path.LastIndexOf('\\') + 1);
            }
            return path.Substring(path.LastIndexOf('\\') + 1);
        }

        public static string GetParentFolderNameFromPath(string path)
        {
            return path.Substring(path.LastIndexOf('\\') + 1);
        }

        public static MacroFile GetMacroFileFromParentByTitle(MacroFile parent, string title)
        {
            foreach (MacroFile file in parent.Children)
            {
                if (file.Title == title) return file;
            }
            return null;
        }

        public static MacroFile GetMacroFileFromPath(string path)
        {
            // trim trailing '\\' character(s)
            while (path.ToCharArray()[path.Length - 1] == '\\')
            {
                path = path.Substring(0, path.Length - 2);
            }

            string[] pathElements = path.Split('\\');
            MacroFile latestFile = null;
            MacroFile parentOfLatestFile = MACRO_FILE_TREE_ROOT;
            foreach (string pathElement in pathElements)
            {
                MacroFile fileElement = GetMacroFileFromParentByTitle(parentOfLatestFile, pathElement);
                if (fileElement != null)
                {
                    latestFile = fileElement;
                }
            }
            return latestFile;
        }


	}
}
