//
// connectAI.cs
//
// 6-9-15
// Masa Maeda
//
// AI for connect four
//

using System;

public class AI
{
	private Random rand;
	private bool player;

	public AI( bool p )
	{
		rand = new Random();
		player = p;
	}

	public AI( bool p, Random r )
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