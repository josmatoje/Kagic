using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Windows.Storage;

namespace Kagic_DAL.Database
{
    public class DDL
    {
        private static string DBName = ApplicationData.Current.LocalFolder.Path + @"\Kagic.db";
        private static SqliteConnection myconnection = new SqliteConnection("Kagic.sqlite");

        public static void createDatabase()
        {
            try
            {
                System.Data.SQLite.SQLiteConnection.CreateFile(DBName);
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

        public static void fillDatabase() {

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
    }
}
