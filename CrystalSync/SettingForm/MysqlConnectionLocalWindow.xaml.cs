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
using System.Windows.Threading;

namespace CrystalSync.SettingForm
{



    /// <summary>
    /// Interaction logic for MysqlConnectionLocalForm.xaml
    /// </summary>
    public partial class MysqlConnectionLocalWindow : Window
    {


        ComboboxItemFillClass itemFillClass = new ComboboxItemFillClass();
        public MainWindow parentForm;


        //private ObservableCollection<States> states = new ObservableCollection<States>();

        public MysqlConnectionLocalWindow()
        {
            InitializeComponent();


           
        }




        //حفظ الاعدادات
        private void saveConnectionSetting()
        {

           

            MysqlConnectionLocalSettings.Default["dbServer"] = serverTxt.Text;
            MysqlConnectionLocalSettings.Default["dbuserName"] = userNameTxt.Text;
            MysqlConnectionLocalSettings.Default["dbPassword"] = passwordTxt.Text;
            MysqlConnectionLocalSettings.Default["dbPort"] = portTxt.Value.ToString();


            MysqlConnectionLocalSettings.Default["dbName"] = (dbListLocalComboBox.SelectedItem as ComboboxItemFillClass).Value.ToString();
            MysqlConnectionLocalSettings.Default["dbTitle"] = (dbListLocalComboBox.SelectedItem as ComboboxItemFillClass).Text.ToString();

            MysqlConnectionLocalSettings.Default.Save();

            ((MainWindow)Application.Current.MainWindow).titleDataLocal.Content = MysqlConnectionLocalSettings.Default["dbTitle"];


        }


        //جلب قواعد البيانات
        private void retriveDb_Button_Click(object sender, RoutedEventArgs e)
        {
            spinnerLoading.Visibility = Visibility.Visible;

            Application.Current.Dispatcher.InvokeAsync(new Action(() =>
            {



                ConnectionStringClass connStr = new ConnectionStringClass();

            string conStr = connStr.formatConnection(serverTxt.Text, portTxt.Value.ToString(), "db_config", userNameTxt.Text, passwordTxt.Text );


            MysqlConnectionClass connClass = new MysqlConnectionClass(conStr);

            if (connClass.isConnected)
            {
                MessageBox.Show("تم الاتصال بنجاح!");
                saveConnectionBtn.IsEnabled = true;

                DataTable lists = connClass.query("SELECT * FROM tbl_databases");

                dbListLocalComboBox.Items.Clear();
                dbListLocalComboBox.IsEnabled = true;
                foreach (DataRow rows in lists.Rows)
                {

                    itemFillClass.appendItem(dbListLocalComboBox, rows["da_title"].ToString(), rows["da_name"].ToString());

                }
                //dbListLocalComboBox.SelectedIndex = 0;

                itemFillClass.selectByText(dbListLocalComboBox, MysqlConnectionLocalSettings.Default["dbTitle"].ToString());

            }

                spinnerLoading.Visibility = Visibility.Hidden;

            }), DispatcherPriority.ContextIdle);


        }


        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            serverTxt.Text = MysqlConnectionLocalSettings.Default["dbServer"].ToString();
            userNameTxt.Text = MysqlConnectionLocalSettings.Default["dbuserName"].ToString();
            passwordTxt.Text = MysqlConnectionLocalSettings.Default["dbPassword"].ToString();

            try
            {
                int numVal = int.Parse(MysqlConnectionLocalSettings.Default["dbPort"].ToString());
                portTxt.Value = numVal;

            }
            finally
            {
            }


            itemFillClass.appendItem(dbListLocalComboBox, MysqlConnectionLocalSettings.Default["dbTitle"].ToString(), MysqlConnectionLocalSettings.Default["dbName"].ToString());

            dbListLocalComboBox.SelectedIndex = 0;

        }

        private void saveConnectionBtn_Click(object sender, RoutedEventArgs e)
        {

            this.saveConnectionSetting();
            this.Close();

        }
    }


}







