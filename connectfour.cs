//
// connectfour.cs
//
// 6-9-15
// Masa Maeda
//
// Connect four program
//

using System;

public class Game
{
	// gameboard
	private char[,] _board;

	public char[,] Board { 
		get 
		{
			char[,] b = new char[6,7];
			Array.Copy( _board, b, 6*7 );
			return b;
		}
	}

	// public constuctor
	public Game()
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
	public Game( char[,] b )
	{
		_board = b;
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
			// More stuff added here
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
		if( _board[0,choice-1] != ' ' )
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

		int i, j, k;
		// vertical wins
		for( i = 0; i < 6-3; i++ )
		{
			for( j = 0; j < 7; j++ )
			{
				for( k = 0; k < 4; k++ )
				{
					if( _board[i+k,j] != token )
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
		// horizontal wins
		for( i = 0; i < 6; i++ )
		{
			for( j = 0; j < 7-3; j++ )
			{
				for( k = 0; k < 4; k++ )
				{
					if( _board[i,j+k] != token )
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
		// downright wins
		for( i = 0; i < 6-3; i++ )
		{
			for( j = 0; j < 7-3; j++ )
			{
				for( k = 0; k < 4; k++ )
				{
					if( _board[i+k,j+k] != token )
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
		// downright wins
		for( i = 3; i < 6; i++ )
		{
			for( j = 0; j < 7-3; j++ )
			{
				for( k = 0; k < 4; k++ )
				{
					if( _board[i-k,j+k] != token )
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

		return false;
	}

	// this will return true if there are any available movies to be played
	public bool MovesAvailable()
	{
		// look at the top of each column of the board to see if there is any empty space
		for( int i = 0; i < 7; i++ )
		{
			if( _board[0, i] == ' ' )
			{
				return true;
			}
		}
		return false;
	}

	public static void Main()
	{
		Random r = new Random();
		Game g = new Game();
		AI ai = new AI( false, r );
		AI ai2 = new AI( true, r );
		bool done = false;
		while( !done )
		{
			Console.Clear();
			g.DrawBoard();
			// while( !g.MakeMove( Console.ReadKey(true).KeyChar - '0', true ) )
			// {
			// 	Console.WriteLine( "Improper move try again\n" );
			// }
			ai2.MakeMove( g );
			if( g.IsWinner( true ) )
			{
				done = true;
				Console.WriteLine( "Player Wins!" );
				continue;
			}
			ai.MakeMove( g );
			if( g.IsWinner( false ) )
			{
				done = true;
				Console.WriteLine( "Computer Wins!" );
				continue;
			}
			if( !g.MovesAvailable() )
			{
				done = true;
				Console.WriteLine( "Its a Draw!" );
				continue;
			}
		}
		g.DrawBoard();

		Console.WriteLine( "Press the any key to continue. . ." );
		Console.ReadKey( true );
	}
}