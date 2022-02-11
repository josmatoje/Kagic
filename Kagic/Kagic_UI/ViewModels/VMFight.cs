using Kagic_Entities;
using Kagic_UI.Models;
using Kagic_UI.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kagic_UI.ViewModels
{
    public class VMFight : clsVMBase
    {

        #region atributes
        clsPlayer realPlayer;
        clsIAPlayer iaPlayer;
        bool isPlayerTurn;
        #endregion

        #region constants
        const int DECK_SIZE = 20;
        const int MAX_REPETED_CARDS_DECK = 2;
    
        #endregion

        #region constructor
        public VMFight()
        {
            realPlayer = new clsPlayer();
            iaPlayer = new clsIAPlayer();
        }
        #endregion

        #region public properties
        public clsPlayer RealPlayer { get => realPlayer; set => realPlayer = value; }
        public clsIAPlayer IaPlayer { get => iaPlayer; set => iaPlayer = value; }
        public bool IsPlayerTurn { get => isPlayerTurn; set => isPlayerTurn = value; }
        #endregion

        #region private methods

        /// <summary>
        /// Metodo para comenzar la partida
        /// </summary>
        private void startGame()
        {
            List<clsCard> cards = new List<clsCard>();
            //cards = DAL.obtenerListadoCartas()
            realPlayer.Deck = CardsDeck(cards);
            iaPlayer.Deck = CardsDeck(cards);
            //random para ver quien empieza isPlayerTurn
        }

        /// <summary>
        /// <b>Headboard: </b>private List<clsCard> CardsDeck(List<clsCard> cardsList)<br/>
        /// <b>Description: </b>This method generate a random deck that can contain only two copies of the same card<br/>
        /// <b>Preconditions: </b> none<br/>
        /// <b>Postconditions: </b> <br/>
        /// </summary>
        /// <param name="cardsList"></param>
        /// <returns></returns>
        private List<clsCard> CardsDeck(List<clsCard> cardsList)
        {
            Random random = new Random();
            List<clsCard> deck= new List<clsCard>();
            int position, counter;

            for(int i = 0; i < DECK_SIZE;)
            {
                position = random.Next(cardsList.Count);
                counter = 0;
                //foreach(clsCard card in deck)
                for(int j = 0; j < deck.Count && counter < MAX_REPETED_CARDS_DECK; j++)
                {
                    if(deck[j].GetType() == cardsList[i].GetType() && deck[j].Id == cardsList[position].Id) //Assess the type of de card and the id 
                    {
                        counter++;
                    }
                }
                if(counter < MAX_REPETED_CARDS_DECK)
                {
                    deck.Add(cardsList[i]);
                    i++;
                }  
            }

            return deck;
        }

        /// <summary>
        /// Metodo para el cambio de turno
        /// </summary>
        private void changeTurn()
        {
            isPlayerTurn = !isPlayerTurn;
            if (isPlayerTurn)
            {
                //realPlayer.setMana();
            }
            else
            {
                //iaPlayer.setMana();
            }
            realPlayer.SelectedCard = null;
            realPlayer.SelectedCriature = null;
            iaPlayer.SelectedCard = null;
            iaPlayer.SelectedCriature = null;
            //realPlayer.setUsedCriatures();
            //iaPlayer.setUsedCriatures();
        }

        /// <summary>
        /// Metodo para comprobar si se ha acabado la partida.
        /// En caso afirmativo, muestra la vista de victoria o derrota
        /// </summary>
        private bool finishGame()
        {
            bool finished = false;
            if(realPlayer.Life <= 0)
            {
                //enviar a vista de error de victoria
                finished = true;

            }
            else if(iaPlayer.Life <= 0)
            {
                //enviar a vista de error de derrota
                finished = true;
            }
            return finished;
        }

        /// <summary>
        /// Metodo principal del combate
        /// </summary>
        private void makeAction()
        {
            if (isPlayerTurn)
            {
                actionForPlayerTurn(realPlayer);
            }
            else
            {
                actionForPlayerTurn(iaPlayer);
            }
            NotifyPropertyChanged("RealPlayer.Hand");
            NotifyPropertyChanged("IaPlayer.Hand");
        }

        /// <summary>
        /// Metodo para realizar las acciones del combate dependiendo del turno
        /// </summary>
        /// <param name="player"></param>
        private void actionForPlayerTurn(clsPlayer player)
        {
            if (player.SelectedCard.GetType() == typeof(clsCriature))
            {
                criaturebattle();
            }
            else
            {
                if (((clsLifeModifyingSpell)player.SelectedCard).IsDamage)
                {
                    sendAttackSpell();
                }
                else
                {
                    sendHealthSpell();
                }
                player.Hand.Remove(player.SelectedCard);
            }
        }

        /// <summary>
        /// <b>Cabecera: </b> private void criaturebattle()<br/>
        /// <b>Descripcion: </b> Method for update the selectedcards'life depends of selectedcards'atack
        /// </summary>
        private void criaturebattle()
        {
            realPlayer.SelectedCriature.Actuallife = realPlayer.SelectedCriature.Actuallife - iaPlayer.SelectedCriature.Atack;
            if(realPlayer.SelectedCriature.Actuallife <= 0){
                realPlayer.PlaceCriatures.Remove(realPlayer.SelectedCriature);
                NotifyPropertyChanged("RealPlayer.PlaceCriatures");
            }
            NotifyPropertyChanged("RealPlayer.SelectedCriature");
            iaPlayer.SelectedCriature.Actuallife = iaPlayer.SelectedCriature.Actuallife - realPlayer.SelectedCriature.Atack;
            if (iaPlayer.SelectedCriature.Actuallife <= 0)
            {
                iaPlayer.PlaceCriatures.Remove(iaPlayer.SelectedCriature);
                NotifyPropertyChanged("IaPlayer.PlaceCriatures");
            }
            NotifyPropertyChanged("IaPlayer.SelectedCriature");
        }

        /// <summary>
        /// Metodo para lanzar hechizo de ataque
        /// </summary>
        private void sendAttackSpell()
        {
            if (isPlayerTurn)
            {
                attackAction(realPlayer, iaPlayer);
            }
            else
            {
                attackAction(iaPlayer, realPlayer);
                
            }
            NotifyPropertyChanged("IaPlayer.PlaceCriatures");
            NotifyPropertyChanged("RealPlayer.PlaceCriatures");
        }

        /// <summary>
        /// Metodo para realizar la accion de lanzar un hechizo de ataque
        /// </summary>
        /// <param name="ofense"></param>
        /// <param name="defensor"></param>
        private void attackAction(clsPlayer ofense, clsPlayer defensor)
        {
            defensor.SelectedCriature.Actuallife = defensor.SelectedCriature.Actuallife - ((clsLifeModifyingSpell)ofense.SelectedCard).Efect;
            if (defensor.SelectedCriature.Actuallife <= 0)
            {
                defensor.PlaceCriatures.Remove(defensor.SelectedCriature);
            }
        }

        /// <summary>
        /// metodo para lanzar hechizos de curacion
        /// </summary>
        private void sendHealthSpell()
        {
            if (isPlayerTurn)
            {
                healthAction(realPlayer);
                NotifyPropertyChanged("RealPlayer.Life");
                NotifyPropertyChanged("RealPlayer.SelectedCriature");

            }
            else
            {
                healthAction(iaPlayer);
                NotifyPropertyChanged("IaPlayer.Life");
                NotifyPropertyChanged("IaPlayer.SelectedCriature");
            }
            
        }

        /// <summary>
        /// Metodo para realizar la accion de curar generico
        /// </summary>
        /// <param name="player"></param>
        private void healthAction(clsPlayer player)
        {
            if (player.SelectedCriature != null)
            {
                player.SelectedCriature.Actuallife = player.SelectedCriature.Actuallife + ((clsLifeModifyingSpell)player.SelectedCard).Efect;
                if (player.SelectedCriature.Actuallife > player.SelectedCriature.Life)
                {
                    player.SelectedCriature.Actuallife = player.SelectedCriature.Life;
                }
            }
            else
            {
                player.Life = player.Life + ((clsLifeModifyingSpell)player.SelectedCard).Efect;
                if (player.Life > clsPlayer.MAX_LIFE)
                {
                    player.Life = clsPlayer.MAX_LIFE;
                }
            }
        }
        #endregion

    }
}
