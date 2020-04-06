using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CrystalSync.SettingForm;
using CrystalSync.Setting;
using CrystalSync.Class;
using System.Data;
using System.Windows.Threading;



namespace CrystalSync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        DataTable myGridDataTable;

        ComboboxItemFillClass itemFillClass = new ComboboxItemFillClass();


        public MainWindow()
        {

          

            InitializeComponent();

          

        }

        private void settingConnectionServer_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            WizardSettingWindow wizWindow = new WizardSettingWindow();
            wizWindow.ShowDialog();




        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string dbServer = MysqlConnectionLocalSettings.Default["dbServer"].ToString();
            


        }

        private void ResyncDataServer_MenuItem_Click(object sender, RoutedEventArgs e)
        {





            //CrystalSyncDBClass sync = new CrystalSyncDBClass();
            CrystalSyncDBClass.syncDataServer();
            this.reloadDataServer();


        }


        


        private void reloadDataServer()
        {


            this.changeStsatSyncDataLabel();


            Application.Current.Dispatcher.InvokeAsync(new Action(() =>
            {


                ConnectionStringClass connStr = new ConnectionStringClass();
                string conStr = connStr.connectionServerSetting();
                MysqlConnectionClass connClass = new MysqlConnectionClass(conStr);
                if (connClass.isConnected){
                
                    this.myGridDataTable = connClass.query("Select it_name,FORMAT(it_balance_qty, 2) as qty  from items");
                    this.myGrids.ItemsSource = myGridDataTable.DefaultView;
                    this.myGrids.Columns[0].Header = "اسم الماد";
                    this.myGrids.Columns[1].Header = "الكمية";
                    this.myGrids.Columns[0].Width = 400;
                    this.myGrids.Columns[1].Width = 100;


                    DataGridInitClass grid = new DataGridInitClass();

                    grid.ColorRow(this.myGrids);
                    //grid.colorRowMins1(this.myGrids);


                    string lastUpdate = connClass.single("Select lastUpdate  from config");

                    this.countRowsLabel.Content = string.Format("عدد المواد : {0}", this.myGrids.Items.Count - 1);
                    this.statSyncDataLabel.Content = "";


                    DateTime date2 = DateTime.Parse(lastUpdate);

                    this.lastUpdateFromServerLabel.Content = string.Format("اخر تحديث من سيرفر : {0}", getRelativeDateTime(date2));
                    this.lastUpdateFromServerLabel.ToolTip = date2;
                    this.clockIconsLastUpdate.ToolTip = date2;
                }
                else
                {

                    resetStatAlbele();

                    WizardSettingWindow wizWindow = new WizardSettingWindow();
                    wizWindow.ShowDialog();

                }

                spinnerLoading.Visibility = Visibility.Hidden;

                try
                {

                    int ftSize = int.Parse(ColorSelectSettings.Default["fontSize"].ToString());
                    this.myGrids.FontSize = ftSize;
                    this.myGrids.Items.Refresh();
                }
                finally
                {
                }

            }), DispatcherPriority.ContextIdle);

            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            resetStatAlbele();



            if (CrystalSync.Properties.Settings.Default["firstStart"].ToString() == "0")
            {
              

                WizardSettingWindow wizardForm = new WizardSettingWindow();

                wizardForm.ShowDialog();
            }
            {
                itemFillClass.appendItem(typeSearchComboBox, "الكل", "all");
                itemFillClass.appendItem(typeSearchComboBox, "اسم المادة", "name");
                itemFillClass.appendItem(typeSearchComboBox, "الكمية", "qty");
                typeSearchComboBox.SelectedIndex = 0;
                reloadDataServer();

            }


           



   




           
            




        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

                reloadDataServer();
         
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
          
                reloadDataServer();
       
        }



        private void showColorSetting_MenuItem_Click(object sender, RoutedEventArgs e)
        {





            ColorSelectWindow colorWindow = new ColorSelectWindow();

            colorWindow.ShowDialog();

        }


        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {

            //CrystalSyncDBClass cystalDB = new CrystalSyncDBClass();


            int q = CrystalSyncDBClass.truncatDataServer();


            reloadDataServer();
            if (q > 0)
            {
                MessageBox.Show("تم مسح البيانات بنجاح");
            }else
            {
                MessageBox.Show("لم تتم العملية بنجاح !");
            }


        }



        private void changeStsatSyncDataLabel()
        {

            statSyncDataLabel.Content = "جاري جلب البيانات";
            spinnerLoading.Visibility = Visibility.Visible;

        }



        // بحث في البيانات
        private void SearchTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {


           if( this.myGrids.Items.Count  <= 0)
            {
                return;
            }

            string sVal = (typeSearchComboBox.SelectedItem as ComboboxItemFillClass).Value.ToString();



            if (sVal == "all")
            {
                string rowFilter = string.Format("[{0}] LIKE '%{2}%' OR [{1}] LIKE '%{2}%' ", "it_name", "qty", searchTxtBox.Text);
                ((DataView)myGrids.ItemsSource).RowFilter = rowFilter;
            }

            if (sVal == "name")
            {
                string rowFilter = string.Format("[{0}] LIKE '%{1}%'", "it_name", searchTxtBox.Text);
                ((DataView)myGrids.ItemsSource).RowFilter = rowFilter;
            }

            if (sVal == "qty")
            {
                string rowFilter = string.Format("[{0}] LIKE '%{1}%'", "qty", searchTxtBox.Text);
                ((DataView)myGrids.ItemsSource).RowFilter = rowFilter;
            }


            this.countRowsLabel.Content = string.Format("عدد نتائج البحث : {0}", this.myGrids.Items.Count-1);


        }

        private void Button_Click_1(object sender, MouseButtonEventArgs e)
        {
            this.reloadDataServer();


        }

        private void serachGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)e.OriginalSource;
            tb.Dispatcher.BeginInvoke(
                new Action(delegate
                {
                    tb.SelectAll();
                }), System.Windows.Threading.DispatcherPriority.Input);
        }

        private void typeSearchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            searchTxtBox.Focus();
        }





        private void myGrids_LoadingRow(object sender, DataGridRowEventArgs e)
        {


            string valSetting = ColorSelectSettings.Default["maxNumber"].ToString();

            int valSettingOut = 0;
            int.TryParse(valSetting, out valSettingOut);

            DataRowView item = e.Row.Item as DataRowView;
            if (item != null)
            {
                DataRow row = item.Row;

                var colValue = row["qty"].ToString();

                float temp;
                if (float.TryParse(colValue, out temp))
                {
                    if ((int) temp <= valSettingOut)
                    {

                        Color colorSetting = (Color)ColorConverter.ConvertFromString(ColorSelectSettings.Default["color"].ToString());

                        e.Row.Background = new SolidColorBrush(colorSetting);

                    }
                    else
                    {
                        SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

                        e.Row.Background = brush;


                    }
                }




            }


        }

        public string getRelativeDateTime(DateTime date)
        {
            TimeSpan ts = DateTime.Now - date;
            if (ts.TotalMinutes < 1)//seconds ago
                return "الان";
            if (ts.TotalHours < 1)//min ago
                return (int)ts.TotalMinutes == 1 ? "1 Minute ago" : (int)ts.TotalMinutes + " دقيقة مضى";
            if (ts.TotalDays < 1)//hours ago
                return (int)ts.TotalHours == 1 ? "1 Hour ago" : (int)ts.TotalHours + " ساعة مضى";
            if (ts.TotalDays < 7)//days ago
                return (int)ts.TotalDays == 1 ? "1 Day ago" : (int)ts.TotalDays + " يوم مضى";
            if (ts.TotalDays < 30.4368)//weeks ago
                return (int)(ts.TotalDays / 7) == 1 ? "1 Week ago" : (int)(ts.TotalDays / 7) + " اسبوع مضى";
            if (ts.TotalDays < 365.242)//months ago
                return (int)(ts.TotalDays / 30.4368) == 1 ? "1 Month ago" : (int)(ts.TotalDays / 30.4368) + " شهر مضى";
            //years ago
            return (int)(ts.TotalDays / 365.242) == 1 ? "1 Year ago" : (int)(ts.TotalDays / 365.242) + " سنة مضى";
        }




        private void resetStatAlbele()
        {

            this.lastUpdateFromServerLabel.Content = "";
            this.countRowsLabel.Content = "";
            this.statSyncDataLabel.Content = "";

            this.titleDataLocal.Content =   MysqlConnectionLocalSettings.Default["dbTitle"].ToString() +"<<LocalDB";
            ;

        }

    }







}
