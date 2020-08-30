using System.Collections.Generic;
using static autopilot.Globals;

namespace autopilot.Utils
{
	class SortFilterUtils
	{
		public static string[] sortOptions = { "A-Z", "Enabled/Disabled", "Created Time", "Last Modified Time" };

		public static void SortFilterMacroList(int sortFormula, string filterText)
		{
			FilterMacros(filterText);
			switch (sortFormula)
			{
				case 0:
					BubbleSortMacros(0);
					return;
				case 1:
					BucketSortMacros(1);
					return;
				case 2:
					BubbleSortMacros(2);
					return;
				case 3:
					BubbleSortMacros(3);
					return;
				default:
					return;
			}
		}

		private static bool Comparison(int sortFormula, MacroFile a, MacroFile b)
		{
			switch (sortFormula)
			{
				case 0:
					return a.Title.CompareTo(b.Title) < 0;
				case 1:
					return a.Title.CompareTo(b.Title) < 0;
				case 2:
					return a.CreatedDateTime.CompareTo(b.CreatedDateTime) < 0;
				case 3:
					return a.LastModifiedDateTime.CompareTo(b.LastModifiedDateTime) < 0;
				default:
					return false;
			}
		}

		private static void BucketSortMacros(int sortFormula)
		{
			List<MacroFile> enabledBucket = new List<MacroFile>();
			List<MacroFile> disabledBucket = new List<MacroFile>();

			BubbleSortMacros(sortFormula);

			for (int i = 0; i < SORTED_FILTERED_MACRO_LIST.Count; i++)
			{
				if (SORTED_FILTERED_MACRO_LIST[i].Enabled)
				{
					enabledBucket.Add(SORTED_FILTERED_MACRO_LIST[i]);
				}
				else
				{
					disabledBucket.Add(SORTED_FILTERED_MACRO_LIST[i]);
				}
			}
			SORTED_FILTERED_MACRO_LIST.Clear();
			foreach (MacroFile item in enabledBucket)
			{
				SORTED_FILTERED_MACRO_LIST.Add(item);
			}
			foreach (MacroFile item in disabledBucket)
			{
				SORTED_FILTERED_MACRO_LIST.Add(item);
			}
		}

		private static void BubbleSortMacros(int sortFormula)
		{
			for (int i = 0; i < SORTED_FILTERED_MACRO_LIST.Count; i++)
			{
				for (int j = 0; j < SORTED_FILTERED_MACRO_LIST.Count; j++)
				{
					if (Comparison(sortFormula, SORTED_FILTERED_MACRO_LIST[i], SORTED_FILTERED_MACRO_LIST[j]))
					{
						MacroFile temp = SORTED_FILTERED_MACRO_LIST[i];
						SORTED_FILTERED_MACRO_LIST[i] = SORTED_FILTERED_MACRO_LIST[j];
						SORTED_FILTERED_MACRO_LIST[j] = temp;
					}
				}
			}
		}

		private static void FilterMacros(string filterText)
		{
			SORTED_FILTERED_MACRO_LIST.Clear();
			foreach (MacroFile macro in MACRO_LIST)
			{
				if (filterText == "" || MacroFileUtils.GetFileNameWithNoMacroExtension(macro.Title).Contains(filterText))
				{
					SORTED_FILTERED_MACRO_LIST.Add(macro);
				}
			}
		}
	}
}
