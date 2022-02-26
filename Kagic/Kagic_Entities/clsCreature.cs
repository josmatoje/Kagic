using System;
using System.Collections.Generic;
using System.Text;

namespace Kagic_Entities
{
    public class clsCreature : clsCard
    {
        #region atributes
        int life;
        int actualLife;
        int attack;
        bool used;
        #endregion

        #region constructors
        //Parameterized constuctor
        public clsCreature(int id, string name, string description, string image, int manacost, int life, int attack) : base(id, name, description, image, manacost)
        {
            this.life = life;
            this.actualLife = life;
            this.attack = attack;
            this.used = true;
        }

        //Default constructor
        public clsCreature() : base()
        {
            this.life = 0;
            this.actualLife = 0;
            this.attack = 0;
            this.used = true;
        }
        #endregion

        #region public properties
        public int Life { get => life; set => life = value; }
        public int ActualLife { get => actualLife; set => actualLife = value; }
        public int Attack { get => attack; set => attack = value; }
        public bool Used { get => used; set => used = value; }

        #endregion
    }
}