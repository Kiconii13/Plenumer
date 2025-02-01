using System.Collections.Generic;
using System.Windows.Forms;

namespace Plenumer
{
    public class NumberedItem
    {
        public string Text { get; set; }
        public List<NumberedItem> SubItems { get; set; } = new List<NumberedItem>();
        public NumberedItem Parent { get; set; } // Dodaj referencu na roditelja

        public override string ToString()
        {
            return Text; // Prikaz u ListBox-u
        }

        public static void PopulateListBox(ListBox listBox, List<NumberedItem> items)
        {
            listBox.DataSource = null;
            listBox.Items.Clear();
            AddItemsWithNumbers(listBox, items, "", null);
        }

        private static void AddItemsWithNumbers(ListBox listBox, List<NumberedItem> items, string prefix, NumberedItem parent)
        {
            for (int i = 0; i < items.Count; i++)
            {
                string currentNumber = prefix + (i + 1);
                items[i].Parent = parent; // Postavi roditelja
                listBox.Items.Add(new ListBoxItem { Item = items[i], DisplayText = $"{currentNumber}. {items[i].Text}" });

                if (items[i].SubItems.Count > 0)
                {
                    AddItemsWithNumbers(listBox, items[i].SubItems, currentNumber + ".", items[i]);
                }
            }
        }
    }

    public class ListBoxItem
    {
        public NumberedItem Item { get; set; }
        public string DisplayText { get; set; }

        public override string ToString()
        {
            return DisplayText;
        }
    }
}
