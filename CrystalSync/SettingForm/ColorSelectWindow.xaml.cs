using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CrystalSync.Class;
using CrystalSync.Setting;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data;

namespace CrystalSync.SettingForm
{



    /// <summary>
    /// Interaction logic for ColorSelectWindow.xaml
    /// </summary>
    public partial class ColorSelectWindow : Window
    {




        public ColorSelectWindow()
        {
            InitializeComponent();

        }


        private void SaveConnectionBtn_Click(object sender, RoutedEventArgs e)
        {

            var color = (CustomWPFColorPicker.ColorPickerControlView)ForeColorPicker;



            ColorSelectSettings.Default["maxNumber"] = maxNumber.Value.ToString();
            ColorSelectSettings.Default["color"] = color.CurrentColor.ToString();


            ColorSelectSettings.Default["fontSize"] = fontSizeTxt.Value.ToString();
            ColorSelectSettings.Default.Save();


            //Console.WriteLine(color.CurrentColor);


            ((MainWindow)Application.Current.MainWindow).myGrids.Items.Refresh();

            this.Close();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                int numVal = int.Parse(ColorSelectSettings.Default["maxNumber"].ToString());
                maxNumber.Value = numVal;


                int ftSize = int.Parse(ColorSelectSettings.Default["fontSize"].ToString());
                fontSizeTxt.Value = ftSize;
            }
            finally
            {
            }


          
        }
    }


}







