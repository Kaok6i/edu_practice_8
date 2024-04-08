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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LangSchool.bd;
using LangSchool.pages;

namespace LangSchool
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Service[] history;
        private int totalCount;
        private int currentCount;
        public MainWindow()
        {
            InitializeComponent();
            BaseService.ItemsSource = App.serviceEntities.Services.ToList();
            RefreshWindow();
        }

        /// <summary>
        /// Используется для фильтрации по скидке
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        private Service[] FilteringDiscount(Service[] history)//Для фильтрации по скидке
        {
            try
            {
                switch(cbFiltering.SelectedIndex)
                {
                    case 0:
                        history = history.ToArray();
                        break;

                        case 1:
                        history = history.Where(x=>x.Discount >=0 & x.Discount <=5).ToArray(); 
                        break;

                        case 2:
                        history = history.Where(x => x.Discount >= 5 & x.Discount <= 15).ToArray();
                        break;

                        case 3:
                        history = history.Where(x => x.Discount >= 15 & x.Discount <= 30).ToArray();
                        break;

                        case 4:
                        history = history.Where(x => x.Discount >= 30 & x.Discount <= 70).ToArray();
                        break;

                        case 5:
                        history = history.Where(x => x.Discount >= 70 & x.Discount <= 100).ToArray();
                        break;
                }
            }
            catch { }
            return history;
        }

        /// <summary>
        /// Используется для сортировки по стоимости
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        private Service[] SortingCost(Service[] history)//Для сортировке по стоимости
        {
            try
            {
                switch (cbSorting.SelectedIndex)
                {
                    case 0:
                        history = history.ToArray();
                        break;

                    case 1:
                        history = history.OrderBy(x => x.Cost).ToArray(); //по возрастающей
                        break;

                    case 2:
                        history = history.OrderByDescending(x => x.Cost).ToArray(); //по уменьшению
                        break;
                }
            }
            catch { }
            return history;
        }

        /// <summary>
        /// Используется для работы Поиска
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private Service[] SearchProduct(Service[] services)//поиск
        {
            try
            {
                if (tbSearch.Text != null)
                {
                    services = services.Where(c => c.Title.ToLower().Contains(tbSearch.Text.ToLower()) |
                    c.Description.ToLower().Contains(tbSearch.Text.ToLower())).ToArray();
                }
                if (services.Length == 0)
                {
                    BaseService.ItemsSource = services;
                    MessageBox.Show("Товаров с таким данными не найдено");
                }
            }
            catch { }

            return services;
        }

        /// <summary>
        /// Для общей работы Фильтрации, Сортировки, Поиска
        /// </summary>
        private void RefreshWindow()//Метод для вывода всех данных 
        {

            history = App.serviceEntities.Services.ToArray();
            history = SortingCost(history);
            history = FilteringDiscount(history);
            history = SearchProduct(history);
            BaseService.ItemsSource = history.ToList();
            totalCount = App.serviceEntities.Services.Count();
            currentCount = history.Count();
            tbCountPost.Text = Convert.ToString(totalCount) + " / " + Convert.ToString(totalCount);
            tbCountPost.Text = Convert.ToString(currentCount) + " / " + Convert.ToString(totalCount);

        }


        public int checkActiveAdim = 0;//Проверка: включен ли режим Админа?

        private void ActiveAdmin_Click(object sender, RoutedEventArgs e)//Активация админ мода
        {
            try
            {
                var sessiaAdmin = new ForActiveAdmin();
                sessiaAdmin.ShowDialog();
                if (sessiaAdmin.key == 1)//Если пароль введён вверно, то показываем кнопки 
                {
                    checkActiveAdim = 1;
                    if (checkActiveAdim == 1)
                    {
                        AddService.Visibility = Visibility.Visible;
                        FormClientsService.Visibility = Visibility.Visible;
                        Style style = new Style//Обращаемся к стилю
                        {
                            TargetType = typeof(Grid)
                        };
                        style.Setters.Add(new Setter(Grid.VisibilityProperty, Visibility.Visible));
                        Application.Current.Resources["AdminMode"] = style;
                    }
                }
                else
                {
                    if (checkActiveAdim == 1)//Выключить режим Администратора
                    {
                        checkActiveAdim = 0;
                        if (checkActiveAdim == 0)
                        {
                            AddService.Visibility = Visibility.Hidden;
                            FormClientsService.Visibility = Visibility.Hidden;
                            Style style = new Style
                            {
                                TargetType = typeof(Grid)
                            };
                            style.Setters.Add(new Setter(Grid.VisibilityProperty, Visibility.Hidden));
                            Application.Current.Resources["AdminMode"] = style;
                        }
                    }
                }
            }
            catch { }
        }

        private void EditService_Click(object sender, RoutedEventArgs e)//Редактирование услуги
        {
            try
            {
                var row = BaseService.SelectedItem as bd.Service;
                if (row == null)
                {
                    MessageBox.Show("Строка не выбрана для редактирования!");
                }
                else
                {
                    ServicesEdit servicesEdit = new ServicesEdit(row);
                    servicesEdit.Show();
                    this.Close();
                }
            }
            catch { }
        }

        private void DeleteService_Click(object sender, RoutedEventArgs e)//Удалние услуги
        {
            try
            {
                var row = BaseService.SelectedItem as bd.Service;
                var idForClientsServiceSerche = App.serviceEntities.ClientServices.FirstOrDefault(x => x.ServiceID == row.idServices);
                if (row == null)
                {
                    MessageBox.Show("Строка не выбрана для удаления!");
                }
                else
                {
                    if (idForClientsServiceSerche == null)
                    {
                        MessageBoxResult messageBox = MessageBox.Show("Вы уверены, что хотите удалить данную строку?", "Предупреждение!",
                       MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (messageBox == MessageBoxResult.Yes)
                        {
                            App.serviceEntities.Services.Remove(row);
                            App.serviceEntities.SaveChanges();
                            MessageBox.Show("Строка успешно удалена!");
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.checkActiveAdim = 1;
                            mainWindow.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Данная запись не может быть удалена!\n" +
                            "По данной услуге существует запись!");
                    }
                }
            }
            catch { }
        }

        private void AddService_Click(object sender, RoutedEventArgs e)//Добавление услуги
        {
            ServicesAdd servicesAdd = new ServicesAdd();
            servicesAdd.Show();
            this.Close();
        }

        private void FormClientsService_Click(object sender, RoutedEventArgs e)
        {
            WindowClientsServices windowClientsServices = new WindowClientsServices();
            windowClientsServices.Show();
            this.Close();
        }

        private void tbSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            RefreshWindow();
        }

        private void cbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshWindow();
        }

        private void cbFiltering_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshWindow();
        }
    }
}
