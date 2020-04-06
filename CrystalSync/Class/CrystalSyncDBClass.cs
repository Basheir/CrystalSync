using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;

//https://github.com/wickyaswal/c-sharp-mysql-database-class

namespace CrystalSync.Class
{
  public static   class CrystalSyncDBClass
    {



        public static  void syncDataServer()
        {




            ConnectionStringClass connStr = new ConnectionStringClass();
            string conStr = connStr.connectionLocalSetting();
            MysqlConnectionClass connClass = new MysqlConnectionClass(conStr);
            IList<ItemClass> items = new List<ItemClass>();

            DataTable lists = connClass.query("SELECT it_name, it_code, it_balance_qty FROM tbl_item LEFT JOIN tbl_item_balance ON tbl_item_balance.it_balance_item_id = tbl_item.it_id");

            foreach (DataRow rows in lists.Rows)
            {

                items.Add(new ItemClass(rows["it_name"].ToString(), rows["it_code"].ToString(), rows["it_balance_qty"].ToString()));


            }


            insertDataToServer(items);




        }


        public static  void insertDataToServer(IList<ItemClass> items)
        {


            CrystalSyncDBClass.truncatDataServer();

            var chunks = items.Chunk(100);
           IList<string> insertStr2 = new List<string>();
           string ins2 = "";

            ConnectionStringClass connStr = new ConnectionStringClass();
            string conStr = connStr.connectionServerSetting();
            MysqlConnectionClass connClass = new MysqlConnectionClass(conStr);

              CrystalSyncDBClass.truncatDataServer();
           



            foreach (var batch in chunks)
            {


                var arr = batch.ToArray();



                for (int i = 0; i < arr.Length; i++) { 
                

                 insertStr2.Add(string.Format("('{0}','{1}','{2}')", MySqlHelper.EscapeString(arr[i].name), MySqlHelper.EscapeString(arr[i].code), MySqlHelper.EscapeString(arr[i].qty)));

                }

                 ins2 = string.Format("INSERT  INTO `items`(`it_name`,`it_code`,`it_balance_qty`) VALUES {0};", String.Join(",", insertStr2));


                var affff =  connClass.nQuery(ins2);

                Console.WriteLine(ins2);

                ins2 = "";
                insertStr2.Clear();


            }
          

            DateTime date1 = DateTime.Now;
            string lstUpdate = String.Format("UPDATE `config` SET `ke` ='{0}';", MySqlHelper.EscapeString(date1.ToString()));

            connClass.nQuery(lstUpdate);


        }




        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> list, int chunkSize)
        {
            if (chunkSize <= 0)
            {
                throw new ArgumentException("chunkSize must be greater than 0.");
            }

            while (list.Any())
            {
                yield return list.Take(chunkSize);
                list = list.Skip(chunkSize);
            }
        }






        // مسح البيانات منسيرفر

        public static  int truncatDataServer()
        {




            ConnectionStringClass connStr = new ConnectionStringClass();
            string conStr = connStr.connectionServerSetting();
            MysqlConnectionClass connClass = new MysqlConnectionClass(conStr);

            int q = connClass.nQuery("TRUNCATE  items;");

            return q;
        }




        public static List<List<T>> SplitList<T>(this List<T> me, int size = 50)
        {
            var list = new List<List<T>>();
            for (int i = 0; i < me.Count; i += size)
                list.Add(me.GetRange(i, Math.Min(size, me.Count - i)));
            return list;
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
