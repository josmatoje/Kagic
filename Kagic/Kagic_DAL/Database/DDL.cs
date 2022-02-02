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
                myconnection.Close();
            }
            catch
            {
                throw;
            }
        }

        public void fillDatabase() {
            myconnection.Open();
            fillTables();
            myconnection.Close();
        }

        private void createTables()
        {
            SqliteCommand createCreatureCard = new SqliteCommand("CREATE TABLE IF NOT EXISTS \"CreatureCard\"(" +                                                                   
                                                                    "[Name] VARCHAR(50) NOT NULL," +
                                                                    "[Description] VARCHAR(255) NULL," +
                                                                    "[Image] TEXT NOT NULL," +
                                                                    "[ManaCost] INTEGER NOT NULL," +
                                                                    "[Used] INTEGER NOT NULL," +
                                                                    "[Life] INTEGER NOT NULL," +
                                                                    "[ActualLife] INTEGER NOT NULL," +
                                                                    "[Attack] INTEGER NOT NULL");

            SqliteCommand createSpellCard = new SqliteCommand("CREATE TABLE IF NOT EXISTS SpellCard(" +
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
