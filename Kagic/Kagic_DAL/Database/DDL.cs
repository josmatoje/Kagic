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
        #region Database path
        //private static string DBName = ApplicationData.Current.LocalFolder.Path + @"\Kagic.db";
        //private static SQLiteConnection myconnection = new SQLiteConnection("Kagic.sqlite", true);
        private static readonly string dbName = "Kagic.sqlite";
        private static string path = ApplicationData.Current.LocalFolder.Path + $"\\{dbName}";
        #endregion

        #region Database creation

        /// <summary>
        /// <b>Headboard: </b>public static void createDatabase()<br/>
        /// <b>Description: </b>This method creates an SQLite Database in local storage filling it with data.<br/>
        /// <b>Preconditions: </b>None<br/>
        /// <b>Postconditions: </b> Database created and data-filled.<br/>
        /// </summary>
        public static void createDatabase()
        {
            try
            {
                if (!File.Exists(path))
                {
                    SQLiteConnection.CreateFile(path);
                }
                SQLiteConnection myconnection = new SQLiteConnection($"Data Source={path}; version=3;");
                myconnection.Open();
                createTables(myconnection);
                fillDatabase(myconnection);
                myconnection.Close();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Database Tables

        /// <summary>
        /// <b>Headboard: </b>private static void fillDatabase(SQLiteConnection myconnection)<br/>
        /// <b>Description: </b>This method checks that the tables of an SQLite Database are empty and fills them with data.<br/>
        /// <b>Preconditions: </b>SQLiteConnection must exists and must be opened.<br/>
        /// <b>Postconditions: </b>Database filled with data.<br/>
        /// </summary>
        /// <param name="myconnection"></param>
        public static void fillDatabase(SQLiteConnection myconnection)
        {

            SQLiteCommand countCreatures = new SQLiteCommand("SELECT COUNT(*) FROM CreatureCards", myconnection);
            SQLiteCommand countSpells = new SQLiteCommand("SELECT COUNT(*) FROM SpellCards", myconnection);

            if (Convert.ToInt32(countCreatures.ExecuteScalar()) <= 0)
            {
                fillTableCreatures(myconnection);
            }
            if (Convert.ToInt32(countSpells.ExecuteScalar()) <= 0)
            {
                fillTableSpells(myconnection);
            }
        }

        #region Data insertions

        /// <summary>
        /// <b>Headboard: </b>private static void fillTableCreatures(SQLiteConnection myconnection)<br/>
        /// <b>Description: </b>This method inserts data into a CreatureCards-named table from an SQLiteDatabase.<br/>
        /// <b>Preconditions: </b>SQLiteConnection must exists and must be opened.<br/>
        /// <b>Postconditions: </b>CreatureCards table filled with data.<br/>
        /// </summary>
        /// <param name="myconnection"></param>
        private static void fillTableCreatures(SQLiteConnection myconnection)
        {                                           // partes de clsCreature(string name, string description, string image, int manacost, int life, int attack)
            SQLiteCommand createCreatureCard = new SQLiteCommand("INSERT INTO CreatureCards (Name, Description, Image, ManaCost, Life, Attack) " +
                                                                 "VALUES (\'Gatete Solar\', \'Dispara fuego por las orejas\', \'/Assets/PRUEBAS/solar_kitten.jpg\', 3, 3, 3 ), "
                                                                 + "(\'SHIFU\', \'EL GRAN SHIFU. Nerfeen al gato.\', \'/Assets/PRUEBAS/shifu.png\', 7, 9, 9 ), "
                                                                 + "(\'Abejita\', \'Cuidao que te pica\', \'/Assets/PRUEBAS/abeja.png\', 2, 1, 3 ), "
                                                                 + "(\'Bichote verde\', \'Da miedo\', \'/Assets/PRUEBAS/bichoverde.png\', 4, 2, 6 ), "
                                                                 + "(\'Gato gordo\', \'Es como shifu pero gordo y sin sombrero\', \'/Assets/PRUEBAS/gatogordo.png\', 5, 7, 4 ), "
                                                                 + "(\'Murciélago\', \'Rápido y furioso\', \'/Assets/PRUEBAS/murcielago.png\', 1, 1, 3 ), "
                                                                 + "(\'El pato\', \'Parece inofensivo pero cuidao\', \'/Assets/PRUEBAS/pato.png\', 3, 2, 5 ), "
                                                                 + "(\'Sirpiente\', \'Sigiloso y letal\', \'/Assets/PRUEBAS/serpiente.png\', 3, 1, 4 ), "
                                                                 + "(\'Don tortuga\', \'Lento pero agunta\', \'/Assets/PRUEBAS/tortuga.png\', 3, 8, 1 ) ",
                                                                 myconnection);

            createCreatureCard.ExecuteNonQuery();
        }

        /// <summary>
        /// <b>Headboard: </b>private static void fillTableSpells(SQLiteConnection myconnection)<br/>
        /// <b>Description: </b>This method inserts data into a SpellCards-named table from an SQLiteDatabase.<br/>
        /// <b>Preconditions: </b>SQLiteConnection must exists and must be opened.<br/>
        /// <b>Postconditions: </b>SpellCards table filled with data.<br/>
        /// </summary>
        /// <param name="myconnection"></param>
        private static void fillTableSpells(SQLiteConnection myconnection)
        {                                           //partes de clsLifeModifyingSpell(string name, string description, string image, int manacost, int effect, bool isDamage, bool isArea)
            SQLiteCommand createSpellCard = new SQLiteCommand("INSERT INTO SpellCards (Name, Description, Image, ManaCost, Effect, IsDamage, IsArea) " +
                                                              "VALUES (\'Seta Venenosa\',\'Envenena a la criatura objetivo\', \'/Assets/PRUEBAS/CartaSeta.png\', 4, 4, 1, 0), "
                                                               + "(\'Seta del Amor\',\'Cura a todas las criaturas con el poder del amor\', \'/Assets/PRUEBAS/CartaSeta.png\', 2, 2, 0, 1), "
                                                               + "(\'Bola de Fuegardo\',\'Pelota en llamas\', \'/Assets/PRUEBAS/CartaSeta.png\', 6, 6, 1, 0), "
                                                               + "(\'Lluvia de meteoros\',\'Arrasa con lo que veas... y generoso no seas!\', \'/Assets/PRUEBAS/CartaSeta.png\', 5, 3, 1, 1), "
                                                               + "(\'Canto de Jigglypuff\',\'AAAAHHHhhhzzzz...\', \'/Assets/PRUEBAS/CartaSeta.png\', 3, 1, 1, 1), "
                                                               + "(\'Canto de Sirena\',\'Revitaliza a todos tus oponentes\', \'/Assets/PRUEBAS/CartaSeta.png\', 4, 2, 0, 1), "
                                                               + "(\'Codigo no comentado\',\'Destroza a un objetivo sin summaries\', \'/Assets/PRUEBAS/CartaSeta.png\', 7, 7, 1, 0) ",
                                                              myconnection);
            createSpellCard.ExecuteNonQuery();
        }

        #endregion

        #region Tables creation

        /// <summary>
        /// <b>Headboard: </b>private static void createTables(SQLiteConnection myconnection)<br/>
        /// <b>Description: </b>This method creates two tables with their respective columns.<br/>
        /// <b>Preconditions: </b>SQLiteConnection must exists and must be opened.<br/>
        /// <b>Postconditions: </b>Tables CreatureCards and SpellCards are created in the Database.<br/>
        /// </summary>
        /// <param name="myconnection"></param>
        private static void createTables(SQLiteConnection myconnection)
        {
            SQLiteCommand createCreatureCard = new SQLiteCommand("CREATE TABLE IF NOT EXISTS CreatureCards(" +
                                                                    "[Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                                                                    "[Name] VARCHAR(50) NOT NULL," +
                                                                    "[Description] VARCHAR(255) NULL," +
                                                                    "[Image] TEXT NOT NULL," +
                                                                    "[ManaCost] INTEGER NOT NULL," +
                                                                    "[Life] INTEGER NOT NULL," +
                                                                    "[Attack] INTEGER NOT NULL)", myconnection);

            SQLiteCommand createSpellCard = new SQLiteCommand("CREATE TABLE IF NOT EXISTS SpellCards(" +
                                                                "[Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                                                                "[Name] VARCHAR(50) NOT NULL," +
                                                                "[Description] VARCHAR(255) NULL," +
                                                                "[Image] TEXT NOT NULL," +
                                                                "[ManaCost] INTEGER NOT NULL," +
                                                                "[Effect] INTEGER NOT NULL," +
                                                                "[IsDamage] INTEGER NOT NULL," +
                                                                "[IsArea] INTEGER NOT NULL)", myconnection);

            createCreatureCard.ExecuteNonQuery();
            createSpellCard.ExecuteNonQuery();
        }
        #endregion

        #endregion
    }
}
