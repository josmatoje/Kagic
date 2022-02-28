using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Kagic_Entities.Utilidades;

namespace Kagic_Entities
{
    public abstract class clsCard : clsVMBase
    {
        #region atributes
        int id;
        string name;
        string description;
        string image;
        int manacost;
        bool isAvaible;
        #endregion

        #region constructors
        //Parameterized constuctor
        public clsCard(int id, string name, string description, string image, int manacost)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.image = image;
            this.manacost = manacost;
            this.isAvaible = false;
        }

        //Default constructor
        public clsCard()
        {
            this.id = 0;
            this.name = " ";
            this.description = " ";
            this.image = " ";
            this.manacost = 0;
            this.isAvaible = false;
        }
        //Copy constructor
        public clsCard(clsCard card)
        {
            if (card != null)
            {
                this.id = card.Id;
                this.name = card.Name;
                this.description = card.Description;
                this.image = card.Image;
                this.manacost = card.Manacost;
                this.isAvaible = card.IsAvaible;
            }
            else
            {
                //this = null;
            }
        }
        #endregion

        #region public properties
        [PrimaryKey, AutoIncrement]
        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string Image { get => image; set => image = value; }
        public int Manacost { get => manacost; set => manacost = value; }
        public bool IsAvaible { get => isAvaible; set => isAvaible = value; }
        public string BACK_IMAGE { get => "/Assets/Images/BackImage.png"; }
        #endregion
    }
}