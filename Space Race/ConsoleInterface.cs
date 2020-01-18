using System;
using Game_Logic_Class;
using Object_Classes;


namespace Space_Race
{
    class Console_Class
    {
        /// <summary>
        /// Runs the methods required to operate the console
        /// version of the Space Race Game.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {      
            DisplayIntroductionMessage();

            Board.SetUpBoard();
            SpaceRaceGame.TotalPlayers();
            SpaceRaceGame.SetUpPlayers();

            while (!SpaceRaceGame.GameFinished())
            {
                SpaceRaceGame.PlayOneRound();
                if(SpaceRaceGame.CheckAllPlayerFuel())
                {
                    AllPlayersOutOfFuel();
                    break;
                }
                SpaceRaceGame.DisplayPlayerRoundInfo();
            }
            DisplayGameFinished();
            RestartGame();

        }//End Main

   
        /// <summary>
        /// Display a welcome message to the console
        /// Pre:    None.
        /// Post:   A welcome message is displayed to the console.
        /// </summary>
        static void DisplayIntroductionMessage()
        {
            Console.WriteLine("Welcome to Space Race.\n");
        } //End DisplayIntroductionMessage

        /// <summary>
        ///If the game has finished, displays
        ///the results to the console including
        ///winners and individual positions of
        ///all players.
        /// Pre: None.
        /// Post: Details of the players displayed as well as winners.
        /// </summary>
        static void DisplayGameFinished()
        {
            if (SpaceRaceGame.GameFinished())
            {

                //Write all the players who have won to the console.
                for (int kiterator = 0; kiterator < SpaceRaceGame.NumberOfPlayers; kiterator++)
                {
                    if (kiterator == 0)
                    {
                        Console.WriteLine
                            (
                            "\n\n\tThe following player(s) finished the game in round {0}\n", 
                            SpaceRaceGame.round - 1
                            );
                    }

                    if (SpaceRaceGame.Players[kiterator].AtFinish)
                    {
                        Console.WriteLine
                            (
                            "\t\t{0}\n", SpaceRaceGame.Players[kiterator].Name
                            );
                    }

                }

                //Write all players information to the console including the winners.
                for (int jiterator = 0; jiterator < SpaceRaceGame.NumberOfPlayers; jiterator++)
                {
                    if (jiterator == 0)
                    {
                        Console.WriteLine("\n\n\tIndividual players finished at the following locations specified.\n");
                    }

                    Console.WriteLine
                        (
                        "\t\tPlayer {0} with {1} yottawatts of power at square {2}", 
                        SpaceRaceGame.Players[jiterator].Name, 
                        SpaceRaceGame.Players[jiterator].RocketFuel, 
                        SpaceRaceGame.Players[jiterator].Position
                        );

                }
            }
        }//End DisplayGameFinished


        /// <summary>
        ///If the game has been complete prompts
        ///the user to decide if they would like
        ///to play a new game and restarts a fresh
        ///game if they decide yes.
        /// Pre: Prompts user to input "y" or "Y".
        /// Post: If input is "y" or "Y" starts a fresh game,
        /// any other input is counted as a no.
        /// </summary>
        static void RestartGame()
        {
            Console.Write("\n\nPlay again? (Y or N): ");
            string input = Console.ReadLine();

            if(input == "y" || input == "Y")
            {
                Console.WriteLine();
                SpaceRaceGame.Players.Clear();
                SpaceRaceGame.TotalPlayers();
                SpaceRaceGame.SetUpPlayers();
                SpaceRaceGame.round = 1;

                //Code below is essentially running Main again
                //if the user decides to play again.
                while (!SpaceRaceGame.GameFinished())
                {
                    SpaceRaceGame.PlayOneRound();
                    if (SpaceRaceGame.CheckAllPlayerFuel())
                    {
                        AllPlayersOutOfFuel();
                        break;
                    }
                    SpaceRaceGame.DisplayPlayerRoundInfo();
                }

                DisplayGameFinished();
                RestartGame();
            }

            //Else gracefully ends the program.
            else if (input != "y" || input != "Y")
            {
                Console.WriteLine("\nThanks for playing Space Race.");
                PressEnter();
            }

            else
            {
                Console.WriteLine("\nPlease enter Y or N.");
                RestartGame();
            }
            
        }//End RestartGame


        /// <summary>
        ///Writes a message to the console alerting
        ///the players that everyone has run out of
        ///fuel and then reports on their final details.
        /// Pre: None.
        /// Post: User alerted all players are out of fuel
        /// and writes their details to the console.
        /// </summary>
        public static void AllPlayersOutOfFuel()
        {
            Console.WriteLine
                (
                "\n\n\n\n\tAll players have run out of fuel in round {0}!\n", 
                SpaceRaceGame.round
                );
            
            for(int playernum = 0; playernum < SpaceRaceGame.NumberOfPlayers; playernum++)
            {
                Console.WriteLine
                    (
                    "\tPlayer {0} finished on square {1} before they ran out of fuel!", 
                    SpaceRaceGame.Players[playernum].Name, 
                    SpaceRaceGame.Players[playernum].Position
                    );
            }
        }//End AllPlayerOutOfFuel

        /// <summary>
        /// Displays a prompt and waits for a keypress.
        /// Pre:  None.
        /// Post: A key has been pressed.
        /// </summary>
        static void PressEnter()
        {
            Console.Write("\nPress Enter to terminate program ...");
            Console.ReadLine();
        } //End PressEnter

    }//End Console class
}
