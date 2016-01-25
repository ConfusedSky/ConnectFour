using System;

// This is a human player
public class HumanPlayer : IPlayer
{
	private bool player;

	public HumanPlayer( bool player )
	{
		this.player = player;
	}

	// Get input from keyboard
	public int MakeMove( GameBoard g )
	{
		int choice = -1;
		Console.Write( "Your Turn!" );
		while( !g.MakeMove( choice = Console.ReadKey(true).KeyChar - '0', player ) )
		{
			Console.Write( "\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b" );
			Console.Write( "Improper move try again" );
		}
		Console.WriteLine();
		return choice;
	}
}