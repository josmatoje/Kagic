using System;
using System.Collections.Generic;
using System.Text;

namespace Kagic_Entities
{
    public class clsLifeModifyingSpell : clsCard
    {

        #region atributes
        int effect;
        bool isDamage;//boolean indicating if the efect makes damage or heal
        bool isArea;
        #endregion

        #region constructors
        //Parameterized constuctor
        public clsLifeModifyingSpell(int id, string name, string description, string image, int manacost, int effect, bool isDamage, bool isArea) : base(id, name, description, image, manacost)
        {
            this.effect = effect;
            this.isDamage = isDamage;
            this.isArea = isArea;
        }
        ////Default constructor
        public clsLifeModifyingSpell() : base()
        {
            this.effect = 0;
            this.isDamage = true;
            this.isArea = true;
        }
        ////Copy constructor
        public clsLifeModifyingSpell(clsLifeModifyingSpell spell) : base(spell)
        {
            if (spell != null)
            {
                this.effect = spell.Effect;
                this.isDamage = spell.IsDamage;
                this.isArea = spell.IsArea;
            }
        }
        #endregion

        #region public properties
        public int Effect { get => effect; set => effect = value; }
        public bool IsDamage { get => isDamage; set => isDamage = value; }
        public bool IsArea { get => isArea; set => isArea = value; }
        #endregion
    }
}