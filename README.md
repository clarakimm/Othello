# Othello
Class Project - BME 121 Digital Computation

Summary of Game Rules:
- Each player selects either the colour black (X) or white (O) to mark their moves throughout the game
- Goal: "Outflank" (place a disc on either end of an opponent's row of discs) and flip the opponent's discs to fill the game board with your colour (or X/O) 
- Each move must result in outflanking and flipping the opponent's discs - i.e. cannot place discs at random locations on the grid 

Interface:
1) Prompts users to enter name, who will make the first move, and game board size
3) Displays game board with the initial set-up and current scores (number of discs on the board)
4) Prompts users to enter a move:

      Option #1: "x,y"- Checks to ensure the placement of the disc will outflank the opponent's discs in any of the 8 directions, executes flip and repeats "Enter your move"    prompt<br/><br/>
      Option #2: "skip" - Passes turn to the opponent<br/><br/>
      Option #3: "quit"- Terminates Game & displays game results <br/><br/>
      
5) Assuming the the player enters in a coordinate in this case, the game goes on until the game board is full, or until there are no valid moves left for both players
6) When there are no valid moves left, the game automatically terminates and displays the game results


My code is contained within the "Othello.cs" file. Code used to display the gameboard was provided in class in the "Bme121.Program-Provided-Code_VERSION2.cs" file.
