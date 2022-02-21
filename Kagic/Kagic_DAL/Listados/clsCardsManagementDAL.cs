using Kagic_Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using Windows.Storage;

namespace Kagic_DAL.Listados
{
    public class clsCardsManagementDAL
    {
        private static readonly string dbName = "Kagic.sqlite";
        private static string path = ApplicationData.Current.LocalFolder.Path + $"\\{dbName}";
        private static readonly SQLiteConnection myConnection = new SQLiteConnection($"Data Source={path}; version=3;");

        public static List<clsCard> getCardsDAL()
        {
            List<clsCard> cards = new List<clsCard>();

            getCreatureCards().ForEach(card => cards.Add(card));
            getSpellCards().ForEach(card => cards.Add(card));

            return cards;
        }

        private static List<clsCard> getCreatureCards()
        {
            List<clsCard> cardList = new List<clsCard>();
            clsCard card;
            SQLiteDataReader reader;
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM CreatureCards");
            try
            {
                myConnection.Open();
                command.Connection = myConnection;
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        card = constructCreatureCard(reader);
                        cardList.Add(card);
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }
            return cardList;
        }

        private static List<clsCard> getSpellCards()
        {
            List<clsCard> cardList = new List<clsCard>();
            clsCard card;
            SQLiteDataReader reader;
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM SpellCards");
            try
            {
                myConnection.Open();
                command.Connection = myConnection;
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        card = constructSpellCard(reader);
                        cardList.Add(card);
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                myConnection.Close();
            }
            return cardList;
        }

        private static clsCreature constructCreatureCard(SQLiteDataReader reader)
        {
            clsCreature constructedCreature = new clsCreature();

            constructedCreature.Id = Convert.ToInt32(reader["Id"]);
            constructedCreature.Name = (string)reader["Name"];
            constructedCreature.Description = (string)reader["Description"];
            constructedCreature.Image = (string)reader["Image"];
            constructedCreature.Manacost = Convert.ToInt32(reader["ManaCost"]);
            constructedCreature.Life = Convert.ToInt32(reader["Life"]);
            constructedCreature.Attack = Convert.ToInt32(reader["Attack"]);

            return constructedCreature;
        }

        private static clsCard constructSpellCard(SQLiteDataReader reader)
        {
            clsLifeModifyingSpell constructedSpell = new clsLifeModifyingSpell();

            constructedSpell.Id = Convert.ToInt32(reader["Id"]);
            constructedSpell.Name = (string)reader["Name"];
            constructedSpell.Description = (string)reader["Description"];
            constructedSpell.Image = (string)reader["Image"];
            constructedSpell.Manacost = Convert.ToInt32(reader["ManaCost"]);
            constructedSpell.Effect = Convert.ToInt32(reader["Effect"]);
            constructedSpell.IsDamage = Convert.ToInt32(reader["IsDamage"]) == 1; //Si es igual a 1 la comparación devuelve true (1 = true)
            constructedSpell.IsArea = Convert.ToInt32(reader["IsArea"]) == 1;

            return constructedSpell;
        }

    }
}