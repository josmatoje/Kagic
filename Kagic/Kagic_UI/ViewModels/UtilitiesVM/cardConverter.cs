using Kagic_Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kagic_UI.ViewModels.UtilitiesVM
{
    public class cardConverter
    {
        public static clsCreature ConvertcardIntoCreature(clsCard carta)
        {
            clsCreature creature = carta as clsCreature;
            return creature;
        }

        public static clsLifeModifyingSpell ConvertcardIntoSpell(clsCard carta)
        {
            clsLifeModifyingSpell spell = carta as clsLifeModifyingSpell;
            return spell;
        }


        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    clsCard card;
        //    if (value.GetType == clsCreature) {

        //    }

        //    return ConvertNumberToURLString((int)value);
        //}

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

