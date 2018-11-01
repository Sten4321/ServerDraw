using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Net;


namespace ClientDraw
{
    public partial class Form1 : Form
    {

        Graphics graphics;

        Pen pen;

        #region Prototype fields
        // coordinates out of screen
        int x = -1;
        int y = -1;
        #endregion;

        //client that draws
        TcpClient drawClient;

        //clients that can see the drawing
        List<TcpClient> slaveClients = new List<TcpClient>();

        // use these instead with server:
        int oldX = -1;
        int oldY = -1;

        int newX = -1;
        int newY = -1;

        Thread messageReceiver;


        //Determines whether it should draw or not
        private bool isDrawing = false;

        public Form1()
        {
            InitializeComponent();
            graphics = panel1.CreateGraphics();

            //Smoother edges
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            pen = new Pen(Color.Black, 6f);

            //Draws smoother lines
            pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;


            //Starts thread that receives messages from DrawClient
            messageReceiver = new Thread(ReceiveMessage);
            messageReceiver.IsBackground = true;
            messageReceiver.Start();

        }
        #region Prototype Methods
        /// <summary>
        /// Allows enables drawing and sets starting position from which to draw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouse"></param>
        private void panel1_MouseDown(object sender, MouseEventArgs mouse)
        {
            //Method should be in drawclient, and should sent its mouse coordinates

            isDrawing = true;


            x = mouse.X;
            y = mouse.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs mouse)
        {
            //Should be in an Update Method
            if (isDrawing && x != -1 && y != -1)
            {
                //draw line from old position to new position

                graphics.DrawLine(pen, new Point(x, y), new Point(mouse.X, mouse.Y));


                //Method should be in drawclient, and should sent its mouse coordinates
                //Update coordinates
                x = mouse.X;
                y = mouse.Y;
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs mouse)
        {
            isDrawing = false;

            x = -1;
            y = -1;

        }



        #endregion

        //To add:

        private void Update()
        {
            while (true)
            {
                if (isDrawing && x != -1 && y != -1)
                {
                    //draw line from old position to new position
                    graphics.DrawLine(pen, new Point(oldX, oldY), new Point(newX, newY));


                    //Update coordinates
                    oldX = newX;
                    oldY = newY;
                }

            }
        }

        /// <summary>
        /// Receives Messages from drawClient
        /// </summary>
        private void ReceiveMessage()
        {
            while (true)
            {

                //receive message from drawclient
                string message = ".";

                MessageHandler(message);


            }

        }
        /// <summary>
        /// Handles received data // not implemented
        /// </summary>
        private void MessageHandler(string message)
        {
            // if  "X,Y"
            if (message.Contains(","))
            {
                //Isolates the X and the Y
                ConvertCoordinates(message);
            }

            //Should it stop or start drawing? binary to minimize package size??? does it even matter???
            else if (message == "0")
            {
                isDrawing = false;
            }
            else if (message == "1")
            {
                isDrawing = true;
            }
        }

        /// <summary>
        /// Converts the string message containing coordinates, and updates the new x&y
        /// </summary>
        /// <param name="message"></param>
        private void ConvertCoordinates(string message)
        {
            //creates string array that contains the elements on each side of ',' char
            string[] coordinates = message.Split(',');

            if (coordinates.Length == 2 /*  or is it 1 because of 0 index???  */ )
            {
                if (coordinates[0].All(char.IsDigit))
                {
                    //Updates new x position
                    Int32.TryParse(coordinates[0], out newX);
                }
                if (coordinates[1].All(char.IsDigit))
                {
                    //Updates new y position
                    Int32.TryParse(coordinates[1], out newY);
                }
            }
        }



    }
}
