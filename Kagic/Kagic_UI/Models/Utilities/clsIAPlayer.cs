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
        /// Selecciona la criatura a la que va a atacar cada criatura y devuelve un array con las posiciones a las que desea atacar
        /// todo: muy lioso, selecciona un objetivo y devuelve enemigo seleccionado
        /// </summary>
        /// <param name="enemyCriatures"></param>
        public int[] AtackCriatures (List<clsCriature> enemyCriatures)
        {
            int j;
            int[] atackList = new int[MAX_PLACE_CRIATURES]; //Con el listado puedes atacar a un enemigo muerto(salvo que se valore el estado de la criatura),
                                                            //hacer selección individual
            for(int i = 0; i<PlaceCriatures.Count; i++)
            {
                if (!PlaceCriatures[i].Used)
                {
                    j = 0;
                    //TODO Ataca de izquierda a derecha
                    do
                    {
                        if (enemyCriatures[j] != null)
                        {
                            atackList[i] = j;
                        }
                        j++;
                    } while (j < enemyCriatures.Count);
                }
                else
                {
                    atackList[i] = -1;
                }
            }

            return atackList;
        }
    }
}
