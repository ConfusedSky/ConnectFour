using System;

public class RemotePlayer : Player
{
	public RemotePlayer( bool p, Random r ) : base( p, r ){}

	public override int MakeMove( GameBoard g )
	{
		return 1;
	}
}