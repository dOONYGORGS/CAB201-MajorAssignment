using System;
using System.Drawing;
using System.ComponentModel;
using Object_Classes;


namespace Game_Logic_Class
{
    public static class SpaceRaceGame
    {
        //Minimum and maximum number of players.
        public const int MIN_PLAYERS = 2;
        public const int MAX_PLAYERS = 6;
        
        //Default number of players.
        private static int numberOfPlayers = 2;

        //Publically accessible int that tracks the current round number.
        public static int round = 1;

    public static int NumberOfPlayers
        {
            get
            {
                return numberOfPlayers;
            }
            set
            {
                numberOfPlayers = value;
            }
        }

        public static string[] names = { "One", "Two", "Three", "Four", "Five", "Six" };  // default values
        
        // Only used in Part B - GUI Implementation, the colours of each player's token
        private static Brush[] playerTokenColours = new Brush[MAX_PLAYERS] { Brushes.Yellow, Brushes.Red,
                                                                       Brushes.Orange, Brushes.White,
                                                                      Brushes.Green, Brushes.DarkViolet};
        /// <summary>
        /// A BindingList is like an array which grows as elements are added to it.
        /// </summary>
        private static BindingList<Player> players = new BindingList<Player>();
        public static BindingList<Player> Players
        {
            get
            {
                return players;
            }
        }

        // The pair of die to be rolled for each round in the console
        // implementation.
        private static Die die1 = new Die(), die2 = new Die();
       

        /// <summary>
        /// Set up the conditions for this game as well as
        ///   creating the required number of players, adding each player 
        ///   to the Binding List and initialize the player's instance variables
        ///   except for playerTokenColour and playerTokenImage in Console implementation.    
        /// Pre:  None.
        /// Post:  Required number of players have been initialsed for start of a game.
        /// </summary>
        public static void SetUpPlayers() 
        {
            for(int iterator = 0; iterator < numberOfPlayers; iterator++)
            {
                players.Add(new Player(names[iterator]));
                players[iterator].Position = Board.START_SQUARE_NUMBER;
                players[iterator].Location = Board.StartSquare;
                players[iterator].RocketFuel = Player.INITIAL_FUEL_AMOUNT;
                players[iterator].HasPower = true;
                players[iterator].AtFinish = false;
                players[iterator].PlayerTokenColour = playerTokenColours[iterator];
            }
     
        }

        /// <summary>
        ///Plays a turn for each player in the 
        ///game completing a round.
        /// Pre: None.
        /// Post: Every player plays at turn.
        /// </summary>
        public static void PlayOneRound() 
        {
            for (int iterator = 0; iterator < numberOfPlayers; iterator++)
            {
                //If the player hasn't finished and they have power let them play
                if (!players[iterator].AtFinish && players[iterator].HasPower)
                {
                    players[iterator].Play(die1, die2);
                }         
            }
            
        }

        /// <summary>
        ///At the end of each round displays all players square locations
        /// and fuel remaining if any into the console.
        /// Pre: None
        /// Post: Displays plays information from the round just played.
        /// </summary>
        public static void DisplayPlayerRoundInfo()
        {
            Console.Write("\nPress Enter to play a round\n");
            Console.ReadLine();
            Console.WriteLine("\tRound {0}\n", round);
 
            for (int playernum = 0; playernum < NumberOfPlayers; playernum++)
            {
                if (players[playernum].HasPower)
                {
                    Console.WriteLine("\tPlayer {0} is on square {1} with {2} yottawatts of power remaining",
                    Players[playernum].Name, Players[playernum].Position,
                    Players[playernum].RocketFuel);
                }

                else if (!players[playernum].HasPower)
                {
                    Console.WriteLine(        
                        "\tPlayer {0} has run out of fuel on square {1}", 
                        players[playernum].Name, players[playernum].Position);
                }
            }
            round++;
        }//End DisplayPlayerRoundInfo

        /// <summary>
        ///Prompts the user to input how many players
        ///will be playing the game. Will reject anything
        ///that is not an integer and any integer that
        ///is not between 2 and 6 exclusively.   
        /// Pre: User is prompted in the console.
        /// Post: Total number of players ascertained and assigned.
        /// </summary>
        public static void TotalPlayers()
        {

            Console.WriteLine("This game is for 2-6 players.");
            Console.Write("How many players (2-6): ");

            string input = Console.ReadLine();
            int.TryParse(input, out int nums);

            Console.WriteLine();

            //Refuse anything but integers between 2 and 6.
            //If anything else is entered recursively call this function
            //To repeat the same steps.
            if (nums > 6 || nums < 2)
            {
                Console.WriteLine("Error: Invalid number of players entered.");
                TotalPlayers();
            }
            else
            {
                numberOfPlayers = nums;
            }
        }//End TotalPlayers

        /// <summary>
        ///Boolean that determines if all the players 
        ///in the game have run out of fuel.   
        /// Pre: None.
        /// Post: Returns true if all players out of fuel else returns false.
        /// </summary>
        public static bool CheckAllPlayerFuel()
        {
            for (int fuelcheck = 0; fuelcheck < NumberOfPlayers; fuelcheck++)
            {
                if(players[fuelcheck].HasPower)
                {
                    return false;
                }
            }
            return true;
        }//End CheckAllPlayerFuel

        /// <summary>
        ///Boolean that checks if any player has
        ///reached the final square and finished the
        ///game.
        /// Pre: None.
        /// Post: Returns true if a player has reached the final square else returns false.
        /// </summary>
        public static bool GameFinished()
        {
            for (int iterator = 0; iterator < NumberOfPlayers; iterator++)
            {
                if (Players[iterator].AtFinish)
                {
                    return true;
                }
            }
            return false;
        }//End GameFinished

    }//End SpaceRaceGame Class
}