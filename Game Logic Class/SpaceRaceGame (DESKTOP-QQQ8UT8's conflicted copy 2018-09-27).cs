using System;
using System.Drawing;
using System.ComponentModel;
using Object_Classes;


namespace Game_Logic_Class
{
    public static class SpaceRaceGame
    {
        // Minimum and maximum number of players.
        public const int MIN_PLAYERS = 2;
        public const int MAX_PLAYERS = 6;
   
        private static int numberOfPlayers = 2;  //default value for test purposes only 
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

        // The pair of die
        private static Die die1 = new Die(), die2 = new Die();
       

        /// <summary>
        /// Set up the conditions for this game as well as
        ///   creating the required number of players, adding each player 
        ///   to the Binding List and initialize the player's instance variables
        ///   except for playerTokenColour and playerTokenImage in Console implementation.
        ///   
        ///     
        /// Pre:  none
        /// Post:  required number of players have been initialsed for start of a game.
        /// </summary>
        public static void SetUpPlayers() 
        {
            // for number of players
            //      create a new player object
            //      initialize player's instance variables for start of a game
            //      add player to the binding list

            for(int iterator = 0; iterator < numberOfPlayers; iterator++)
            {
                players.Add(new Player(names[iterator]));
                players[iterator].Position = Board.START_SQUARE_NUMBER;
                players[iterator].Location = Board.StartSquare;
                players[iterator].RocketFuel = Player.INITIAL_FUEL_AMOUNT;
                players[iterator].HasPower = true;
                players[iterator].AtFinish = false;

            }

                
        }

            /// <summary>
            ///  Plays one round of a game
            /// </summary>
        public static void PlayOneRound() 
        {
            for (int iterator = 0; iterator < numberOfPlayers; iterator++)
            {
                if (!players[iterator].AtFinish && players[iterator].HasPower)
                {
                    players[iterator].Play(die1, die2);
                }         
            }
            
        }

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
                    Console.WriteLine("\tPlayer {0} has run out of fuel on square {1}", players[playernum].Name, players[playernum].Position);
                }
                
                
            }
            round++;
        }

        public static void TotalPlayers()
        {

            Console.WriteLine("This game is for 2-6 players.");
            Console.Write("How many players (2-6): ");

            string input = Console.ReadLine();
            int.TryParse(input, out int nums);

            Console.WriteLine();

            if (nums > 6)
            {
                Console.WriteLine("Error: Invalid number of players entered.");
                TotalPlayers();
            }
            else if (nums < 2)
            {
                Console.WriteLine("Error: Invalid number of players entered.");
                TotalPlayers();
            }
            else
            {
                numberOfPlayers = nums;
            }
        }

    }//end SnakesAndLadders
}