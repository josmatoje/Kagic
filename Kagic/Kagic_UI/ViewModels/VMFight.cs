using Kagic_BL;
using Kagic_Entities;
using Kagic_UI.Models;
using Kagic_UI.Models.Utilities;
using Kagic_UI.ViewModels.UtilitiesVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

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
                SetLastSelectedCard(selectedCard);
                //NotifyPropertyChanged(nameof(LastSelectedCard));
                attackContraryPlayerCommand.RaiseCanExecuteChanged();
                healthPlayerCommand.RaiseCanExecuteChanged();
                //}
                selectedCreature = null;
                NotifyPropertyChanged(nameof(SelectedCreature));

            }
        }

        public clsCreature SelectedCreature
        {
            get => selectedCreature;
            set
            {
                selectedCreature = value;
                bool successfulTry = false;
                if (lastSelectedCard[0] is clsCreature)
                {
                    if (realPlayer.Hand.Contains(lastSelectedCard[0]))
                    {
                        successfulTry = TryPutCreature(realPlayer);
                    }
                    else
                    {
                        successfulTry = TryAttackCreature(realPlayer, iaPlayer);
                    }
                }
                else if (lastSelectedCard[0] is clsLifeModifyingSpell)//Spell
                {
                    successfulTry = TrySendSpell(realPlayer);
                }

                if (successfulTry)
                {
                    UpdateSelectedCardsForNewAction();
                }
                else
                {
                    SetLastSelectedCard(value);
                    selectedCard = null;
                    NotifyPropertyChanged(nameof(SelectedCard));
                    selectedCreature = null;
                    NotifyPropertyChanged(nameof(SelectedCreature));
                }

                attackContraryPlayerCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<clsCard> LastSelectedCard { get => lastSelectedCard; }

        public bool CardDetailsVisibility { get => cardDetailsVisibility; set => cardDetailsVisibility = value; }

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

            if (realPlayer.SelectedCardIndex > -1)
            {
                damage = ((clsLifeModifyingSpell)realPlayer.Hand[realPlayer.SelectedCardIndex]).Effect;
                realPlayer.PutCard();
            }
            else
            {
                damage = realPlayer.PlaceCreatures[realPlayer.SelectedCreatureIndex].Attack;
                realPlayer.PlaceCreatures[realPlayer.SelectedCreatureIndex].Used = true;
            }

            AttackContraryPlayer(damage);
            attackContraryPlayerCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        ///     <Headboard>private bool atackContraryPlayerCommand_CanExecute()</cabecera>
        ///     <Description></descripcion>
        /// </summary>
        /// <returns></returns>
        private bool atackContraryPlayerCommand_CanExecute()
        {
            return (realPlayer.SelectedCardIndex > -1 && selectedCard is clsLifeModifyingSpell && ((clsLifeModifyingSpell)selectedCard).IsDamage && selectedCard.IsAvaible) || (realPlayer.SelectedCreatureIndex > -1 && !realPlayer.PlaceCreatures[realPlayer.SelectedCreatureIndex].Used && NoEnemiesFront());
        }

        /// <summary>
        ///     <Headboard>private void passTurnCommand_Executed()</cabecera>
        ///     <Description>Calls changeTurn's method </descripcion> 
        /// </summary>
        private void healthPlayerCommand_Executed()
        {
            int health = 0;

            health = ((clsLifeModifyingSpell)realPlayer.Hand[realPlayer.SelectedCardIndex]).Effect;

            realPlayer.Life += health;
            if (realPlayer.Life > clsPlayer.MAX_LIFE)
            {
                realPlayer.Life = clsPlayer.MAX_LIFE;
            }
            realPlayer.PutCard();
        }

        /// <summary>
        ///     <Headboard>private bool atackContraryPlayerCommand_CanExecute()</cabecera>
        ///     <Description></descripcion>
        /// </summary>
        /// <returns></returns>
        private bool healthPlayerCommand_CanExecute()
        {
            return realPlayer.SelectedCardIndex != -1 && selectedCard is clsLifeModifyingSpell && !((clsLifeModifyingSpell)selectedCard).IsDamage && selectedCard.IsAvaible;
        }
        #endregion

        #region private methods

        #region game management
        /// <summary>
        ///     <Headboard>private void startGame()</cabecera>
        ///     <Description>First method done when a game start. Prepare the decks of the players</descripcion>
        /// </summary>
        async private void StartGame()
        {
            List<clsCard> cards = new List<clsCard>(clsCardsManagementBL.getCardsListBL());
            realPlayer = new clsPlayer(CardsDeck(cards));
            iaPlayer = new clsIAPlayer(CardsDeck(cards));
            lastSelectedCard = new ObservableCollection<clsCard>();
            SetLastSelectedCard(null);
            //random para ver quien empieza isPlayerTurn
            //isPlayerTurn = (new Random()).Next(10) > 5;
            isPlayerTurn = true;
            await Task.Delay(1000);
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
                    if (cardsList[position] is clsCreature)
                    {
                        deck.Add(new clsCreatureNotified(cardsList[position] as clsCreature));
                    }
                    else
                    {
                        deck.Add(new clsLifeModifyingSpell(cardsList[position] as clsLifeModifyingSpell));
                    }
                    i++;
                }
            }

            return deck;
        }

        /// <summary>
        ///     <Headboard>private void changeTurn()</cabecera>
        ///     <Description>it is done when a turn finishes. Set mana used and  reset selected cards</descripcion>
        /// </summary>
        private async void ChangeTurn()
        {
            isPlayerTurn = !isPlayerTurn;
            await Task.Delay(1000);
            if (isPlayerTurn)
            {
                realPlayer.SetMana();
                realPlayer.DrawCard();
                await Task.Delay(500);
                realPlayer.SetUsedCreatures();
                NotifyPropertyChanged(nameof(RealPlayer));
                UpdateSelectedCardsForNewAction();
            }
            else
            {
                iaPlayer.SetMana();
                iaPlayer.DrawCard();
                await Task.Delay(1000);
                iaPlayer.SetUsedCreatures();
                //NotifyPropertyChanged(nameof(IaPlayer));
                UpdateSelectedCardsForNewAction();
                //metodo accion ia
                IaTurn();
            }

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
            selectedCard = null;
            NotifyPropertyChanged(nameof(SelectedCard));
            selectedCreature = null;
            NotifyPropertyChanged(nameof(SelectedCreature));
            SetLastSelectedCard(null);
        }

        /// <summary>
        /// <b>Headboard: </b>private void iaTurn()<br/>
        /// <b>Description: </b>akes<br/>
        /// <b>Preconditions: </b>None<br/>
        /// <b>Postconditions: </b>Hand updated<br/>
        /// </summary>
        private async void IaTurn()
        {
            int enemyCreatureIndex;
            bool usingCards = true, targetSelected;
            //place creatures
            while (usingCards)
            {
                if (iaPlayer.SelectHandCard())
                {
                    selectedCard = iaPlayer.Hand[iaPlayer.SelectedCardIndex];
                    SetLastSelectedCard(selectedCard);
                    if (selectedCard is clsCreature)
                    {
                        if (iaPlayer.PickPlace())
                        {
                            selectedCreature = iaPlayer.PlaceCreatures[iaPlayer.SelectedCreatureIndex];
                            TryPutCreature(iaPlayer);
                            await Task.Delay(1000);
                        }
                        else
                        {
                            usingCards = false;
                        }
                    }
                    else
                    {
                        await Task.Delay(1000);
                        if ((selectedCard as clsLifeModifyingSpell).IsDamage)
                        {
                            if (NoEnemiesFront())
                            {
                                AttackContraryPlayer(((clsLifeModifyingSpell)selectedCard).Effect);
                                iaPlayer.PutCard();
                                await Task.Delay(1000);
                            }
                            else
                            {
                                enemyCreatureIndex = iaPlayer.PickEnemyCreature(realPlayer.PlaceCreatures);
                                if (enemyCreatureIndex > -1) //If there are enemies
                                {
                                    realPlayer.SelectedCreatureIndex = enemyCreatureIndex;
                                    selectedCreature = realPlayer.PlaceCreatures[realPlayer.SelectedCreatureIndex];
                                    TrySendSpell(iaPlayer);
                                    await Task.Delay(1000);
                                }
                            }
                        }
                        else //Healing
                        {
                            targetSelected = false;
                            for (int i = 0; i < iaPlayer.PlaceCreatures.Count && !targetSelected; i++)
                            {
                                if (iaPlayer.PlaceCreatures[i].ActualLife < iaPlayer.PlaceCreatures[i].Life)
                                {
                                    selectedCreature = iaPlayer.PlaceCreatures[i];
                                    iaPlayer.SelectedCreatureIndex = i;
                                    TrySendSpell(IaPlayer);
                                    await Task.Delay(1000);
                                    targetSelected = true;
                                }
                            }
                            if (!targetSelected) //Si no se ha usado la carta
                            {
                                if (IaPlayer.Life < clsPlayer.MAX_LIFE)
                                {
                                    iaPlayer.Life += ((clsLifeModifyingSpell)iaPlayer.Hand[iaPlayer.SelectedCardIndex]).Effect;
                                    if (iaPlayer.Life > clsPlayer.MAX_LIFE)
                                    {
                                        iaPlayer.Life = clsPlayer.MAX_LIFE;
                                    }
                                    iaPlayer.PutCard();
                                    await Task.Delay(1000);
                                }
                                else
                                {
                                    selectedCard.IsAvaible = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    usingCards = false;
                }
            }

            //attack enemies  
            while (iaPlayer.PickOwnCreature()) //While select creatures that can attack ande there is enemies infront
            {
                if (!NoEnemiesFront())
                {
                    //selectedCreature = iaPlayer.PlaceCreatures[iaPlayer.SelectedCreatureIndex];
                    //SetLastSelectedCard(selectedCreature);
                    enemyCreatureIndex = iaPlayer.PickEnemyCreature(realPlayer.PlaceCreatures);
                    if (enemyCreatureIndex > -1)
                    {
                        //selectedCreature = realPlayer.PlaceCreatures[enemyCreatureIndex];
                        realPlayer.SelectedCreatureIndex = enemyCreatureIndex;
                        Creaturebattle();
                        await Task.Delay(1000);
                    }
                }
                else
                {
                    AttackContraryPlayer(iaPlayer.PlaceCreatures[iaPlayer.SelectedCreatureIndex].Attack);
                    iaPlayer.PlaceCreatures[iaPlayer.SelectedCreatureIndex].Used = true;
                }
            }

            ChangeTurn();
        }

        /// <summary>
        /// <b>Headboard: </b>private void TryPutCreature()<br/>
        /// <b>Description: </b>Check if is posible to put the selected card on the batelfield and, if it is, do it<br/>
        /// <b>Preconditions: </b> It muss be called after change de selectedCreature but before change the lastSelectedCard<br/>
        /// <b>Postconditions: </b> None<br/>
        /// <returns>Boolean indicating if the try was successful</returns>
        /// </summary>
        private bool TryPutCreature(clsPlayer player)
        {
            bool successfulTry = false;
            if (lastSelectedCard != null && lastSelectedCard.Contains(selectedCard) && //Si la ultima carta seleccionada es una carta de tu mano que has seleccionado justo antes
                selectedCreature != null && selectedCreature.Id == 0 &&
                player.SelectedCardIndex > -1 && player.SelectedCreatureIndex > -1 &&
                player.PlaceCreatures[player.SelectedCreatureIndex].Id == 0)
            {
                player.PutCard();
                //Una vez colocada la carta se modifican a -1 las criaturas seleccionadas
                //UpdateSelectedCardsForNewAction();
                SetLastSelectedCard(null);
                successfulTry = true;
            }
            return successfulTry;
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
            cardDetailsVisibility = (card != null && card.Id > 0 && !(card is clsCreature && (card as clsCreature).ActualLife <= 0)); //Id==0 -> new clsCreature();
            NotifyPropertyChanged(nameof(CardDetailsVisibility));
            lastSelectedCard.Clear();
            lastSelectedCard.Add(card);
        }
        #endregion

        #region attack management

        #region creature battle

        /// <summary>
        /// <b>Headboard: </b>private void TryPutCreature()<br/>
        /// <b>Description: </b>Check if is posible to put the selected card on the batelfield and, if it is, do it<br/>
        /// <b>Preconditions: </b> It muss be called after change de selectedCreature but before change the lastSelectedCard<br/>
        /// <b>Postconditions: </b> None<br/>
        /// <returns>Boolean indicating if the try was successful</returns>
        /// </summary>
        private bool TryAttackCreature(clsPlayer attacker, clsPlayer defensor)
        {
            bool successfulTry = false;
            if (lastSelectedCard != null && selectedCreature != null &&
                selectedCreature.Id > 0)
            {
                if (attacker.SelectedCreatureIndex > -1 && defensor.SelectedCreatureIndex > -1)
                {
                    if (!attacker.PlaceCreatures[attacker.SelectedCreatureIndex].Used)
                    {
                        if (lastSelectedCard.Contains(attacker.PlaceCreatures[attacker.SelectedCreatureIndex]))
                        {
                            if (selectedCreature == defensor.PlaceCreatures[defensor.SelectedCreatureIndex])
                            {
                                Creaturebattle();
                                //Una vez realizado el ataque se modifican a -1 las criaturas seleccionadas
                                SetLastSelectedCard(null);
                                successfulTry = true;
                            }
                        }
                    }
                }
            }
            return successfulTry;
        }

        /// <summary>
        /// <b>Headboard: </b> private void creaturebattle()<br/>
        /// <b>Description: </b> Method for update the criatures'life depends of criatures'atack
        /// </summary>
        private void Creaturebattle()
        {
            realPlayer.PlaceCreatures[realPlayer.SelectedCreatureIndex].ActualLife -= iaPlayer.PlaceCreatures[iaPlayer.SelectedCreatureIndex].Attack;
            realPlayer.PlaceCreatures[realPlayer.SelectedCreatureIndex].Used = true;
            iaPlayer.PlaceCreatures[iaPlayer.SelectedCreatureIndex].ActualLife -= realPlayer.PlaceCreatures[realPlayer.SelectedCreatureIndex].Attack;
            iaPlayer.PlaceCreatures[iaPlayer.SelectedCreatureIndex].Used = true;
            if (realPlayer.PlaceCreatures[realPlayer.SelectedCreatureIndex].ActualLife <= 0)
            {
                //Forma de setear a una criatura por defecto para mantener espacios
                realPlayer.PlaceCreatures[realPlayer.SelectedCreatureIndex] = new clsCreatureNotified();//-----------------------------------
                //Forma de eliminar de la lista
                //realPlayer.PlaceCreatures.RemoveAt(realPlayer.SelectedCreature);
            }
            if (iaPlayer.PlaceCreatures[iaPlayer.SelectedCreatureIndex].ActualLife <= 0)
            {
                //Forma de setear a una criatura por defecto para mantener espacios
                iaPlayer.PlaceCreatures[iaPlayer.SelectedCreatureIndex] = new clsCreatureNotified();//------------------------------------
                //Forma de eliminar de la lista
                //iaPlayer.PlaceCreatures.RemoveAt(iaPlayer.SelectedCreature);
            }


        }

        #endregion

        #region spells
        /// <summary>
        ///     <Headboard>private void trysendSpell(clsPlayer player)</cabecera>
        ///     <Description>Try to send a spell to a creature, update the player hand and do the effects on the creatures if is posible</descripcion>
        /// </summary>
        /// <param name="player"></param>
        /// <returns>Boolean indicating if the try was successful</returns>
        private bool TrySendSpell(clsPlayer player)
        {
            bool successfulTry = false;
            if (lastSelectedCard[0] is clsLifeModifyingSpell)
            {
                if (selectedCreature != null && lastSelectedCard != null)
                {
                    if (selectedCreature.Id > 0 && lastSelectedCard[0].Id > 0)
                    {
                        if (player.SelectedCardIndex > -1 && (realPlayer.SelectedCreatureIndex > -1 || iaPlayer.SelectedCreatureIndex > -1))
                        {
                            if (player.Hand[player.SelectedCardIndex].IsAvaible)
                            {
                                SendSpell(player);
                                player.PutCard();
                                SetLastSelectedCard(null);
                                successfulTry = true;
                            }
                        }
                    }
                }
            }
            return successfulTry;
        }

        /// <summary>
        /// <b>Headboard: </b> private void SendSpell()<br/>
        /// <b>Description: </b> This method do an action in all the creatures if it is a area spell and on the selected if it is a none area spell
        /// </summary>
        private void SendSpell(clsPlayer player)
        {
            clsPlayer targetPlayer = realPlayer.SelectedCreatureIndex > -1 && realPlayer.PlaceCreatures[realPlayer.SelectedCreatureIndex] == selectedCreature ? realPlayer : iaPlayer;
            bool isDamage = ((clsLifeModifyingSpell)selectedCard).IsDamage;
            int effect = ((clsLifeModifyingSpell)selectedCard).Effect;

            if (((clsLifeModifyingSpell)player.Hand[player.SelectedCardIndex]).IsArea) //Valorar si es de area o no
            {
                for (int i = 0; i < targetPlayer.PlaceCreatures.Count; i++)
                {
                    targetPlayer.SelectedCreatureIndex = i; //Aqui peta tambien
                    if (selectedCreature is null)
                    {
                        selectedCreature = targetPlayer.PlaceCreatures[i];
                    }
                    if (targetPlayer.PlaceCreatures[i].Id > 0)
                    {
                        AttackOrHealthSpell(targetPlayer, isDamage, effect);
                    }
                }
            }
            else
            {
                AttackOrHealthSpell(targetPlayer, isDamage, effect);
            }
        }

        /// <summary>
        /// <b>Headboard: </b> private void AttackOrHealthSpell (clsPlayer player)<br/>
        /// <b>Description: </b> Select between a attack and a health action in function of the damage property of the selected card
        /// </summary>
        /// <param name="player"></param>
        /// <param name="isDamage"></param>
        /// <param name="effect"></param>
        private void AttackOrHealthSpell(clsPlayer player, bool isDamage, int effect)
        {
            if (isDamage)
            {
                AttackAction(player, effect);
            }
            else
            {
                HealthAction(player, effect);
            }
        }

        /// <summary>
        /// <b>Headboard: </b> private void attackAction(clsPlayer ofense, clsPlayer defensor)<br/>
        /// <b>Description: </b> Method for make spell attack action
        /// </summary>
        /// <param name="player"></param>
        /// <param name="damage"></param>
        private void AttackAction(clsPlayer player, int damage)
        {
            player.PlaceCreatures[player.SelectedCreatureIndex].ActualLife -= damage;
            if (player.PlaceCreatures[player.SelectedCreatureIndex].ActualLife <= 0)
            {
                player.PlaceCreatures[player.SelectedCreatureIndex] = new clsCreatureNotified();//---------------------------------------------------
            }
        }


        /// <summary>
        /// <b>Headboard: </b> private void healthAction(clsPlayer player)<br/>
        /// <b>Description: </b> Method for make spell health action
        /// </summary>
        /// <param name="player"></param>
        /// <param name="health"></param>
        private void HealthAction(clsPlayer player, int health)
        {
            player.PlaceCreatures[player.SelectedCreatureIndex].ActualLife += health;
            if (player.PlaceCreatures[player.SelectedCreatureIndex].ActualLife > player.PlaceCreatures[player.SelectedCreatureIndex].Life)
            {
                player.PlaceCreatures[player.SelectedCreatureIndex].ActualLife = player.PlaceCreatures[player.SelectedCreatureIndex].Life;
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
            }
            else
            {
                iaPlayer.Life -= damage;
            }
            FinishGame();
        }
        #endregion

        #endregion

        #endregion

    }
}
