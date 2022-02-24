using Kagic_BL;
using Kagic_Entities;
using Kagic_UI.Models;
using Kagic_UI.Models.Utilities;
using Kagic_UI.ViewModels.UtilitiesVM;
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
        DelegateCommand attackContraryPlayerCommand;
        clsCard selectedCard;
        clsCreature selectedCreature;
        clsCard lastSelectedCard;
        #endregion

        #region constants
        const int DECK_SIZE = 10;
        const int MAX_REPETED_CARDS_DECK = 2;

        #endregion

        #region constructor
        public VMFight()
        {
            passTurnCommand = new DelegateCommand(passTurnCommand_Executed, passTurnCommand_CanExecute);
            StartGame();
        }
        #endregion

        #region public properties
        public clsPlayer RealPlayer { get => realPlayer; set => realPlayer = value; }
        public clsIAPlayer IaPlayer { get => iaPlayer; set => iaPlayer = value; }
        public bool IsPlayerTurn
        {
            get => isPlayerTurn;
            set => isPlayerTurn = value;
        }
        public DelegateCommand PassTurnCommand
        {
            get
            {

                return passTurnCommand;
            }
        }
        public DelegateCommand AttackContraryPlayerCommand
        {
            get
            {
                attackContraryPlayerCommand = new DelegateCommand(atackContraryPlayerCommand_Executed, atackContraryPlayerCommand_CanExecute);
                return attackContraryPlayerCommand;
            }
        }
        public clsCard SelectedCard
        {
            get => selectedCard;
            set
            {
                selectedCard = value;
                lastSelectedCard = value;
            }
        }

        public clsCreature SelectedCreature
        { 
            get => selectedCreature;
            set 
            {
                selectedCreature = value;
                TryPutCreature();
                TryAttackCreature();
                lastSelectedCard = value;
            }
        }

        public clsCard LastSelectedCard { get => lastSelectedCard; }
        #endregion

        #region commands
        /// <summary>
        ///     <Headboard>private void passTurnCommand_Executed()</cabecera>
        ///     <Description>Calls changeTurn's method </descripcion> 
        /// </summary>
        private void passTurnCommand_Executed()
        {
            changeTurn();
        }

        /// <summary>
        ///     <Headboard>private bool passTurnCommand_CanExecute()</cabecera>
        ///     <Description>When isPlayerTurn is false, valido is true</descripcion>
        /// </summary>
        /// <returns></returns>
        private bool passTurnCommand_CanExecute()
        {
            return isPlayerTurn;
        }

        /// <summary>
        ///     <Headboard>private void passTurnCommand_Executed()</cabecera>
        ///     <Description>Calls changeTurn's method </descripcion> 
        /// </summary>
        private void atackContraryPlayerCommand_Executed()
        {
            int damage = 0;

            if (isPlayerTurn == false)
            {
                if (realPlayer.SelectedCard != -1)
                {
                    damage = ((clsLifeModifyingSpell)realPlayer.Hand[realPlayer.SelectedCard]).Effect;
                }
                else
                {
                    damage = realPlayer.PlaceCreatures[realPlayer.SelectedCreature].Attack;
                }
            }
            else
            {
                if (iaPlayer.SelectedCard != -1)
                {
                    damage = ((clsLifeModifyingSpell)iaPlayer.Hand[iaPlayer.SelectedCard]).Effect;
                }
                else
                {
                    damage = iaPlayer.PlaceCreatures[iaPlayer.SelectedCreature].Attack;
                }
            }

            attackContraryPlayer(damage);
        }

        /// <summary>
        ///     <Headboard>private bool atackContraryPlayerCommand_CanExecute()</cabecera>
        ///     <Description></descripcion>
        /// </summary>
        /// <returns></returns>
        private bool atackContraryPlayerCommand_CanExecute()
        {
            bool valido = true;
            clsPlayer player = realPlayer;

            if (isPlayerTurn == false)
            {
                player = iaPlayer;
            }

            if (player.SelectedCard == -1 && noEnemiesFront())
            {
                valido = false;
            }
            return valido;
        }
        #endregion

        #region private methods

        /// <summary>
        ///     <Headboard>private void startGame()</cabecera>
        ///     <Description>First method done when a game start. Prepare the decks of the players</descripcion>
        /// </summary>
        private void StartGame()
        {
            List<clsCard> cards = new List<clsCard>(clsCardsManagementBL.getCardsListBL());
            realPlayer = new clsPlayer(CardsDeck(cards));
            iaPlayer = new clsIAPlayer(CardsDeck(cards));
            //random para ver quien empieza isPlayerTurn
            //isPlayerTurn = (new Random()).Next(10) > 5;
            isPlayerTurn = true;
            realPlayer.DrawCard();           
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
            List<clsCard> deck = new List<clsCard>();
            int position, counter, i=0;

            while (i< DECK_SIZE)
            {
                position = random.Next(cardsList.Count);
                counter = 0;
                //foreach (clsCard card in deck) but exit if the condition is exceeded
                for (int j = 0; j < deck.Count && counter < MAX_REPETED_CARDS_DECK; j++)
                {
                    if (deck[j].GetType() == cardsList[i].GetType() && deck[j].Id == cardsList[position].Id) //Assess the type of de card and the id 
                    {
                        counter++;
                    }
                }
                if (counter < MAX_REPETED_CARDS_DECK) //Solo inserta y aumenta la posicion de insercion del array si la carta no se ha repetido el maximo permitido
                {
                    deck.Add(cardsList[position]);
                    i++;
                }
            }

            return deck;
        }

        /// <summary>
        ///     <Headboard>private void changeTurn()</cabecera>
        ///     <Description>it is done when a turn finishes. Set mana used and  reset selected cards</descripcion>
        /// </summary>
        private void changeTurn()
        {
            isPlayerTurn = !isPlayerTurn;
            if (isPlayerTurn)
            {
                realPlayer.SetMana();
                realPlayer.DrawCard();
                NotifyPropertyChanged("realPlayer.Hand");
                realPlayer.SetUsedCreatures();
            }
            else
            {
                iaPlayer.SetMana();
                iaPlayer.DrawCard();
                NotifyPropertyChanged("iaPlayer.Hand");
                iaPlayer.SetUsedCreatures();

            }
            UpdateSelectedCardsForNewTurn();
            if (!isPlayerTurn)
            {

            }
        }

        /// <summary>
        /// <b>Headboard: </b>private void UpdateSelectedCardsForNewTurn()<br/>
        /// <b>Description: </b>This method add a card to the hand each turn, if deck is empty, life decrease<br/>
        /// <b>Preconditions: </b>None<br/>
        /// <b>Postconditions: </b>Hand updated<br/>
        /// </summary>
        private void UpdateSelectedCardsForNewTurn()
        {
            realPlayer.SelectedCard = -1;
            realPlayer.SelectedCreature = -1;
            iaPlayer.SelectedCard = -1;
            iaPlayer.SelectedCreature = -1;
            passTurnCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        ///     <Headboard>private bool finishGame()</cabecera>
        ///     <Description>Method to validate that anyone's life is 0 or lower </descripcion>
        /// </summary>
        /// <returns></returns>
        private bool finishGame()
        {
            bool finished = false;
            if (realPlayer.Life <= 0)
            {
                //enviar a vista de error de victoria
                finished = true;
                //Aqui mensaje de derrota
            }
            else if (iaPlayer.Life <= 0)
            {
                //enviar a vista de error de derrota
                finished = true;

            }
            return finished;
        }

        /// <summary>
        ///     <Headboard>private void makeAction()</cabecera>
        ///     <Description>Method to make the action during turn </descripcion>
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
        ///     <Headboard>private void actionForPlayerTurn(clsPlayer player)</cabecera>
        ///     <Description>Method to choice the action depends of the player </descripcion>
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
                player.Hand.RemoveAt(player.SelectedCard);
            }
        }

        /// <summary>
        /// <b>Headboard: </b> private void creaturebattle()<br/>
        /// <b>Description: </b> Method for update the criatures'life depends of criatures'atack
        /// </summary>
        private void creaturebattle()
        {
            realPlayer.PlaceCreatures[realPlayer.SelectedCreature].Actuallife = realPlayer.PlaceCreatures[realPlayer.SelectedCreature].Actuallife - iaPlayer.PlaceCreatures[iaPlayer.SelectedCreature].Attack;
            if (realPlayer.PlaceCreatures[realPlayer.SelectedCreature].Actuallife <= 0)
            {
                //Forma de setear a null para mantener espacios
                realPlayer.PlaceCreatures[realPlayer.SelectedCreature] = null;
                //Forma de eliminar de la lista
                //realPlayer.PlaceCreatures.RemoveAt(realPlayer.SelectedCreature);
                NotifyPropertyChanged("RealPlayer.PlaceCreatures");
            }
            NotifyPropertyChanged("RealPlayer.PlaceCreatures");
            iaPlayer.PlaceCreatures[iaPlayer.SelectedCreature].Actuallife = iaPlayer.PlaceCreatures[iaPlayer.SelectedCreature].Actuallife - realPlayer.PlaceCreatures[realPlayer.SelectedCreature].Attack;
            if (iaPlayer.PlaceCreatures[iaPlayer.SelectedCreature].Actuallife <= 0)
            {
                //Forma de setear a null para mantener espacios
                iaPlayer.PlaceCreatures[iaPlayer.SelectedCreature] = null;
                //Forma de eliminar de la lista
                //iaPlayer.PlaceCreatures.RemoveAt(iaPlayer.SelectedCreature);
                NotifyPropertyChanged("IaPlayer.PlaceCreatures");
            }
            NotifyPropertyChanged("IaPlayer.SelectedCreature");
        }

        /// <summary>
        /// <b>Headboard: </b> private void sendAttackSpell()<br/>
        /// <b>Description: </b> Method for make spell attack action
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
        /// <b>Headboard: </b> private void attackAction(clsPlayer ofense, clsPlayer defensor)<br/>
        /// <b>Description: </b> Method for make spell attack action
        /// </summary>
        /// <param name="ofense"></param>
        /// <param name="defensor"></param>
        private void attackAction(clsPlayer ofense, clsPlayer defensor)
        {
            defensor.PlaceCreatures[defensor.SelectedCreature].Actuallife = defensor.PlaceCreatures[defensor.SelectedCreature].Actuallife - ((clsLifeModifyingSpell)ofense.Hand[ofense.SelectedCard]).Effect;
            if (defensor.PlaceCreatures[defensor.SelectedCreature].Actuallife <= 0)
            {
                defensor.PlaceCreatures.RemoveAt(realPlayer.SelectedCreature);
            }
        }

        /// <summary>
        /// <b>Headboard: </b> private void sendHealthSpell()<br/>
        /// <b>Description: </b> Method for make spell health action
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
        /// <b>Headboard: </b> private void healthAction(clsPlayer player)<br/>
        /// <b>Description: </b> Method for make spell health action, and Santi sucks
        /// </summary>
        /// 
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool noEnemiesFront()
        {
            bool noEnemies = true;
            clsPlayer player = realPlayer;

            if (isPlayerTurn == false)
            {
                player = iaPlayer;
            }

            foreach (clsCreature auxCreature in player.PlaceCreatures)
            {
                if (auxCreature.Id != -1)
                {
                    noEnemies = false;
                }
            }
            return noEnemies;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="damage"></param>
        private void attackContraryPlayer(int damage)
        {
            if (isPlayerTurn == false)
            {
                realPlayer.Life -= damage;
                NotifyPropertyChanged("RealPlayer.ProgresBarLife");
            }
            else
            {
                iaPlayer.Life -= damage;
                NotifyPropertyChanged("IaPlayer.ProgresBarLife");
            }
            finishGame();
        }

        /// <summary>
        /// 
        /// </summary>
        private void TryPutCreature()
        {
            if (lastSelectedCard == selectedCard) //No ess asi
            {
                //Metodo para usar tanto la ia como el usuario (Controlar que no desaparezcan cartas)
                if (selectedCreature != null && selectedCreature.Id == 0 && realPlayer.SelectedCard != -1 && realPlayer.SelectedCreature != -1)
                {
                    realPlayer.PutCard();
                    realPlayer.SelectedCard = -1;
                    realPlayer.SelectedCreature = -1;
                    selectedCard = null;
                    selectedCreature = null;
                    NotifyPropertyChanged("RealPlayer.Hand");
                    NotifyPropertyChanged("RealPlayer.PlaceCreatures");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void TryAttackCreature()
        {
            //
        }
        #endregion

    }
}
