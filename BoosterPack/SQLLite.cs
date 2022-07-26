using System;
using System.Text;
using System.Data;
using System.Data.SQLite;

namespace BoosterPack
{
    public class DataClass
    {
        private SQLiteConnection sqlite;
        private bool _keepAlive;

        public DataClass(string path, bool keepAlive = false)
        {
            //This part killed me in the beginning.  I was specifying "DataSource"
            //instead of "Data Source"
            sqlite = new SQLiteConnection("Data Source=" + path);
            _keepAlive = keepAlive;
            sqlite.Open();  //Initiate connection to the db
        }

        public DataTable selectQuery(string query)
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();

            try
            {
                SQLiteCommand cmd;
                cmd = sqlite.CreateCommand();
                cmd.CommandText = query;  //set the passed query
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt); //fill the datasource
            }
            catch (SQLiteException)
            {
                //Add your exception code here.
            }
            if(!_keepAlive) sqlite.Close();
            return dt;
        }

        public void Close()
        {
            sqlite.Close();
        }
    }
}
