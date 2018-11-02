using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Text;
using System.Threading;

namespace ClientViewDraw
{
    static class View
    {
        private static TcpClient slaveClient;
        private static StreamReader sReader;
        private static StreamWriter sWriter; //Maybe if we want to make a game
        private static bool connected;
        private static int portNumber = 2565;
        private static string[] cords;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ViewForm());
            ClientSetUp();
        }

        private static void ClientSetUp()
        {
            slaveClient = new TcpClient();
            slaveClient.Connect(IPAddress.Any, portNumber);//Lets the client connect.
            GetDrawing();
        }
        private static void GetDrawing()
        {
            NetworkStream getStream = slaveClient.GetStream();//Gets info from client
            sReader = new StreamReader(getStream, Encoding.UTF8);
            sWriter = new StreamWriter(getStream, Encoding.UTF8);

            connected = true;

            while (connected)
            {
                try
                {

                }
                catch(Exception e)
                {
                    Thread.CurrentThread.Abort();
                }
            }

        }
    }
}
