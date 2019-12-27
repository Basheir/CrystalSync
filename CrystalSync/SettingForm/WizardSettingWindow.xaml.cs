using CrystalSync.Class;
using CrystalSync.Setting;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace CrystalSync
{
    /// <summary>
    /// Interaction logic for WizardSettingWindow.xaml
    /// </summary>
    public partial class WizardSettingWindow : Window
    {
        public WizardSettingWindow()
        {
            InitializeComponent();
        }



      

        //حفظ الاعدادات
        private void saveConnectionSettingLocal()
        {



            MysqlConnectionLocalSettings.Default["dbServer"] = serverTxtL.Text;
            MysqlConnectionLocalSettings.Default["dbuserName"] = userNameTxtL.Text;
            MysqlConnectionLocalSettings.Default["dbPassword"] = passwordTxtL.Text;
            MysqlConnectionLocalSettings.Default["dbPort"] = portTxtL.Value.ToString();


            MysqlConnectionLocalSettings.Default["dbName"] = (dbListLocalComboBoxL.SelectedItem as ComboboxItemFillClass).Value.ToString();
            MysqlConnectionLocalSettings.Default["dbTitle"] = (dbListLocalComboBoxL.SelectedItem as ComboboxItemFillClass).Text.ToString();

            MysqlConnectionLocalSettings.Default.Save();

            ((MainWindow)Application.Current.MainWindow).titleDataLocal.Content = MysqlConnectionLocalSettings.Default["dbTitle"];


        }

        private void saveConnectionSettingServer()
        {



            MysqlConnectionServerSettings.Default["dbServer"] = serverTxtS.Text;
            MysqlConnectionServerSettings.Default["dbName"] = dbNameTxtS.Text;
            MysqlConnectionServerSettings.Default["dbuserName"] = userNameTxtS.Text;
            MysqlConnectionServerSettings.Default["dbPassword"] = passwordTxtS.Text;
            MysqlConnectionServerSettings.Default["dbPort"] = portTxtS.Value.ToString();
            MysqlConnectionServerSettings.Default.Save();

            // حفظ تم اعددا الاتصال بالسيرفر بنجاح
            CrystalSync.Properties.Settings.Default["firstStart"] = "1";
            CrystalSync.Properties.Settings.Default.Save();

            this.Close();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PageServer.CanFinish = false;
            serverTxtL.Text = MysqlConnectionLocalSettings.Default["dbServer"].ToString();
            userNameTxtL.Text = MysqlConnectionLocalSettings.Default["dbuserName"].ToString();
            passwordTxtL.Text = MysqlConnectionLocalSettings.Default["dbPassword"].ToString();
            try
            {
                int numVal = int.Parse(MysqlConnectionLocalSettings.Default["dbPort"].ToString());
                portTxtL.Value = numVal;

            }
            finally
            {
            }

            ComboboxItemFillClass itemFillClassLocal = new ComboboxItemFillClass();

            itemFillClassLocal.appendItem(dbListLocalComboBoxL, MysqlConnectionLocalSettings.Default["dbTitle"].ToString(), MysqlConnectionLocalSettings.Default["dbName"].ToString());

            dbListLocalComboBoxL.SelectedIndex = 0;



            // Loading Server Data From Setting


            serverTxtS.Text = MysqlConnectionServerSettings.Default["dbServer"].ToString();
            dbNameTxtS.Text = MysqlConnectionServerSettings.Default["dbName"].ToString();
            userNameTxtS.Text = MysqlConnectionServerSettings.Default["dbuserName"].ToString();
            passwordTxtS.Text = MysqlConnectionServerSettings.Default["dbPassword"].ToString();


            try
            {
                int numVal = int.Parse(MysqlConnectionServerSettings.Default["dbPort"].ToString());
                portTxtS.Value = numVal;

            }
            finally
            {
            }



        }

        //جلب قواعد البيانات
        private void retriveDbLocal_Button_Click(object sender, RoutedEventArgs e)
        {
            spinnerLoadingL.Visibility = Visibility.Visible;
            ComboboxItemFillClass itemFillClassL = new ComboboxItemFillClass();


            PageLocal.CanSelectNextPage = false;

            Application.Current.Dispatcher.InvokeAsync(new Action(() =>
            {



                ConnectionStringClass connStr = new ConnectionStringClass();

                string conStr = connStr.formatConnection(serverTxtL.Text, portTxtL.Value.ToString(), "db_config", userNameTxtL.Text, passwordTxtL.Text);


                MysqlConnectionClass connClass = new MysqlConnectionClass(conStr);

                if (connClass.isConnected)
                {
                    MessageBox.Show("تم الاتصال بنجاح!");

                    DataTable lists = connClass.query("SELECT * FROM tbl_databases");

                    dbListLocalComboBoxL.Items.Clear();
                    dbListLocalComboBoxL.IsEnabled = true;
                    foreach (DataRow rows in lists.Rows)
                    {

                        itemFillClassL.appendItem(dbListLocalComboBoxL, rows["da_title"].ToString(), rows["da_name"].ToString());

                    }
                    //dbListLocalComboBox.SelectedIndex = 0;

                    itemFillClassL.selectByText(dbListLocalComboBoxL, MysqlConnectionLocalSettings.Default["dbTitle"].ToString());
                    PageLocal.CanSelectNextPage = true;
                    //saveConnectionSettingLocal();
                }

                spinnerLoadingL.Visibility = Visibility.Hidden;

            }), DispatcherPriority.ContextIdle);


        }


        // Connection Server Test
        private void saveConnectionServer_Button_Click(object sender, RoutedEventArgs e)
        {

            spinnerLoadingS.Visibility = Visibility.Visible;
            

            Application.Current.Dispatcher.InvokeAsync(new Action(() =>
            {

                ConnectionStringClass connStr = new ConnectionStringClass();

                string conStr = connStr.formatConnection(serverTxtS.Text, portTxtS.Value.ToString(), dbNameTxtS.Text, userNameTxtS.Text, passwordTxtS.Text);


                MysqlConnectionClass connClass = new MysqlConnectionClass(conStr);

                if (connClass.isConnected)
                {
                    MessageBox.Show("تم الاتصال بنجاح!");
                    //this.saveConnectionSettingServer();
                    PageServer.CanFinish = true;
                }

                spinnerLoadingS.Visibility = Visibility.Hidden;

            }), DispatcherPriority.ContextIdle);




        }

        private void Wizard_Finish(object sender, Xceed.Wpf.Toolkit.Core.CancelRoutedEventArgs e)
        {
            this.saveConnectionSettingLocal();
            this.saveConnectionSettingServer();
        }
    }
}
