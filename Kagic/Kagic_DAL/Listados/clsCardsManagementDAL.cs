using Kagic_Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using Windows.Storage;
using System.Collections.ObjectModel;

namespace Kagic_DAL.Listados
{
    public class clsCardsManagementDAL
    {
        private static readonly string dbName = "Kagic.sqlite";
        private static string path = ApplicationData.Current.LocalFolder.Path + $"\\{dbName}";
        private static readonly SQLiteConnection myConnection = new SQLiteConnection($"Data Source={path}; version=3;");

        public static ObservableCollection<clsCard> getCardsDAL()
        {
            ObservableCollection<clsCard> cards = new ObservableCollection<clsCard>();

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
            return new clsCreature( Convert.ToInt32(reader["Id"]),
                                    (string)reader["Name"],
                                    (string)reader["Description"],
                                    (string)reader["Image"],
                                    Convert.ToInt32(reader["ManaCost"]),
                                    Convert.ToInt32(reader["Life"]),
                                    Convert.ToInt32(reader["Attack"]));
        }

        private static clsCard constructSpellCard(SQLiteDataReader reader)
        {
            return new clsLifeModifyingSpell(Convert.ToInt32(reader["Id"]),
                                            (string)reader["Name"],
                                            (string)reader["Description"],
                                            (string)reader["Image"],
                                            Convert.ToInt32(reader["ManaCost"]),
                                            Convert.ToInt32(reader["Effect"]),
                                            Convert.ToInt32(reader["IsDamage"]) == 1, //Si es igual a 1 la comparación devuelve true (1 = true)
                                            Convert.ToInt32(reader["IsArea"]) == 1);
        }

    }
}