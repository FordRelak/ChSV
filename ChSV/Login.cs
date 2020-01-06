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
        static string address = "192.168.1.11";
        static int port = 8008;
        
        //
        Socket socket;

        //
        private void Login_Load(object sender, EventArgs e)
        {
            try
            {
                //
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
                //
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //
                socket.Connect(iPEndPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

        }


        //
        private void LoginButton_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(SendLoginPass));
            thread.Start();
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
                socket.Send(data);

                //Переменные для получение сообщения
                int bytes = 0; //кол-во полученных байт. то же самое что и data.Length
                StringBuilder answer = new StringBuilder(); //Ответ от сервера
                data = new byte[256]; //

                //Получение ответа
                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    answer.Append(Encoding.Unicode.GetString(data, 0, bytes));
                } while (socket.Available > 0);

                string ans = answer.ToString();

                //Что делать с ответом?!
                if (ans.Contains("true"))
                {
                    MessageBox.Show(ans.Remove(0, 4));
                    //Hide();
                    //Chat chat = new Chat();
                    //chat.ShowDialog();
                    //Close();
                }
                else if (ans.Contains("false"))
                {
                    MessageBox.Show(ans.Remove(0, 5));
                }
                else
                {
                    MessageBox.Show(ans);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
