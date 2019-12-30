/*

Сюда крч будем заметки писать.
Зачекай потом в сервере эту дичь

*/


using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ChSV
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        //
        static string address = "88.135.50.215";
        static int port = 8008;

        //
        TcpClient tcpClient;
        NetworkStream stream;
        Thread threadSend, threadLogin;

        //
        private void Login_Load(object sender, EventArgs e)
        {
            //
        }

        //
        private void LoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                LoginButton.Enabled = false;

                threadLogin = new Thread(new ThreadStart(LoginThread));
                threadLogin.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoginThread()
        {
            try
            {
                //
                tcpClient = new TcpClient();
                //Удалось ли подключиться 
                var result = tcpClient.BeginConnect(IPAddress.Parse(address), port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
                //
                if (!success)
                {
                    throw new Exception("Не подключен");
                }
                stream = tcpClient.GetStream();

                threadSend = new Thread(new ThreadStart(SendLoginPass));
                threadSend.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                LoginButton.Enabled = true;
            }
        }

        //
        string LoginZ = "LoginZ";

        //
        string login, pass;

        //
        private void SendLoginPass()
        {
            try
            {
                //
                login = LoginTextBox.Text;
                pass = PassTextBox.Text;

                //Запрос + логин + пароль
                string message = LoginZ + ' ' + login + ' ' + pass;
                byte[] data = Encoding.Unicode.GetBytes(message);
                //Отправка
                stream.Write(data, 0, data.Length);

                //Переменные для получение сообщения
                int bytes = 0; //кол-во полученных байт. то же самое что и data.Length
                StringBuilder answer = new StringBuilder(); //Ответ от сервера
                data = new byte[256]; //

                //Получение ответа
                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    answer.Append(Encoding.Unicode.GetString(data, 0, bytes));
                } while (stream.DataAvailable);

                string ans = answer.ToString();

                //Что делать с ответом?!
                if (ans.Contains("true"))
                {
                    MessageBox.Show(ans.Remove(0, 4), "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Hide();
                    //Chat chat = new Chat();
                    //chat.ShowDialog();
                    //Close();
                }
                else if (ans.Contains("false"))
                {
                    throw new Exception(ans.Remove(0, 5));
                }
                else
                {
                    throw new Exception(ans);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                LoginButton.Enabled = true;
                CloseAll();
            }
        }

        //
        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseAll();
        }

        //Закрывает все сокеты, клиенты, потоки и всю такую дичь
        private void CloseAll()
        {
            if (stream != null)
            {
                stream.Close();
            }
            if (tcpClient != null)
            {
                tcpClient.Close();
            }
            if (threadSend != null)
            {
                if (threadSend.IsAlive)
                {
                    threadSend.Abort();
                }
            }
            if (threadLogin != null)
            {
                if (threadLogin.IsAlive)
                {
                    threadLogin.Abort();
                }
            }
        }

    }
}
