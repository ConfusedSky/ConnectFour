using System;

// This is a human player
public class HumanPlayer : IPlayer
{
	// Get input from keyboard
	public int MakeMove( Game g )
	{
		int choice = -1;
		Console.Write( "Your Turn!" );
		while( !g.MakeMove( choice = Console.ReadKey(true).KeyChar - '0', true ) )
		{
			Console.Write( "\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b" );
			Console.Write( "Improper move try again" );
		}
		Console.WriteLine();
		return choice;
	}
}