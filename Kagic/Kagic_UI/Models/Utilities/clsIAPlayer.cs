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
        /// Selecciona la primera carta que pueda usar la mano de la IA y devuelve true si ha seleccionado alguna y false en caso contrario
        /// </summary>
        public bool SelectHandCard()
        {
            SelectedCard = null;
            for (int i = 0; i<Hand.Count || SelectedCard is null; i++)
            {
                if (Hand[i].Manacost < TotalMana - UsedMana)
                {
                    SelectedCard = Hand[i];
                    UsedMana+=Hand[i].Manacost;
                }
            }
            return SelectedCard is null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="positionCriature">The index of the placedCriatures list for the criature that is attacking</param>
        /// <param name="enemyCriatures"></param>
        /// <returns></returns>
        public int AtackCriatures (int positionCriature,List<clsCriature> enemyCriatures)
        {
            int enemyIndex;
            int atackPlace=-1; //Posición a la que va a atacar
            for(int i = 0; i<PlaceCriatures.Count; i++)
            {
                if (!PlaceCriatures[i].Used)
                {
                    enemyIndex = 0;
                    //TODO Ataca de izquierda a derecha, mejorar valorando las criaturas que no han atacado los ataques de tus propias criaturas y las vidas de las criaturas enemigas
                    if (enemyCriatures[enemyIndex] != null)
                    {
                        atackPlace = enemyIndex;
                    }
                    enemyIndex++;
                }
            }

            return atackPlace;
        }
    }
}
