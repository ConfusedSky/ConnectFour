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
	// Random instance this AI is using
	private Random rand;
	// Whether this is an X(true) or an O(false)
	private bool player;

	// Node that represents a move in the tree
	private class GameNode
	{
		// gamestate this node represents
		public Game Gamestate;
		// this node's score
		public double Score;
		// Constructor
		public GameNode( Game g )
		{
			Gamestate = g;
			Score = 0;
		}
	}

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
	// Chooses next move based on a tree of possible moves and min maxes its way up the tree
	public void MakeMove( Game g )
	{
		// if any moves will result in a win use it
		// and block any wins that the player might have
		int i;
		
		if( CanWin( g, player, out i ) );
		else if( CanWin( g, !player, out i ) );
		else 
		{
			i = RecurseBestMove( new GameNode(g), 10 );
		}

		g.MakeMove( i, player );
	}

	// // recursive method to choose the best move based on a minmax tree
	// // Gamenode is the current state of the game
	// // layer is how many nodes down we are
	// // playing is if we are playing or the other player is playing
	private int RecurseBestMove( GameNode g, int layer, double alpha = -double.MaxValue, double beta = double.MaxValue, bool playing = true )
	{
		// move with the best score
		int i;
		// new gamestate
		GameNode g2;
		// best score
		double bestScore;
		// if there are no moves left this node is a zero score
		if( !g.Gamestate.MovesAvailable() )
		{
			i = 0;
		}
		// if the playing player can go
		else if( CanWin( g.Gamestate, !(playing ^ player ), out i ) )
		{
			g.Score = ( (playing)?(1):(-1) ) * Math.Pow( 1.2, layer );
		}
		// else if we are at the end calculate the score of each terminal node that isnt a win
		else if( layer == 0 )
		{
			i = 0;
		}
		// else generate new nodes and run the minmax algorithm
		else if( playing )
		{
			bestScore = -double.MaxValue;
			for( int j = 1; j < 8; j++ )
			{
				// if this is a valid move
				if( g.Gamestate.ValidMove( j ) )
				{
					// make a new node
					g2 = new GameNode( new Game( g.Gamestate.Board ) );
					// make the move
					g2.Gamestate.MakeMove( j, player );
					// run this method again for the new move
					RecurseBestMove( g2, layer - 1, alpha, beta, false );
					// if the current score is the best one set the return value to j
					// and the best value to g2.Score
					// if they are the same there is a 50 50 chance it will choose the new one
					if( ( g2.Score > bestScore ) || (g2.Score == bestScore && rand.Next( 0, 2 ) == 0)  )
					{
						i = j;
						bestScore = g2.Score;
						// if the alpha value is smaller than the best score the best score is the new alpha
						if( bestScore > alpha )
						{
							alpha = bestScore;
						}
					}
					// if there is no way to get a better value from this branch stop looking
					if( beta <= alpha )
					{
						break;
					}
				}
			}
			g.Score = bestScore;
		}
		else
		{
			bestScore = double.MaxValue;
			for( int j = 1; j < 8; j++ )
			{
				// if this is a valid move
				if( g.Gamestate.ValidMove( j ) )
				{
					// make a new node
					g2 = new GameNode( new Game( g.Gamestate.Board ) );
					// make the move
					g2.Gamestate.MakeMove( j, !player );
					// run this method again for the new move
					RecurseBestMove( g2, layer - 1, alpha, beta, true );
					// if the current score is the best one set the return value to j
					// and the best value to g2.Score
					// if they are the same there is a 50 50 chance it will choose the new one
					if( ( g2.Score < bestScore ) || (g2.Score == bestScore && rand.Next( 0, 2 ) == 0)  )
					{
						i = j;
						bestScore = g2.Score;
						// if the beta value is bigger than the best score the best score is the new beta
						if( bestScore < beta )
						{
							beta = bestScore;
						}
					}
					// if there is no way to get a better value from this branch stop looking
					if( beta <= alpha )
					{
						break;
					}
				}
			}
			g.Score = bestScore;
		}

		return i;
	}

	// const int MAXLAYER = 7;

	// // recursive method to choose the best move based on a minmax tree
	// // Gamenode is the current state of the game
	// // layer is how many nodes down we are
	// // playing is if we are playing or the other player is playing
	// private int RecurseBestMove( GameNode g, int layer = 0, bool playing = true )
	// {
	// 	// move with the best score
	// 	int i;
	// 	// when there are no more moves left
	// 	if( !g.Gamestate.MovesAvailable() )
	// 	{
	// 		i = 0;
	// 	}
	// 	// if playing player can win
	// 	else if( CanWin( g.Gamestate, !(playing ^ player), out i ) )
	// 	{
	// 		g.Score = ((playing)?(1):(-1)) * Math.Pow( 1.2, (MAXLAYER-layer+1) );
	// 	}
	// 	// cuttoff before new nodes are made but after can win is calculated
	// 	else if( layer == MAXLAYER )
	// 	{
	// 		i = 0;
	// 	}
	// 	// generate new nodes and calculate choice from there
	// 	else
	// 	{
	// 		// new gamestate
	// 		GameNode g2;
	// 		// best score
	// 		double bestScore = -double.MaxValue;
	// 		for( int j = 1; j < 8; j++ )
	// 		{
	// 			// if this is a valid move
	// 			if( g.Gamestate.ValidMove( j ) )
	// 			{
	// 				// make a new node
	// 				g2 = new GameNode( new Game( g.Gamestate.Board ) );
	// 				// make the move
	// 				g2.Gamestate.MakeMove( j, !(playing ^ player) );
	// 				// run this method again for the new move
	// 				RecurseBestMove( g2, layer + 1, !playing );
	// 				// add the new score to this one
	// 				g.Score += g2.Score;
	// 				// if the current score is the best one set the return value to j
	// 				// and the best value to g2.Score
	// 				// if they are the same there is a 50 50 chance it will choose the new one
	// 				if( ( g2.Score > bestScore ) || (g2.Score == bestScore && rand.Next( 0, 2 ) == 0)  )
	// 				{
	// 					i = j;
	// 					bestScore = g2.Score;
	// 				}
	// 				// // debug code
	// 				// if( layer < 2 )
	// 				// {
	// 				// 	Console.WriteLine( "{0} {1} {2}", i, bestScore, g2.Score );
	// 				// }
	// 			}
	// 		}
	// 	}
	// 	return i;
	// }

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