using System;
using System.Linq;

// This class contains logic for playing games
public class Game
{
	// Plays a game and returns true if player 1 wins
	public static bool PlayGame( IPlayer p1, IPlayer p2 )
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
				return true;
			}

			g.MakeMove( p2.MakeMove( new GameBoard( g.Board ) ), false );

			Console.Clear();
			g.DrawBoard();

			if( g.IsWinner( false ) )
			{
				Console.WriteLine( "Player 2 Wins!" );
				return false;
			}

			if( !g.MovesAvailable() )
			{
				Console.WriteLine( "It's a draw!" );
				return false;
			}
		}
	}

	private static int GetChoice( int count )
	{
		int choice = Console.ReadKey(true).KeyChar - '0';
		while( choice > count || choice < 1 )
		{
			Console.WriteLine( "Improper choice try again" );
			choice = Console.ReadKey(true).KeyChar - '0';
		}
		return choice - 1 ;
	}

	public static void Main()
	{
		Random r = new Random();
		IPlayer p1;
		IPlayer p2;

		// PlayGame( p1, p2 );

		var playerTypes = typeof( IPlayer ).Assembly.GetTypes().Where( (x) => x.GetInterfaces().Count( (y) => y == typeof( IPlayer ) ) == 1 ).ToArray();

		Console.WriteLine( "Choose Player 1's type: " );

		for( int i = 0; i < playerTypes.Count(); i++ )
		{
			Console.WriteLine( "{0}. {1}", i+1, playerTypes[i] );
		}

		GetChoice( playerTypes.Count() );

		Console.WriteLine();

		Console.WriteLine( "Choose Player 2's type: " );

		for( int i = 0; i < playerTypes.Count(); i++ )
		{
			Console.WriteLine( "{0}. {1}", i+1, playerTypes[i] );
		}

		GetChoice( playerTypes.Count() );

		Console.WriteLine();

		Console.WriteLine( "Press the any key to continue. . ." );
		Console.ReadKey( true );
	}
}