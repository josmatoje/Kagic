using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Kagic_Entities;

namespace Kagic_UI.Models.Utilities
{
    public class MyDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Creature { get; set; }
        public DataTemplate DamageSpell { get; set; }
        public DataTemplate HealingSpell { get; set; }
        public DataTemplate Empty { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            DataTemplate dataTemplate = null;
            clsCard card = (clsCard)item;
            if (card is clsCreature)
            {
                if(card==null || card.Id == 0)
                {
                    dataTemplate = Empty;
                }
                else
                {
                    dataTemplate = Creature;
                }
            }
            else
            {
                if(((clsLifeModifyingSpell)card).IsDamage)
                {
                    dataTemplate = DamageSpell;
                }
                else
                {
                    dataTemplate = HealingSpell;
                }
            }

            return dataTemplate;
        }
    }
}
