using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerDraw
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();
        }


        private void SendCoordinates()
        {
            //REMEMBER

            //relative to form
           Point coordinate = PointToClient(Cursor.Position); 


            //send that shit
        }
    }
}
