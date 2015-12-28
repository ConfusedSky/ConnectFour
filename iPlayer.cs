// All players ai or human need to implement this interface
public interface IPlayer
{
	// Also responsible for printing out some text
	// chooses the move to make next
	int MakeMove( GameBoard g );
}