using System;

// This is a human player
public class HumanPlayer : Player
{
	public HumanPlayer( bool p, Random r ) : base( p, r ){}

	// Get input from keyboard
	public override int MakeMove( GameBoard g )
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