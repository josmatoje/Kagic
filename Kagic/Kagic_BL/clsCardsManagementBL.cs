using Kagic_Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Kagic_DAL.Listados;

namespace Kagic_BL
{
    public class clsCardsManagementBL
    {
        /// <summary>
        /// <b>Headboard: </b>public static List<clsCard> getCardsListBL()<br/>
        /// <b>Description: </b>This method calls DAL`s one which gets a list of all the cards in the database<br/>
        /// <b>Preconditions: </b> none<br/>
        /// <b>Postconditions: </b> you get a list of all the cards in the database<br/>
        /// </summary>
        /// <returns>List<clsCard></returns>
        public static List<clsCard> getCardsListBL()
        {
            return clsCardsManagementDAL.getCardsDAL();
        }
    }
}
