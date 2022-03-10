using Kagic_Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Kagic_DAL
{
    public class CardsManagementDAL
    {
        /// <summary>
        /// <b>Headboard: </b>public static List<clsCard> getCardsListDAL()<br/>
        /// <b>Description: </b>This method collect the cards from the database<br/>
        /// <b>Preconditions: </b> none<br/>
        /// <b>Postconditions: </b> you get a list of all the cards in the database<br/>
        /// </summary>
        /// <returns>List<clsCard></returns>
        public static ObservableCollection<clsCard> getCardsListDAL()
        {
            ObservableCollection<clsCard> cardsList = new ObservableCollection<clsCard>();

            cardsList.Add(new clsCreature(1,"Gatete Solar", "Dispara fuego por las orejas", "/Assets/PRUEBAS/solar_kitten.jpg", 3, 3, 3));
            cardsList.Add(new clsCreature(2, "SHIFU", "EL GRAN SHIFU. Nerfeen al gato", "/Assets/PRUEBAS/shifu.png", 7, 9, 9));
            cardsList.Add(new clsCreature(3, "Abejita", "Cuidao que te pica", "/Assets/PRUEBAS/abeja.png", 2, 1, 3));
            cardsList.Add(new clsCreature(4, "Bichote verde", "Da miedo", "/Assets/PRUEBAS/bichoverde.png", 4, 2, 6));
            cardsList.Add(new clsCreature(5, "Gato gordo", "Es como shifu pero gordo y sin sombrero", "/Assets/PRUEBAS/gatogordo.png", 5, 7, 4));
            cardsList.Add(new clsCreature(6, "Murciélago", "Rápido y furioso", "/Assets/PRUEBAS/murcielago.png", 1, 1, 3));
            cardsList.Add(new clsCreature(7, "El pato", "Parece inofensivo pero cuidao", "/Assets/PRUEBAS/pato.png", 3, 2, 5));
            cardsList.Add(new clsCreature(8, "Sirpiente", "Sigiloso y letal", "/Assets/PRUEBAS/serpiente.png", 3, 1, 4));
            cardsList.Add(new clsCreature(9, "Don tortuga", "Lento pero aguanta", "/Assets/PRUEBAS/tortuga.png", 3, 8, 1));
            cardsList.Add(new clsCreature(10, "Rana mágica", "Rana y encima mágica", "/Assets/PRUEBAS/rana.png", 4, 4, 3));
            cardsList.Add(new clsCreature(11, "Loro", "Un poco pesao pero buena gente", "/Assets/PRUEBAS/loro.png", 3, 2, 4));
            cardsList.Add(new clsCreature(12, "Tiburón", "Da miedo pero es bastante inocente", "/Assets/PRUEBAS/tiburon.png", 4, 5, 2));
            cardsList.Add(new clsCreature(13, "Berenjeno", "Tiene muy mal humor", "/Assets/PRUEBAS/berenjena.png", 1, 1, 2));
            cardsList.Add(new clsLifeModifyingSpell(14, "Seta Venenosa", "Envenena a la criatura objetivo", "/Assets/PRUEBAS/CartaSeta.png", 4, 4, true, false));
            cardsList.Add(new clsLifeModifyingSpell(15, "Seta del Amor", "Cura a todas las criaturas con el poder del amor", "/Assets/PRUEBAS/setacura.png", 2, 2, false, true));
            cardsList.Add(new clsLifeModifyingSpell(16, "Piedra", "Es una piedra", "/Assets/PRUEBAS/piedra.png", 6, 6, true, false));
            cardsList.Add(new clsLifeModifyingSpell(17, "Pescao pocho", "Causa daño en todos los enemigos del rival", "/Assets/PRUEBAS/espinas.png", 4, 3, true, true));
            cardsList.Add(new clsLifeModifyingSpell(18, "Señor Donut", "Está triste pero rico", "/Assets/PRUEBAS/donut.png", 2, 2, false, false));
            cardsList.Add(new clsLifeModifyingSpell(19, "Perita", "Alegría pal cuerpo", "/Assets/PRUEBAS/pera.png", 4, 3, false, false));
              
            return cardsList;
        }
    }
}
