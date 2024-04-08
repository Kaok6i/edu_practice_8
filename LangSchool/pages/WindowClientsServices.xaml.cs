using LangSchool.bd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для WindowClientsServices.xaml
    /// </summary>
    public partial class WindowClientsServices : Window
    {
        private ClientService[] history;
        private int totalCount;
        public WindowClientsServices()
        {
            InitializeComponent();
            BaseClientService.ItemsSource = App.serviceEntities.ClientServices.ToList();
            RefreshWindow();
        }

        /// <summary>
        /// Используется для сортировки по стоимости
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        private ClientService[] SortingCost(ClientService[] history)//Для сортировке по стоимости
        {
            try
            {
                switch (cbSorting.SelectedIndex)
                {
                    case 0:
                        history = history.ToArray();
                        break;

                    case 1:
                        history = history.OrderBy(x => x.StartTime).ToArray(); //по возрастающей
                        break;

                    case 2:
                        history = history.OrderByDescending(x => x.StartTime).ToArray(); //по уменьшению
                        break;
                }
            }
            catch { }
            return history;
        }

        private void RefreshWindow()//Метод для вывода всех данных 
        {
            history = App.serviceEntities.ClientServices.ToArray();
            history = SortingCost(history);
            BaseClientService.ItemsSource = history.ToList();
            totalCount = App.serviceEntities.Services.Count();
            tbCountPost.Text = Convert.ToString(totalCount) + " / " + Convert.ToString(totalCount);
        }

        private void AddClientService_Click(object sender, RoutedEventArgs e)//Добавить клиента на услугу
        {
            ClientServiceAdd clientServiceAdd = new ClientServiceAdd();
            clientServiceAdd.Show();
            this.Close();
        }

        private void FormFirtsWindow_Click(object sender, RoutedEventArgs e)//Выход
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.checkActiveAdim = 1;
            mainWindow.AddService.Visibility = Visibility.Visible;
            mainWindow.FormClientsService.Visibility = Visibility.Visible;
            mainWindow.Show();
            this.Close();
        }

        private void cbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshWindow();
        }
    }
}
