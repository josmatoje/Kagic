using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Kagic_UI.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Game : Page
    {
        public Game()
        {
            this.InitializeComponent();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void TutorialButtonClick(object sender, RoutedEventArgs e)
        {
            ttBtnNextTurn.IsOpen = true;
        }

        private void TeachingTipClosed(Microsoft.UI.Xaml.Controls.TeachingTip sender, Microsoft.UI.Xaml.Controls.TeachingTipClosedEventArgs args)
        {
            Microsoft.UI.Xaml.Controls.TeachingTip tt = (Microsoft.UI.Xaml.Controls.TeachingTip)sender;
            switch (tt.Name) {
                case "ttBtnNextTurn": ttGdPlayerCreatures.IsOpen = true;
                    break;
                       
                case "ttGdPlayerCreatures": ttGdPlayerCards.IsOpen = true;
                    break;

                case "ttGdPlayerCards": ttRpActualMana.IsOpen = true;
                    break;

                case "ttRpActualMana": ttButtonAttackContrary.IsOpen = true;
                    break;

                case "ttButtonAttackContrary": ttButtonHealthyou.IsOpen = true;
                    break;

                case "ttButtonHealthyou": ttSelectedCard.IsOpen = true;
                    break;
            }
        }
    }
}
