using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chkam05.DotNetTools.CSVParser.Tools
{
    class StringListTools
    {

        //  METHODS

        #region TYPICAL STRINGS LIST METHODS

        /// <summary> Replace specific items in string list with new. </summary>
        /// <param name="stringList"> List of strings that contains items to replace. </param>
        /// <param name="currentItem"> Item to replace. </param>
        /// <param name="newItem"> New item to insert as replacement. </param>
        /// <returns> List of strings with replaced items. </returns>
        public static List<string> ReplaceItems(List<string> stringList, string currentItem = null, string newItem = "")
        {
            //  Resolve how many items to replace are in list.
            var foundItems = stringList.Where(i => i == currentItem).ToList();

            //  Replace items.
            if (foundItems != null && foundItems.Any())
                foreach (var attempt in Enumerable.Range(0, foundItems.Count))
                    stringList[stringList.FindIndex(idx => idx.Equals(currentItem))] = newItem;

            return stringList;
        }

        /// <summary> Remove specific items in string list. </summary>
        /// <param name="stringList"> List of strings that contains items to remove. </param>
        /// <param name="currentItem"> Item to remove. </param>
        /// <returns> List of strings without removed items. </returns>
        public static List<string> RemoveItems(List<string> stringList, string currentItem = null)
        {
            //  Resolve how many items to remove are in list.
            var foundItems = stringList.Where(i => i == currentItem).ToList();

            //  Replace items.
            if (foundItems != null && foundItems.Any())
                foreach (var attempt in Enumerable.Range(0, foundItems.Count))
                    stringList.Remove(currentItem);

            return stringList;
        }

        /// <summary> Expand string list with new elements. </summary>
        /// <param name="stringList"> List of strings to expand. </param>
        /// <param name="newLength"> New lenght of strings list. </param>
        /// <param name="itemData"> Item to insert as single expanded element. </param>
        /// <returns> Expanded list of strings. </returns>
        public static List<string> ExpandList(List<string> stringList, int newLength, string itemData = "")
        {
            //  Calculate diffrence between current elements count and new length.
            var diffrence = newLength - stringList.Count;

            //  Expand list with new elements containing itemData.
            if (diffrence > 0)
                stringList.AddRange(from i in Enumerable.Range(0, diffrence) select itemData);

            return stringList;
        }

        #endregion TYPICAL STRINGS LIST METHODS

    }
}
