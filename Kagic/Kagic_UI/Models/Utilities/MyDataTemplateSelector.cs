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

        protected override DataTemplate SelectTemplateCore(object item)
        {    
            return item is clsCreature ? Creature : ((clsLifeModifyingSpell)item).IsDamage ? DamageSpell : HealingSpell;
        }
    }
}
