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
			return _board;
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

	// Plays a game and returns true if player 1 wins
	public bool playGame()
	{
		IPlayer p1 = new HumanPlayer();
		IPlayer p2 = new HumanPlayer();

		Console.Clear();
		DrawBoard();

		while( true )
		{
			MakeMove( p1.MakeMove( new Game( Board ) ), true );

			Console.Clear();
			DrawBoard();

			if( IsWinner( true ) )
			{
				Console.WriteLine( "Player 1 Wins!" );
				return true;
			}

			MakeMove( p2.MakeMove( new Game( Board ) ), false );

			Console.Clear();
			DrawBoard();

			if( IsWinner( false ) )
			{
				Console.WriteLine( "Player 2 Wins!" );
				return false;
			}

			if( !MovesAvailable() )
			{
				Console.WriteLine( "It's a draw!" );
				return false;
			}
		}
	}

	public static void Main()
	{
		// Random r = new Random();
		Game g = new Game();
		// AI ai = new AI( false, r );
		// AI ai2 = new AI( true, r );
		// bool done = false;

		// Console.Clear();
		// g.DrawBoard();
		// while( !done )
		// {
		// 	// Console.WriteLine( AI.CalculatePosition( g, debug: true ) );
		// 	Console.WriteLine( "Your Turn!" );
		// 	while( !g.MakeMove( Console.ReadKey(true).KeyChar - '0', true ) )
		// 	{
		// 		Console.WriteLine( "Improper move try again\n" );
		// 	}
		// 	// ai2.MakeMove( g );
		// 	Console.Clear();
		// 	g.DrawBoard();
		// 	if( g.IsWinner( true ) )
		// 	{
		// 		done = true;
		// 		Console.WriteLine( "Player Wins!" );
		// 		continue;
		// 	}
		// 	// Console.WriteLine( AI.CalculatePosition( g, debug: true ) );
		// 	Console.WriteLine( "Computer is thinking..." );
		// 	// while( !g.MakeMove( Console.ReadKey(true).KeyChar - '0', false ) )
		// 	// {
		// 	// 	Console.WriteLine( "Improper move try again\n" );
		// 	// }
		// 	ai.MakeMove( g );
		// 	Console.Clear();
		// 	g.DrawBoard();
		// 	if( g.IsWinner( false ) )
		// 	{
		// 		done = true;
		// 		Console.WriteLine( "Computer Wins!" );
		// 		continue;
		// 	}
		// 	if( !g.MovesAvailable() )
		// 	{
		// 		done = true;
		// 		Console.WriteLine( "Its a Draw!" );
		// 		continue;
		// 	}
		// }

		g.playGame();

		Console.WriteLine( "Press the any key to continue. . ." );
		Console.ReadKey( true );
	}
}