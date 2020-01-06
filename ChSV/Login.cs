/*

Сюда крч будем заметки писать.
Зачекай потом в сервере эту дичь

*/










using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChSV
{
    public partial class Login : Form
    {
        private string server;
        private int port;

        BackgroundWorker bw;

        bool CheckIn = false;

        public Login()
        {
            InitializeComponent();
            label5.Text = "";
            IPcomboBox.Text = "88.135.50.215";
            PorttextBox.Text = "8008";

            //bw = new BackgroundWorker();
            //bw.WorkerSupportsCancellation = true;
            //bw.DoWork += (obj, ea) => Checker(); 
            //bw.RunWorkerAsync(); 
        }

        IPEndPoint iPEndPoint;
        Socket socket, checkSocket;
        System.Windows.Forms.Timer timer_connection = new System.Windows.Forms.Timer();

        //private void Checker()
        //{
            
        //    while (!CheckIn)
        //    {
        //        Thread.Sleep(1000);
        //        if(InvokeRequired)
        //        {
        //            Invoke(new Action(() =>
        //            {
        //                server = IPcomboBox.Text;
        //                port = Convert.ToInt32(PorttextBox.Text);
                        
        //            }));
        //        }

        //        iPEndPoint = new IPEndPoint(IPAddress.Parse(server), port);//К серваку

        //        checkSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);// сокет
        //                                                                                                  //IAsyncResult result = checkSocket.BeginConnect(server, port, null, null);
        //                                                                                                  //bool success = result.AsyncWaitHandle.WaitOne(1000, true);

        //        checkSocket.ConnectAsync(iPEndPoint); // подключение
        //        if (timer_connection != null)
        //        {
        //            timer_connection.Dispose();
        //        }
              
        //        timer_connection.Interval = 1000;
        //        timer_connection.Tick += new EventHandler(timer_connection_Tick);
        //        timer_connection.Start();




        //        //catch (Exception)
        //        //{
        //        //    label6.Text = "Offline";
        //        //}
        //        //finally
        //        //{
        //        //    label6.Text = "Online";
        //        //    //checkSocket.Disconnect();

        //        //}
        //        string message = "PingZ";
        //        byte[] data = Encoding.Unicode.GetBytes(message);
        //        checkSocket.Send(data);
        //        checkSocket.Shutdown(SocketShutdown.Both);
        //        checkSocket.Close();

        //    }
        //}

        //private void timer_connection_Tick(object sender, EventArgs e)
        //{
        //    if (!checkSocket.Connected)
        //    {
        //        MessageBox.Show("Connection Timeout");
        //        //m_clientSocket = null;

        //        timer_connection.Stop();
        //    }
        //}

        private void LoginButton_Click(object sender, EventArgs e)
        {

            //Связь с БД, если есть акк то изи заход
            //А если нет, то иди нахер создавать акк

            label5.Text = "";
            if (IPcomboBox.Text == "")
            {
                label5.Text = "Введите IP Сервера";
            }
            if(PorttextBox.Text == "")
            {
                label5.Text += "Введите Порт Сервера";
            }

            server = IPcomboBox.Text;
            port = Convert.ToInt32(PorttextBox.Text);

            try
            {
                iPEndPoint = new IPEndPoint(IPAddress.Parse(server), port);//К серваку

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);// сокет

                try
                {
                    socket.Connect(iPEndPoint); // подключение
                }
                catch (Exception)
                {
                    label5.Text = "Что-то \r\n с серваком";
                }


                //Отправка
                string login = LogintextBox.Text;
                string pass = PasswordtextBox.Text;
                string message = "LoginZ "+ login + " " + pass;
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);

                //Получение
                data = new byte[256]; // буфер
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // кол-во полученных байт

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                } while (socket.Available > 0);

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();

                string rec = builder.ToString();

                if (rec.IndexOf("true") != -1)
                {
                    MessageBox.Show("ff");
                    //Hide();
                    //Client client = new Client();
                    //client.ShowDialog();
                    //Close();
                }
                else
                {

                    MessageBox.Show("Такого пользователя нет", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
