using Kagic_Entities;
using Kagic_UI.ViewModels.UtilitiesVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kagic_UI.Models
{
    public class clsCreatureNotified : clsCreature, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
