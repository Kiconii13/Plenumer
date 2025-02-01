using System.Collections.Generic;
using System.Windows.Forms;

namespace Plenumer
{
    public class NumberedItem
    {
        public string Text { get; set; }
        public List<NumberedItem> SubItems { get; set; } = new List<NumberedItem>();
        public NumberedItem Parent { get; set; } // Dodaj referencu na roditelja
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
