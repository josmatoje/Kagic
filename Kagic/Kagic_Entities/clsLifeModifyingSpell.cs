using System;
using System.Collections.Generic;
using System.Text;

namespace Kagic_Entities
{
    public class clsLifeModifyingSpell : clsCard
    {

        #region atributes
        public int efect;
        public bool isDamage;//boolean indicating if the efect makes damage or heal
        public bool isArea;
        #endregion

        #region constructors
        //Parameterized constuctor
        public clsLifeModifyingSpell(int id, string name, string description, string image, int manacost, bool used,int efecto, bool danhoCuracion, bool area) : base( id,  name,  description,  image,  manacost,  used)
        {
            this.efect = efecto;
            this.isDamage = danhoCuracion;
            this.isArea = area;
        }

        ////Default constructor
        public clsLifeModifyingSpell() : base()
        {
            this.efect = 0;
            this.isDamage = true;
            this.isArea = true;
        }
        #endregion

        #region public properties
        public int Efect { get => efect; set => efect = value; }
        public bool IsDamage { get => isDamage; set => isDamage = value; }
        public bool IsArea { get => isArea; set => isArea = value; }
        #endregion
    }
}
