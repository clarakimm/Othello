#nullable enable
using System;
using static System.Console;

namespace Bme121
{   
    class Player
    {
        public readonly string Colour;
        public readonly string Symbol;
        public readonly string Name;
        
        public Player( string Colour, string Symbol, string Name )
        {
            this.Colour = Colour;
            this.Symbol = Symbol;
            this.Name = Name;
        }
    }
    
    static partial class Program
    {
        static void Welcome( )
        {
			WriteLine("Welcome to Othello!");
        }
        
        // Collect a player name or set to default.
        
        static Player NewPlayer(string colour, string symbol, string defaultName)
        {
            Write("Type in {0} ({1}) player name [or <Enter> for '{2}']: ", colour, symbol, defaultName);
            string nameResponse = ReadLine();
            if (nameResponse.Length != 0)
            {
				return new Player(colour, symbol, nameResponse); 
			}
			else
			{
				return new Player(colour, symbol, defaultName); 
			}
        }
        
        // Determine which player goes first or default.
        
        static int GetFirstTurn( Player[ ] players, int defaultFirst )
        {
            bool getFirstTurnCheck = false;
            
            while (!getFirstTurnCheck) // ERROR CHECK: while the response is invalid, keep looping to ask the user for an input until it is valid
            {
				Write("Choose who will play first [or <Enter> for 'White']: ");
				string turnResponse = ReadLine().ToLower(); //ToLower() is used to set user input to lower case no matter what --> ensures code is NOT case-sensitive 
				
				if (turnResponse.Length == 0 || turnResponse == "white" || turnResponse == players[1].Name) 
				{
					getFirstTurnCheck = true; //exit while loop since conditions are satisfied 
					return 1;
				}
				else if (turnResponse == "black" || turnResponse == players[0].Name)
				{
					getFirstTurnCheck = true;
					return 0;
				}
				else 
				{
					WriteLine("Invalid response - player does not exist. Please try again.");
					getFirstTurnCheck = false;
				}
			}
			return 1;
        }
        
        // Get a board size (between 4 and 26 and even) or default.
        
        static int GetBoardSize( string direction, int defaultSize )
        {
			bool getBoardSizeCheck = false;
			
			while (! getBoardSizeCheck)
			{
				Write ("Enter an integer for the number of {0} [or <Enter> for {1}] **IMPORTANT: the integer must be greater than/equal to 4, less than/equal to 26, and be even: ", direction, defaultSize);
				string boardSizeResponse = ReadLine();
				if (boardSizeResponse.Length != 0)
				{
					int boardSizeResponseConverted = int.Parse(boardSizeResponse);
					
					if (boardSizeResponseConverted > 4 && boardSizeResponseConverted < 26 && (boardSizeResponseConverted % 2) == 0  || boardSizeResponseConverted == 4 || boardSizeResponseConverted == 26)
					{
						getBoardSizeCheck = true;
						return boardSizeResponseConverted;
					}
					else 
					{
						WriteLine ("Invalid size, please try again.");
						getBoardSizeCheck = false;
					}
				}
				else 
				{
					return defaultSize;
				}
			}
			return defaultSize;
        }
        
        // Get a move from a player.
        
        static string GetMove( Player player )
        {
            bool getMoveCheck = false;
            
            while (! getMoveCheck)
            {
				WriteLine("Turn: {0} ({1} disc [{2}])", player.Name, player.Colour, player.Symbol);
				WriteLine("Choose a cell (row, column) to place your disc in.");
				WriteLine("Type 'skip' to surrender your turn. Type 'quit' to end the game.");
				Write ("Enter your move (e.g. 'a,d', 'skip', or 'quit'): ");
				string moveResponse = ReadLine().ToLower(); //ToLower() ensures that the code is NOT case-sensitive (e.g. 'skip', 'SKIP', and 'sKiP' would all be stored as 'skip') 
				
				if (moveResponse.Length >= 3)
				{
					getMoveCheck = true;
					return moveResponse;
				}
				else 
				{
					WriteLine ("Invalid move. Please try again.");
					getMoveCheck = false;
				}
			}
			return "quit";  
        }
        
        // Try to make a move. Return true if it worked.
        
        static bool TryMove( string[ , ] board, Player player, string move )
        {
				string r = move.Substring(0,1); //collect first element 
				string c = move.Substring(2);	//collect third element  
				
				if (move == "skip")
				{
					WriteLine(player.Name + "'s turn was skipped.");
					return true;
				}
				else if (move.Length == 3) 
				{
					int rIndex = IndexAtLetter(r);
					int cIndex = IndexAtLetter(c);
					
					if (rIndex >= 0 && rIndex < board.GetLength(0) && cIndex >= 0 && cIndex < board.GetLength(1))
					{
						if (board[rIndex, cIndex] == " ")
						{
							bool right = TryDirection (board, player, rIndex, 0, cIndex, 1);
							bool left = TryDirection (board, player, rIndex, 0, cIndex, -1);
							bool top = TryDirection (board, player, rIndex, -1, cIndex, 0);
							bool bottom = TryDirection (board, player, rIndex, 1, cIndex, 0);
							bool diagonalRT = TryDirection (board, player, rIndex, 1, cIndex, -1); 
							bool diagonalLT = TryDirection (board, player, rIndex, -1, cIndex, -1); 
							bool diagonalRD = TryDirection (board, player, rIndex, 1, cIndex, 1);
							bool diagonalLD = TryDirection (board, player, rIndex, -1, cIndex, 1);
							
							if (right || left || top || bottom || diagonalRT || diagonalLT || diagonalRD || diagonalLD)
							{
								return true;
							}
							return false;
						} 
						return false;
					}
					return false;
				}
				return false;
		}
        
        // Check if move can be made along the line for one direction. 
        
        static bool TryDirection( string[ , ] board, Player player,int moveRow, int deltaRow, int moveCol, int deltaCol ) 
        {
			if (moveRow + deltaRow >=0 && moveRow + deltaRow < board.GetLength(0) && moveCol + deltaCol >=0 && moveCol + deltaCol < board.GetLength(1)) //check if neighboring cell exists (within range of board's size) 
			{
				if (board [moveRow + deltaRow, moveCol + deltaCol] != player.Symbol && board [moveRow + deltaRow, moveCol + deltaCol]!= " ") //check that the neighboring cell contains opponent's marker  
				{
					for (int multiple = 1; multiple >=1 ; multiple ++) 
					{
						if ((moveRow + deltaRow*multiple) >= board.GetLength(0) || (moveCol + deltaCol * multiple) >= board.GetLength(1)) //rule out situations where move is invalid (index of cells along the line does not fit into the board's size) 
						{
							return false;
						} 
						if ((moveRow + deltaRow*multiple) < 0 || (moveCol + deltaCol * multiple) < 0)
						{
							return false;
						}
						
						string cell = board[(moveRow + deltaRow * multiple), (moveCol + deltaCol * multiple)];
						if (cell == " ") 
						{
							return false;
						}
						
						if (cell == player.Symbol) //once or if the player's marker is reached at some point along the line, the move is valid 
						{
							//flip 	
							for (int newMultiple = 1; newMultiple <= multiple; newMultiple ++) 
							{
								board[(moveRow + deltaRow * newMultiple), (moveCol + deltaCol * newMultiple)] = player.Symbol;
							}
							board[moveRow, moveCol] = player.Symbol;
							
							return true;	
						} 
					}
					return false;
				}
				return false;
			}
			return false; 
		
		}
        
        // Count the discs to find the score for a player.
        
        static int GetScore( string[ , ] board, Player player )//for one player 
        {
			int count = 0;
            for (int i = 0; i < board.GetLength(0); i++)
            {
				for (int j = 0; j < board.GetLength(1); j++)
				{
					if (board[i,j] == player.Symbol)
					{
						count ++; 
					}
				}
			}
            return count;
        }
        
        // Display scores for all players.
        
        static void DisplayScores( string[ , ] board, Player[ ] players )
        {
			int scoreBlack = GetScore(board, players[0]);
			int scoreWhite = GetScore(board, players[1]);
			
			Write ("Scores | ");
			WriteLine (players[0].Name + ": " + scoreBlack + "  " + players[1].Name + ": " + scoreWhite);		
        }
        
        // Display winner(s) and categorize their win over the defeated player(s).
        
        static void DisplayWinners( string[ , ] board, Player[ ] players )
        {
			int scoreBlack = GetScore(board, players[0]);
			int scoreWhite = GetScore(board, players[1]);
			int scoreDiff = Math.Abs(scoreBlack - scoreWhite); //taking the absolute value of the difference ensures that even if scoreBlack is less than scoreWhite, the difference will be stored as a positive integer 
			string winner;
			string category = " ";
			
			if (scoreBlack > scoreWhite || scoreWhite > scoreBlack)
			{
				if (scoreBlack > scoreWhite)
				{
					winner = players[0].Name;
					WriteLine("Winner: " + winner);
					if (scoreDiff == 54 || scoreDiff > 54 && scoreDiff < 64 || scoreDiff == 65)
					{
						category = "perfect";
					}
					else if (scoreDiff == 40 || scoreDiff > 40 && scoreDiff < 52 || scoreDiff == 52)
					{
						category = "walkaway";
					}
					else if (scoreDiff == 26 || scoreDiff > 26 && scoreDiff < 38 || scoreDiff == 38)
					{
						category = "fight";
					}
					else if (scoreDiff == 12 || scoreDiff > 12 && scoreDiff < 24 || scoreDiff == 24)
					{
						category = "hot";
					}
					else if (scoreDiff == 2 || scoreDiff > 2 && scoreDiff < 10 || scoreDiff == 10)
					{
						category = "close";
					}
					WriteLine ("Defeated " + players[1].Name + " by a " + category + " game.");	
				}
				
				else if (scoreWhite > scoreBlack)
				{
					winner = players[1].Name;
					WriteLine("Winner: " + winner);
					if (scoreDiff == 54 || scoreDiff > 54 && scoreDiff < 64 || scoreDiff == 65)
					{
						category = "perfect";
					}
					else if (scoreDiff == 40 || scoreDiff > 40 && scoreDiff < 52 || scoreDiff == 52)
					{
						category = "walkaway";
					}
					else if (scoreDiff == 26 || scoreDiff > 26 && scoreDiff < 38 || scoreDiff == 38)
					{
						category = "fight";
					}
					else if (scoreDiff == 12 || scoreDiff > 12 && scoreDiff < 24 || scoreDiff == 24)
					{
						category = "hot";
					}
					else if (scoreDiff == 2 || scoreDiff > 2 && scoreDiff < 10 || scoreDiff == 10)
					{
						category = "close";
					}
					WriteLine ("Defeated " + players[0].Name + " by a " + category + " game.");	
				}
			}
			else 
			{
				WriteLine ("The game ended in a tie.");
			}
        }
        
        //Automatically check if there are available moves left for player.
        
        static bool AvailableMoves (string[ , ] board, Player player, int deltaRowSweep, int deltaColSweep, int rowSweep, int colSweep)
        {
			if (board[rowSweep, colSweep] == " ")
			{	
				if (rowSweep + deltaRowSweep >=0 && rowSweep + deltaRowSweep < board.GetLength(0) && colSweep + deltaColSweep >=0 && colSweep+ deltaColSweep < board.GetLength(1))
				{
					if (board [rowSweep + deltaRowSweep, colSweep + deltaColSweep] != player.Symbol && board [rowSweep + deltaRowSweep, colSweep + deltaColSweep]!= " ")
					{
						for (int p = 1; p >=1 ; p ++)
						{
							if ((rowSweep + deltaRowSweep * p) >= board.GetLength(0) || (colSweep + deltaColSweep * p) >= board.GetLength(1))
							{
								return false;
							} 
							if ((rowSweep + deltaRowSweep * p) < 0 || (colSweep + deltaColSweep * p) < 0)
							{
								return false;
							}
							
							string nextCell = board[(rowSweep + deltaRowSweep * p), (colSweep + deltaColSweep * p)];
							if (nextCell == " ") 
							{
								return false;
							}
							if (nextCell == player.Symbol)
							{
								return true;
							}
						}
					} 
				}
			}
			return false;
		}
		
		//Apply AvailableMoves method to each cell on the board.
		
		static bool ApplyAvailableMoves (string[ , ] board, Player player)
        {
			for (int rowSweep = 0; rowSweep < board.GetLength(0); rowSweep ++)
			{
				for (int colSweep = 0; colSweep < board.GetLength(1); colSweep ++)
					{
						bool rightAvailableMoves = AvailableMoves(board, player, 0, 1, rowSweep, colSweep);
						bool leftAvailableMoves = AvailableMoves(board, player, 0, -1, rowSweep, colSweep);
						bool topAvailableMoves = AvailableMoves(board, player, -1, 0, rowSweep, colSweep);
						bool bottomAvailableMoves = AvailableMoves(board, player, 1, 0, rowSweep, colSweep);
						bool diagonalRTAvailableMoves = AvailableMoves(board, player, 1, -1, rowSweep, colSweep);
						bool diagonalLTAvailableMoves = AvailableMoves(board, player, -1, -1, rowSweep, colSweep);
						bool diagonalRDAvailableMoves = AvailableMoves(board, player, 1, 1, rowSweep, colSweep);
						bool diagonalLDAvailableMoves = AvailableMoves(board, player, -1, 1, rowSweep, colSweep);
						
						if (rightAvailableMoves || leftAvailableMoves || topAvailableMoves || bottomAvailableMoves || diagonalRTAvailableMoves || diagonalLTAvailableMoves || diagonalRDAvailableMoves || diagonalLDAvailableMoves)
						{
							return true;
						}
					}
			}	
			return false;
		}
		
		static void Main( )
        {	
            Welcome( );
            
            //play class, variable name =player, allocate the array with {a set of intital values}
            Player[ ] players = new Player[ ] 
            {
               //create Player object  
                NewPlayer( colour: "black", symbol: "X", defaultName: "Black" ),
                NewPlayer( colour: "white", symbol: "O", defaultName: "White" ),
            };
            
            int turn = GetFirstTurn( players, defaultFirst:0);
            int rows = GetBoardSize( direction: "rows",    defaultSize: 8 );
            int cols = GetBoardSize( direction: "columns", defaultSize: 8 );
            string[ , ] game = NewBoard( rows, cols );
            
            // Play the game.
            
            bool gameOver = false;
            
            Welcome( );
            
            while( ! gameOver ) 
            {
                DisplayBoard( game ); 
                DisplayScores( game, players );
                bool moveAvailabilityCheck = ApplyAvailableMoves(game, players[turn]); //call ApplyAvailableMoves method  
				if (! moveAvailabilityCheck) //if there are no valid moves left for the player...
				{
					WriteLine ("No legal moves left for " + players[turn].Name + ". Turn was passed to opponent.");
					turn = (turn + 1) % players.Length; //skip to opponent's turn 
					
					moveAvailabilityCheck = ApplyAvailableMoves(game, players[turn]); //check to see if opponent has valid moves left
					
					if (! moveAvailabilityCheck) //given that the first player has no valid moves remaining, if the second player also has no valid remaining...
					{
						WriteLine("No legal moves left for " + players[turn].Name +".");
						break; //the while loop designed to keep the game running 'breaks' and the game ends (no legal moves left) 
					}
				}
				
                string move = GetMove( players[ turn ] );
                
                if( move == "quit") gameOver = true;
                else
                {
                    bool madeMove = TryMove( game, players[ turn ], move );
                    if( madeMove ) turn = ( turn + 1 ) % players.Length;
                    else 
                    {
                        Write( "Your choice didn't work!" );
                        Write( " Press <Enter> to try again." );
                        ReadLine( ); 
                    }
                }
            }
            
            // Show fhe final results.
            
            DisplayWinners( game, players );
            
        }
    }
}
