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
        ObservableCollection<clsCard> lastSelectedCard;

        private ObservableCollection<clsCard> playerCards;
        private ObservableCollection<clsCreature> playerCreatures;
        private ObservableCollection<clsCreature> iaCreatures;
        private ObservableCollection<clsCard> iaCards;

        private string number = "/Assets/Images/Numbers/2.png";

        //private string backImage = clsCard.BACK_IMAGE;

        public VMPruebas()
        {
            card = new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, false, false);
            lastSelectedCard = new ObservableCollection<clsCard>();
            lastSelectedCard.Clear();
            lastSelectedCard.Add(card);
            playerCards = new ObservableCollection<clsCard>
            {
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, true, false),
                new clsCreature(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, 2),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, false, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, true, false)
            };

            iaCards = new ObservableCollection<clsCard>
            {
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, true, false),
                new clsLifeModifyingSpell(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, true, false)
            };

            playerCreatures = new ObservableCollection<clsCreature>
            {
                new clsCreature(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, 2),
                new clsCreature(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, 2),
                new clsCreature(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, 2),
                new clsCreature(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, 2),
                new clsCreature(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, 2)
            };

            iaCreatures = new ObservableCollection<clsCreature>
            {
                new clsCreature(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, 2),
                new clsCreature(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, 2),
                new clsCreature(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, 2),
                new clsCreature(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, 2),
                new clsCreature(1, "Seta venenosa", "Resta 2 de vida a un enemigo", "/Assets/PRUEBAS/solar_kitten.jpg", 2, 2, 2)
            };
        }

        public ObservableCollection<clsCard> LastSelectedCard { get => lastSelectedCard; }
        public clsCard Card { get => card; set => card = value; }
        public ObservableCollection<clsCard> IaCards { get => iaCards; set => iaCards = value; }
        public ObservableCollection<clsCard> PlayerCards { get => playerCards; set => playerCards = value; }
        public string Number { get => number; set => number = value; }
        public ObservableCollection<clsCreature> PlayerCreatures { get => playerCreatures; set => playerCreatures = value; }
        public ObservableCollection<clsCreature> IaCreatures { get => iaCreatures; set => iaCreatures = value; }
        //public string BackImage { get => backImage;}
    }
}
