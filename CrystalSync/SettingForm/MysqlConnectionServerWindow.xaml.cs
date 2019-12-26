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
using System.Windows.Threading;
using CrystalSync.Class;
using CrystalSync.Setting;

namespace CrystalSync.SettingForm
{
    /// <summary>
    /// Interaction logic for MysqlConnectionLocalForm.xaml
    /// </summary>
    public partial class MysqlConnectionServerWindow : Window
    {


        public MysqlConnectionServerWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            spinnerLoading.Visibility = Visibility.Visible;

            Application.Current.Dispatcher.InvokeAsync(new Action(() =>
            {

                ConnectionStringClass connStr = new ConnectionStringClass();

            string conStr = connStr.formatConnection(serverTxt.Text, portTxt.Value.ToString(), dbNameTxt.Text, userNameTxt.Text, passwordTxt.Text);


            MysqlConnectionClass connClass = new MysqlConnectionClass(conStr);

            if (connClass.isConnected)
            {
                MessageBox.Show("تم الاتصال بنجاح!");
                saveConnectionBtn.IsEnabled = true;
                this.saveConnectionSetting();
            }

                spinnerLoading.Visibility = Visibility.Hidden;

            }), DispatcherPriority.ContextIdle);




        }

        private void saveConnectionSetting()
        {



            MysqlConnectionServerSettings.Default["dbServer"] = serverTxt.Text;
            MysqlConnectionServerSettings.Default["dbName"] = dbNameTxt.Text;
            MysqlConnectionServerSettings.Default["dbuserName"] = userNameTxt.Text;
            MysqlConnectionServerSettings.Default["dbPassword"] = passwordTxt.Text;
            MysqlConnectionServerSettings.Default["dbPort"] = portTxt.Value.ToString();
            MysqlConnectionServerSettings.Default.Save();

            // حفظ تم اعددا الاتصال بالسيرفر بنجاح
            CrystalSync.Properties.Settings.Default["firstStart"] = "1";
            CrystalSync.Properties.Settings.Default.Save();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            serverTxt.Text = MysqlConnectionServerSettings.Default["dbServer"].ToString()  ;
            dbNameTxt.Text = MysqlConnectionServerSettings.Default["dbName"].ToString();
            userNameTxt.Text = MysqlConnectionServerSettings.Default["dbuserName"].ToString();
            passwordTxt.Text = MysqlConnectionServerSettings.Default["dbPassword"].ToString() ;


            try
            {
                int numVal = int.Parse(MysqlConnectionServerSettings.Default["dbPort"].ToString());
                portTxt.Value = numVal;

            }
            finally 
            {
            }


            //portTxt.Value = 3306;

        }

        private void saveConnectionBtn_Click(object sender, RoutedEventArgs e)
        {
            this.saveConnectionSetting();
            this.Close();
        }
    }
}
