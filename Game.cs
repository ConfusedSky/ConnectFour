using System;
using System.Linq;

// This class contains logic for playing games
public static class Game
{
	public enum GameResult
	{
		// Player 1 wins
		Win,
		// Player 2 wins
		Loss,
		// It's a tie
		Tie
	}

	// Plays a game and returns true if player 1 wins
	public static GameResult PlayGame( Player p1, Player p2 )
	{
		GameBoard g = new GameBoard();

		Console.Clear();
		g.DrawBoard();

		while( true )
		{
			g.MakeMove( p1.MakeMove( new GameBoard( g.Board ) ), true );

			Console.Clear();
			g.DrawBoard();

			if( g.IsWinner( true ) )
			{
				Console.WriteLine( "Player 1 Wins!" );
				return GameResult.Win;
			}

			g.MakeMove( p2.MakeMove( new GameBoard( g.Board ) ), false );

			Console.Clear();
			g.DrawBoard();

			if( g.IsWinner( false ) )
			{
				Console.WriteLine( "Player 2 Wins!" );
				return GameResult.Loss;
			}

			if( !g.MovesAvailable() )
			{
				Console.WriteLine( "It's a draw!" );
				return GameResult.Tie;
			}
		}
	}

	// Gets a choice from 1 to count inclusive 
	private static int GetChoice( int count )
	{
		int choice = Console.ReadKey(true).KeyChar - '0';
		while( choice > count || choice < 1 )
		{
			Console.WriteLine( "Improper choice try again" );
			choice = Console.ReadKey(true).KeyChar - '0';
		}
		return choice;
	}

	// Alows the user to choose which of the player types each player is
	private static void ManualChoice( Type[] playerTypes, Random r, out Player p1, out Player p2 )
	{
		Console.WriteLine( "Choose Player 1's type: " );

		for( int i = 0; i < playerTypes.Count(); i++ )
		{
			Console.WriteLine( "{0}. {1}", i+1, playerTypes[i] );
		}

		Type p1Type = playerTypes[ GetChoice( playerTypes.Count() ) - 1 ];

		Console.WriteLine();

		Console.WriteLine( "Player 1 chose {0}", p1Type );

		Console.WriteLine();

		Console.WriteLine( "Choose Player 2's type: " );

		for( int i = 0; i < playerTypes.Count(); i++ )
		{
			Console.WriteLine( "{0}. {1}", i+1, playerTypes[i] );
		}

		Type p2Type = playerTypes[ GetChoice( playerTypes.Count() ) - 1 ];

		Console.WriteLine();

		Console.WriteLine( "Player 2 chose {0}", p2Type );

		Console.WriteLine();

		p1 = (Player)Activator.CreateInstance( p1Type, new object[] { true, r } );
		p2 = (Player)Activator.CreateInstance( p2Type, new object[] { false, r } );
	}

	public static void Main()
	{
		Random r = new Random();
		Player p1;
		Player p2;

		// Find all the types that are decendants of player in this assembly and therefore can be played with
		Type[] playerTypes = typeof( Player ).Assembly.GetTypes().Where( (x) => x.BaseType == typeof( Player ) ).ToArray();

		Console.WriteLine( "Choose gamemode:" );
		Console.WriteLine( "1. Single game, Manual Choice");

		int choice = GetChoice( 1 );
		switch( choice )
		{
			case 1:
				Console.WriteLine();

				ManualChoice( playerTypes, r, out p1, out p2 );

				Console.WriteLine( "Press the any key to continue. . ." );
				Console.ReadKey( true );

				PlayGame( p1, p2 );
				break;
			default:
				// Shouldn't ever happen
				Console.WriteLine( choice + " is an invalid choice." );
				break;
		}

		Console.WriteLine( "Press the any key to continue. . ." );
		Console.ReadKey( true );
	}
}