using System;
using System.Net;
using System.Net.Sockets;

using System.Text;

public class RemotePlayer : Player
{
	private Socket connection;

	public RemotePlayer( bool p, Random r ) : base( p, r )
	{
		IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        // Create a TCP/IP socket.
        Socket listener = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp );

        // Bind the socket to the local endpoint and 
        // listen for incoming connections.
        try 
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);

            Console.Write( "Waiting for connection... " );

            connection = listener.Accept();

            Console.WriteLine( connection.RemoteEndPoint.ToString() );

            SendString( connection, "Connected " + ( (player)?('X'):('O') ) );
        } 
        catch (Exception e) 
        {
            Console.WriteLine(e.ToString());
        }
	}

	~RemotePlayer()
	{
		try
		{
			SendString( connection, "Disconect" );

			connection.Shutdown( SocketShutdown.Both );
			connection.Close();
		}
		catch( Exception e )
		{
			Console.WriteLine( e.ToString() );
		}
	}

	public override int MakeMove( GameBoard g )
	{
		int result;
		string message;

		SendString( connection, CerealizeBoard( g ) );
		Console.WriteLine( "Waiting for response..." );

		while( true )
		{
			message = ReceiveString( connection );
			result = DecerealizeMove( message );

			if( result == -1 )
			{
				Console.WriteLine( "Got an invalid move from the client..." );
				Console.WriteLine( "Press the any key to continue. . .\n" );
				Console.ReadKey( true );
			}
			else
			{
				break;
			}
		}
		return result;
	}

	public static string CerealizeBoard( GameBoard g )
	{
		char[,] board = g.Board;

		string result = "Move ";

		foreach( char i in board )
		{
			result += ( i == ' ')?( '_' ):( i );
			result += ',';
		}

		return result.Substring( 0, result.Length - 1 );
	}

	public static int DecerealizeMove( string s )
	{
		int result;

		s = s.Split( new char[] { ' ' } )[1];

		Int32.TryParse( s, out result );

		if( result < 1 || result > 7 )
		{
			result = -1;
		}

		return result;
	}

	public static char[,] DecerealizeBoard( string s )
	{
		char[,] board = new char[6,7];

		s = s.Split( new char[] { ' ' } )[1];

		String[] ss = s.Split( new char[] { ',' } );

		int k = 0;
		for( int i = 0; i < 6; i++ )
		{
			for( int j = 0; j < 7; j++ )
			{
				board[i,j] = (ss[k][0] == '_')?( ' ' ):( ss[k][0] );
				k++;
			}
		}

		return board;
	}

	public static void SendString( Socket s, string str )
	{
		s.Send( Encoding.ASCII.GetBytes( str + "<EOF>" ) );
	}

	public static string ReceiveString( Socket s )
	{
		byte[] data;
		string result = "";

		while (true) 
		{
		    data = new byte[1024];
		    int bytesRec = s.Receive(data);
		    result += Encoding.ASCII.GetString(data,0,bytesRec);
		    if (result.IndexOf("<EOF>") > -1) {
		        break;
		    }
		}

		return result.Substring( 0, result.Length - 5 );
	}
}