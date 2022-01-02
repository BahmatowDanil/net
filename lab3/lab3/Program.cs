using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;

namespace lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener serverSocket = new TcpListener(8888);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;

            serverSocket.Start();
            Console.WriteLine(" >> " + "Server Started");

            counter = 0;
            while (true)
            {
                counter += 1;
                clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine(" >> " + "Client No:" + Convert.ToString(counter) + " started!");
                handleClinet client = new handleClinet();
                client.startClient(clientSocket, Convert.ToString(counter));
            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine(" >> " + "exit");
            Console.ReadLine();
        }
    }

    //Class to handle each client request separatly
    public class handleClinet
    {
        TcpClient clientSocket;
        string clNo;
        public void startClient(TcpClient inClientSocket, string clineNo)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }

        private string parseCommand(string cmd)
        {
            string alph = "abcdefghijklmnopqrstuvwxyz" +
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                ".!,?0123456789-+=)(*";
            CubeConGen cubeCon  =   new CubeConGen();
            MaHash2 maHash2 = new MaHash2();
            Vernam vernam = new Vernam(alph);
            string res = "";
            string[] words = cmd.Split();
            switch (words[0].ToLower())
            {
                case "hello":
                    res = words[0] + " variant " + words[1];
                    break;
                case "bye":
                    res = words[0] + " variant " + words[1];
                    break;
                case "encrypt":
                    string ekey = "" + cubeCon.generateElem(Math.Abs(maHash2.calculate(words[2]) % 999));
                    res = '"' + vernam.Crypt(words[1], ekey) + '"';
                    break;
                case "decrypt":
                    string dkey = "" + cubeCon.generateElem(Math.Abs(maHash2.calculate(words[2]) % 999));
                    res = '"' + vernam.Crypt(words[1], dkey) + '"';
                    break;
                default:
                    break;
            }
            return res;
        }
        private void doChat()
        {
            int requestCount = 0;
            string dataFromClient = null;
            Byte[] sendBytes = null;
            string serverResponse = null;
            string rCount = null;
            requestCount = 0;

            while ((true))
            {
                try
                {
                    //read
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    byte[] bytesFrom = new byte[clientSocket.ReceiveBufferSize];
                    networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                    dataFromClient = Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    Console.WriteLine(" >> " + "From client - " + clNo + " " + dataFromClient);

                    //write
                    serverResponse = "Server -> Client(" + clNo + ") " + parseCommand(dataFromClient);
                    if (dataFromClient.Split()[0] == "bye")
                    {
                        sendBytes = Encoding.ASCII.GetBytes(serverResponse + " | Server shut down... ");
                        networkStream.Write(sendBytes, 0, sendBytes.Length);
                        networkStream.Flush();
                        Console.WriteLine(" >> " + serverResponse + "\n>>server shut down... ");
                        return;
                    }
                    else
                    {
                        sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                        networkStream.Write(sendBytes, 0, sendBytes.Length);
                        networkStream.Flush();
                        Console.WriteLine(" >> " + serverResponse + dataFromClient.Split()[0]);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(" >> " + ex.ToString());
                }
            }
        }
    }
}
