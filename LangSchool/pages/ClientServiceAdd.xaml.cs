using LangSchool.bd;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LangSchool.pages
{
    /// <summary>
    /// Логика взаимодействия для ClientServiceAdd.xaml
    /// </summary>
    public partial class ClientServiceAdd : Window
    {
        public ClientServiceAdd()
        {
            InitializeComponent();
            cbClient.ItemsSource = App.serviceEntities.Clients.Select(x => x.FirstName + " " + x.LastName + " "
                + x.Patronymic).ToList();
            cbServices.ItemsSource = App.serviceEntities.Services.Select(x=>x.Title).ToList();
            dpDateStart.DisplayDateStart = DateTime.Now;
        }

        private void Add_Click(object sender, RoutedEventArgs e)//Добавлеине
        {
            try
            {
                if (cbClient.Text == null | cbServices.Text == null | dpDateStart.Text == null | tbTimeStart.Text == null)
                {
                    MessageBox.Show("Заполните все поля!");
                }
                else
                {
                    var regex = new Regex(@"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
                    if (!regex.IsMatch(tbTimeStart.Text))
                    {
                        MessageBox.Show("Введите время в формате (HH:MM)!",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        int forIDClient = App.serviceEntities.Clients.Where(x => x.FirstName + " " + x.LastName + " "
                        + x.Patronymic == cbClient.Text).Select(x => x.idClient).FirstOrDefault();

                        int forIDService = App.serviceEntities.Services.Where(x => x.Title == cbServices.Text)
                            .Select(x => x.idServices).FirstOrDefault();

                        string fullTime = dpDateStart.Text + " " + tbTimeStart.Text;

                        bd.ClientService clientService = new bd.ClientService()
                        {
                            ClientID = forIDClient,
                            ServiceID = forIDService,
                            StartTime = Convert.ToDateTime(fullTime)
                        };
                        App.serviceEntities.ClientServices.Add(clientService);
                        App.serviceEntities.SaveChanges();
                        MessageBox.Show("Данные успешно добавлены!");
                        WindowClientsServices windowClientsServices = new WindowClientsServices();
                        windowClientsServices.Show();
                        this.Close();
                    }
                }
            }
            catch 
            {
                MessageBox.Show("Одна из строк имела неверный формат!");
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)//Выход
        {
            WindowClientsServices windowClientsServices = new WindowClientsServices();
            windowClientsServices.Show();
            this.Close();
        }

        private void tbTimeStart_KeyDown(object sender, KeyEventArgs e)//Запрет на ввод букв
        {
            if ((e.Key < Key.NumPad0 || e.Key > Key.NumPad9) && (e.Key < Key.D0 || e.Key > Key.D9))
            {
                e.Handled = true;
            }
        }

        private void tbTimeStart_PreviewKeyDown(object sender, KeyEventArgs e)//Запрет на ввод минуса и пробела
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            if (e.Key == Key.OemMinus)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Subtract)
            {
                e.Handled = true;
            }
        }
    }
}
