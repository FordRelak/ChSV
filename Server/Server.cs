/*
 WorkHoursCounter: 2 
  
 Заметки
 
 Список команд для сервера
 1) LoginZ - Запрос на авторизацию
 2) FileZ - Запрос на скачивание файла

 Конструкция запроса:
 1) <Тип запроса> <логин> <пароль>
 2) <Тип запроса> <Путь к файлу> (В доработке)

 Надо реализовать:
 1) Потоки через List
 2) Switch для запросов



*/
using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Data;
using System.Data.SQLite; // SQLITE

using System.Drawing;
using System.Linq;

using System.Net;
using System.Net.Sockets;

using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Server
{
    public partial class Server : Form
    {
        // Адрес сервера в локалке
        private const string IP_server = "192.168.1.11";
        private const int port = 8008;

        // SQLITE
        SQLiteConnection DB; // Путь к БД
        SQLiteDataReader SQL; // Переменная для чтения из БД
        SQLiteCommand CMD; // Переменная для исполенения команд
        string constr; // Строка подключения к БД

        DataTable dt; // Переменная таблицы

        BackgroundWorker bw; // Переменная для потока

        bool CheckW = false; // Проверка активности сервера

        public Server()
        {
            InitializeComponent();

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
        }

        //
        //  Код, отвечающий за запуск сервера и его работу (2.1)
        //

        private void StartServer_Click(object sender, EventArgs e)
        {
            // Смена названия кнопки
            if ((sender as Button).Text == "Запустить сервер")
                (sender as Button).Text = "Закрыть сервер";
            else
                (sender as Button).Text = "Запустить сервер";

            // Запуск/Закрытие сервера
            if (CheckW == false)
            {
               
                server.Start();
                StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Сервер запущен";
                CheckW = true;
            }
            else
            {
                //bw.CancelAsync(); // Закрытие сервера
                server.Stop();
                StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Сервер закрыт";
                CheckW = false;
            }
        }

        IPAddress IPAddress;
        TcpListener server = null;

        void Listner()
        {

            IPAddress = IPAddress.Parse(IP_server);
            server = new TcpListener(IPAddress, port);

            while(!CheckW)
            {
                
            }

            try
            {
                Console.WriteLine("Сервер работает");

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();

                    new Thread(() =>
                       {
                           NetworkStream stream = null;

                           BeginInvoke((MethodInvoker)(() => 
                           {
                               try
                               {
                                   stream = client.GetStream();
                                   byte[] data = new byte[64]; // буфер для получаемых данных

                                   // получаем сообщение
                                   StringBuilder builder = new StringBuilder();
                                   int bytes = 0;
                                   do
                                   {
                                       bytes = stream.Read(data, 0, data.Length);
                                       builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                                   }
                                   while (stream.DataAvailable);
                                   string str = builder.ToString(); // Итоговая строка

                                   // Разделяем str на слова
                                   string[] lp = str.Split(' ');

                                   // Ветвление кода на основе типа запроса из lp[0]
                                   // lp[0] - первое слово в str, означающее тип запроса

                                   switch (lp[0])
                                   {
                                       case "LoginZ": // Авторизация
                                           string[] ServAnsw = new string[2];
                                           ServAnsw = CheckUser(lp[1], lp[2]);

                                           // отправляем ответ
                                           if (ServAnsw[0] == "true")
                                           {
                                               data = Encoding.Unicode.GetBytes((ServAnsw[0] + ServAnsw[1]).ToString());
                                               stream.Write(data, 0, data.Length);
                                               StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Успешное подключение: ";
                                           }
                                           else
                                           {
                                               data = Encoding.Unicode.GetBytes((ServAnsw[0] + ServAnsw[1]).ToString());
                                               stream.Write(data, 0, data.Length);
                                               StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Отклонено: ";
                                           }

                                           break;
                                       case "FileZ":

                                           break;
                                       case "PingZ":
                                           StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Пинг от: ";

                                           break;
                                       default:
                                           data = Encoding.Unicode.GetBytes("error");
                                           stream.Write(data, 0, data.Length);
                                           StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Отправлено сообщение об ошибке: ";
                                           break;



                                   }
                               }
                               catch (Exception ex)
                               {
                                   StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Ошибка:" + ex.Message;
                               }
                               finally
                               {
                                   if (stream != null)
                                       stream.Close();
                                   if (client != null)
                                       client.Close();
                               }
                           }));
                       }).Start();
                }
          
            }
            catch (Exception ex)
            {
                StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Ошибка:" + ex.Message;
            }
        }

        // Метод для определения правильности введённых данных для входа
        protected string[] CheckUser(string login, string pass)
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

        //
        //  Конец(2.1)
        //

    }


    public class ClientObject
    {
        public TcpClient client;
        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
        }

        public void Process()
        {
            
        }
    }
}
