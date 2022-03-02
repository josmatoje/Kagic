using Kagic_BL;
using Kagic_Entities;
using Kagic_UI.Models;
using Kagic_UI.Models.Utilities;
using Kagic_UI.ViewModels.UtilitiesVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        DelegateCommand healthPlayerCommand;
        clsCard selectedCard;
        clsCreature selectedCreature;
        ObservableCollection <clsCard> lastSelectedCard;
        bool cardDetailsVisibility;
        #endregion

        #region constants
        const int DECK_SIZE = 20;
        const int MAX_REPETED_CARDS_DECK = 2;

        #endregion

        #region constructor
        public VMFight()
        {
            passTurnCommand = new DelegateCommand(passTurnCommand_Executed, passTurnCommand_CanExecute);
            lastSelectedCard = new ObservableCollection<clsCard>();
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
        public clsCard SelectedCard
        {
            get => selectedCard;
            set
            {
                selectedCard = value;
                //if (value != null && value.Id > 0) //Never could be null or ID <= 0 
                //{
                    SetLastSelectedCard(value);
                    NotifyPropertyChanged(nameof(LastSelectedCard));
                //}
                attackContraryPlayerCommand.RaiseCanExecuteChanged();
                healthPlayerCommand.RaiseCanExecuteChanged();

            }
        }

        public clsCreature SelectedCreature
        { 
            get => selectedCreature;
            set 
            {
                selectedCreature = value;
                //if (IsPlayerTurn) //Si sucede en el turno de la IA se controlará de otra forma --- ¡¡El seter siempre sucede en el turno del jugador!!
                if(lastSelectedCard[0] is clsCreature)
                {
                    TryPutCreature(realPlayer);
                    TryAttackCreature(realPlayer,iaPlayer);
                }
                else //Spell
                {
                    TrySendSpell(RealPlayer);
                }
                
                if(value != null && value.Id > 0 && value.ActualLife > 0) //Value se ve modificado por los metodos anteriores, 
                {
                    SetLastSelectedCard(value);
                }
                else
                {
                    selectedCreature = new clsCreature();
                    SetLastSelectedCard(new clsCreature());
                }
                attackContraryPlayerCommand.RaiseCanExecuteChanged(); 
            }
        }

        public ObservableCollection<clsCard> LastSelectedCard { get => lastSelectedCard;}

        public bool CardDetailsVisibility { get => cardDetailsVisibility; set => cardDetailsVisibility = value;}

        #region commands getters
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

        public DelegateCommand HealthPlayerCommand 
        {
            get
            {
                healthPlayerCommand = new DelegateCommand(healthPlayerCommand_Executed, healthPlayerCommand_CanExecute);
               return healthPlayerCommand;
            }
        }
        #endregion


        #endregion

        #region commands executed and canexecuted
        /// <summary>
        ///     <Headboard>private void passTurnCommand_Executed()</cabecera>
        ///     <Description>Calls changeTurn's method </descripcion> 
        /// </summary>
        private void passTurnCommand_Executed()
        {
            ChangeTurn();
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

            if (realPlayer.SelectedCard != -1)
            {
                damage = ((clsLifeModifyingSpell)realPlayer.Hand[realPlayer.SelectedCard]).Effect;
            }
            else
            {
                damage = realPlayer.PlaceCreatures[realPlayer.SelectedCreature].Attack;
            }

            AttackContraryPlayer(damage);
            realPlayer.PutCard();
        }

        /// <summary>
        ///     <Headboard>private bool atackContraryPlayerCommand_CanExecute()</cabecera>
        ///     <Description></descripcion>
        /// </summary>
        /// <returns></returns>
        private bool atackContraryPlayerCommand_CanExecute()
        {
            return  (realPlayer.SelectedCard != -1 && selectedCard is clsLifeModifyingSpell && ((clsLifeModifyingSpell)selectedCard).IsDamage && selectedCard.IsAvaible) || (realPlayer.SelectedCreature != -1  && !realPlayer.PlaceCreatures[realPlayer.SelectedCreature].Used && NoEnemiesFront());
        }

        /// <summary>
        ///     <Headboard>private void passTurnCommand_Executed()</cabecera>
        ///     <Description>Calls changeTurn's method </descripcion> 
        /// </summary>
        private void healthPlayerCommand_Executed()
        {
            int health = 0;
           
            health = ((clsLifeModifyingSpell)realPlayer.Hand[realPlayer.SelectedCard]).Effect;

            realPlayer.Life += health;
            if (realPlayer.Life > clsPlayer.MAX_LIFE)
            {
                realPlayer.Life = clsPlayer.MAX_LIFE;
            }
            realPlayer.PutCard();
            NotifyPropertyChanged(nameof(RealPlayer));
        }

        /// <summary>
        ///     <Headboard>private bool atackContraryPlayerCommand_CanExecute()</cabecera>
        ///     <Description></descripcion>
        /// </summary>
        /// <returns></returns>
        private bool healthPlayerCommand_CanExecute()
        {
            return realPlayer.SelectedCard != -1 && selectedCard is clsLifeModifyingSpell && !((clsLifeModifyingSpell)selectedCard).IsDamage && selectedCard.IsAvaible;
        }
        #endregion

        #region private methods

        #region game management
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
            cardDetailsVisibility = true;
            realPlayer.DrawCard();           
        }

        /// <summary>
        ///     <Headboard>private bool finishGame()</cabecera>
        ///     <Description>Method to validate that anyone's life is 0 or lower </descripcion>
        /// </summary>
        /// <returns></returns>
        private bool FinishGame()
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
            int position, counter, i = 0;

            while (i < DECK_SIZE)
            {
                position = random.Next(cardsList.Count);
                counter = 0;
                //foreach (clsCard card in deck) but exit if the condition is exceeded
                for (int j = 0; j < deck.Count && counter <= MAX_REPETED_CARDS_DECK; j++)
                {
                    if (deck[j].GetType() == cardsList[position].GetType() && deck[j].Id == cardsList[position].Id) //Assess the type of de card and the id 
                    {
                        counter++;
                    }
                }
                if (counter < MAX_REPETED_CARDS_DECK) //Solo inserta y aumenta la posicion de insercion del array si la carta no se ha repetido el maximo permitido
                {
                    if(cardsList[position] is clsCreature)
                    {
                        deck.Add(new clsCreature (cardsList[position] as clsCreature));
                    }
                    else
                    {
                        deck.Add(new clsLifeModifyingSpell(cardsList[position] as clsLifeModifyingSpell));
                    }
                    i++;
                }
                //Mazo de cartas 
                //deck.Add(new clsCreature (cardsList.ElementAt(5) as clsCreature));

            }

            return deck;
        }

        /// <summary>
        ///     <Headboard>private void changeTurn()</cabecera>
        ///     <Description>it is done when a turn finishes. Set mana used and  reset selected cards</descripcion>
        /// </summary>
        private void ChangeTurn()
        {
            isPlayerTurn = !isPlayerTurn;
            if (isPlayerTurn)
            {
                realPlayer.SetMana();
                realPlayer.DrawCard();
                realPlayer.SetUsedCreatures();
                NotifyPropertyChanged(nameof(RealPlayer));
                UpdateSelectedCardsForNewAction();
            }
            else
            {
                iaPlayer.SetMana();
                iaPlayer.DrawCard();
                iaPlayer.SetUsedCreatures();
                //NotifyPropertyChanged(nameof(IaPlayer));
                UpdateSelectedCardsForNewAction();
                //metodo accion ia
                IaTurn();
            }
            NotifyPropertyChanged(nameof(RealPlayer)); 
            NotifyPropertyChanged(nameof(IaPlayer));


            passTurnCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// <b>Headboard: </b>private void UpdateSelectedCardsForNewAction()<br/>
        /// <b>Description: </b>Update the selected cards from the viewmodel and the index from the players<br/>
        /// <b>Preconditions: </b>None<br/>
        /// <b>Postconditions: </b>Hand updated<br/>
        /// </summary>
        private void UpdateSelectedCardsForNewAction()
        {
            realPlayer.SelectedCard = -1;
            realPlayer.SelectedCreature = -1;
            iaPlayer.SelectedCard = -1;
            iaPlayer.SelectedCreature = -1;
            selectedCard = new clsCreature();
            NotifyPropertyChanged(nameof(SelectedCard));
            selectedCreature = new clsCreature();
            NotifyPropertyChanged(nameof(SelectedCreature));
            SetLastSelectedCard(selectedCard);
        }

        /// <summary>
        /// <b>Headboard: </b>private void iaTurn()<br/>
        /// <b>Description: </b>akes<br/>
        /// <b>Preconditions: </b>None<br/>
        /// <b>Postconditions: </b>Hand updated<br/>
        /// </summary>
        private void IaTurn()
        {
            int enemyCreatureIndex;
            bool usingCards=true, atackCreatures;

            //place creatures
            while (usingCards)
            {
                if (iaPlayer.SelectHandCard()) 
                {
                    selectedCard = iaPlayer.Hand[iaPlayer.SelectedCard];
                    SetLastSelectedCard(selectedCard);
                    if (selectedCard is clsCreature)
                    {
                        if (iaPlayer.PickPlace())
                        {
                            selectedCreature = iaPlayer.PlaceCreatures[iaPlayer.SelectedCreature];
                            TryPutCreature(iaPlayer);
                        }
                        else
                        {
                            usingCards = false;
                        }
                    }
                    else
                    {
                        if (NoEnemiesFront())
                        {
                            AttackContraryPlayer(((clsLifeModifyingSpell)selectedCard).Effect);
                        }
                        else
                        {
                            realPlayer.SelectedCreature = iaPlayer.PickEnemyCreature(realPlayer.PlaceCreatures);
                            AttackAction(iaPlayer, realPlayer);                           
                        }
                        iaPlayer.PutCard();
                    }                  
                }
                else
                {
                    usingCards = false;
                }
            }

            //attack enemies
            atackCreatures = !NoEnemiesFront();
            while (iaPlayer.PickOwnCreature() && atackCreatures) //While select creatures that can attack ande there is enemies infront
            {
                selectedCreature = iaPlayer.PlaceCreatures[iaPlayer.SelectedCreature];
                SetLastSelectedCard(selectedCreature);
                enemyCreatureIndex = iaPlayer.PickEnemyCreature(realPlayer.PlaceCreatures);
                if(enemyCreatureIndex != -1)
                {
                    selectedCreature = realPlayer.PlaceCreatures[enemyCreatureIndex];
                    realPlayer.SelectedCreature = enemyCreatureIndex;
                    //TryAttackCreature(iaPlayer, realPlayer);
                    Creaturebattle();
                }
                else
                {
                    atackCreatures=false;
                }
            }

            ChangeTurn();
        }

        /// <summary>
        /// <b>Headboard: </b>private void TryPutCreature()<br/>
        /// <b>Description: </b>Check if is posible to put the selected card on the batelfield and, if it is, do it<br/>
        /// <b>Preconditions: </b> It muss be called after change de selectedCreature but before change the lastSelectedCard<br/>
        /// <b>Postconditions: </b> None<br/>
        /// </summary>
        private void TryPutCreature(clsPlayer player)
        {
            if (lastSelectedCard != null && lastSelectedCard.Contains(selectedCard) && //Si la ultima carta seleccionada es una carta de tu mano que has seleccionado justo antes
                selectedCreature != null && selectedCreature.Id == 0 && 
                player.SelectedCard > -1 && player.SelectedCreature > -1 &&
                player.PlaceCreatures[player.SelectedCreature].Id == 0)
            {
                player.PutCard();
                //Una vez colocada la carta se modifican a -1 las criaturas seleccionadas
                NotifyPropertyChanged(nameof(RealPlayer));
                UpdateSelectedCardsForNewAction();
            }
        }

        /// <summary>
        /// <b>Headboard: </b>private void SetLastSelectedCard(clsCard card)<br/>
        /// <b>Description: </b>Change the las selectedCard and the cardDetailsVisibility<br/>
        /// <b>Preconditions: </b> None<br/>
        /// <b>Postconditions: </b> None<br/>
        /// </summary>
        /// <param name="card"></param>
        private void SetLastSelectedCard(clsCard card)
        {
            cardDetailsVisibility =(card != null && card.Id > 0); //Id==0 -> new clsCreature();
            NotifyPropertyChanged(nameof(CardDetailsVisibility));
            lastSelectedCard.Clear();
            lastSelectedCard.Add(card);
            NotifyPropertyChanged(nameof(LastSelectedCard));
        }
        #endregion

        #region attack management

        #region creature battle
        
        /// <summary>
        /// <b>Headboard: </b>private void TryPutCreature()<br/>
        /// <b>Description: </b>Check if is posible to put the selected card on the batelfield and, if it is, do it<br/>
        /// <b>Preconditions: </b> It muss be called after change de selectedCreature but before change the lastSelectedCard<br/>
        /// <b>Postconditions: </b> None<br/>
        /// </summary>
        private void TryAttackCreature(clsPlayer attacker, clsPlayer defensor)
        {
            if (lastSelectedCard != null && selectedCreature != null && 
                selectedCreature.Id > 0)
            {
                if(attacker.SelectedCreature > -1 && defensor.SelectedCreature > -1)
                {
                    if(!attacker.PlaceCreatures[attacker.SelectedCreature].Used)
                    {
                        if (lastSelectedCard.Contains(attacker.PlaceCreatures[attacker.SelectedCreature]))
                        {
                            if (selectedCreature == defensor.PlaceCreatures[defensor.SelectedCreature])
                            {
                                Creaturebattle();
                                //Una vez realizado el ataque se modifican a -1 las criaturas seleccionadas
                                UpdateSelectedCardsForNewAction();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// <b>Headboard: </b> private void creaturebattle()<br/>
        /// <b>Description: </b> Method for update the criatures'life depends of criatures'atack
        /// </summary>
        private void Creaturebattle()
        {
            realPlayer.PlaceCreatures[realPlayer.SelectedCreature].ActualLife -=  iaPlayer.PlaceCreatures[iaPlayer.SelectedCreature].Attack;
            realPlayer.PlaceCreatures[realPlayer.SelectedCreature].Used = true;
            iaPlayer.PlaceCreatures[iaPlayer.SelectedCreature].ActualLife -=  realPlayer.PlaceCreatures[realPlayer.SelectedCreature].Attack;
            iaPlayer.PlaceCreatures[iaPlayer.SelectedCreature].Used = true;
            if (realPlayer.PlaceCreatures[realPlayer.SelectedCreature].ActualLife <= 0)
            {
                //Forma de setear a una criatura por defecto para mantener espacios
                realPlayer.PlaceCreatures[realPlayer.SelectedCreature] = new clsCreature();
                //Forma de eliminar de la lista
                //realPlayer.PlaceCreatures.RemoveAt(realPlayer.SelectedCreature);
            }
            if (iaPlayer.PlaceCreatures[iaPlayer.SelectedCreature].ActualLife <= 0)
            {
                //Forma de setear a una criatura por defecto para mantener espacios
                iaPlayer.PlaceCreatures[iaPlayer.SelectedCreature] = new clsCreature();
                //Forma de eliminar de la lista
                //iaPlayer.PlaceCreatures.RemoveAt(iaPlayer.SelectedCreature);
            }
        }

        #endregion

        #region spells
        /// <summary>
        ///     <Headboard>private void trysendSpell(clsPlayer player)</cabecera>
        ///     <Description>Try to send a spell and update the player hand</descripcion>
        /// </summary>
        /// <param name="player"></param>
        private void TrySendSpell(clsPlayer player)
        {
            if (lastSelectedCard[0] is clsLifeModifyingSpell && player.Hand[player.SelectedCard].IsAvaible &&
                selectedCreature != null && lastSelectedCard != null &&
                selectedCreature.Id > 0 && lastSelectedCard[0].Id > 0 &&
                player.SelectedCard > -1 && player.SelectedCreature > -1)
            {
                SendSpell();
                player.PutCard();
                //player.Hand.RemoveAt(player.SelectedCard);
                UpdateSelectedCardsForNewAction();
            }
        }

        /// <summary>
        /// <b>Headboard: </b> private void SendSpell()<br/>
        /// <b>Description: </b> This method do an action in all the creatures if it is a area spell and on the selected if it is a none area spell
        /// </summary>
        private void SendSpell()
        {
            clsPlayer player = realPlayer.PlaceCreatures[realPlayer.SelectedCreature] == selectedCreature ? realPlayer : iaPlayer;

            if (((clsLifeModifyingSpell)player.Hand[player.SelectedCard]).IsArea) //Valorar si es de area o no
            {
                for (int i = 0; i < player.PlaceCreatures.Count; i++)
                {
                    player.SelectedCreature = i;
                    if (player.PlaceCreatures[i].Id > 0)
                    {
                        AttackOrHealthSpell(player);
                    }
                }
            }
            else
            {
                AttackOrHealthSpell(player);
            }
            NotifyPropertyChanged(nameof(RealPlayer.PlaceCreatures));

        }

        /// <summary>
        /// <b>Headboard: </b> private void AttackOrHealthSpell (clsPlayer player)<br/>
        /// <b>Description: </b> Select between a attack and a health action in function of the damage property of the selected card
        /// </summary>
        /// <param name="player"></param>
        private void AttackOrHealthSpell (clsPlayer player)
        {
            if (((clsLifeModifyingSpell)player.Hand[player.SelectedCard]).IsDamage)
            {
                AttackAction(player);
            }
            else
            {
                HealthAction(player);
            }
        }

        /// <summary>
        /// <b>Headboard: </b> private void attackAction(clsPlayer ofense, clsPlayer defensor)<br/>
        /// <b>Description: </b> Method for make spell attack action
        /// </summary>
        /// <param name="ofense"></param>
        /// <param name="defensor"></param>
        private void AttackAction(clsPlayer player)
        {
            player.PlaceCreatures[player.SelectedCreature].ActualLife -= ((clsLifeModifyingSpell)lastSelectedCard[0]).Effect;
            if (player.PlaceCreatures[player.SelectedCreature].ActualLife <= 0)
            {
                player.PlaceCreatures.RemoveAt(realPlayer.SelectedCreature);
            }
        }


        /// <summary>
        /// <b>Headboard: </b> private void healthAction(clsPlayer player)<br/>
        /// <b>Description: </b> Method for make spell health action, and Santi sucks
        /// </summary>
        /// 
        /// <param name="player"></param>
        private void HealthAction(clsPlayer player)
        {
            player.PlaceCreatures[player.SelectedCreature].ActualLife += ((clsLifeModifyingSpell)player.Hand[player.SelectedCard]).Effect;
            if (player.PlaceCreatures[player.SelectedCreature].ActualLife > player.PlaceCreatures[player.SelectedCreature].Life)
            {
                player.PlaceCreatures[player.SelectedCreature].ActualLife = player.PlaceCreatures[player.SelectedCreature].Life;
            }
        }
        #endregion

        #region attack contrary direct
        /// <summary>
        /// <b>Headboard: </b>private bool noEnemiesFront()<br/>
        /// <b>Description: </b>Return if the other player battelfield has creatures on it<br/>
        /// <b>Preconditions: </b>None<br/>
        /// <b>Postconditions: </b> None<br/>
        /// </summary>
        /// <returns></returns>
        private bool NoEnemiesFront()
        {
            bool noEnemies = true;
            clsPlayer player = isPlayerTurn ? iaPlayer : realPlayer;

            for (int i = 0; i < player.PlaceCreatures.Count && noEnemies; i++)
            {
                    noEnemies = player.PlaceCreatures[i].Id == 0;
            }
            return noEnemies;
        }

        /// <summary>
        /// <b>Headboard: </b>private void attackContraryPlayer(int damage)<br/>
        /// <b>Description: </b>Modifies the player life and check if the match is finished<br/>
        /// <b>Preconditions: </b>None<br/>
        /// <b>Postconditions: </b> None<br/>
        /// </summary>
        /// <param name="damage"></param>
        private void AttackContraryPlayer(int damage)
        {
            if (!isPlayerTurn)
            {
                realPlayer.Life -= damage;
                NotifyPropertyChanged(nameof(RealPlayer));
            }
            else
            {
                iaPlayer.Life -= damage;
                NotifyPropertyChanged(nameof(IaPlayer));
            }
            FinishGame();
        }
        #endregion

        #endregion

        #endregion

    }
}
