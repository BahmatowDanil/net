using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Text;

namespace lab4
{
    public partial class Form1 : Form
    {
        TcpClient clientSocket = new TcpClient();
        NetworkStream serverStream;

        public Form1()
        {
            InitializeComponent();
            msg("Клиент начал работу");
            clientSocket.Connect("127.0.0.1", 8888);
            label1.Text = "Подключено к серверу...";
        }


        private void button1_Click(object sender, EventArgs e)
        {
            NetworkStream serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(sendMSG());
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            byte[] inStream = new byte[10025];
            serverStream.Read(inStream, 0, inStream.Length);
            string returndata = Encoding.ASCII.GetString(inStream);
            msg("Данные с сервера: " + returndata);
        }
        public string sendMSG()
        {
            return textBox2.Text + "$";
        }
        public void msg(string mesg)
        {
            textBox1.Text = textBox1.Text + Environment.NewLine + " >> " + mesg;
        }
    }
}
