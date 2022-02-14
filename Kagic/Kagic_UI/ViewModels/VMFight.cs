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
        ///     <cabecera>private void BuscarCommand_Executed()</cabecera>
        ///     <descripcion>
        ///         Método para realizar la función del command buscar al ser ejecutado.
        ///         El command busca en la lista de personas, aquellos nombres o apellidos que contengan el contenido del textbox txbBarraBusqueda
        ///     </descripcion> 
        /// </summary>
        private void passTurnCommand_Executed()
        {
            changeTurn();
        }

        /// <summary>
        ///     <cabecera>private bool PuedeEjercutarBuscarCommand()</cabecera>
        ///     <descripcion>
        ///         Método para comprobar que el command buscar se puede ejecutar.
        ///         El boton se habilitará si la cadena txbBarraBusqueda es diferente de nula o no esta vacía
        ///     </descripcion>
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
            realPlayer.SelectedCreature = null;
            iaPlayer.SelectedCard = null;
            iaPlayer.SelectedCreature = null;
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
            if (player.SelectedCard.GetType() == typeof(clsCreature))
            {
                creaturebattle();
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
        /// <b>Cabecera: </b> private void creaturebattle()<br/>
        /// <b>Descripcion: </b> Method for update the selectedcards'life depends of selectedcards'atack
        /// </summary>
        private void creaturebattle()
        {
            realPlayer.SelectedCreature.Actuallife = realPlayer.SelectedCreature.Actuallife - iaPlayer.SelectedCreature.Atack;
            if(realPlayer.SelectedCreature.Actuallife <= 0){
                realPlayer.PlaceCreatures.Remove(realPlayer.SelectedCreature);
                NotifyPropertyChanged("RealPlayer.PlaceCreatures");
            }
            NotifyPropertyChanged("RealPlayer.SelectedCreature");
            iaPlayer.SelectedCreature.Actuallife = iaPlayer.SelectedCreature.Actuallife - realPlayer.SelectedCreature.Atack;
            if (iaPlayer.SelectedCreature.Actuallife <= 0)
            {
                iaPlayer.PlaceCreatures.Remove(iaPlayer.SelectedCreature);
                NotifyPropertyChanged("IaPlayer.PlaceCreatures");
            }
            NotifyPropertyChanged("IaPlayer.SelectedCreature");
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
            NotifyPropertyChanged("IaPlayer.PlaceCreatures");
            NotifyPropertyChanged("RealPlayer.PlaceCreatures");
        }

        /// <summary>
        /// Metodo para realizar la accion de lanzar un hechizo de ataque
        /// </summary>
        /// <param name="ofense"></param>
        /// <param name="defensor"></param>
        private void attackAction(clsPlayer ofense, clsPlayer defensor)
        {
            defensor.SelectedCreature.Actuallife = defensor.SelectedCreature.Actuallife - ((clsLifeModifyingSpell)ofense.SelectedCard).Effect;
            if (defensor.SelectedCreature.Actuallife <= 0)
            {
                defensor.PlaceCreatures.Remove(defensor.SelectedCreature);
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
                NotifyPropertyChanged("RealPlayer.SelectedCreature");

            }
            else
            {
                healthAction(iaPlayer);
                NotifyPropertyChanged("IaPlayer.Life");
                NotifyPropertyChanged("IaPlayer.SelectedCreature");
            }
            
        }

        /// <summary>
        /// Metodo para realizar la accion de curar generico
        /// </summary>
        /// <param name="player"></param>
        private void healthAction(clsPlayer player)
        {
            if (player.SelectedCreature != null)
            {
                player.SelectedCreature.Actuallife = player.SelectedCreature.Actuallife + ((clsLifeModifyingSpell)player.SelectedCard).Effect;
                if (player.SelectedCreature.Actuallife > player.SelectedCreature.Life)
                {
                    player.SelectedCreature.Actuallife = player.SelectedCreature.Life;
                }
            }
            else
            {
                player.Life = player.Life + ((clsLifeModifyingSpell)player.SelectedCard).Effect;
                if (player.Life > clsPlayer.MAX_LIFE)
                {
                    player.Life = clsPlayer.MAX_LIFE;
                }
            }
        }
        #endregion

    }
}
