using Kagic_Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Kagic_BL
{
    public class CardsManagement
    {
        /// <summary>
        /// <b>Headboard: </b>public static ObservableCollection<clsCard> getCardsListBL()<br/>
        /// <b>Description: </b>This method calls DAL`s one which gets a list of all the cards in the database<br/>
        /// <b>Preconditions: </b> anyone<br/>
        /// <b>Postconditions: </b> you get a list of all the cards in the database<br/>
        /// </summary>
        /// <returns>ObservableCollection<clsCard></returns>
        public static ObservableCollection<clsCard> getCardsListBL()
        {
            ObservableCollection<clsCard> cardsList = new ObservableCollection<clsCard>();

            return cardsList;
        }
    }
}
