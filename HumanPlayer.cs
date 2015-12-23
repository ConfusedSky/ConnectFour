using System;

// This is a human player
public class HumanPlayer : IPlayer
{
	// Get input from keyboard
	public int MakeMove( Game g )
	{
		int choice = -1;
		Console.WriteLine( "Your Turn!" );
		while( !g.MakeMove( choice = Console.ReadKey(true).KeyChar - '0', true ) )
		{
			Console.WriteLine( "Improper move try again\n" );
		}
		return choice;
	}
}