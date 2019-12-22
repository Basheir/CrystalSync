using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;
using CrystalSync.Setting;
using System.Windows.Controls;

//https://github.com/wickyaswal/c-sharp-mysql-database-class

namespace CrystalSync.Class
{
    class CrystalSyncDBClass {

    

        public void syncDataServer()
        {
            ConnectionStringClass connStr = new ConnectionStringClass();
            string conStr = connStr.connectionLocalSetting();
            MysqlConnectionClass connClass = new MysqlConnectionClass(conStr);
            IList<ItemClass> items = new List<ItemClass>();



            DataTable lists = connClass.query("SELECT it_name, it_code, it_balance_qty FROM tbl_item LEFT JOIN tbl_item_balance ON tbl_item_balance.it_balance_item_id = tbl_item.it_id");

            foreach (DataRow rows in lists.Rows)
            {

                items.Add(new ItemClass (rows["it_name"].ToString(), rows["it_code"].ToString(), rows["it_balance_qty"].ToString()));

            }


            insertDataToServer(items);




        }


        public void insertDataToServer(IList<ItemClass> items)
        {


            this.truncatDataServer();

            IList<string> insertStr = new List<string>();

            foreach (var item in items) {

                Console.WriteLine(item.name);

                insertStr.Add(String.Format("('{0}','{1}','{2}')", MySqlHelper.EscapeString(item.name), MySqlHelper.EscapeString(item.code), MySqlHelper.EscapeString(item.qty)));

            }


            ConnectionStringClass connStr = new ConnectionStringClass();
            string conStr = connStr.connectionServerSetting();
            MysqlConnectionClass connClass = new MysqlConnectionClass(conStr);

            string ins = String.Format("INSERT INTO `items`(`it_name`,`it_code`,`it_balance_qty`) VALUES {0};", String.Join(",", insertStr));

            connClass.nQuery(ins);



            DateTime date1 = DateTime.Now;
            string lstUpdate = String.Format("UPDATE `config` SET `ke` ='{0}';", MySqlHelper.EscapeString(date1.ToString()));

            connClass.nQuery(lstUpdate);


        }



        // مسح البيانات منسيرفر

        public int truncatDataServer()
        {
            ConnectionStringClass connStr = new ConnectionStringClass();
            string conStr = connStr.connectionServerSetting();
            MysqlConnectionClass connClass = new MysqlConnectionClass(conStr);

           int q = connClass.nQuery("TRUNCATE  items;");

            return q;
        }










    }
}

public class ItemClass
{
    public ItemClass(string itName, string itCode, string itQty)
    {
        this.name = itName;
        this.code = itCode;
        this.qty = itQty;
    }
    public string name { set; get; }
    public string code { set; get; }
    public string qty { set; get; }
}