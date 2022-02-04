using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kagic_DAL.Database
{
    public class DDL
    {
        SqliteConnection myconnection = new SqliteConnection("Kagic.sqlite");

        public void createDatabase()
        {
            try
            {
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

        public void fillDatabase() {

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

        private void fillTableCreatures()
        {
            SqliteCommand createCreatureCard = new SqliteCommand("INSERT INTO \"CreatureCard \" VALUES ");
        }

        private void createTables()
        {
            SqliteCommand createCreatureCard = new SqliteCommand("CREATE TABLE IF NOT EXISTS \"CreatureCard\"(" +
                                                                    "[Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +                                                                   
                                                                    "[Name] VARCHAR(50) NOT NULL," +
                                                                    "[Description] VARCHAR(255) NULL," +
                                                                    "[Image] TEXT NOT NULL," +
                                                                    "[ManaCost] INTEGER NOT NULL," +
                                                                    "[Used] INTEGER NOT NULL," +
                                                                    "[Life] INTEGER NOT NULL," +
                                                                    "[ActualLife] INTEGER NOT NULL," +
                                                                    "[Attack] INTEGER NOT NULL");

            SqliteCommand createSpellCard = new SqliteCommand("CREATE TABLE IF NOT EXISTS SpellCard(" +
                                                                "[Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                                                                "[Name] VARCHAR(50) NOT NULL," +
                                                                "[Description] VARCHAR(255) NULL," +
                                                                "[Image] TEXT NOT NULL," +
                                                                "[ManaCost] INTEGER NOT NULL," +
                                                                "[Used] INTEGER NOT NULL");

            createCreatureCard.ExecuteNonQuery();
            createSpellCard.ExecuteNonQuery();
        }
    }
}
