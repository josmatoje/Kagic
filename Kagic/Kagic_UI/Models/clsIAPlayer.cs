using Kagic_Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kagic_UI.Models.Utilities
{
    public class clsIAPlayer : clsPlayer
    {
        #region constructor
        public clsIAPlayer(List<clsCard> deck) : base(deck) { }
        #endregion

        #region public methods
        /// <summary>
        /// <b>Headboard: </b>public bool SelectHandCard()<br/>
        /// <b>Description: </b>This method select the first card in the hand that could be used and returns a boolean.<br/>
        /// <b>Preconditions: </b>Hand must be different of null<br/>
        /// <b>Postconditions: </b>Selected card can be played with the available mana<br/>
        /// </summary>
        /// <returns>bool: true if a card has been selected</returns>
        public bool SelectHandCard()
        {
            selectedCardIndex = -1;
            for (int i = 0; i < Hand.Count && selectedCardIndex == -1; i++)
            {
                if (Hand[i].IsAvaible)
                    selectedCardIndex = i;
            }
            return selectedCardIndex > -1; //Devuelve true si alguna carta ha sido seleccionada
        }

        /// <summary>
        /// <b>Headboard: </b>public int PickPlace()<br/>
        /// <b>Description: </b>This method select where to place a selected card on the battlefield<br/>
        /// <b>Preconditions: </b> None<br/>
        /// <b>Postconditions: </b> Selected creature has change if the return is true<br/>
        /// </summary>
        /// <returns>int indicating de position of the target</returns>
        public bool PickPlace()
        {
            bool picked = false;
            for (int i = 0; i < placeCreatures.Count && !picked; i++)
            {
                if (placeCreatures[i].Id == 0)
                {
                    selectedCreatureIndex = i;
                    picked = true;
                }
            }
            return picked;
        }

        /// <summary>
        /// <b>Headboard: </b>public int PickOwnCreature()<br/>
        /// <b>Description: </b>This method select a Creature of your own batelfield<br/>
        /// <b>Preconditions: </b> None<br/>
        /// <b>Postconditions: </b> The picked creature exist on the battelfield<br/>
        /// </summary>
        /// <returns>boolean indicating if the method has picked </returns>
        public bool PickOwnCreature()
        {
            bool picked = false;
            selectedCreatureIndex = -1;
            for (int i = 0; i < placeCreatures.Count && !picked; i++)
            {
                if (!placeCreatures[i].Used) //PlaceCreatures[i].Id != 0 && <-- Id==0 implica que es una criatura por defecto y que used es true
                {
                    selectedCreatureIndex = i;
                    picked = true;
                }
            }

            return picked;
        }

        /// <summary>
        /// <b>Headboard: </b>public int PickEnemyCreature(List<clsCreature> enemyCreatures)<br/>
        /// <b>Description: </b>This method select the target of the enemy creature <br/>
        /// <b>Preconditions: </b>selected card must be different of null<br/>
        /// <b>Postconditions: </b> <br/>
        /// </summary>
        /// <returns>int indicating de position of the target or -1 if ther is none enemy creatures</returns>
        public int PickEnemyCreature(ObservableCollection<clsCreatureNotified> enemyCreatures)//-------------------------------------------------------------------------------
        {
            int atackPlace = -1; //Posición a la que va a atacar
            for (int i = 0; i < enemyCreatures.Count && atackPlace == -1; i++)
            {
                //TODO Ataca de izquierda a derecha, mejorar valorando las criaturas que no han atacado los ataques de tus propias criaturas y las vidas de las criaturas enemigas
                if (enemyCreatures[i] != null && enemyCreatures[i].Id != 0)
                {
                    atackPlace = i;
                }
            }

            return atackPlace;
        }
        #endregion
    }
}