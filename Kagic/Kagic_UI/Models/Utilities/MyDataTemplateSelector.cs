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
        public DataTemplate Spell { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            clsCard card = (clsCard)item;
            if (card is clsCreature)
            {
                return Creature;
            }
            else
            {
                return Spell;
            }
        }
    }
}
