using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ChSV
{
    public partial class Chat : Form
    {
        string userName;
        //
        static string address = "88.135.50.215";
        static int port = 8008;
        //
        static TcpClient client;
        static NetworkStream stream;

        //
        Thread tSendMessage, tBroadCast;

        public Chat(string userName)
        {
            InitializeComponent();
            this.userName = userName;
        }

        //
        private void SendButton_Click(object sender, EventArgs e)
        {
            //
            tSendMessage = new Thread(new ThreadStart(SendMessage));
            tSendMessage.Start();
        }

        //Отправить сообщение (в отдельный поток)
        private void SendMessage()
        {
            try
            {
                string message = MessageTextBox.Text;
                byte[] data = Encoding.Unicode.GetBytes(message);
                //Отправить
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CloseAll();
            }
        }

        //Транслирование изменения чата (в отдельный поток)
        private void BroadCast()
        {
            try
            {
                while (true)
                {
                    byte[] data = new byte[128];
                    int bytes = 0;
                    bytes = stream.Read(data, 0, data.Length);
                    StringBuilder builder = new StringBuilder();
                    builder.Append( Encoding.Unicode.GetString(data, 0, bytes));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CloseAll();
            }
        }

        //
        private void Chat_Load(object sender, EventArgs e)
        {
            try
            {
                //
                client = new TcpClient();
                //Удалось ли подключиться 
                var result = client.BeginConnect(IPAddress.Parse(address), port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
                //
                if (!success)
                {
                    throw new Exception("Не подключен!");
                }

                //
                stream = client.GetStream();

                //
                tBroadCast = new Thread(new ThreadStart(BroadCast));
                tBroadCast.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CloseAll();
            }
        }

        //Закрыть всё что есть
        private void CloseAll()
        {
            if (tBroadCast != null && tBroadCast.IsAlive)
            {
                tBroadCast.Abort();
            }
            if (tSendMessage != null && tSendMessage.IsAlive)
            {
                tSendMessage.Abort();
            }
            if (client!= null)
            {
                client.Close();
            }
            if (stream != null)
            {
                stream.Close();
            }
        }
    }
}
