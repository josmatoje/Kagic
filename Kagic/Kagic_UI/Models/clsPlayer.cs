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
        List<clsCreature> placeCriatures;
        clsCard selectedCard;
        clsCreature selectedCriature;

        #endregion

        #region constant
        public const int MAX_MANA = 10;
        public const int MAX_LIFE = 10;
        public const int MAX_HAND_CARDS = 7;
        public const int MAX_PLACE_CRIATURES = 5;
        #endregion

        #region constructor
        //Parametrized constructor
        public clsPlayer(List<clsCard> deck)
        {
            this.life = MAX_LIFE;
            this.totalMana = 0;
            this.usedMana = 0;
            this.deck = deck;
            InitialHand();
            this.placeCriatures = new List<clsCreature>(MAX_PLACE_CRIATURES);
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
        public List<clsCreature> PlaceCreatures { get => placeCriatures; set => placeCriatures = value; }
        public clsCard SelectedCard { get => selectedCard; set => selectedCard = value; }
        public clsCriature SelectedCreature { get => selectedCriature; set => selectedCriature = value; }

        #endregion

        #region public methods
        /// <summary>
        /// <b>Headboard: </b>private void DrawCard()<br/>
        /// <b>Description: </b>This method add a card to the hand each turn, if deck is empty, life goes down<br/>
        /// <b>Preconditions: </b> Anyone<br/>
        /// <b>Postconditions: </b> Hand updated<br/>
        /// </summary>
        public void DrawCard()
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
        /// <summary>
        /// <b>Headboard: </b>private void PutCard()<br/>
        /// <b>Description: </b>This method add a card to the hand each turn, if deck is empty, life goes down<br/>
        /// <b>Preconditions: </b> Anyone<br/>
        /// <b>Postconditions: </b> Hand and field updated<br/>
        /// </summary>
        public void PutCard()
        {

        }
        /// <summary>
        /// <b>Headboard: </b>public void SetUsedCriatures()<br/>
        /// <b>Description: </b>Set all the creatures on the field to unsed (used = false)<br/>
        /// <b>Preconditions: </b> Anyone<br/>
        /// <b>Postconditions: </b> Criatures are updated<br/>
        /// </summary>
        public void SetUsedCriatures()
        {

        }
        #endregion

        #region private methods
        /// <summary>
        /// <b>Headboard: </b>private void InitialHand()<br/>
        /// <b>Description: </b>This method add to the hand the first 3 cards of the deck<br/>
        /// <b>Preconditions: </b> Anyone<br/>
        /// <b>Postconditions: </b> Hand updated<br/>
        /// </summary>
        private void InitialHand()
        {
            this.hand = new List<clsCard>();
            for(int i=0; i<3; i++)
            {
                hand.Add(deck[0]);
                deck.RemoveAt(0);
            }
        }
        #endregion

    }
}
