using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace ChSV
{
    public partial class Chat : Form
    {
        string userName;
        //
        static string address = "88.135.50.215";
        static int port = 8009;
        //
        TcpClient clientChat = new TcpClient();
        NetworkStream streamChat = null;

        //
        BinaryWriter writer = null;
        BinaryReader reader = null;

        //
        BackgroundWorker bSendMessage = new BackgroundWorker();
        BackgroundWorker bGetMessages = new BackgroundWorker();

        /// <summary>
        /// Команда
        /// </summary>
        string ChatZ = "ChatZ", CloseZ = "CloseZ", ErrorZ = "ErrorZ", ActivUsersListZ = "ActivUsersListZ";

        /// <summary>
        /// Имя участников чата
        /// </summary>
        string allMembers;

        /// <summary>
        /// Выход из цикла потока получения сообщений чата
        /// </summary>
        bool checkExit = true;

        /// <summary>
        /// Подключение к серверу
        /// </summary>
        bool haveConnect = true;

        /// <summary>
        /// Конструктор второй формы
        /// </summary>
        /// <param name="userName">Имя пользователся</param>
        /// <param name="tcpClient">Подключение клиента</param>
        public Chat(string userName, TcpClient tcpClient)
        {
            InitializeComponent();
            try
            {
                this.userName = userName;
                clientChat = tcpClient;
                streamChat = clientChat.GetStream();

                writer = new BinaryWriter(streamChat);
                reader = new BinaryReader(streamChat);

                bSendMessage.WorkerSupportsCancellation = true;
                bSendMessage.DoWork += SendMessage;

                bGetMessages.WorkerSupportsCancellation = true;
                bGetMessages.DoWork += GetMessage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Chat", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Disconnect();
            }
        }

        /// <summary>
        /// При загрузке формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Chat_Load(object sender, EventArgs e)
        {
            try
            {
                //
                bGetMessages.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Chat_Load", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// При закрытие формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Chat_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                checkExit = false;

                if (clientChat != null)
                {
                    if (streamChat == null)
                        streamChat = clientChat.GetStream();
                    if (clientChat.Connected)
                    {
                        writer = new BinaryWriter(streamChat);
                        string message = CloseZ + " " + userName;
                        //Отправка
                        writer.Write(message);
                    }
                }

                //
                if (bSendMessage.IsBusy)
                    bSendMessage.CancelAsync();
                if (bGetMessages.IsBusy)
                    bGetMessages.CancelAsync();
                if (streamChat != null)
                    streamChat.Close();
                if (clientChat != null)
                    clientChat.Close();
            }
            catch (Exception /*ex*/)
            {
                //MessageBox.Show(ex.ToString(), "Chat_FormClosing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //ErrorSend();
                Application.Exit();
            }
        }

        /// <summary>
        /// Кнопка "Отправить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendButton_Click(object sender, EventArgs e)
        {
            try
            {
                //
                bSendMessage.RunWorkerAsync();
            }
            catch (Exception)
            {
                if (bSendMessage.IsBusy)
                    bSendMessage.CancelAsync();
            }
        }

        /// <summary>
        /// Отправить сообщение (в отдельный поток)
        /// </summary>
        private void SendMessage(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (bSendMessage.CancellationPending)
                    return;
                if (!clientChat.Connected)
                    Close();
                if (streamChat == null)
                    streamChat = clientChat.GetStream();

                //Отправить    
                string message = ChatZ + ' ' + userName + ": " + MessageTextBox.Text;
                BeginInvoke((MethodInvoker)(() =>
                {
                    MessageTextBox.Clear();
                }));
                if (!clientChat.Connected)
                    return;
                ConnectTimer.Start();
                writer.Write(message);
                writer.Flush();
                SendButton.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "SendMessage", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //ErrorSend();
                Disconnect();
            }
        }

        /// <summary>
        /// Транслирование изменения чата (в отдельный поток)
        /// </summary>
        private void GetMessage(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(500);
            try
            {
                while (checkExit)
                {
                    if (bGetMessages.CancellationPending)
                        return;
                    if (streamChat == null)
                        streamChat = clientChat.GetStream();
                    //Получить
                    if (streamChat.DataAvailable)
                    {
                        string mesFromServer = reader.ReadString();
                        if (mesFromServer.IndexOf("DisconnectZ") == 0)
                        {
                            CloseChat();
                        }
                        else if (mesFromServer.IndexOf("ActivUsersListZ") == 0)
                        {
                            new Thread(new ParameterizedThreadStart(AddMembers)).Start(mesFromServer);
                        }
                        else
                        {
                            new Thread(new ParameterizedThreadStart(AddChatMessage)).Start(mesFromServer);
                        }
                        mesFromServer = "";
                    }
                    else if (!haveConnect)
                    {
                        ConnectTimer.Stop();
                        throw new Exception("Сервер не отвечает.");
                    }
                    else if (haveConnect)
                        ConnectTimer.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "GetMessage", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //ErrorSend();
                Disconnect();
            }
        }

        /// <summary>
        /// Добавить участников чата (в отдельный поток)
        /// </summary>
        private void AddMembers(object members)
        {
            Thread.Sleep(1000);
            lock (this)
            {
                if (MemberListBox.Items.Count != 0)
                    MemberListBox.Items.Clear();
                string[] memberArray = members.ToString().Remove(0,ActivUsersListZ.Length+1).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var member in memberArray)
                {
                    MemberListBox.Items.Add(member);
                    ChatListBox.SelectedIndex = ChatListBox.Items.Count - 1;
                    ChatListBox.SelectedIndex = -1;
                }
            }

        }

        /// <summary>
        /// Добавить сообщение в чат (в отдельный поток)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddChatMessage(object message)
        {
            lock (this)
            {
                ChatListBox.Items.Add((message.ToString()));
                ChatListBox.SelectedIndex = ChatListBox.Items.Count - 1;
                ChatListBox.SelectedIndex = -1;
            }
        }

        private void MessageTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (MessageTextBox.Focused && e.KeyChar==13)
            {
                SendButton_Click(sender, e);
            }
        }

        /// <summary>
        /// Таймер проверки соединения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectTimer_Tick(object sender, EventArgs e)
        {
            Ping ping = new Ping();
            var reply = ping.Send(IPAddress.Parse(address), 1);
            haveConnect = reply.Status != IPStatus.Success;
        }

        /// <summary>
        /// Закрытие окна чата и переход к окну логин/пароль
        /// </summary>
        private void CloseChat()
        {
            try
            {
                streamChat.Close();
                clientChat.Close();
                checkExit = false;
                var result = MessageBox.Show("Вас отключили от сервера.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                    BeginInvoke((MethodInvoker)(() =>
                    {
                        Close();
                    }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "CloseChat", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Отключиться от сервера
        /// </summary>
        private void Disconnect()
        {
            if (streamChat != null)
                streamChat.Close();//отключение потока
            if (clientChat != null)
                clientChat.Close();//отключение клиента
            Environment.Exit(0); //завершение процесса
        }

        private void GetFile()
        {
            
        }

    }
}
