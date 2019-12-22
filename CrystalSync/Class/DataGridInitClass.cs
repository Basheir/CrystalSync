using CrystalSync.Setting;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CrystalSync.Class
{
    class DataGridInitClass
    {



        public void ColorRow(DataGrid dg)
        {
            for (int i = 0; i < dg.Items.Count; i++)
            {
                DataGridRow row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(i);

                if (row != null)
                {
                    int index = row.GetIndex();
                    if (index % 2 == 0)
                    {
                        SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(100, 255, 104, 0));
                        row.Background = brush;
                    }
                    else
                    {
                        SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(100, 255, 232, 0));
                        row.Background = brush;
                    }
                }
            }
        }



        public void colorRowMins1(DataGrid dg)
        {


            //if (item != null)
            //{
            //    DataRow row = item.Row;

            //    var colValue = row["qty"].ToString();

            //    if (colValue == "0.00")
            //    {
            //        Color colorSetting = (Color)ColorConverter.ConvertFromString(ColorSelectSettings.Default["color"].ToString());

            //        e.Row.Background = new SolidColorBrush(colorSetting);
            //    }
            //    else
            //    {
            //        e.Row.Background = new SolidColorBrush(Colors.White);

            //    }

            //}


            var converter = new System.Windows.Media.BrushConverter();
            var brush = (Brush)converter.ConvertFromString(ColorSelectSettings.Default["color"].ToString());



            foreach (var item in dg.Items)
            {


                var row = dg.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;

              

                if (row != null)

                {

                    var colValue = row;

                    //if (item == "0.00")
                    //{

                    //}
                    //else
                    //{

                    //}


                    row.Background = brush;

                }
            }








        }


    }
}
