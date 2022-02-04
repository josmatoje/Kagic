using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;
using Windows.Storage;

namespace Kagic_DAL.Database
{
    public class DDL
    {
        //private static string DBName = ApplicationData.Current.LocalFolder.Path + @"\Kagic.db";
        private static SQLiteConnection myconnection = new SQLiteConnection("Kagic.sqlite", true);

        public static void createDatabase()
        {
            try
            {
                //System.Data.SQLite.SQLiteConnection.CreateFile(myconnection);
                myconnection.Open();
                createTables();
                fillDatabase();
                myconnection.Close();
            }
            catch
            {
                throw;
            }
        }

        public static void fillDatabase()
        {

            SqliteCommand countCreatures = new SqliteCommand("SELECT COUNT(*) FROM \"CreatureCard\"");
            SqliteCommand countSpells = new SqliteCommand("SELECT COUNT(*) FROM \"SpellCard\"");

            if (countCreatures.ExecuteNonQuery() <= 0)
            {
                fillTableCreatures();
            }
            if (countSpells.ExecuteNonQuery() <= 0)
            {
                fillTableSpells();
            }

        }

        private static void fillTableCreatures()
        {
            SqliteCommand createCreatureCard = new SqliteCommand("INSERT INTO \"CreatureCard\" (Name, Description, Image, ManaCost, Life, Attack) " +
                                                                 "VALUES (\'Gatete Solar\', \'Dispara fuego por las orejas\', \'\\Assets\\PRUEBAS\\solar_kitten.jpg\', 3, 3, 3 )");

            createCreatureCard.ExecuteNonQuery();
        }

        private static void fillTableSpells()
        {
            SqliteCommand createSpellCard = new SqliteCommand("INSERT INTO \"SpellCard\" (Name, Description, Image, ManaCost) " +
                                                              "VALUES (\'Seta Venenosa\',\'Envenena a la criatura objetivo\', \'\\Assets\\PRUEBAS\\CartaSeta.png\', 4, 4, 1, 0)");
            createSpellCard.ExecuteNonQuery();
        }

        private static void createTables()
        {
            SqliteCommand createCreatureCard = new SqliteCommand("CREATE TABLE IF NOT EXISTS \"CreatureCard\"(" +
                                                                    "[Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                                                                    "[Name] VARCHAR(50) NOT NULL," +
                                                                    "[Description] VARCHAR(255) NULL," +
                                                                    "[Image] TEXT NOT NULL," +
                                                                    "[ManaCost] INTEGER NOT NULL," +
                                                                    "[Life] INTEGER NOT NULL," +
                                                                    "[Attack] INTEGER NOT NULL");

            SqliteCommand createSpellCard = new SqliteCommand("CREATE TABLE IF NOT EXISTS SpellCard(" +
                                                                "[Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                                                                "[Name] VARCHAR(50) NOT NULL," +
                                                                "[Description] VARCHAR(255) NULL," +
                                                                "[Image] TEXT NOT NULL," +
                                                                "[ManaCost] INTEGER NOT NULL," +
                                                                "[Effect] INTEGER NOT NULL," +
                                                                "[IsDamage] INTEGER NOT NULL," +
                                                                "[IsArea] INTEGER NOT NULL");

            createCreatureCard.ExecuteNonQuery();
            createSpellCard.ExecuteNonQuery();
        }

        //public static void createDatabase()
        //{

        //    SQLiteConnection sqlite_conn;
        //    // Create a new database connection:
        //    sqlite_conn = new SQLiteConnection("Data Source = kagic.db; Version = 3; New = True; Compress = True; ");
        // // Open the connection:
        // try
        //    {
        //        sqlite_conn.Open();
        //        sqlite_conn.Close();
        //    }
        //    catch
        //    {
        //        throw;
        //    }

        //}

        //static void CreateTable(SQLiteConnection conn)
        //{

        //    SQLiteCommand sqlite_cmd;
        //    string Createsql = "CREATE TABLE SampleTable (Col1 VARCHAR(20), Col2 INT)";
        //   string Createsql1 = "CREATE TABLE SampleTable1 (Col1 VARCHAR(20), Col2 INT)";
        //   sqlite_cmd = conn.CreateCommand();
        //    sqlite_cmd.CommandText = Createsql;
        //    sqlite_cmd.ExecuteNonQuery();
        //    sqlite_cmd.CommandText = Createsql1;
        //    sqlite_cmd.ExecuteNonQuery();

        //}

        //static void InsertData(SQLiteConnection conn)
        //{
        //    SQLiteCommand sqlite_cmd;
        //    sqlite_cmd = conn.CreateCommand();
        //    sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test Text ', 1); ";
        //   sqlite_cmd.ExecuteNonQuery();
        //    sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test1 Text1 ', 2); ";
        //   sqlite_cmd.ExecuteNonQuery();
        //    sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test2 Text2 ', 3); ";
        //   sqlite_cmd.ExecuteNonQuery();
        //    sqlite_cmd.CommandText = "INSERT INTO SampleTable1 (Col1, Col2) VALUES('Test3 Text3 ', 3); ";
        //   sqlite_cmd.ExecuteNonQuery();

        //}

        //static void ReadData(SQLiteConnection conn)
        //{
        //    SQLiteDataReader sqlite_datareader;
        //    SQLiteCommand sqlite_cmd;
        //    sqlite_cmd = conn.CreateCommand();
        //    sqlite_cmd.CommandText = "SELECT * FROM SampleTable";

        //    sqlite_datareader = sqlite_cmd.ExecuteReader();
        //    while (sqlite_datareader.Read())
        //    {
        //        string myreader = sqlite_datareader.GetString(0);
        //        Console.WriteLine(myreader);
        //    }
        //    conn.Close();
        //}
    }
}
