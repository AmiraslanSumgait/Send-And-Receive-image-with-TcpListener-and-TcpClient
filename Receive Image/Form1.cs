using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Receive_Image
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        TcpListener tcpListener;
        Socket socket;
        NetworkStream ns;
        Thread th;
        void ReceiveImage()
        {
            try
            {
                tcpListener = new TcpListener(19999);
                tcpListener.Start();
                socket = tcpListener.AcceptSocket();
                ns = new NetworkStream(socket);
                pictureBox1.Image = Image.FromStream(ns);
                tcpListener.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            th = new Thread(new ThreadStart(ReceiveImage));
            th.Start();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            tcpListener.Stop();
            th.Abort();
        }
    }
}
