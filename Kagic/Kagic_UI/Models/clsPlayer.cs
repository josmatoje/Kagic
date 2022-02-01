using Kagic_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kagic_UI.Models
{
    public class clsPlayer
    {
        #region atributes
        int life;
        int totalMana;
        int usedMana;
        List<clsCard> deck;
        List<clsCard> hand;
        List<clsCriature> placeCriatures;
        clsCard selectedCard;

        #endregion

        #region constant
        protected const int MAX_MANA = 10;
        protected const int MAX_LIFE= 10;
        protected const int MAX_HAND_CARDS = 7;
        protected const int MAX_PLACE_CRIATURES = 5;
        #endregion

        #region constructor
        //Parametrized constructor
        public clsPlayer(List<clsCard> deck)
        {
            this.life = MAX_LIFE;
            this.totalMana = 0;
            this.usedMana = 0;
            this.deck = deck;
            initialHand();
            this.placeCriatures = new List<clsCriature>(MAX_PLACE_CRIATURES);
            this.selectedCard = null;
        }

        //Default constructor
        public clsPlayer(){}
        #endregion

        #region public properties
        public int Life { get => life; set => life = value; }
        public int TotalMana { get => totalMana; set => totalMana = value; }
        public int UsedMana { get => usedMana; set => usedMana = value; }
        public List<clsCard> Deck { get => deck; set => deck = value; }
        public List<clsCard> Hand { get => hand; set => hand = value; }
        public List<clsCriature> PlaceCriatures { get => placeCriatures; set => placeCriatures = value; }
        public clsCard SelectedCard { get => selectedCard; set => selectedCard = value; }

        #endregion

        #region private methods
        private void InitialHand()
        {
            this.hand = new List<clsCard>();
            for(int i=0; i<3; i++)
            {
                hand.Add(deck[0]);
                deck.RemoveAt(0);
            }
        }

        private void DrawCard()
        {
            if (deck.Count == 0)
            {
                life--; //TODO incrementar la vida que se resta a medida que avanzan los turnos?
            }
            else
            { 
                if (hand.Count < MAX_HAND_CARDS)
                {
                    hand.Add(deck[0]);
                }
                deck.RemoveAt(0);
            }
        }
        #endregion

    }
}
