using Kagic_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kagic_UI.Models.Utilities
{
    public class clsIAPlayer : clsPlayer
    {
        /// <summary>
        /// <b>Headboard: </b>public bool SelectHandCard()<br/>
        /// <b>Description: </b>This method select the first card in the hand that could be used and returns a boolean.<br/>
        /// <b>Preconditions: </b>Hand must be different of null<br/>
        /// <b>Postconditions: </b>Selected card can be played with the available mana<br/>
        /// </summary>
        /// <returns>bool: true if a card has been selected</returns>
        public bool SelectHandCard()
        {
            for (int i = 0; i<Hand.Count || SelectedCard==-1; i++)
            {
                if (Hand[i].Manacost < TotalMana - UsedMana)
                    SelectedCard = i;
            }
            return SelectedCard !=-1; //Devuelve true si alguna carta ha sido seleccionadad
        }

        //TODO cambiar summary
        /// <summary>
        /// <b>Headboard: </b>public int PickCreature()<br/>
        /// <b>Description: </b>This method select the target of the enemy creature<br/>
        /// <b>Preconditions: </b>selected card must be different of null<br/>
        /// <b>Postconditions: </b> <br/>
        /// </summary>
        /// <returns>int indicating de position of the target</returns>
        public bool PickCreature()
        {
            bool picked = false;
            for(int i = 0; i<PlaceCreatures.Count || !picked; i++)
            {
                if (!PlaceCreatures[i].Used)
                {    
                    SelectedCard = i;
                    picked = true;
                }
            }

            return picked;
        }

        //TODO cambiar summary
        /// <summary>
        /// <b>Headboard: </b>public int PickEnemyCreature(List<clsCreature> enemyCreatures)<br/>
        /// <b>Description: </b>This method select the target of the enemy creature <br/>
        /// <b>Preconditions: </b>selected card must be different of null<br/>
        /// <b>Postconditions: </b> <br/>
        /// </summary>
        /// <returns>int indicating de position of the target</returns>
        public int PickEnemyCreature(List<clsCreature> enemyCreatures)
        {
            int atackPlace = -1; //Posición a la que va a atacar
            for (int i = 0; i < enemyCreatures.Count; i++)
            {
                //TODO Ataca de izquierda a derecha, mejorar valorando las criaturas que no han atacado los ataques de tus propias criaturas y las vidas de las criaturas enemigas
                if (enemyCreatures[i] != null)
                {
                    atackPlace = i;
                }
            }

            return atackPlace;
        }
    }
}
