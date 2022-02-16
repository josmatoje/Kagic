using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kagic_Entities;

namespace Kagic_UI.ViewModels
{
    public class VMPruebas
    {
        private clsCard card;

        private ObservableCollection<clsCard> playerCards;
        private ObservableCollection<clsCard> iaCards;

        private string number = "/Assets/Images/Numbers/1.png";

        //private string backImage = clsCard.BACK_IMAGE;

        public VMPruebas()
        {
            card = new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/Carta Azul.png", 2, 2, true, false);

            playerCards = new ObservableCollection<clsCard>
            {
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/CartaSeta.png", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/CartaSeta.png", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/CartaSeta.png", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/CartaSeta.png", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/CartaSeta.png", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/CartaSeta.png", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/CartaSeta.png", 2, 2, true, false)
            };

            iaCards = new ObservableCollection<clsCard>
            {
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/CartaSeta.png", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/CartaSeta.png", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/CartaSeta.png", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/CartaSeta.png", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/CartaSeta.png", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/CartaSeta.png", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/CartaSeta.png", 2, 2, true, false)
            };
        }

        public clsCard Card { get => card; set => card = value; }
        public ObservableCollection<clsCard> IaCards { get => iaCards; set => iaCards = value; }
        public ObservableCollection<clsCard> PlayerCards { get => playerCards; set => playerCards = value; }
        public string Number { get => number; set => number = value; }
        //public string BackImage { get => backImage;}
    }
}
