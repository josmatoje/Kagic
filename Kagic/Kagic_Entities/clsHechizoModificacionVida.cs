using System;
using System.Collections.Generic;
using System.Text;

namespace Kagic_Entities
{
    public class clsHechizoModificacionVida : clsCarta
    {

        #region atributos
        int efecto;
        bool danhoCuracion;//booleano para indicar si el efecto es de danho o curacion. True -> danho, False -> curacion
        bool area;
        #endregion

        #region constructores
        //Constuctor por parametros
        public clsHechizoModificacionVida(int id, string name, string description, string image, int manacost, bool used,int efecto, bool danhoCuracion, bool area) : base( id,  name,  description,  image,  manacost,  used)
        {
            this.efecto = efecto;
            this.danhoCuracion = danhoCuracion;
            this.area = area;
        }

        //Constructor por defecto
        public clsHechizoModificacionVida() : base()
        {
            this.efecto = 0;
            this.danhoCuracion = true;
            this.area = true;
        }
        #endregion

        #region propiedades publicas
        public int Efecto { get => efecto; set => efecto = value; }
        public bool DanhoCuracion { get => danhoCuracion; set => danhoCuracion = value; }
        public bool Area { get => area; set => area = value; }
        #endregion
    }
}
