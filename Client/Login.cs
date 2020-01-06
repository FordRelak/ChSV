/*

Сюда крч будем заметки писать.
Зачекай потом в сервере эту дичь

*/


using Client;
using System;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ChSV
{
    public partial class Login : Form
    {
        /// <summary>
        /// Поток для отправления логина и пароля
        /// </summary>
        BackgroundWorker bSend = new BackgroundWorker();
        /// <summary>
        /// Поток для открытия второй формы
        /// </summary>
        BackgroundWorker bOpenChat = new BackgroundWorker();

        //
        BinaryWriter writer;
        BinaryReader reader;

        //
        TcpClient clientLogin = null;
        NetworkStream streamLogin = null;

        //
        static string address = "88.135.50.215";
        static int port = 8009;

        /// <summary>
        /// Команда для определение действия. LoginZ - вход
        /// </summary>
        string LoginZ = "LoginZ";
        /// <summary>
        /// Команда для определение действия. CloseZ - закрытие формы
        /// </summary>
        string CloseZ = "CloseZ";
        /// <summary>
        /// Команда для определение действия. DisconnectZ - при отключении от серва
        /// </summary>
        string DisconnectZ = "DisconnectZ";

        //string ErrorZ = "ErrorZ";

        /// <summary>
        /// Логин пользователя
        /// </summary>
        string login;
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        string pass;

        /// <summary>
        /// Создание формы
        /// </summary>
        public Login()
        {
            InitializeComponent();

            bSend.WorkerSupportsCancellation = true;
            bSend.DoWork += (obj, ea) => SendLoginPass();
        }

        /// <summary>
        /// Кнопка "Войти"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                //
                if (bSend.IsBusy)
                    bSend.CancelAsync();

                if (LoginButton.Enabled)
                    LoginButton.Enabled = false;

                //
                clientLogin = new TcpClient();
                //Удалось ли подключиться 
                //var result = clientLogin.BeginConnect(IPAddress.Parse(address), port, null, null);
                //var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));

                if (!clientLogin.Connected)
                {
                    try
                    {
                        clientLogin.Connect(System.Net.IPAddress.Parse(address), port);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Не удалось подключиться.", "Сервер не отвечает", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    clientLogin.SendTimeout = 1000;
                    clientLogin.ReceiveTimeout = 5000;
                }

                if (!clientLogin.Connected)
                    throw new Exception("Не подключен!");
                //
                SendLoginPass();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "LoginButton_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EnabledButton();
                return;
            }
        }

        /// <summary>
        /// Отправить логин+пароль
        /// </summary>
        private void SendLoginPass()
        {
            System.Threading.Thread.Sleep(100);
            try
            {
                //
                if (streamLogin == null)
                    streamLogin = clientLogin.GetStream();
                if (!(streamLogin.CanRead && streamLogin.CanWrite))
                    streamLogin = clientLogin.GetStream();
                //
                writer = new BinaryWriter(streamLogin);
                reader = new BinaryReader(streamLogin);

                //
                login = LoginTextBox.Text;
                pass = PassTextBox.Text;

                //Запрос + логин + пароль
                string message = LoginZ + ' ' + login + ' ' + pass;
                //Отправка
                writer.Write(message);

                StringBuilder stringBuilder = new StringBuilder();

                //Получение ответа
                try
                {
                    if (clientLogin.Connected)
                        stringBuilder.Append(reader.ReadString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Получение сообщения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    EnabledButton();
                    Disconnect();
                    return;
                }
                //
                string ans = stringBuilder.ToString();

                //Что делать с ответом?!
                if (ans.Contains("true"))
                {
                    Hide();
                    Chat chat = new Chat(login, clientLogin);
                    chat.ShowDialog();
                    Show();
                    EnabledButton();
                }
                else if (ans.Contains("DisconnectZ"))
                {
                    throw new Exception(/*ans.Remove(0, 11)*/);
                }
                else if (ans.Contains("false"))
                {
                    throw new Exception(/*ans.Remove(0, 5)*/);
                }
                else
                {
                    throw new Exception(ans);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "SendLoginPass", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EnabledButton();
            }
        }

        /// <summary>
        /// Закрытие формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //
                if (reader != null)
                    reader.Close();
                if (writer != null)
                    writer.Close();
                if (streamLogin != null)
                    streamLogin.Close();
                if (clientLogin != null)
                    clientLogin.Close();
                if (bSend.IsBusy)
                    bSend.CancelAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Login_FormClosing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Вкл/выкл кнопки
        /// </summary>
        private void EnabledButton()
        {
            if (!LoginButton.Enabled)
            {
                LoginButton.Enabled = true;
            }
        }

        /// <summary>
        /// Отключение от сервера
        /// </summary>
        private void Disconnect()
        {
            if (!(streamLogin.CanRead && streamLogin.CanWrite))
                streamLogin = clientLogin.GetStream();

            string message = DisconnectZ + " " + login + " " + "отключился";
            try
            {
                if (clientLogin.Connected)
                    writer.Write(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Сообщение об отключении не отправлено", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (streamLogin != null)
                streamLogin.Close();//отключение потока
            if (clientLogin != null)
                clientLogin.Close();//отключение клиента
        }
    }
}
