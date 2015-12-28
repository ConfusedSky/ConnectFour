//
// connectAI.cs
//
// 6-9-15
// Masa Maeda
//
// AI for connect four
//

using System;

public class OGAI : IPlayer
{
	private Random rand;
	private bool player;

	public OGAI( bool p )
	{
		rand = new Random();
		player = p;
	}

	public OGAI( bool p, Random r )
	{
		rand = r;
		player = p;
	}

	// Lets the AI make its move
	// takes the game its playing and a token its using
	public int MakeMove( GameBoard g )
	{
		// game to test moves on
		GameBoard gprime;
		// if any moves will result in a win use it
		// and block any wins that the player might have
		int i;
		if( CanWin( g, player, out i ) )
		{
			return i;
		}
		if( CanWin( g, !player, out i ) )
		{
			return i;
		}
		// else choose a random move that wont give the player a win
		int r;		
		const int MAXTRIES = 500;		
		// number of tries before giving up and trying something completely random		
		int tries = 0;		
		// make sure the move is a valid move		
		bool valid;		
		do 		
		{		
			r = rand.Next( 1, 8 );		
			gprime = new GameBoard( g.Board );		
			valid = gprime.MakeMove( r, player );		
			tries++;		
			// while the player can win in the next move or the next move is not valid		
		} while( ( CanWin( gprime, !player, out i ) && tries < MAXTRIES ) || !valid  );		
		return r;
	}	

	// private helper function which determines if a player can win in one move
	private bool CanWin( GameBoard g, bool player, out int move )
	{
		move = 1;
		GameBoard gprime = new GameBoard( g.Board );
		for( int i = 1; i < 8; i++ )
		{
			gprime.MakeMove( i, player );
			if( gprime.IsWinner( player ) )
			{
				move = i;
				return true;
			}
			gprime = new GameBoard( g.Board );
		}

		return false;
	}
}