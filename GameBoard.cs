using System;

// Board that the connect 4 game is played on
public class GameBoard
{
	// gameboard
	private char[,] _board;

	public char[,] Board { 
		get 
		{
			return _board;
		}
	}

	// public constuctor
	public GameBoard()
	{
		_board = new char[6,7];
		for( int i = 0; i < 6; i++ )
		{
			for( int j = 0; j < 7; j++ )
			{
				_board[i,j] = ' ';
			}
		}
	}

	// copy constuctor
	public GameBoard( char[,] b )
	{
		_board = new char[6,7];
		Array.Copy( b, _board, 6*7 );
	}

	public void DrawBoard()
	{
		Console.WriteLine( "1 2 3 4 5 6 7" );
		for( int i = 0; i < 6; i++ )
		{
			for( int j = 0; j < 7; j++ )
			{
				if( _board[i,j] == 'X' )
				{
					Console.ForegroundColor = ConsoleColor.Red;
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Blue;
				}
				Console.Write( _board[i,j] + " " );
			}
			Console.WriteLine("");
		}
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine( "_____________" );
	}

	// makes a move 
	// places a token in the location indicatied by player
	// returns false if you made an invalid choice
	public bool MakeMove( int choice, bool player )
	{
		char token = ((player)?'X':'O');
		// out of bounds
		if( choice < 1 || choice > 7 )
		{
			return false;
		}
		// column is full
		if( !ValidMove( choice ) )
		{
			return false;
		}
		// available spot
		int i = 0;
		// go down the column until you hit a 'wall'
		while( (i+1 < 6) && (_board[i+1,choice-1] == ' ' ) )
		{
			i++;
		}
		_board[i,choice-1] = token;
		return true;
	}

	// checks to see if character placing token won
	// returns true if he won
	public bool IsWinner( bool player )
	{
		char token = ((player)?'X':'O');
		int[,] limits = 
		{
			// i lower, i upper, j lower, j upper, k multiplier for i, k multiplier for j
			{ 0, 3, 0, 7,  1, 0 }, // vertical
			{ 0, 6, 0, 4,  0, 1 }, // horizontal
			{ 0, 3, 0, 4,  1, 1 }, // downright
			{ 3, 6, 0, 4, -1, 1 } // upright
		};

		int i, j, k, l;
		for( l = 0; l < 4; l++ )
		{
			for( i = limits[l,0]; i < limits[l,1]; i++ )
			{
				for( j = limits[l,2]; j < limits[l,3]; j++ )
				{
					for( k = 0; k < 4; k++ )
					{
						if( _board[i+k*limits[l,4],j+k*limits[l,5]] != token )
						{
							break;
						}
					}
					if( k == 4 )
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// checks to see if move is valid
	public bool ValidMove( int i )
	{
		return _board[0, i-1] == ' ';
	}

	// this will return true if there are any available movies to be played
	public bool MovesAvailable()
	{
		// look at the top of each column of the board to see if there is any empty space
		for( int i = 1; i < 8; i++ )
		{
			if( ValidMove( i ) )
			{
				return true;
			}
		}
		return false;
	}
}