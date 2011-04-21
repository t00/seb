using System;

using System.Windows.Forms;
using System.Net.Sockets;
using System.Text; 



namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            msg("Client Started");
            clientSocket.Connect("127.0.0.1", 8888);
            label1.Text = "Client Socket Program - Server Connected ...";
        }


        private void button1_Click(object sender, EventArgs e)
        {
            NetworkStream serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("Message from Client$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            byte[] inStream = new byte[10025];
            serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
            string returndata = System.Text.Encoding.ASCII.GetString(inStream);
            msg("Data from Server : " + returndata);
        }


        public void msg(string mesg)
        {
            textBox1.Text = textBox1.Text + Environment.NewLine + " >> " + mesg;
        } 
    }
}

