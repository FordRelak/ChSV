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
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Server
{
    public partial class Server : Form
    {
        // Адрес сервера в локалке
        private const string server = "192.168.1.11";
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

            //
            // Процесс подключения к БД (1.1)
            //

            var exePath = AppDomain.CurrentDomain.BaseDirectory; // Путь до .exe файла.
            var path = System.IO.Path.Combine(exePath, "FT.db"); // Путь до .exe файла + название БД
            constr = $"Data Source={path};Version=3;"; // Строка подключения к БД

            // Подключение к БД
            DB = new SQLiteConnection(constr);
            DB.Open();

            //
            // Конец(1.1)
            //
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
                bw = new BackgroundWorker(); 
                bw.WorkerSupportsCancellation = true;
                bw.DoWork += (obj, ea) => Listner(); // Метод прослушки порта 8008
                bw.RunWorkerAsync(); // Открытие сервера
                StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Сервер запущен";
                CheckW = true;
            }
            else
            {
                bw.CancelAsync(); // Закрытие сервера
                StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Сервер закрыт";
                CheckW = false;
            }
        }

        void Listner()
        {
            //получаем адреса для запуска сокета
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(server), port);

            //создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {

                //связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(iPEndPoint);

                //начинам прослушивание
                listenSocket.Listen(10);

                Console.WriteLine("Сервер работает");

                while (!bw.CancellationPending)
                {
                    Socket handler = listenSocket.Accept();

                    //получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; //кол-во полученых байтов
                    byte[] data = new byte[256];
                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    } while (handler.Available > 0);
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
                                handler.Send(data);
                                StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Успешное подключение: " + handler.LocalEndPoint.ToString();
                            }
                            else
                            {
                                data = Encoding.Unicode.GetBytes((ServAnsw[0] + ServAnsw[1]).ToString());
                                handler.Send(data);
                                StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Отклонено: " + handler.LocalEndPoint.ToString();
                            }

                            break;
                        case "FileZ":





                            break;

                        default:
                            data = Encoding.Unicode.GetBytes("error");
                            handler.Send(data);
                            StatusBox.Text += "\r\n(" + DateTime.Now.ToString() + ") Отправлено сообщение об ошибке: " + handler.LocalEndPoint.ToString();
                            break;
                    }


                    //Делим строку на login и pass


                   
                    


                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Метод для определения правильности введённых данных для входа
        private string[] CheckUser(string login, string pass)
        {
            string[] str = new string[2];

            // Поиск логина/пароля
            CMD = DB.CreateCommand();
            CMD.CommandText = "SELECT * FROM user;";
            SQL = CMD.ExecuteReader();
            if (SQL.HasRows)
            {
                while (SQL.Read())
                {
                    // Исключение: Неправильный пароль
                    if (SQL["Login"].ToString() == login && SQL["Password"].ToString() != pass)
                    {
                        str[0] = "false";
                        str[1] = "Пользователя с таким логином - не существует";
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
            return str;
        }

        //
        //  Конец(2.1)
        //

    }
}
