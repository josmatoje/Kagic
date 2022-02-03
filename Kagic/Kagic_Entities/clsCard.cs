using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kagic_Entities
{
    public abstract class clsCard 
    {
        #region atributes
        int id;
        string name;
        string description;
        string image;
        int manacost;
        #endregion

        #region constants
        const string BACK_IMAGE = " ";
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
        }

        //Default constructor
        public clsCard()
        {
            this.id = 0;
            this.name = " ";
            this.description = " ";
            this.image = " ";
            this.manacost = 0;
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
            }
            else
            {
                this = null;
            }
        }
        #endregion

        #region public properties
        [PrimaryKey, AutoIncrement]
        public int Id { get => id;}
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string Image { get => image; set => image = value; }
        public int Manacost { get => manacost; set => manacost = value; }
        #endregion
    }
}
