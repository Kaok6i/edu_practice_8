using Microsoft.Win32;
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
using static System.Net.Mime.MediaTypeNames;

namespace LangSchool.pages
{
    /// <summary>
    /// Логика взаимодействия для ServicesAdd.xaml
    /// </summary>
    public partial class ServicesAdd : Window
    {
        string fileName;//для пути фотографии
        public ServicesAdd()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)//Добавление
        {
            try
            {
                if (tbTitle.Text == "" | tbCost.Text == "" | tbDuration.Text == "" | tbDescription.Text == "" | tbDiscount.Text == "" | Images.Source == null)
                {
                    MessageBox.Show("Заполните все поля!");
                }
                else
                {
                    if (Convert.ToInt32(tbDuration.Text) <=240)
                    {
                        var forSearchDoubleName = App.serviceEntities.Services.Where(x => x.Title == tbTitle.Text).FirstOrDefault();
                        if (forSearchDoubleName == null)
                        {
                            bd.Service service = new bd.Service()
                            {
                                Title = tbTitle.Text,
                                Cost = Convert.ToDecimal(tbCost.Text),
                                DurationInSeconds = Convert.ToInt32(tbDuration.Text),
                                Description = tbDescription.Text,
                                Discount = Convert.ToInt32(tbDiscount.Text),
                                Photo = ForImages()
                            };
                            App.serviceEntities.Services.Add(service);
                            App.serviceEntities.SaveChanges();
                            MessageBox.Show("Данные успешно добавлены!");
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.checkActiveAdim = 1;
                            mainWindow.AddService.Visibility = Visibility.Visible;
                            mainWindow.FormClientsService.Visibility = Visibility.Visible;
                            mainWindow.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Услуга с таким названием уже сущестует!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Услуга не может превышать 4 часов!");
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
            MainWindow mainWindow = new MainWindow();
            mainWindow.checkActiveAdim = 1;
            mainWindow.AddService.Visibility = Visibility.Visible;
            mainWindow.FormClientsService.Visibility = Visibility.Visible;
            mainWindow.Show();
            this.Close();
        }

        private void tbDuration_KeyDown(object sender, KeyEventArgs e)//Запрет на ввод букв
        {
            if (e.Key < Key.NumPad0 || e.Key > Key.NumPad9)
            {
                e.Handled = true;
            }
            
        }

        private void tbDuration_PreviewKeyDown(object sender, KeyEventArgs e)//Запрет на ввод минуса и пробела
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            if(e.Key == Key.OemMinus)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Subtract)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Для работы с фотографиями
        /// </summary>
        /// <returns></returns>
        private byte[] ForImages()
        {
            string path = fileName;//Содержит путь к картинкам + их названия
            byte[] imageInBytes = System.IO.File.ReadAllBytes(path);//Перевод в массив байт
            return imageInBytes;
        }

        private void btEdit_Click(object sender, RoutedEventArgs e)//Добавление фотки и её изменение
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.ShowDialog();//Открытие проводника
                fileName = openFileDialog.FileName;//Передаём в нашу глобавльную переменную путь + название
                Images.Source = new BitmapImage(new Uri("" + fileName));
                ForImages();
            }
            catch
            { }
        }
    }
}
