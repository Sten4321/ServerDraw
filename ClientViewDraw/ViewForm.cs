using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientViewDraw
{
    public partial class ViewForm : Form
    {
        private static TcpClient slaveClient;
        private static StreamReader sReader;
        private static StreamWriter sWriter; //Maybe if we want to make a game
        private static bool connected;
        private static int portNumber = 25565;
        private static string iPAddress = "10.131.69.236";
        private static string[] cords;

        public ViewForm()
        {
            InitializeComponent();
        }

        private void ViewForm_Load(object sender, EventArgs e)
        {
            ClientSetUp();
        }


        private void ClientSetUp()
        {
            slaveClient = new TcpClient();
            slaveClient.Connect(IPAddress.Parse(iPAddress), portNumber);//Lets the client connect.

            GetDrawing();
        }
        private void GetDrawing()
        {
            NetworkStream getStream = slaveClient.GetStream();//Gets info from client
            sReader = new StreamReader(getStream, Encoding.UTF8);
            sWriter = new StreamWriter(getStream, Encoding.UTF8);
            sWriter.WriteLine("View");
            sWriter.Flush();
            connected = true;

            while (connected)
            {
                try
                {
                    Draw(sReader);
                }
                catch (Exception)
                {
                    Thread.CurrentThread.Abort();
                }
                Thread.Sleep(17);
            }

        }

        private void Draw(StreamReader sReader)
        {

        }
    }
}
