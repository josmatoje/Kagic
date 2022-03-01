using Kagic_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Kagic_UI.Models.Utilities
{
    public class BattlefieldDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Creature { get; set; }
        public DataTemplate Empty { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            DataTemplate dataTemplate = null;
            clsCard card = (clsCard)item;
            dataTemplate = (card == null || card.Id == 0) ? Empty : Creature;
            return dataTemplate;
        }
    }
}
