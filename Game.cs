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

	// Gets a choice from 1 to count inclusive only allows a single character
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

	// Allows the players to be swaped
	private static void Swap( Random r, ref Player p1, ref Player p2 )
	{
		Player t = p1;
		p1 = (Player)Activator.CreateInstance( p2.GetType(), new object[] { true, r } );
		p2 = (Player)Activator.CreateInstance( t.GetType(), new object[] { false, r } );
	}

	private static int GetInt()
	{
		string number = "";
		ConsoleKeyInfo key;

		do
		{
			key = Console.ReadKey(true);

			// Backspace Should Not Work
			if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Spacebar && key.KeyChar >= '0' && key.KeyChar <= '9' )
			{
   				number += key.KeyChar;
   				Console.Write( key.KeyChar );
			}
			else if (key.Key == ConsoleKey.Backspace && number.Length > 0)
			{
      			number = number.Substring(0, (number.Length - 1));
    			Console.Write( "\b \b" );
			}
		} while( key.Key != ConsoleKey.Enter );

		if( number == "" )
			return 0;

		return Int32.Parse( number );
	}

	// Single Game, Manual Choice
	public static void SGMC( Type[] playerTypes, Random r )
	{
		Player p1;
		Player p2;

		ManualChoice( playerTypes, r, out p1, out p2 );

		Console.WriteLine( "Press the any key to continue. . ." );
		Console.ReadKey( true );

		PlayGame( p1, p2 );
	}

	// X Games, Manual Choice
	public static void XGMC( Type[] playerTypes, Random r )
	{
		Player p1;
		Player p2;

		int totalWins = 0, totalLosses = 0, totalTies = 0;

		Console.Write( "How many games do you want to play?: ");
		int nGames = GetInt();

		while( nGames < 1 )
		{
			Console.WriteLine();
			Console.Write( "You must enter a positive integer for the number of games to play. Try again: " ); 
			nGames = GetInt();
		}

		Console.WriteLine();
		Console.WriteLine();

		ManualChoice( playerTypes, r, out p1, out p2 );

		Console.WriteLine( "Press the any key to continue. . ." );
		Console.ReadKey( true );

		for( int i = 0; i < nGames; i++ )
		{
			switch ( PlayGame( p1, p2 ) )
			{
				case GameResult.Win:
					totalWins++;
					break;
				case GameResult.Loss:
					totalLosses++;
					break;
				default:
					totalTies++;
					break;

			}
		}

		Console.Clear();

		Console.WriteLine( "Results" );
		Console.WriteLine( "-------" );
		Console.WriteLine( "Total Wins(Percent): {0}({1}%)", totalWins, (double)totalWins/nGames*100 );
		Console.WriteLine( "Total Losses(Percent): {0}({1}%)", totalLosses, (double)totalLosses/nGames*100 );
		Console.WriteLine( "Total Ties(Percent): {0}({1}%)", totalTies, (double)totalTies/nGames*100 );
		Console.WriteLine();
	}

	// X Games, Manual Choice, Swap, X Games
	public static void XGMCS( Type[] playerTypes, Random r )
	{
		Player p1;
		Player p2;

		int totalWins = 0, totalLosses = 0, totalTies = 0;
		int totalWinsSwap = 0, totalLossesSwap = 0, totalTiesSwap = 0;

		Console.Write( "How many games do you want to play?: ");
		int nGames = GetInt();

		while( nGames < 1 )
		{
			Console.WriteLine();
			Console.Write( "You must enter a positive integer for the number of games to play. Try again: " ); 
			nGames = GetInt();
		}

		Console.WriteLine();
		Console.WriteLine();

		ManualChoice( playerTypes, r, out p1, out p2 );

		Console.WriteLine( "Press the any key to continue. . ." );
		Console.ReadKey( true );

		for( int i = 0; i < nGames; i++ )
		{
			switch ( PlayGame( p1, p2 ) )
			{
				case GameResult.Win:
					totalWins++;
					break;
				case GameResult.Loss:
					totalLosses++;
					break;
				default:
					totalTies++;
					break;

			}
		}

		Swap( r, ref p1, ref p2 );

		for( int i = 0; i < nGames; i++ )
		{
			switch ( PlayGame( p1, p2 ) )
			{
				case GameResult.Win:
					totalWinsSwap++;
					break;
				case GameResult.Loss:
					totalLossesSwap++;
					break;
				default:
					totalTiesSwap++;
					break;

			}
		}

		Console.Clear();

		Console.WriteLine( "Results({0} first)", p2.GetType() );
		Console.WriteLine( "-------" );
		Console.WriteLine( "Total Wins(Percent): {0}({1}%)", totalWins, (double)totalWins/nGames*100 );
		Console.WriteLine( "Total Losses(Percent): {0}({1}%)", totalLosses, (double)totalLosses/nGames*100 );
		Console.WriteLine( "Total Ties(Percent): {0}({1}%)", totalTies, (double)totalTies/nGames*100 );
		Console.WriteLine();
		Console.WriteLine( "Results({0} first)", p1.GetType() );
		Console.WriteLine( "-------" );
		Console.WriteLine( "Total Wins(Percent): {0}({1}%)", totalWinsSwap, (double)totalWinsSwap/nGames*100 );
		Console.WriteLine( "Total Losses(Percent): {0}({1}%)", totalLossesSwap, (double)totalLossesSwap/nGames*100 );
		Console.WriteLine( "Total Ties(Percent): {0}({1}%)", totalTiesSwap, (double)totalTiesSwap/nGames*100 );
		Console.WriteLine();
	}

	public static void ConnectToServer( Type[] playerTypes, Random r )
	{	
		Console.WriteLine( "Choose a player type: " );

		for( int i = 0; i < playerTypes.Count(); i++ )
		{
			Console.WriteLine( "{0}. {1}", i+1, playerTypes[i] );
		}

		Type p1Type = playerTypes[ GetChoice( playerTypes.Count() ) - 1 ];

		Console.WriteLine();

		Console.WriteLine( "You chose {0}", p1Type );

		Console.WriteLine();
	}

	public static void Main()
	{
		Random r = new Random();

		// Find all the types that are decendants of player in this assembly and therefore can be played with
		Type[] playerTypes = typeof( Player ).Assembly.GetTypes().Where( (x) => x.BaseType == typeof( Player ) ).ToArray();

		Console.WriteLine( "Choose gamemode:" );
		Console.WriteLine( "1. Single Game, Manual Choice" );
		Console.WriteLine( "2. X Games, Manual Choice" );
		Console.WriteLine( "3. X Games, Manual Choice, Swap, X Games" );
		Console.WriteLine( "4. Connect to another client through RemotePlayer" );
		Console.WriteLine();

		int choice = GetChoice( 4 );

		switch( choice )
		{
			case 1:
				SGMC( playerTypes, r );
				break;
			case 2:
				XGMC( playerTypes, r );
				break;
			case 3:
				XGMCS( playerTypes, r );
				break;
			case 4:
				ConnectToServer( playerTypes, r );
				break;
			default:
				Console.WriteLine( choice + " has not been implemented yet." );
				Console.WriteLine();
				break;
		}

		Console.WriteLine( "Press the any key to continue. . ." );
		Console.ReadKey( true );
	}
}