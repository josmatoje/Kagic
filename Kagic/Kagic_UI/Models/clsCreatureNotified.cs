using Kagic_Entities;
using Kagic_UI.ViewModels.UtilitiesVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kagic_UI.Models
{
    public class clsCreatureNotified : clsVMBase
    {
        clsCreature creature;

        public clsCreature Creature 
        { 
            get => creature; 
            set 
            {
                creature = value;
                NotifyPropertyChanged(nameof(clsCreature.ActualLife));
            }
        }
    }
}
