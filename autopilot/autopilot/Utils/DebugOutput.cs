using System;
using System.Collections.Generic;

namespace autopilot.Utils
{
	public class DebugOutput
	{
		private static object NullableOutputFormatter(object input)
		{
			if (null == input) return "[NULL]";
			if ("" == input.ToString()) return "[EMPTY]";
			return input;
		}

		private static string DumpCommandList(List<Command> commandList)
		{
			if (null == commandList) return null;
			string commandListString = "";
			if (commandList.Count == 0)
			{
				commandListString += "[NO COMMANDS]";
			}
			else
			{
				for (int i = 0; i < commandList.Count; i++)
				{
					Command c = commandList[i];
					string commandArguments = "";
					if (null == c.Arguments)
					{
						commandArguments = "[NO ARGUMENTS]";
					}
					else
					{
						foreach (KeyValuePair<string, string> arg in c.Arguments)
						{
							commandArguments += "Key: " + arg.Key + ", Value: " + NullableOutputFormatter(arg.Value) + "; ";
						}
					}
					commandListString += "\n" + i + ": " +  c.Title + " | " + commandArguments;
				}
			}
			return commandListString;
		}

		public static void DumpMacroInfo(MacroFile macroFile)
		{
			Console.WriteLine("MacroFile {0} info: \n" +
				"MacroFile bind: {1} \n" +
				"MacroFile commands: {2} \n" +
				"MacroFile enabled: {3} \n" +
				"MacroFile creation datetime: {4} \n" +
				"MacroFile last modified datetime: {5}",
				NullableOutputFormatter(macroFile.Title),
				NullableOutputFormatter(macroFile.Bind),
				NullableOutputFormatter(DumpCommandList(macroFile.Commands)),
				NullableOutputFormatter(macroFile.Enabled),
				NullableOutputFormatter(macroFile.CreatedDateTime),
				NullableOutputFormatter(macroFile.LastModifiedDateTime)
				);
		}
	}
}
