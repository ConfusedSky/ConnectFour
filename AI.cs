using System;

// Ai for connect 4
public class AI : IPlayer
{
	// Random instance this AI is using
	private Random rand;
	// Whether this is an X(true) or an O(false)
	private bool player;

	// Node that represents a move in the tree
	private class GameNode
	{
		// gamestate this node represents
		public GameBoard Gamestate;
		// this node's score
		public double Score;
		// Constructor
		public GameNode( GameBoard g )
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
	// takes the game its playing
	// Chooses next move based on a tree of possible moves and min maxes its way up the tree
	public int MakeMove( GameBoard g )
	{
		System.Console.WriteLine( "Computer is thinking..." );
		// if any moves will result in a win use it
		// and block any wins that the player might have
		int i;

		i = RecurseBestMove( new GameNode(g), 9 );

		return i;
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
		// if the playing player can win
		if( CanWin( g.Gamestate, !(playing ^ player ), out i ) )
		{
			g.Score = ( (playing)?(1e10):(-1e10) ) * (layer/2 + 1);
		}
		// if the not playing player can win block the win
		else if( CanWin( g.Gamestate, (playing ^ player ), out i ) )
		{
			g.Score = ( (!playing)?(1e10):(-1e10) ) * (layer/2 + 1);
		}
		// else if we are at the end calculate the score of each terminal node that isnt a win
		else if( layer == 0 )
		{
			g.Score = CalculatePosition( g.Gamestate, player );
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
					g2 = new GameNode( new GameBoard( g.Gamestate.Board ) );
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
					g2 = new GameNode( new GameBoard( g.Gamestate.Board ) );
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

	// returns the value of the current gamestate
	public static double CalculatePosition( GameBoard g, bool player = true, bool debug = false )
	{
		// calculated value, positive if its our token negative if its theirs, current lines value, number of found tokens
		double d = 0, x = 0, y = 0, n = 0, z = 0;
		// gameboard
		char[,] board = g.Board;
		// whether this token is one of ours or one of theirs
		char token, otherToken;
		// represents all of the directions
		int[,] direction = 
		{
			{ -1, -1 },
			{ -1,  0 },
			{ -1,  1 },
			{  0,  1 },
			{  1,  1 },
			{  1,  0 },
			{  1, -1 },
			{  0, -1 }
		};

		for( int i = 0; i < 6; i++ )
		{
			for( int j = 0; j < 7; j++ )
			{
				// initialize x to zero
				x = 0;
				token = board[i,j];
				otherToken = ( token == 'X' )?'O':'X';
				if( token == ((player)?'X':'O') )
				{
					x = 1;
				}
				else if( token == ((!player)?'X':'O') )
				{
					x = -1;
				}

				z = 0;

				// if there is an adjacent token of the same type
				if( x != 0 )
				{
					// go in a direction until you hit something that isn't 
					for( int k = 0; k < 8; k++ )
					{
						// lines value is initialized to be either positive or negative depending on the token
						y = x;
						// we have found one
						n = 1;
						// look along the line
						for( int l = 1; l <= 3; l++ )
						{
							// if we reach a dead end 
							if( ( i+l*direction[k,0] < 0 || i+l*direction[k,0] >= 6 ||
								  j+l*direction[k,1] < 0 || j+l*direction[k,1] >= 7 ||
								  board[ i+l*direction[k,0], j+l*direction[k,1] ] == otherToken ) )		
							{
								// // and there is a dead end on the other side too this line is worth nothing
								// if( ( i-direction[k,0] < 0 || i-direction[k,0] >= 6 ||
								//       j-direction[k,1] < 0 || j-direction[k,1] >= 7 ||
								//       board[ i-direction[k,0], j-direction[k,1] ] == otherToken ) )
								{
									y = 0;
								}
								// regardless we break
								break;
							}
							// however if we run into another one of our own its worth more points
							if( board[ i+l*direction[k,0], j+l*direction[k,1] ] == token )
							{
								y *= Math.Pow( 7, n );
								n++;
							}
						}
						// add current line to total
						d += y;
						z += y;
					}
				}

				if( debug ) Console.Write( "{0,-5}", z );
			}
			if( debug ) Console.WriteLine();
		}

		// for tests if a token is next to a token of the same color 
		return d;
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