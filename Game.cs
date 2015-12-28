using System;

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

	public static void Main()
	{
		Random r = new Random();
		IPlayer p1 = new OGAI( true, r );
		IPlayer p2 = new AI( false, r );

		PlayGame( p1, p2 );

		Console.WriteLine( "Press the any key to continue. . ." );
		Console.ReadKey( true );
	}
}