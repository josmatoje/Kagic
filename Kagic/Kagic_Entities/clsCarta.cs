using System;
using System.Collections.Generic;
using System.Text;

namespace Kagic_Entities
{
    public abstract class clsCarta 
    {
        #region atributos
        int id;
        string name;
        string description;
        string image;
        int manacost;
        bool used;
        #endregion

        #region constantes
        const string BACK_IMAGE = " ";
        #endregion

        #region constructores
        //Constuctor por parametros
        public clsCarta(int id, string name, string description, string image, int manacost, bool used)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.image = image;
            this.manacost = manacost;
            this.used = used;
        }

        //Constructor por defecto
        public clsCarta()
        {
            this.id = 0;
            this.name = " ";
            this.description = " ";
            this.image = " ";
            this.manacost = 0;
            this.used = true;
        }
        #endregion

        #region propiedades publicas
        public int Id { get => id;}
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string Image { get => image; set => image = value; }
        public int Manacost { get => manacost; set => manacost = value; }
        public bool Used { get => used; set => used = value; }
        #endregion
    }
}
