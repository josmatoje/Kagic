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
        public clsCreatureNotified(clsCreature creature) : base(creature)
        {
        }

        public clsCreatureNotified() : base()
        {
        }

        public clsCreatureNotified(int id, string name, string description, string image, int manacost, int life, int attack) : base(id, name, description, image, manacost, life, attack)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override int ActualLife 
        { 
            get => actualLife;
            set
            {
                actualLife = value;
                NotifyPropertyChanged(nameof(ActualLife));
            }
        }

    }
}
