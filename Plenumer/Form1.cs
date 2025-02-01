using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Plenumer
{
    public partial class Form1 : Form
    {
        private List<NumberedItem> items;
        public static void PopulateListBox(ListBox listBox, List<NumberedItem> items)
        {
            List<ListBoxItem> listBoxItems = new List<ListBoxItem>();
            AddItemsWithNumbers(listBoxItems, items, "", null);

            listBox.DataSource = null;  // Resetovanje pre ažuriranja
            listBox.DataSource = listBoxItems;
            listBox.DisplayMember = "DisplayText";  // Prikaz teksta u ListBox-u
        }

        private static void AddItemsWithNumbers(List<ListBoxItem> listBoxItems, List<NumberedItem> items, string prefix, NumberedItem parent)
        {
            for (int i = 0; i < items.Count; i++)
            {
                string currentNumber = prefix + (i + 1);
                items[i].Parent = parent; // Postavi roditelja

                listBoxItems.Add(new ListBoxItem { Item = items[i], DisplayText = $"{currentNumber}. {items[i].Text}" });

                if (items[i].SubItems.Count > 0)
                {
                    AddItemsWithNumbers(listBoxItems, items[i].SubItems, currentNumber + ".", items[i]);
                }
            }
        }

        private void RemoveSelectedItem()
        {
            if (listBox1.SelectedItem is ListBoxItem selectedItem)
            {
                var item = selectedItem.Item;
                var parentList = item.Parent?.SubItems ?? items; // Lista kojoj pripada stavka

                parentList.Remove(item); // Uklanjanje stavke

                NumberedItem.PopulateListBox(listBox1, items); // Osvežavanje liste
            }
        }

        private void MoveUp()
        {
            if (listBox1.SelectedItem is ListBoxItem selectedItem)
            {
                var item = selectedItem.Item;
                var parentList = item.Parent?.SubItems ?? items;

                int index = parentList.IndexOf(item);
                if (index > 0)
                {
                    parentList.RemoveAt(index);
                    parentList.Insert(index - 1, item);
                    PopulateListBox(listBox1, items);
                    listBox1.SelectedIndex = index - 1; // Selektuj pomereni element
                }
            }
        }

        private void MoveDown()
        {
            if (listBox1.SelectedItem is ListBoxItem selectedItem)
            {
                var item = selectedItem.Item;
                var parentList = item.Parent?.SubItems ?? items;

                int index = parentList.IndexOf(item);
                if (index < parentList.Count - 1)
                {
                    parentList.RemoveAt(index);
                    parentList.Insert(index + 1, item);
                    PopulateListBox(listBox1, items);
                    listBox1.SelectedIndex = index + 1; // Selektuj pomereni index
                }
            }
        }

        private void PromoteItem()
        {
            if (listBox1.SelectedItem is ListBoxItem selectedItem)
            {
                var item = selectedItem.Item;
                if (item.Parent == null) return; // Ako nema roditelja, ne može da se podigne

                var parentList = item.Parent.SubItems;
                var grandParentList = item.Parent.Parent?.SubItems ?? items;

                int index = parentList.IndexOf(item);
                parentList.RemoveAt(index);
                grandParentList.Add(item);
                item.Parent = item.Parent.Parent; // Postavi novog roditelja

                int currentIndex = listBox1.SelectedIndex;
                PopulateListBox(listBox1, items);
                listBox1.SelectedIndex = currentIndex;
            }
        }

        private void DemoteItem()
        {
            if (listBox1.SelectedItem is ListBoxItem selectedItem)
            {
                var item = selectedItem.Item;
                var parentList = item.Parent?.SubItems ?? items;

                int index = parentList.IndexOf(item);
                if (index > 0)
                {
                    var newParent = parentList[index - 1];
                    parentList.RemoveAt(index);
                    newParent.SubItems.Add(item);
                    item.Parent = newParent;
                    int currentIndex = listBox1.SelectedIndex;
                    PopulateListBox(listBox1, items);
                    listBox1.SelectedIndex = currentIndex;
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NumberedItem item = new NumberedItem();
            item.Text = textBox1.Text;
            if(items.Count != 0) items.Insert(items.Count-1, item);
            else items.Insert(0, item);
            PopulateListBox(listBox1, items);
            listBox1.SelectedIndex = listBox1.Items.Count - 2;
            textBox1.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            items = new List<NumberedItem>
            {
                new NumberedItem { Text = "Usvajanje dnevnog reda"
                },
                new NumberedItem { Text = "Usvajanje moderatora, zapisničara i brojača glasova" },
                new NumberedItem { Text = "Izveštaj radnih grupa" },
                new NumberedItem { Text = "Dalji tok blokade" },
                new NumberedItem { Text = "Razno" }
            };

            PopulateListBox(listBox1, items);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MoveUp();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MoveDown();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            DemoteItem();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            PromoteItem();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            RemoveSelectedItem();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            button3.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int sec = int.Parse(label5.Text);
            int min = int.Parse(label1.Text);

            if (sec == 0 && min == 0) 
            {
                timer1.Enabled = false;
                MessageBox.Show("Isteklo je vreme za diskusiju!");
                label5.Text = "00";
                label1.Text = "20";
                button3.Enabled = false;
                return;
            }
            else if(sec == 0)
            {
                label5.Text = "59";
                label1.Text = (min-1).ToString();
                return;
            }
            else
            {
                label5.Text = (sec-1).ToString();
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            MessageBox.Show("Diskusija je završena!");
            label5.Text = "00";
            label1.Text = "20";
            button3.Enabled = false;
            return;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int sec = int.Parse(label6.Text);
            if(sec == 0)
            {
                label6.Text = "00";
                timer2.Enabled = false;
                button12.Enabled = false;
                MessageBox.Show("Vreme za izlaganje je isteklo!");
                return;
            }
            label6.Text = (sec-1).ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            label6.Text = "20";
            button12.Enabled = true;
            timer2.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            label6.Text = "120";
            button12.Enabled = true;
            timer2.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            label6.Text = "45";
            button12.Enabled = true;
            timer2.Enabled = true;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            button12.Enabled = false;
            label6.Text = "00";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text.Length > 0) button1.Enabled = true;
            else button1.Enabled = false;
        }
    }
}
