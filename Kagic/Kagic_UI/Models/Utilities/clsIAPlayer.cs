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
        /// <b>Headboard: </b> public bool SelectHandCard()<br/>
        /// <b>Description: </b>This method select the first card in the hand that could be used and returns a boolean <br/>
        /// <b>Preconditions: </b> hand diferent of null<br/>
        /// <b>Postconditions: </b>Selected card can be played with the available mana<br/>
        /// </summary>
        /// <returns>bool: true if a card has selected</returns>
        public bool SelectHandCard()
        {
            SelectedCard = null;
            for (int i = 0; i<Hand.Count || SelectedCard is null; i++)
            {
                if (Hand[i].Manacost < TotalMana - UsedMana)
                {
                    SelectedCard = Hand[i];
                }
            }
            return ! (SelectedCard is null); //
        }

        //TODO cambiar summary
        /// <summary>
        /// <b>Headboard: </b>  public int PickEnemyCriature(List<clsCreature> enemyCriatures)<br/>
        /// <b>Description: </b>This method select the target of the enemy criature <br/>
        /// <b>Preconditions: </b> selected card different of null<br/>
        /// <b>Postconditions: </b> <br/>
        /// </summary>
        /// <returns>int indicating de position of the target</returns>
        public bool PickCriature()
        {
            bool picked = false;
            for(int i = 0; i<PlaceCreatures.Count || !picked; i++)
            {
                if (!PlaceCreatures[i].Used)
                {    
                    SelectedCard = PlaceCreatures [i];
                    picked = true;
                }
            }

            return picked;
        }

        //TODO cambiar summary
        /// <summary>
        /// <b>Headboard: </b>  public int PickEnemyCriature(List<clsCreature> enemyCriatures)<br/>
        /// <b>Description: </b>This method select the target of the enemy criature <br/>
        /// <b>Preconditions: </b> selected card different of null<br/>
        /// <b>Postconditions: </b> <br/>
        /// </summary>
        /// <returns>int indicating de position of the target</returns>
        public int PickEnemyCriature(List<clsCreature> enemyCriatures)
        {
            int atackPlace = -1; //Posición a la que va a atacar
            for (int i = 0; i < enemyCriatures.Count; i++)
            {
                //TODO Ataca de izquierda a derecha, mejorar valorando las criaturas que no han atacado los ataques de tus propias criaturas y las vidas de las criaturas enemigas
                if (enemyCriatures[i] != null)
                {
                    atackPlace = i;
                }
            }

            return atackPlace;
        }
    }
}
