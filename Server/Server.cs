/*
 WorkHoursCounter: 54
 Последние изменения: 05/01/2020

 Заметки
 
 Список команд для сервера
 1) LoginZ - Запрос на авторизацию
 2) FileZ - Запрос на скачивание файла
 3) ChatZ - Вывод в чат сообщения
 4) ClozeZ - Выход из приложения
 5) ErrorZ - Получения информации об ошибке
 6) ActivUsersListZ

 Конструкция запроса:
 1) <Тип запроса> <логин> <пароль>
 2) <Тип запроса> <Путь к файлу> (В доработке)
 3) <Тип запроса> <сообщения>
 4) <Тип запроса>
 5) <Тип запроса> <ошибка>

 Надо реализовать:
 1) Режимы работы сервера
 2) Адекватное выключение и включение сервера
 3) 


*/
using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Data;
using System.Data.SQLite; // SQLITE

using System.Drawing;
using System.Linq;

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Server
{
    public partial class Server : Form
    {
        /// <summary>
        /// IP Адрес сервера
        /// </summary>
        private string IP_Server = "192.168.1.11";
        /// <summary>
        /// Порт сервера
        /// </summary>
        private int Port_server = 8009; 
        /// <summary>
        /// Переменная работы сервера
        /// </summary>
        TcpListener server = null;

        // SQLITE
        /// <summary>
        /// Путь к БД
        /// </summary>
        SQLiteConnection DB;
        /// <summary>
        /// Переменная для чтения из БД
        /// </summary>
        SQLiteDataReader SQL;
        /// <summary>
        /// Переменная для исполенения команд
        /// </summary>
        SQLiteCommand CMD;
        /// <summary>
        /// Строка подключения к БД
        /// </summary>
        string constr; 

        /// <summary>
        /// Переменные отдельных потоков
        /// </summary>
        BackgroundWorker bw,bw2,bw3;
        /// <summary>
        /// Контейнер переменных, хранящих данные о состоянии соединения
        /// </summary>
        public List<ClientObject> CO = new List<ClientObject>();
        /// <summary>
        /// Контейнер переменных потока, которые поддерживают соединение
        /// </summary>
        List<BackgroundWorker> BWL = new List<BackgroundWorker>();
        /// <summary>
        /// Поле описывающее состояние работы сервера
        /// </summary>
        bool CheckServerStatus = false;

        // Начало работы приложения
        public Server()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            var exePath = AppDomain.CurrentDomain.BaseDirectory; // Путь до .exe файла.
            var path = System.IO.Path.Combine(exePath, "ChSV.db"); // Путь до .exe файла + название БД
            constr = $"Data Source={path};Version=3;"; // Строка подключения к БД

            // Подключение к БД
            DB = new SQLiteConnection(constr);
            DB.Open();


            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += (obj, ea) => Listner(); // Метод прослушки порта 8008
            bw.RunWorkerAsync(); // Открытие сервера

            Thread.Sleep(10);

            bw2 = new BackgroundWorker();
            bw2.WorkerSupportsCancellation = true;
            bw2.DoWork += (obj, ea) => ActivUsersList(); // Метод прослушки порта 8008
            bw2.RunWorkerAsync(); // Открытие сервера

            //bw3 = new BackgroundWorker();
            //bw3.WorkerSupportsCancellation = true;
            //bw3.DoWork += (obj, ea) => ActivUsersList(); // Метод прослушки порта 8008
            //bw3.RunWorkerAsync(); // Открытие сервера


            StatusBox.Text += "(" + DateTime.Now.ToString() + ") Программа запущена";
            ChatTB.Text += "(" + DateTime.Now.ToString() + ") Чат запущен";
        }


        void Listner()
        {
            IPAddress IPAddress = IPAddress.Parse(IP_Server);
            server = new TcpListener(IPAddress, Port_server);

            while(!CheckServerStatus) { }
            Thread.Sleep(50);
            
            try
            {
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();

                    CO.Add(new ClientObject(client, this));

                    User_LB.Items.Add(CO.Last().MYIpClient);
                    int counter = 0;
                    foreach (var item in CO)
                    {
                        if (item.MYIpClient == CO.Last().MYIpClient)                    
                            counter++;
                    }

                    if (counter > 1)
                    {
                        //CO.Remove(CO.Last());
                        CO.Last().SecondConnect = true;
                        //continue;
                    }

                    BWL.Add(new BackgroundWorker());
                    BWL.Last().WorkerSupportsCancellation = true;
                    
                    BWL.Last().DoWork += (obj, ea) => CO.Last().ConnectStream(); // Метод проверки соединений
                    BWL.Last().RunWorkerAsync(); // 
                    CO.Last().bw_s = BWL.Last();
                   
                }
            }
            catch (Exception ex)
            {
                StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Ошибка: " + ex.ToString() + "\r\n Место ошибки: Метод Listner";
                server = null;
                
                while (!CheckServerStatus) { }
                Thread.Sleep(50);
                Listner();
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        void ActivUsersList()
        {
            while(true)
            {
                Thread.Sleep(5000);
                try
                {
                    if (CO.Count == 0)
                        continue;
                    if (bw2.CancellationPending)
                    {
                        break;
                    }

                    string StreamMes = "ActivUsersListZ";
                    Ping x; PingReply reply;
                    foreach (var item in CO)
                    {
                        //x = new Ping();
                        //reply = x.Send(item.MYIpClient);
                        
                        //if (reply.Status != IPStatus.Success && item.Connected == true)
                        //{
                        //    KillConnect(item, item.bw_s, 3);
                        //}
                        //else
                        //   
                        try
                        {
                            NetworkStream stream1 = item.client.GetStream();
                            StreamMes += " " + item.UserName;
                            //stream1.Close();
                            //stream1 = null;
                        }
                        catch(Exception)
                        {
                            KillConnect(item, item.bw_s, 3);
                        }



                    }


                    System.IO.BinaryWriter writer1 = null;
                    foreach (var item in CO)
                    {
                        if (item.Connected == true)
                        {
                            NetworkStream stream2 = item.client.GetStream();
                            writer1 = new System.IO.BinaryWriter(stream2);
                            writer1.Write(StreamMes);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Ошибка: " + ex.ToString() + "\r\n Место ошибки: Метод ActivUsersListZ";
                }
            }
        }

        

        // Отправка сообщения от имени администратора
        private void SendM_Click(object sender, EventArgs e)
        {
            string ChatZstr = STB.Text;
            string StreamMes = "\r\n(" + DateTime.Now.ToString() + ") Server: " + ChatZstr;
            System.IO.BinaryWriter writer1 = null;
            foreach (var item in CO)
            {
                NetworkStream stream1 = null;
                try
                {
                    stream1 = item.client.GetStream();

                }
                catch(Exception)
                {

                }
                if (item.Connected == true)
                {
                    
                    writer1 = new System.IO.BinaryWriter(stream1);
                    writer1.Write(StreamMes);
                }
            }
            ChatTB.Text += "\r\n(" + DateTime.Now.ToString() + ") Server: " + ChatZstr;
            ChatTB.SelectionStart = ChatTB.Text.Length;
            ChatTB.ScrollToCaret();
            //TcpClient tcpClient = new TcpClient();
            //tcpClient.Connect(IPAddress.Parse("192.168.1.11"), 20000);
        }

        // Выбор соединения, которое необходимо завершить
        private void CloseCon_Click(object sender, EventArgs e)
        {
            foreach (var item in CO)
            {
                if (User_LB.SelectedItem.ToString() == item.MYIpClient)
                { 
                    KillConnect(item, item.bw_s,2);
                    break;
                }
            }
        }

        // Запуск сервера
        private void запускToolStripMenuItem_Click(object sender, EventArgs e)
        {
            остановкаToolStripMenuItem.Enabled = true;
            запускToolStripMenuItem.Enabled = false;
            server.Start();
            StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Сервер запущен";
            CheckServerStatus = true;
        }

        // Остановка сервера
        private void остановкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            остановкаToolStripMenuItem.Enabled = false;
            запускToolStripMenuItem.Enabled = true;
            MethodKP();
            server.Stop();
            StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Сервер закрыт";
            CheckServerStatus = false;
        }

        // Выбор режима работы
        private void WcServer_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (WcServer_CB.Text)
            {
                case "Глобальный":
                    IpServer_TB.Text = "192.168.1.11";
                    IpServer_TB.ReadOnly = true;
                    ServerParam_GB.Enabled = true;
                    break;
                case "Локальный":
                    IpServer_TB.Text = "127.0.0.1";
                    IpServer_TB.ReadOnly = true;
                    ServerParam_GB.Enabled = true;
                    break;
                case "Кастомный":
                    ServerParam_GB.Enabled = true;
                    break;
            }
        }

        // Сохранение настроек 
        private void SavePS_TB_Click(object sender, EventArgs e)
        {
            try
            {
                IP_Server = IpServer_TB.Text;
                Port_server = Convert.ToInt32(PortServer_TB.Text);
              
            }
            catch (Exception ex)
            {
                StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Ошибка: " + ex.ToString() + "\r\n Место ошибки: " + (sender as Button).Text;
            }
        }

        /// <summary>
        /// Метод для завершения всех активных соединений
        /// </summary>
        void MethodKP()
        {
            
            foreach (var item in CO)
            {
                if (CO.Count == 0)
                    break;
                else
                {
                    KillConnect(item, item.bw_s, 3);
                    MethodKP();
                    break;
                }
            }
        }

        
        /// <summary> Метод для завершения соединения </summary>
        /// <param name="clientObject"> <summary> Переменная соединения </summary> </param> 
        /// <param name="t"> <summary> Переменная потока, в котором поддерживается соединение </summary> </param>
        /// <param name="Choice"> <summary> Выбор конфигурации удаления: 1 - Два соединения; 2 - Удаление вручную; 3 - Удаление в ходе работы программы; </summary> </param>
        public void KillConnect(ClientObject clientObject, BackgroundWorker t, int Choice)
        {
            if (clientObject.InProccesKill == false)
                clientObject.InProccesKill = true;
            else
                return;
            try
            {
                int counter = 0;
                switch (Choice)
                {

                    case 1: // 2 соединения
                        foreach (var item in User_LB.Items)
                        {
                            if (item.ToString() == clientObject.MYIpClient)
                            {
                                counter++;
                                if (counter > 1)
                                {
                                    User_LB.Items.Remove(item);
                                    break;
                                }
                            }
                        }
                        break;
                    case 2: // Сам

                        User_LB.Items.Remove(User_LB.SelectedItem);

                        break;
                    case 3:
                        foreach (var item in User_LB.Items)
                        {
                            if (item.ToString() == clientObject.MYIpClient)
                            {
                                User_LB.Items.Remove(item);
                                break;
                            }
                        }
                        break;
                    default:
                        break;
                }

                clientObject.DisconectMes();
                Thread.Sleep(50);
                BWL.Remove(t);
                CO.Remove(clientObject);

            }
            catch (Exception ex)
            {
                StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Ошибка: " + ex.ToString() + "\r\n Место ошибки: Метод KillConnect";
            }

        }

        // Метод для определения правильности введённых данных для входа
        public string[] CheckUser(string login, string pass)
        {
            string[] str = new string[2];

            // Поиск логина/пароля
            CMD = DB.CreateCommand();
            CMD.CommandText = "SELECT * FROM users_ChSV;";
            SQL = CMD.ExecuteReader();
            if (SQL.HasRows)
            {
                while (SQL.Read())
                {
                    // Исключение: Неправильный пароль
                    if (SQL["Login"].ToString() == login && SQL["Password"].ToString() != pass)
                    {
                        str[0] = "false";
                        str[1] = "Неправильный пароль";
                        StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Соединение отклонено";
                        SQL.Close();
                        return str;
                    }
                    else
                    if (SQL["Login"].ToString() == login && SQL["Password"].ToString() == pass)
                    {
                        // UserID = Convert.ToInt32(SQL["ID_Пользователя"]);
                        // Если в столбце "Сотрудник" стоит "1", то вход будет под соответствующей учётной записи
                        //if (Convert.ToBoolean(SQL["Статус"]) == true)
                        //{
                        //    Status = true;
                        //}
                        str[0] = "true";
                        str[1] = "Успех";
                        StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Соединение одобрено";
                        SQL.Close();
                        return str;
                    }
                }
            }
            else
            {
                str[0] = "false";
                str[1] = "База данных пользователей пуста";
                StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Соединение отклонено ";
                SQL.Close();
                return str;
            }
            str[0] = "false";
            str[1] = "Пользователя с таким логином - не существует";
            StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Соединение отклонено ";
            SQL.Close();
            return str;
        }

        // Закрытие формы
        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            MethodKP();
            //bw2.CancelAsync();
            //KillConnect(CO.Last(), CO.Last().bw_s, 2);
        }
    }


    public class ClientObject
    {
        public TcpClient client;
        Server FS; // Ссылка на форму
        /// <summary>
        /// Поток данных
        /// </summary>
        NetworkStream stream = null;
        /// <summary>
        /// Ссылка на поток, в котором работает метод ConnectStream
        /// </summary>
        public BackgroundWorker bw_s;
        /// <summary>
        /// Поток данных для записи
        /// </summary>
        System.IO.BinaryWriter writer;
        /// <summary>
        /// Поток данных для чтения
        /// </summary>
        System.IO.BinaryReader reader;
        /// <summary>
        /// IP адресс подключившегося
        /// </summary>
        public string MYIpClient { get; set; }
        /// <summary>
        /// Состояние подключения
        /// </summary>
        public bool Connected { get; set; }
        
        public bool SecondConnect { get; set; }

        public bool Call2Discon { get; set; }

        public bool InProccesKill { get; set; }

        public string UserName { get; set; }

        public ClientObject(TcpClient tcpClient,Server FS)
        {
            InProccesKill = false;
            SecondConnect = false;
            client = tcpClient;
            this.FS = FS;
            MYIpClient = Convert.ToString(((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Address);
        }

        

        // Метод, в котором выполняется обработка сообщений от подключившегося
        public void ConnectStream()
        {
            try
            {
                stream = client.GetStream();
                writer = new System.IO.BinaryWriter(stream);
                reader = new System.IO.BinaryReader(stream);
                while (true)
                {
                    if (stream == null) // Завершение потока, если был запрос на разрыв соединения
                        break;

                    // получаем сообщение
                    string str = "";
                    do
                    {
                        try
                        {
                            if (SecondConnect == true)
                            {
                                writer.Write("У вас уже есть активное соединение!");
                                FS.StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Попытка создать 2 соединения: " + MYIpClient;
                                FS.KillConnect(this, bw_s, 1);
                                return;
                            }
                            if (Call2Discon == true)
                            {
                                return;
                            }
                            if (stream.DataAvailable)
                            {
                                str = reader.ReadString();
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            FS.StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Ошибка:" + ex.ToString();
                        }

                    } while (true);


                    // Разделяем str на слова
                    string[] lp = str.Split(' ');

                    // Ветвление кода на основе типа запроса из lp[0]
                    // lp[0] - первое слово в str, означающее тип запроса
                    string[] ServAnsw = new string[2];
                    switch (lp[0])
                    {
                        case "LoginZ": // Авторизация

                            ServAnsw = FS.CheckUser(lp[1], lp[2]); // Проверка введённых логина и пароля

                            // отправляем ответ
                            if (ServAnsw[0] == "true")
                            {
                                UserName = lp[1];
                                writer.Write(("(" + DateTime.Now.ToString() + ") " + ServAnsw[0] + ServAnsw[1]).ToString());
                                Connected = true;
                                FS.StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Успешное подключение: " + MYIpClient;
                            }
                            else
                            {
                                writer.Write(("(" + DateTime.Now.ToString() + ") " + ServAnsw[0] + ServAnsw[1]).ToString());
                                FS.StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Отклонено: " + MYIpClient;
                            }
                            break;
                        case "ChatZ":
                            string ChatZstr = ""; // Сообщение в чате
                            foreach (var item in FS.CO)
                            {
                                if (item.Connected == true)
                                {
                                    NetworkStream stream1 = item.client.GetStream();
                                    lp[0] = "";
                                    ChatZstr = String.Join(" ", lp);

                                    System.IO.BinaryWriter writer1 = new System.IO.BinaryWriter(stream1);
                                    writer1.Write(("(" + DateTime.Now.ToString() + ") " + ChatZstr).ToString());
                                }

                            }
                            FS.ChatTB.Text += "\r\n(" + DateTime.Now.ToString() + "): " + ChatZstr;
                            FS.ChatTB.SelectionStart = FS.ChatTB.Text.Length;
                            FS.ChatTB.ScrollToCaret();
                            break;
                        case "CloseZ": // Закрытие соединения
                                       //FS.StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") " + MYIpClient + " Завершил соединение";
                            Connected = false;
                            FS.KillConnect(this, bw_s, 3);
                            break;
                        default:
                            writer.Write("error");
                            FS.StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Отправлено сообщение об ошибке: ";
                            break;
                    }

                }
            }
            catch (SocketException ex)
            {
                Connected = false;
                FS.KillConnect(this, bw_s, 3);         
            }
            catch (Exception ex)
            {
                FS.StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Ошибка: " + ex.ToString()  + "\r\n Место ошибки: Метод ConnectStream";
            }
        }

        public void DisconectMes()
        {
            try
            {
                Call2Discon = true;
                try
                {
                    if (stream == null)
                        stream = client.GetStream();
                    if (writer == null)
                        writer = new System.IO.BinaryWriter(stream);
                    writer.Write("DisconnectZ Вы отключены от сервера");
                    writer.Close();
                }
                catch (Exception)
                {

                }

                FS.StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Завершил соединение: " + MYIpClient;
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
                if (client != null)
                {
                    client.Close();
                    client = null;
                }
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
            }
            catch (Exception ex)
            {
                FS.StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Ошибка: " + ex.ToString() + "\r\n Место ошибки: Метод DisconectMes";
            }
        }

        



    }
}
