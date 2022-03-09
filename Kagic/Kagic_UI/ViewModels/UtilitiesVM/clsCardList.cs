using Kagic_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kagic_UI.ViewModels
{
    public class clsCardList
    {
        List<clsCard> cardList = insertCard();

        public List<clsCard> CardList { get => cardList; }

        private static List<clsCard> insertCard()
        {
            List<clsCard> cardList = new List<clsCard>();



            return cardList;
        }
    }
}
