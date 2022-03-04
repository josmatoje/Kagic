using Kagic_Entities;
using Kagic_UI.ViewModels.UtilitiesVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kagic_UI.Models
{
    public class clsPlayer : clsVMBase
    {
        #region atributes
        protected int life;
        protected int totalMana;
        protected int remainingMana;
        protected List<clsCard> deck;
        protected ObservableCollection<clsCard> hand;
        protected ObservableCollection<clsCreatureNotified> placeCreatures;
        protected int selectedCard;
        protected int selectedCreature;
        #endregion

        #region constant
        public const int MAX_MANA = 10;
        public const int MAX_LIFE = 20;
        public const int MAX_HAND_CARDS = 7;
        public const int MAX_PLACE_CREATURES = 5;
        #endregion

        #region constructor
        //Parametrized constructor
        public clsPlayer(List<clsCard> deck)
        {
            this.life = MAX_LIFE;
            this.totalMana = 10; //Modificado para pruebas CAMBIAR A 1!
            this.remainingMana = totalMana;
            this.deck = deck;
            InitialHand();
            this.placeCreatures = new ObservableCollection<clsCreatureNotified>();
            InitializePlaceCreatures();
            //SetAvaibleCards();
            this.selectedCard = -1;
            this.selectedCreature = -1;
        }

        //Default constructor
        public clsPlayer() { }
        #endregion

        #region public properties
        public int Life 
        { 
            get => life;
            set 
            { 
                life = value;
                NotifyPropertyChanged("Life");
            }
        }
        public int TotalMana 
        { 
            get => totalMana;
            set 
            {
                totalMana = value;
                NotifyPropertyChanged("TotalMana");
            }
        }
        public int RemainingMana { get => remainingMana; set => remainingMana = value; }
        public List<clsCard> Deck { get => deck; set => deck = value; }
        public ObservableCollection<clsCard> Hand { get => hand; set => hand = value; }
        public ObservableCollection<clsCreatureNotified> PlaceCreatures { get => placeCreatures; set => placeCreatures = value; }
        public int SelectedCard 
        { 
            get => selectedCard;
            set
            {
                selectedCard = value;
                NotifyPropertyChanged("SelectedCard");
            }
        }
        public int SelectedCreature 
        { 
            get => selectedCreature; 
            set 
            { 
                selectedCreature = value;
                NotifyPropertyChanged("SelectedCreature");
            }
        }
        public int ProgresBarLife { get => life * (100 / MAX_LIFE); }
        public int ProgresBarMana { get => remainingMana * (100 / MAX_MANA); }

        #endregion

        #region public methods
        /// <summary>
        /// <b>Headboard: </b>private void DrawCard()<br/>
        /// <b>Description: </b>This method add a card to the hand each turn, if deck is empty, life decrease<br/>
        /// <b>Preconditions: </b>None<br/>
        /// <b>Postconditions: </b>Hand updated<br/>
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
                    hand.Add(deck[0]);
                deck.RemoveAt(0);
            }
            SetAvaibleCards();
        }

        /// <summary>
        /// <b>Headboard: </b>private void PutCard()<br/>
        /// <b>Description: </b>This method use a card and update the usedMana. Also place a creature from the player hand to the batelfield if selected card is a creature.<br/>
        /// <b>Preconditions: </b>None<br/>
        /// <b>Postconditions: </b>Hand and field updated<br/>
        /// </summary>
        public void PutCard()
        {
            if (selectedCard > -1 && hand[selectedCard].IsAvaible)
            {
                if (hand[selectedCard] is clsCreatureNotified)
                {
                    placeCreatures[selectedCreature] =  hand[selectedCard] as clsCreatureNotified;
                }
                remainingMana -= hand[selectedCard].Manacost;
                NotifyPropertyChanged(nameof(RemainingMana));
                NotifyPropertyChanged(nameof(ProgresBarMana));
                hand.RemoveAt(selectedCard);
                SetAvaibleCards();
            }
        }

        /// <summary>
        /// <b>Headboard: </b>public void SetMana()<br/>
        /// <b>Description: </b>Increases the total mana to the limit and set the used to zero<br/>
        /// <b>Preconditions: </b>None<br/>
        /// <b>Postconditions: </b>Manas are updated<br/>
        /// </summary>
        public void SetMana()
        {
            if (totalMana < MAX_MANA)
            {
                totalMana++;
            }
            remainingMana = totalMana;
        }

        /// <summary>
        /// <b>Headboard: </b>public void SetUsedCreatures()<br/>
        /// <b>Description: </b>Set all the creatures on the field to unused (used = false)<br/>
        /// <b>Preconditions: </b>None<br/>
        /// <b>Postconditions: </b>Creatures are updated<br/>
        /// </summary>
        public void SetUsedCreatures()
        {
            foreach (clsCreature creature in placeCreatures)
            {
                if(creature != null && creature.Id != 0)
                    creature.Used = false;
            }
        }
        /// <summary>
        /// <b>Headboard: </b>public void SetAvaibleCards()<br/>
        /// <b>Description: </b>Change the availability of the hand cards in view of the actual mana<br/>
        /// <b>Preconditions: </b>None<br/>
        /// <b>Postconditions: </b>cards are updated<br/>
        /// </summary>
        public void SetAvaibleCards()
        {
            foreach (clsCard card in hand)
                card.IsAvaible = card.Manacost <= remainingMana;
        }
        #endregion

        #region private methods
        /// <summary>
        /// <b>Headboard: </b>private void InitialHand()<br/>
        /// <b>Description: </b>This method add to the hand the first 3 cards of the deck<br/>
        /// <b>Preconditions: </b>None<br/>
        /// <b>Postconditions: </b>Hand updated<br/>
        /// </summary>
        private void InitialHand()
        {
            this.hand = new ObservableCollection<clsCard>();
            for (int i = 0; i < 3; i++)
            {
                this.hand.Add(deck[0]);
                this.deck.RemoveAt(0);
            }
        }

        /// <summary>
        /// <b>Headboard: </b>private void InitializePlaceCreatures()<br/>
        /// <b>Description: </b>This method generate default creatures for the Place creatures list<br/>
        /// <b>Preconditions: </b>None<br/>
        /// <b>Postconditions: </b>None<br/>
        /// </summary>
        private void InitializePlaceCreatures()
        {
            for (int i = 0; i < MAX_PLACE_CREATURES; i++)
                placeCreatures.Add(new clsCreatureNotified());
        }
        #endregion

    }
}
