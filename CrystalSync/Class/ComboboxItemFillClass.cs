using System;
using System.Windows.Controls;

namespace CrystalSync.Class
{



    //dbListLocalComboBox.SelectedItem as ComboboxItemFillClass).Value.ToString()
    public class ComboboxItemFillClass
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }


        public void appendItem(ComboBox cmBox ,string txt,string val)
        {

            ComboboxItemFillClass item = new ComboboxItemFillClass() ;
            item.Text = txt;
            item.Value = val;
            cmBox.Items.Add(item);


        }

        public void selectByText(ComboBox cmBox, string txt)
        {

            int Selected = 0;
            int count = cmBox.Items.Count;
            for (int i = 0; (i <= (count - 1)); i++)
            {
                cmBox.SelectedIndex = i;
                if ((string)(cmBox.SelectedItem.ToString()) == txt)
                {
                    Selected = i;
                }

            }

            cmBox.SelectedIndex = Selected;

        }






    }
}
