using System;

// All players ai or human need to implement this interface
public abstract class Player
{
	// Random instance this player is using
	private Random _rand;
	// Whether this is an X(true) or an O(false)
	private bool _player;

	protected Random rand
	{
		get
		{
			return _rand;
		}
		private set
		{
			_rand = value;
		}
	}

	protected bool player
	{
		get
		{
			return _player;
		}
		private set
		{
			_player = value;
		}
	}

	public Player( bool p )
	{
		rand = new Random();
		player = p;
	}

	public Player( bool p, Random r )
	{
		rand = r;
		player = p;
	}

	// chooses the move to make next
	// Also responsible for printing out some text
	public abstract int MakeMove( GameBoard g );
}