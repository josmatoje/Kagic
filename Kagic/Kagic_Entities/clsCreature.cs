﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Kagic_Entities
{
    public class clsCreature : clsCard
    {
        #region atributes
        int life;
        int actuallife;
        int attack;
        bool used;
        #endregion

        #region constructors
        //Parameterized constuctor
        public clsCreature(int id, string name, string description, string image, int manacost, int life, int actuallife, int atack) : base( id,  name,  description,  image,  manacost)
        {
            this.life = life;
            this.actuallife = actuallife;
            this.attack = atack;
            this.used = true;   
        }

        //Default constructor
        public clsCreature() : base()
        {
            this.life = 0;
            this.actuallife = 0;
            this.attack = 0;
        }
        #endregion

        #region public properties
        public int Life { get => life; set => life = value; }
        public int Actuallife { get => actuallife; set => actuallife = value; }
        public int Atack { get => attack; set => attack = value; }
        public bool Used { get => used; set => used = value; }

        #endregion
    }
}