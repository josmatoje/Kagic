using System;
using System.Collections.Generic;
using System.Text;

namespace Kagic_Entities
{
    public class clsCriatura : clsCarta
    {
        #region atributos
        int life;
        int actuallife;
        int atack;
        #endregion

        #region constructores
        //Constuctor por parametros
        public clsCriatura(int id, string name, string description, string image, int manacost, bool used,int life, int actuallife, int atack) : base( id,  name,  description,  image,  manacost,  used)
        {
            this.life = life;
            this.actuallife = actuallife;
            this.atack = atack;
        }

        //Constructor por defecto
        public clsCriatura() : base()
        {
            this.life = 0;
            this.actuallife = 0;
            this.atack = 0;
        }
        #endregion

        #region propiedades publicas
        public int Life { get => life; set => life = value; }
        public int Actuallife { get => actuallife; set => actuallife = value; }
        public int Atack { get => atack; set => atack = value; }

        #endregion
    }
}
