//
// connectAI.cs
//
// 6-9-15
// Masa Maeda
//
// AI for connect four
//

using System;

public class OGAI
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
	public void MakeMove( Game g )
	{
		// game to test moves on
		Game gprime;
		// if any moves will result in a win use it
		// and block any wins that the player might have
		int i;
		if( CanWin( g, player, out i ) )
		{
			g.MakeMove( i, player );
			return;
		}
		if( CanWin( g, !player, out i ) )
		{
			g.MakeMove( i, player );
			return;
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
			gprime = new Game( g.Board );		
			valid = gprime.MakeMove( r, player );		
			tries++;		
			// while the player can win in the next move or the next move is not valid		
		} while( ( CanWin( gprime, !player, out i ) && tries < MAXTRIES ) || !valid  );		
		g.MakeMove( r, player );
	}	

	// private helper function which determines if a player can win in one move
	private bool CanWin( Game g, bool player, out int move )
	{
		move = 1;
		Game gprime = new Game( g.Board );
		for( int i = 1; i < 8; i++ )
		{
			gprime.MakeMove( i, player );
			if( gprime.IsWinner( player ) )
			{
				move = i;
				return true;
			}
			gprime = new Game( g.Board );
		}

		return false;
	}
}