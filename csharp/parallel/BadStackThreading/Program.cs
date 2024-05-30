using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

using Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
listener.Bind(new IPEndPoint(IPAddress.Loopback, 0));
listener.Listen();

using Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
client.Connect(listener.LocalEndPoint!);

using Socket server = listener.Accept();
_ = server.SendAsync(Enumerable.Repeat((byte)65,2).ToArray());

//var mres = new ManualResetEventSlim();
byte[] buffer = new byte[1];

var stream = new NetworkStream(client);

ReadAgain();
Cleanup();

void Cleanup()
{
    stream.Close(1);
    client.Close();
    server.Close();
    listener.Close();
   // mres.Set(); 
};

void ReadAgain()
{
    stream.BeginRead(buffer, 0, 1, iar =>
    {
        if (stream.EndRead(iar) != 0)
        {
            Console.WriteLine(new String(System.Text.Encoding.UTF8.GetString(buffer)));
            ReadAgain(); // uh oh!
        }
    }, null);
};

//mres.Wait();