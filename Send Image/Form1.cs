using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Send_Image
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        MemoryStream ms;
        TcpClient client;
        NetworkStream ns;
        BinaryWriter bw;
        private void sendButton_Click(object sender, EventArgs e)
        {
            Bitmap captureBitmap = new Bitmap(1024, 768, PixelFormat.Format32bppArgb);


           

            //Creating a Rectangle object which will  

            //capture our Current Screen
            Rectangle captureRectangle = Screen.AllScreens[0].Bounds;
            //Creating a New Graphics Object
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);
            //Copying Image from The Screen
            captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);
            var imageWithBytes = ImageToByteArray(captureBitmap);
            pictureBox1.Image = captureBitmap;
            try
            {
                // ms = new MemoryStream();
                // ms.Close();
                client = new TcpClient("127.0.0.1", 19999);
                ns = client.GetStream();  
                bw = new BinaryWriter(ns); 
                bw.Write(imageWithBytes); 
                bw.Close();
                ns.Close();
                //client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public byte[] ImageToByteArray(Image imageIn)
        {
            ImageConverter converter = new ImageConverter();
            var bytes = (byte[])converter.ConvertTo(imageIn, typeof(byte[]));
            return bytes;
        }
        string GetIpAddress()
        {
            IPHostEntry host;
            string localhost = "?";
            host = Dns.GetHostEntry(Dns.GetHostName()); // return hostname
            
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localhost = ip.ToString();
                }
            }
            return localhost;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            txtServer.Text = GetIpAddress();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}
