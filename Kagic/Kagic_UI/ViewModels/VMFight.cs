﻿using Kagic_Entities;
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
        DelegateCommand passTurnCommand;
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
        public bool IsPlayerTurn 
        { 
            get => isPlayerTurn;
            set
            {
                isPlayerTurn = value;
                passTurnCommand.RaiseCanExecuteChanged();
            }
        }
        public DelegateCommand PassTurnCommand 
        {
            get 
            {
                passTurnCommand = new DelegateCommand(passTurnCommand_Executed, passTurnCommand_CanExecute);
                return passTurnCommand;
            }
            set => passTurnCommand = value; 
        }
        #endregion

        #region commands
        /// <summary>
        ///     <cabecera>private void passTurnCommand_Executed()</cabecera>
        ///     <descripcion>Calls changeTurn's method </descripcion> 
        /// </summary>
        private void passTurnCommand_Executed()
        {
            changeTurn();
        }

        /// <summary>
        ///     <cabecera>private bool passTurnCommand_CanExecute()</cabecera>
        ///     <descripcion>When isPlayerTurn is false, valido is true</descripcion>
        /// </summary>
        /// <returns></returns>
        private bool passTurnCommand_CanExecute()
        {
            bool valido = true;

            if (isPlayerTurn == false)
                valido = false;
            {

            }
            return valido;
        }
        #endregion

        #region private methods

        /// <summary>
        ///     <cabecera>private void startGame()</cabecera>
        ///     <descripcion>First method done when a game start. Prepare the decks of the players</descripcion>
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
        ///     <cabecera>private void changeTurn()</cabecera>
        ///     <descripcion>it is done when a turn finishes. Set mana used and  reset selected cards</descripcion>
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
            realPlayer.SelectedCard = -1;
            realPlayer.SelectedCreature = -1;
            iaPlayer.SelectedCard = -1;
            iaPlayer.SelectedCreature = -1;
            //realPlayer.setUsedCriatures();
            //iaPlayer.setUsedCriatures();
                }
        /// <summary>
        ///     <cabecera>private bool finishGame()</cabecera>
        ///     <descripcion>Method to validate that anyone's life is 0 or lower </descripcion>
        /// </summary>
        /// <returns></returns>
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
        ///     <cabecera>private void makeAction()</cabecera>
        ///     <descripcion>Method to make the action during turn </descripcion>
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
        ///     <cabecera>private void actionForPlayerTurn(clsPlayer player)</cabecera>
        ///     <descripcion>Method to choice the action depends of the player </descripcion>
        /// </summary>
        /// <param name="player"></param>
        private void actionForPlayerTurn(clsPlayer player)
        {
            if (player.SelectedCard.GetType() == typeof(clsCreature))
            {
                creaturebattle();
            }
            else
            {
                if (((clsLifeModifyingSpell)player.Hand[player.SelectedCard]).IsDamage)
                {
                    sendAttackSpell();
                }
                else
                {
                    sendHealthSpell();
                }
                player.Hand.Remove(player.Hand[player.SelectedCard]);
            }
        }

        /// <summary>
        /// <b>Cabecera: </b> private void creaturebattle()<br/>
        /// <b>Descripcion: </b> Method for update the criatures'life depends of criatures'atack
        /// </summary>
        private void creaturebattle()
        {
            realPlayer.PlaceCreatures[realPlayer.SelectedCreature].Actuallife = realPlayer.PlaceCreatures[realPlayer.SelectedCreature].Actuallife - iaPlayer.PlaceCreatures[iaPlayer.SelectedCreature].Attack;
            if(realPlayer.PlaceCreatures[realPlayer.SelectedCreature].Actuallife <= 0){
                realPlayer.PlaceCreatures.Remove(realPlayer.PlaceCreatures[realPlayer.SelectedCreature]);
                NotifyPropertyChanged("RealPlayer.PlaceCreatures");
            }
            NotifyPropertyChanged("RealPlayer.PlaceCreatures");
            iaPlayer.PlaceCreatures[iaPlayer.SelectedCreature].Actuallife = iaPlayer.PlaceCreatures[iaPlayer.SelectedCreature].Actuallife - realPlayer.PlaceCreatures[realPlayer.SelectedCreature].Attack;
            if (iaPlayer.PlaceCreatures[iaPlayer.SelectedCreature].Actuallife <= 0)
            {
                iaPlayer.PlaceCreatures.Remove(iaPlayer.PlaceCreatures[realPlayer.SelectedCreature]);
                NotifyPropertyChanged("IaPlayer.PlaceCreatures");
            }
            NotifyPropertyChanged("IaPlayer.SelectedCreature");
        }

        /// <summary>
        /// <b>Cabecera: </b> private void sendAttackSpell()<br/>
        /// <b>Descripcion: </b> Method for make spell attack action
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
            NotifyPropertyChanged("IaPlayer.PlaceCreatures");
            NotifyPropertyChanged("RealPlayer.PlaceCreatures");
        }

        /// <summary>
        /// <b>Cabecera: </b> private void attackAction(clsPlayer ofense, clsPlayer defensor)<br/>
        /// <b>Descripcion: </b> Method for make spell attack action
        /// </summary>
        /// <param name="ofense"></param>
        /// <param name="defensor"></param>
        private void attackAction(clsPlayer ofense, clsPlayer defensor)
        {
            defensor.PlaceCreatures[defensor.SelectedCreature].Actuallife = defensor.PlaceCreatures[defensor.SelectedCreature].Actuallife - ((clsLifeModifyingSpell)ofense.Hand[ofense.SelectedCard]).Effect;
            if (defensor.PlaceCreatures[defensor.SelectedCreature].Actuallife <= 0)
            {
                defensor.PlaceCreatures.Remove(defensor.PlaceCreatures[realPlayer.SelectedCreature]);
            }
        }

        /// <summary>
        /// <b>Cabecera: </b> private void sendHealthSpell()<br/>
        /// <b>Descripcion: </b> Method for make spell health action
        /// </summary>
        private void sendHealthSpell()
        {
            if (isPlayerTurn)
            {
                healthAction(realPlayer);
                NotifyPropertyChanged("RealPlayer.Life");

            }
            else
            {
                healthAction(iaPlayer);
                NotifyPropertyChanged("IaPlayer.Life");
            }
            
        }

        /// <summary>
        /// <b>Cabecera: </b> private void healthAction(clsPlayer player)<br/>
        /// <b>Descripcion: </b> Method for make spell health action
        /// </summary>
        /// <param name="player"></param>
        private void healthAction(clsPlayer player)
        {
            if (player.PlaceCreatures[player.SelectedCreature] != null)
            {
                player.PlaceCreatures[player.SelectedCreature].Actuallife = player.PlaceCreatures[player.SelectedCreature].Actuallife + ((clsLifeModifyingSpell)player.Hand[player.SelectedCard]).Effect;
                if (player.PlaceCreatures[player.SelectedCreature].Actuallife > player.PlaceCreatures[player.SelectedCreature].Life)
                {
                    player.PlaceCreatures[player.SelectedCreature].Actuallife = player.PlaceCreatures[player.SelectedCreature].Life;
                }
            }
            else
            {
                player.Life = player.Life + ((clsLifeModifyingSpell)player.Hand[player.SelectedCard]).Effect;
                if (player.Life > clsPlayer.MAX_LIFE)
                {
                    player.Life = clsPlayer.MAX_LIFE;
                }
            }
        }

       
        #endregion

    }
}
