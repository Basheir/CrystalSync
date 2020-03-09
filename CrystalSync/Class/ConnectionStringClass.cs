using System;
using System.Collections.Generic;
using CrystalSync.Setting;


namespace CrystalSync.Class
{
    class ConnectionStringClass
    {



        public string formatConnection(string dbServer, string dbPort, string dbName, string dbuserName, string dbPassword)
        {

            string str = String.Format("server={0};  Port={1}; database={2}; uid={3}; pwd={4};  Character Set=utf8; Convert Zero Datetime=True; SslMode=None", dbServer, dbPort, dbName, dbuserName, dbPassword);
            return str;
        }


        public string connectionLocalSetting()
        {

            string dbServer = MysqlConnectionLocalSettings.Default["dbServer"].ToString();
            string dbName = MysqlConnectionLocalSettings.Default["dbName"].ToString();
            string dbuserName = MysqlConnectionLocalSettings.Default["dbuserName"].ToString();
            string dbPassword = MysqlConnectionLocalSettings.Default["dbPassword"].ToString();
            string dbPort = MysqlConnectionLocalSettings.Default["dbPort"].ToString();
            string myConnectionString = this.formatConnection( dbServer, dbPort, dbName,dbuserName, dbPassword );

            return myConnectionString;
        }


        public string connectionServerSetting()
        {

            string dbServer = MysqlConnectionServerSettings.Default["dbServer"].ToString();
            string dbName = MysqlConnectionServerSettings.Default["dbName"].ToString();
            string dbuserName = MysqlConnectionServerSettings.Default["dbuserName"].ToString();
            string dbPassword = MysqlConnectionServerSettings.Default["dbPassword"].ToString();
            string dbPort = MysqlConnectionServerSettings.Default["dbPort"].ToString();

            string str = this.formatConnection(dbServer, dbPort, dbName, dbuserName, dbPassword);
            return str;
        }

    }
}
