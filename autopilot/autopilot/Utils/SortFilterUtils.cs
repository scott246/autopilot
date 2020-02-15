using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static autopilot.Globals;

namespace autopilot.Utils
{
	class SortFilterUtils
    {
        public static string[] sortOptions = { "A-Z", "Enabled/Disabled", "Creation Time" };
        public static string filterText = "";

        public static void SortFilterMacroList(int sortFormula)
        {
            FilterMacros();
            switch (sortFormula)
            {
                case 0:
                    BubbleSortMacros();
                    return;
                case 1:
                    BucketSortMacros();
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
                    return false;
                default:
                    return false;
            }
        }

        private static void BucketSortMacros()
        {
            List<MacroFile> enabledBucket = new List<MacroFile>();
            List<MacroFile> disabledBucket = new List<MacroFile>();

            BubbleSortMacros();

            for (int i = 0; i < SORTED_FILTERED_MACRO_LIST.Count; i++)
            {
                if (SORTED_FILTERED_MACRO_LIST[i].Enabled)
                {
                    enabledBucket.Add(SORTED_FILTERED_MACRO_LIST[i]);
                }
                else disabledBucket.Add(SORTED_FILTERED_MACRO_LIST[i]);
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

        private static void BubbleSortMacros()
        {
            for (int i = 0; i < SORTED_FILTERED_MACRO_LIST.Count; i++)
            {
                for (int j = 0; j < SORTED_FILTERED_MACRO_LIST.Count; j++)
                {
                    if (Comparison(0, SORTED_FILTERED_MACRO_LIST[i], SORTED_FILTERED_MACRO_LIST[j]))
                    {
                        MacroFile temp = SORTED_FILTERED_MACRO_LIST[i];
                        SORTED_FILTERED_MACRO_LIST[i] = SORTED_FILTERED_MACRO_LIST[j];
                        SORTED_FILTERED_MACRO_LIST[j] = temp;
                    }
                }
            }
        }

        private static void FilterMacros()
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
