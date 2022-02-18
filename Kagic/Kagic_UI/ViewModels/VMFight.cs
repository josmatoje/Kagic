using Kagic_Entities;
using Kagic_UI.Models;
using Kagic_UI.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kagic_UI.ViewModels
{
    public class VMFight
    {

        #region atributes
        clsPlayer realPlayer;
        clsIAPlayer iaPlayer;
        //List<clsCard> playerDeck;
        //List<clsCard> iaDeck;
        #endregion

        #region constants
        const int DECK_SIZE = 20;
        const int MAX_REPETED_CARDS_DECK = 2;
        #endregion

        #region constructor
        public VMFight()
        {
            realPlayer = new clsPlayer();
            iaPlayer = new clsIAPlayer();
        }
        #endregion

        #region public properties

        #endregion

        #region private methods
        /// <summary>
        /// <b>Headboard: </b>private List<clsCard> CardsDeck(List<clsCard> cardsList)<br/>
        /// <b>Description: </b>This method generate a random deck that can contain only two copies of the same card<br/>
        /// <b>Preconditions: </b> none<br/>
        /// <b>Postconditions: </b> <br/>
        /// </summary>
        /// <param name="cardsList"></param>
        /// <returns></returns>
        private List<clsCard> CardsDeck(List<clsCard> cardsList)
        {
            Random random = new Random();
            List<clsCard> deck= new List<clsCard>();
            int position, counter;

            for(int i = 0; i < DECK_SIZE;)
            {
                position = random.Next(cardsList.Count);
                counter = 0;
                //foreach(clsCard card in deck)
                for(int j = 0; j < deck.Count && counter < MAX_REPETED_CARDS_DECK; j++)
                {
                    if(deck[j].GetType() == cardsList[i].GetType() && deck[j].Id == cardsList[position].Id) //Assess the type of de card and the id 
                    {
                        counter++;
                    }
                }
                if(counter < MAX_REPETED_CARDS_DECK)
                {
                    deck.Add(cardsList[i]);
                    i++;
                }
                
            }

            return deck;
        }
        #endregion

    }
}
