using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace SpecialistGUI
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

        static public ulong getLastID(String chatID)
        {
            String sqlQuery;
            if (!isOpen)
                return 0;
            try
            {
                sqlQuery = "SELECT MAX(id) FROM messages";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows[0].ItemArray[0].ToString() == "") return 0;
                return Convert.ToUInt64(dt.Rows[0].ItemArray[0].ToString());
            }
            catch (SQLiteException ex)
            { Console.WriteLine("Error: " + ex.Message); }

            return 0;
        }

        static public ulong putMessage(String mess, String chatID)
        {
            String sqlQuery;
            if (!isOpen)
                return 0;

            try
            {
                SQLiteCommand cmd = new SQLiteCommand("insert into messages (message,chatID,isUsers) values('" + mess + "','" + chatID + "', '0')");
                cmd.Connection = m_dbConn;
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            { Console.WriteLine("Error: " + ex.Message); }

            try
            {
                sqlQuery = "SELECT MAX(id) FROM messages";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return ((ulong)Convert.ToInt32(dt.Rows[0].ItemArray[0].ToString()));
            }
            catch (SQLiteException ex)
            { Console.WriteLine("Error: " + ex.Message); }
            return 0;
        }

        static public List<ChatMess> getMessages(String chatID, String id)
        {
            DataTable dTable = new DataTable();
            String sqlQuery;
            List<ChatMess> list = null;
            if (!isOpen)
                return list;

            try
            {
                sqlQuery = "select * from messages where chatID = '" + chatID + "' and id > " + id + " and isUsers = 1";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                adapter.Fill(dTable);
                list = new List<ChatMess>();
                for (int i = 0; i < dTable.Rows.Count; i++)
                {
                    list.Add(new ChatMess(dTable.Rows[i].ItemArray[0].ToString(),
                        dTable.Rows[i].ItemArray[1].ToString(),
                        dTable.Rows[i].ItemArray[2].ToString(),
                        dTable.Rows[i].ItemArray[3].ToString()));
                }
            }

            catch (SQLiteException ex)
            { Console.WriteLine("Error: " + ex.Message); }
            return list;
        }

        static public List<String> getChatIDs()
        {
            DataTable dTable = new DataTable();
            String sqlQuery;
            List<String> FullID = null;
            if (!isOpen)
                return FullID;
            try
            {
                sqlQuery = "SELECT * FROM queries where status = 0";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                adapter.Fill(dTable);
                FullID = new List<String>();
                for (int i = 0; i < dTable.Rows.Count; i++)
                {
                    FullID.Add(dTable.Rows[i].ItemArray[1].ToString());
                }
            }
            catch (SQLiteException ex)
            { Console.WriteLine("Error: " + ex.Message); }

            return FullID;
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
                        dTable.Rows[i].ItemArray[3].ToString()));
                }
            }
            catch (SQLiteException ex)
            { Console.WriteLine("Error: " + ex.Message); }
            return list;
        }

        static public void putQuery(String chatID)
        {
            String sqlQuery;
            if (!isOpen)
                return;

            try
            {
                SQLiteCommand cmd = new SQLiteCommand("insert into queries (chatID, status) values('" + chatID + "'," + 0 + ")");
                cmd.Connection = m_dbConn;
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            { Console.WriteLine("Error: " + ex.Message); }
        }

        static public int checkQuery(String chatID)
        {
            String sqlQuery;
            if (!isOpen)
                return -1;

            try
            {
                sqlQuery = "SELECT status FROM queries where chatID = '" + chatID + "'";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return (dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0].ItemArray[0]) : -1);
            }
            catch (SQLiteException ex)
            { Console.WriteLine("Error: " + ex.Message); }

            return -1;
        }

        static public List<ulong> getQueries()
        {
            DataTable dTable = new DataTable();
            String sqlQuery;
            List<ulong> list = null;
            if (!isOpen)
                return list;

            try
            {
                sqlQuery = "select chatID from queries where status = 0";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                adapter.Fill(dTable);
                list = new List<ulong>();
                for (int i = 0; i < dTable.Rows.Count; i++)
                {
                    list.Add(Convert.ToUInt64(dTable.Rows[i].ItemArray[0].ToString()));
                }
            }
            catch (SQLiteException ex)
            { Console.WriteLine("Error: " + ex.Message); }
            return list;
        }

        static public void acceptQuery(String chatID)
        {
            if (!isOpen)
                return;

            try
            {
                SQLiteCommand cmd = new SQLiteCommand("UPDATE queries SET status = 1 WHERE chatID = '" + chatID + "'");
                cmd.Connection = m_dbConn;
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            { Console.WriteLine("Error: " + ex.Message); }
        }

        static public void clearQuery(String chatID)
        {
            if (!isOpen)
                return;

            try
            {
                SQLiteCommand cmd = new SQLiteCommand("DELETE FROM queries WHERE chatID = '" + chatID + "'");
                cmd.Connection = m_dbConn;
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            { Console.WriteLine("Error: " + ex.Message); }
        }
    }
}