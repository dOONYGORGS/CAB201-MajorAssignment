using Game_Logic_Class;
using Object_Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI_Class
{
    public partial class SpaceRaceForm : Form
    {
        // The numbers of rows and columns on the screen.
        const int NUM_OF_ROWS = 7;
        const int NUM_OF_COLUMNS = 8;

        //Track what round the game is in
        private int roundCounter = 0;

        //Track the selection of single step mode
        private bool singleStep = false;

        //Track player element in single step mode
        //and which turn.
        private int turnCounter = 0;

        //New dies to be used
        private static Die die1 = new Die(), die2 = new Die();

        // When we update what's on the screen, we show the movement of a player 
        // by removing them from their old square and adding them to their new square.
        // This enum makes it clear that we need to do both.
        enum TypeOfGuiUpdate { AddPlayer, RemovePlayer };


        public SpaceRaceForm()
        {
            InitializeComponent();

            Board.SetUpBoard();
            ResizeGUIGameBoard();
            SetUpGUIGameBoard();
            SetupPlayersDataGridView();
            DetermineNumberOfPlayers();
            SpaceRaceGame.SetUpPlayers();
            PrepareToPlayGame();
        }

        /// <summary>
        /// Handle the Exit button being clicked.
        /// Pre:  the Exit button is clicked.
        /// Post: the game is terminated immediately
        /// </summary>
        private void exitButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Resizes the entire form, so that the individual squares have their correct size, 
        /// as specified by SquareControl.SQUARE_SIZE.  
        /// This method allows us to set the entire form's size to approximately correct value 
        /// when using Visual Studio's Designer, rather than having to get its size correct to the last pixel.
        /// Pre:  none.
        /// Post: the board has the correct size.
        /// </summary>
        private void ResizeGUIGameBoard()
        {
            const int SQUARE_SIZE = SquareControl.SQUARE_SIZE;
            int currentHeight = tableLayoutPanel.Size.Height;
            int currentWidth = tableLayoutPanel.Size.Width;
            int desiredHeight = SQUARE_SIZE * NUM_OF_ROWS;
            int desiredWidth = SQUARE_SIZE * NUM_OF_COLUMNS;
            int increaseInHeight = desiredHeight - currentHeight;
            int increaseInWidth = desiredWidth - currentWidth;
            this.Size += new Size(increaseInWidth, increaseInHeight);
            tableLayoutPanel.Size = new Size(desiredWidth, desiredHeight);

        }// ResizeGUIGameBoard


        /// <summary>
        /// Creates a SquareControl for each square and adds it to the appropriate square of the tableLayoutPanel.
        /// Pre:  none.
        /// Post: the tableLayoutPanel contains all the SquareControl objects for displaying the board.
        /// </summary>
        private void SetUpGUIGameBoard()
        {
            for (int squareNum = Board.START_SQUARE_NUMBER; squareNum <= Board.FINISH_SQUARE_NUMBER; squareNum++)
            {
                Square square = Board.Squares[squareNum];
                SquareControl squareControl = new SquareControl(square, SpaceRaceGame.Players);
                AddControlToTableLayoutPanel(squareControl, squareNum);
            }//endfor

        }// end SetupGameBoard

        private void AddControlToTableLayoutPanel(Control control, int squareNum)
        {
            int screenRow = 0;
            int screenCol = 0;
            MapSquareNumToScreenRowAndColumn(squareNum, out screenRow, out screenCol);
            tableLayoutPanel.Controls.Add(control, screenCol, screenRow);
        }// end Add Control


        /// <summary>
        /// For a given square number, tells you the corresponding row and column number
        /// on the TableLayoutPanel.
        /// Pre:  none.
        /// Post: returns the row and column numbers, via "out" parameters.
        /// </summary>
        /// <param name="squareNumber">The input square number.</param>
        /// <param name="rowNumber">The output row number.</param>
        /// <param name="columnNumber">The output column number.</param>
        private static void MapSquareNumToScreenRowAndColumn(int squareNum, out int screenRow, out int screenCol)
        {
            screenRow = 0;
            screenCol = 0;

            screenRow = (NUM_OF_ROWS - 1) - squareNum / 8;

            if (screenRow % 2 == 0)
            {
                screenCol = squareNum % 8;
            }
            else if (screenRow % 2 != 0)
            {
                screenCol = (NUM_OF_COLUMNS - 1) - squareNum % 8;
            }

        }//end MapSquareNumToScreenRowAndColumn


        private void SetupPlayersDataGridView()
        {
            // Stop the playersDataGridView from using all Player columns.
            playersDataGridView.AutoGenerateColumns = false;
            // Tell the playersDataGridView what its real source of data is.
            playersDataGridView.DataSource = SpaceRaceGame.Players;

        }// end SetUpPlayersDataGridView



        /// <summary>
        /// Obtains the current "selected item" from the ComboBox
        ///  and
        ///  sets the NumberOfPlayers in the SpaceRaceGame class.
        ///  Pre: none
        ///  Post: NumberOfPlayers in SpaceRaceGame class has been updated
        /// </summary>
        private void DetermineNumberOfPlayers()
        {
            // Store the SelectedItem property of the ComboBox in a string
            string comboBoxPlayers = playerNumberComboBox.SelectedItem.ToString();
            // Parse string to a number
            int.TryParse(comboBoxPlayers, out int numplayers);
            // Set the NumberOfPlayers in the SpaceRaceGame class to that number
            SpaceRaceGame.NumberOfPlayers = numplayers;
        }//end DetermineNumberOfPlayers

        /// <summary>
        /// The players' tokens are placed on the Start square
        /// </summary>
        private void PrepareToPlayGame()
        {
            UpdatePlayersGuiLocations(TypeOfGuiUpdate.RemovePlayer);
            SpaceRaceGame.Players.Clear();
            SetupPlayersDataGridView();
            DetermineNumberOfPlayers();
            SpaceRaceGame.SetUpPlayers();
            UpdatePlayersGuiLocations(TypeOfGuiUpdate.AddPlayer);

        }//end PrepareToPlay()


        /// <summary>
        /// Tells you which SquareControl object is associated with a given square number.
        /// Pre:  a valid squareNumber is specified; and
        ///       the tableLayoutPanel is properly constructed.
        /// Post: the SquareControl object associated with the square number is returned.
        /// </summary>
        /// <param name="squareNumber">The square number.</param>
        /// <returns>Returns the SquareControl object associated with the square number.</returns>
        private SquareControl SquareControlAt(int squareNum)
        {
            int screenRow;
            int screenCol;
            MapSquareNumToScreenRowAndColumn(squareNum, out screenRow, out screenCol);
            return (SquareControl)tableLayoutPanel.GetControlFromPosition(screenCol, screenRow);
        }


        /// <summary>
        /// Tells you the current square number of a given player.
        /// Pre:  a valid playerNumber is specified.
        /// Post: the square number of the player is returned.
        /// </summary>
        /// <param name="playerNumber">The player number.</param>
        /// <returns>Returns the square number of the player.</returns>
        private int GetSquareNumberOfPlayer(int playerNumber)
        {
            int squarelocation = SpaceRaceGame.Players[playerNumber].Position;
            return squarelocation;
        }//end GetSquareNumberOfPlayer


        /// <summary>
        /// When the SquareControl objects are updated (when players move to a new square),
        /// the board's TableLayoutPanel is not updated immediately.  
        /// Each time that players move, this method must be called so that the board's TableLayoutPanel 
        /// is told to refresh what it is displaying.
        /// Pre:  none.
        /// Post: the board's TableLayoutPanel shows the latest information 
        ///       from the collection of SquareControl objects in the TableLayoutPanel.
        /// </summary>
        private void RefreshBoardTablePanelLayout()
        {
            tableLayoutPanel.Invalidate(true);
        }

        /// <summary>
        /// When the Player objects are updated (location, etc),
        /// the players DataGridView is not updated immediately.  
        /// Each time that those player objects are updated, this method must be called 
        /// so that the players DataGridView is told to refresh what it is displaying.
        /// Pre:  none.
        /// Post: the players DataGridView shows the latest information 
        ///       from the collection of Player objects in the SpaceRaceGame.
        /// </summary>
        private void UpdatesPlayersDataGridView()
        {
            SpaceRaceGame.Players.ResetBindings();
        }

        /// <summary>
        /// At several places in the program's code, it is necessary to update the GUI board,
        /// so that player's tokens are removed from their old squares
        /// or added to their new squares. E.g. at the end of a round of play or 
        /// when the Reset button has been clicked.
        /// 
        /// Moving all players from their old to their new squares requires this method to be called twice: 
        /// once with the parameter typeOfGuiUpdate set to RemovePlayer, and once with it set to AddPlayer.
        /// In between those two calls, the players locations must be changed. 
        /// Otherwise, you won't see any change on the screen.
        /// 
        /// Pre:  the Players objects in the SpaceRaceGame have each players' current locations
        /// Post: the GUI board is updated to match 
        /// </summary>
        private void UpdatePlayersGuiLocations(TypeOfGuiUpdate typeOfGuiUpdate)
        {
            for (int iterator = 0; iterator < SpaceRaceGame.NumberOfPlayers; iterator++)
            {
                int squarenum = GetSquareNumberOfPlayer(iterator);
                SquareControl square = SquareControlAt(squarenum);

                if (typeOfGuiUpdate == TypeOfGuiUpdate.AddPlayer)
                {
                    square.ContainsPlayers[iterator] = true;
                }
                else if (typeOfGuiUpdate == TypeOfGuiUpdate.RemovePlayer)
                {
                    square.ContainsPlayers[iterator] = false;
                }

            }
            RefreshBoardTablePanelLayout();
        } //end UpdatePlayersGuiLocations


        /// <summary>
        ///Operates the single step function of the GUI implementation
        /// Pre: None
        /// Post: Plays a turn for a single player
        /// </summary>
        private void PlayOneTurn()
        {
            //Turn counter is used here to iterate through the player list one by one.
            if (!SpaceRaceGame.Players[turnCounter].AtFinish && SpaceRaceGame.Players[turnCounter].HasPower)
            {
                SpaceRaceGame.Players[turnCounter].Play(die1, die2);
            }

            turnCounter++;

            //Prevent the turncounter from exceeding the length of the players array
            //Reset it back to 0 if it has reached the end.
            if (turnCounter == SpaceRaceGame.NumberOfPlayers)
            {
                turnCounter = 0;
            }
        }

        /// <summary>
        ///Displays a message box with all the players who have finished the game.
        /// Pre: None
        /// Post: Displays a message box.
        /// </summary>
        private void DisplayFinishMessageBox()
        {
            string finishMsg = "The following player(s) finished the game\n\n";
            string finishedplayers = "";

            for (int iterator = 0; iterator < SpaceRaceGame.NumberOfPlayers; iterator++)
            {
                if (SpaceRaceGame.Players[iterator].AtFinish)
                {
                    finishedplayers += SpaceRaceGame.Players[iterator].Name + "\n\n";
                }
            }

            MessageBox.Show(finishMsg + finishedplayers);
        }

        /// <summary>
        ///Displays a message box when all the players in the game have
        ///run out of fuel.
        /// Pre: None
        /// Post: Displays a message box.
        /// </summary>
        private void DisplayAllOutOfFuelMessageBox()
        {
            string outOfFuel = "All players have run out of fuel!";

            MessageBox.Show(outOfFuel);
        }

        private void rollDiceButton_Click(object sender, EventArgs e)
        {
            UpdatePlayersGuiLocations(TypeOfGuiUpdate.RemovePlayer);

            if (!singleStep)
            {
                roundCounter++;
                SpaceRaceGame.PlayOneRound();
            }

            else if (singleStep)
            {
                PlayOneTurn();
            }

            UpdatePlayersGuiLocations(TypeOfGuiUpdate.AddPlayer);
            UpdatesPlayersDataGridView();

            if (roundCounter > 0)
            {
                roundDisabler();
            }
            else if (turnCounter > 0)
            {
                turnDisabler();
            }
            else if (turnCounter == 0 || turnCounter == SpaceRaceGame.NumberOfPlayers)
            {
                startOfTurnDisabler();
            }

            if (SpaceRaceGame.GameFinished())
            {
                guiGameFinished();
            }

            if (SpaceRaceGame.CheckAllPlayerFuel())
            {
                allPlayersOutOfFuel();
            }
        }

        private void startOfTurnDisabler()
        {
            exitButton.Enabled = true;
            resetGameButton.Enabled = true;
            playersDataGridView.Columns[1].ReadOnly = true;
            playerNumberComboBox.Enabled = false;
        }

        private void turnDisabler()
        {
            exitButton.Enabled = false;
            resetGameButton.Enabled = false;
            playersDataGridView.Columns[1].ReadOnly = true;
            playerNumberComboBox.Enabled = false;
        }

        private void roundDisabler()
        {
            resetGameButton.Enabled = true;
            playersDataGridView.Columns[1].ReadOnly = true;
            playerNumberComboBox.Enabled = false;
        }

        private void allPlayersOutOfFuel()
        {
            DisplayAllOutOfFuelMessageBox();
            rollDiceButton.Enabled = false;
            radioButtonsBox.Enabled = false;
            exitButton.Enabled = true;
            playerNumberComboBox.Enabled = false;
            resetGameButton.Enabled = true;
        }

        private void guiGameFinished()
        {
            DisplayFinishMessageBox();
            rollDiceButton.Enabled = false;
            radioButtonsBox.Enabled = false;
            exitButton.Enabled = true;
            playerNumberComboBox.Enabled = false;
            resetGameButton.Enabled = true;
        }

        private void resetGameButton_Click(object sender, EventArgs e)
        {
            roundCounter = 0;
            turnCounter = 0;

            resetGameButton.Enabled = false;
            rollDiceButton.Enabled = false;
            playerNumberComboBox.Enabled = true;
            playersDataGridView.Columns[1].ReadOnly = false;
            radioButtonsBox.Enabled = true;
            yesRadioButton.Checked = false;
            noRadioButton.Checked = false;
            PrepareToPlayGame();

        }

        private void playerNumberComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            playerNumberComboBox.Enabled = false;
            radioButtonsBox.Enabled = true;
            rollDiceButton.Enabled = false;
            PrepareToPlayGame();
        }

        private void noRadioButton_Click(object sender, EventArgs e)
        {
            rollDiceButton.Enabled = true;
            exitButton.Enabled = true;
            singleStep = false;
            radioButtonsBox.Enabled = false;
        }

        private void yesRadioButton_Click(object sender, EventArgs e)
        {
            rollDiceButton.Enabled = true;
            singleStep = true;
            radioButtonsBox.Enabled = false;
        }
    }// end class
}
