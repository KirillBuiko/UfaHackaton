using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using WindowsFormsApp1;

namespace MedBot_Test
{
    static class DBClass
    {
        public static SQLiteConnection m_dbConn;
        private static bool isOpen = false;

        public static void connect(String path)
        {
            if (isOpen) return;
            m_dbConn = new SQLiteConnection();

            if (!File.Exists(path))
                SQLiteConnection.CreateFile(path);

            try
            {
                m_dbConn = new SQLiteConnection("Data Source=" + path + ";Version=3;");
                m_dbConn.Open();

                //m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS Catalog (id INTEGER PRIMARY KEY AUTOINCREMENT, author TEXT, book TEXT)";
                //m_sqlCmd.ExecuteNonQuery();

                Console.WriteLine("DB connected!");
                isOpen = true;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("Error: DB not connected!");
            }
        }

        public static void close()
        {
            if (!isOpen) return;
            try
            {
                m_dbConn.Close();
                isOpen = false;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("Error: DB not closed!");
            }
        }

        static public List<Comand> getComands(String parentID)
        {
            DataTable dTable = new DataTable();
            String sqlQuery;
            List<Comand> list = null;
            if (!isOpen)
                return list;

            try
            {
                sqlQuery = "SELECT * FROM main where ParentID = '" + parentID + "'";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                adapter.Fill(dTable);
                list = new List<Comand>();
                for (int i = 0; i < dTable.Rows.Count; i++)
                {
                    list.Add(new Comand(dTable.Rows[i].ItemArray[0].ToString(),
                        dTable.Rows[i].ItemArray[1].ToString(),
                        dTable.Rows[i].ItemArray[2].ToString(),
                        dTable.Rows[i].ItemArray[3].ToString(),
                        dTable.Rows[i].ItemArray[4].ToString()));
                }
            }
            catch (SQLiteException ex)
            { Console.WriteLine("Error: " + ex.Message); }
            return list;
        }

        static public Comand getComand(String id)
        {
            DataTable dTable = new DataTable();
            String sqlQuery;
            Comand list = null;
            if (!isOpen)
                return list;

            try
            {
                sqlQuery = "SELECT * FROM main where id = '" + id + "'";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                    list = new Comand(dTable.Rows[0].ItemArray[0].ToString(),
                            dTable.Rows[0].ItemArray[1].ToString(),
                            dTable.Rows[0].ItemArray[2].ToString(),
                            dTable.Rows[0].ItemArray[3].ToString(),
                            dTable.Rows[0].ItemArray[4].ToString());
            }
            catch (SQLiteException ex)
            { Console.WriteLine("Error: " + ex.Message); }
            return list;
        }

        static public ulong putMessage(String mess, String chatID)
        {
            String sqlQuery;
            if (!isOpen)
                return 0;

            try
            {
                SQLiteCommand cmd = new SQLiteCommand("insert into messages (message,chatID,isUsers) values('" + mess + "','" + chatID + "', '1')");
                cmd.Connection = m_dbConn;
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            { Console.WriteLine("Error: " + ex.Message); }

            try
            {
                sqlQuery = "SELECT MAX(`id`) FROM messages";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return ((ulong)Convert.ToInt32(dt.Rows[0].ItemArray[0].ToString()));
            }
            catch (SQLiteException ex)
            { Console.WriteLine("Error: " + ex.Message); }
            return 0;
        }

        static public void putComand(Comand comand)
        {
            if (!isOpen)
                return;

            try
            {
                SQLiteCommand cmd = new SQLiteCommand("insert into main (Name, childID, ParentID, type) values('" + comand.Text + "', '','" + comand.ParentID + "', '" + comand.Type + "')");
                cmd.Connection = m_dbConn;
                cmd.ExecuteNonQuery();
                cmd.CommandText = ("update main set childID = childID || " + comand.Id +" || ';' where id = '" + comand.ParentID + "'");
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            { Console.WriteLine("Error: " + ex.Message); }
        }
    }
}