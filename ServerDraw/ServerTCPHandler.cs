using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerDraw
{
    class ServerTCPHandler
    {
        public static readonly object key = new object();
        private static List<string> lines = new List<string>();

        /// <summary>
        /// handles the list of lines used
        /// </summary>
        public static List<string> Lines
        {
            get
            {
                lock (key)
                {
                    return lines;
                }
            }
            set
            {
                lock (key)
                {
                    lines = value;
                }
            }
        }

        public int Port = 25565;//The port to connect to
        private TcpListener _server;
        private Boolean _isRunning;

        /// <summary>
        /// pre startup setup
        /// </summary>
        public void TcpListener()
        {
            Console.WriteLine("Server for storing Draw Data");
            TcpServer(Port);
        }

        /// <summary>
        /// starts the server
        /// </summary>
        /// <param name="port"></param>
        public void TcpServer(int port)
        {
            _server = new TcpListener(IPAddress.Any, port);
            _server.Start();

            _isRunning = true;

            LoopClients();
        }

        /// <summary>
        /// looks for clients
        /// </summary>
        public void LoopClients()
        {
            while (_isRunning)
            {
                // wait for client connection
                TcpClient newClient = _server.AcceptTcpClient();
                // client found.
                // create a thread to handle communication
                Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                t.Start(newClient);
                t.IsBackground = true;
            }
        }

        /// <summary>
        /// handles each client
        /// </summary>
        /// <param name="obj"></param>
        public void HandleClient(object obj)
        {
            // retrieve client from parameter passed to thread
            TcpClient client = (TcpClient)obj;
            // sets two streams
            StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
            StreamReader sReader = new StreamReader(client.GetStream(), Encoding.ASCII);
            // you could use the NetworkStream to read and write, 
            // but there is no forcing flush, even when requested
            Boolean bClientConnected = true;
            String sData = null;

            IPEndPoint endPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            IPEndPoint localPoint = (IPEndPoint)client.Client.LocalEndPoint;

            Console.WriteLine("Connected");
            while (bClientConnected)
            {
                // reads from stream
                try
                {
                    Console.WriteLine("Client > " + sData);
                    Console.WriteLine("Remote host port: " + endPoint.Port.ToString() + " Local socket port: " + localPoint.Port.ToString());

                    sData = sReader.ReadLine();
                }
                catch (Exception)
                {
                    Console.WriteLine(endPoint.Port.ToString() + " " + localPoint.Port.ToString() + " lukkede forbindelsen");
                    bClientConnected = false;
                }

                if (sData == "Draw")
                {
                    Console.WriteLine("Connected Draw");
                    while (bClientConnected)
                    {
                        try
                        {
                            sData = sReader.ReadLine();
                            DrawingHandler(sData);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine(endPoint.Port.ToString() + " " + localPoint.Port.ToString() + " lukkede forbindelsen");
                            bClientConnected = false;
                        }
                    }
                }
                else if (sData == "View")
                {
                    Console.WriteLine("Connected View");
                    try
                    {
                        SuccesHandeler(sWriter);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(endPoint.Port.ToString() + " " + localPoint.Port.ToString() + " lukkede forbindelsen");
                        bClientConnected = false;
                    }
                }
            }
        }

        /// <summary>
        /// handles what is replied
        /// </summary>
        /// <param name="sWriter"></param>
        private void SuccesHandeler(StreamWriter sWriter)
        {
            for (int i = 0; i < Lines.Count; i++)
            {
                sWriter.WriteLine(lines[i]);
            }
        }

        /// <summary>
        /// handles what happens with incomming data
        /// </summary>
        /// <param name="sData"></param>
        /// <param name="sWriter"></param>
        private void DrawingHandler(string sData)
        {
            Lines.Add(sData);
        }
    }
}
