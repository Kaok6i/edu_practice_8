using LangSchool.bd;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting.Channels;
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
    /// Логика взаимодействия для ServicesEdit.xaml
    /// </summary>
    public partial class ServicesEdit : Window
    {
        bd.Service service;
        public ServicesEdit(bd.Service service)
        {
            InitializeComponent();
            this.service = service;
            this.DataContext = service;
            identity = service.idServices;
        }
        int identity;
        private void Edit_Click(object sender, RoutedEventArgs e)//Редактировать 
        {
            try
            {
                if (tbTitle.Text == "" | tbCost.Text == "" | tbDuration.Text == "" | tbDescription.Text == "" | tbDiscount.Text == "" | Images.Source == null)
                {
                    MessageBox.Show("Заполните все поля!");
                }
                else
                {
                    if (Convert.ToInt32(tbDuration.Text) <= 240)
                    {
                        var services = App.serviceEntities.Services.FirstOrDefault(x => x.idServices == identity);
                        try
                        {
                            services.Title = tbTitle.Text;
                            services.Cost = Convert.ToDecimal(tbCost.Text);
                            services.DurationInSeconds = Convert.ToInt32(tbDuration.Text);
                            services.Description = tbDescription.Text;
                            services.Discount = Convert.ToInt32(tbDiscount.Text);
                            services.Photo = ForImages();
                        }
                        catch { }
                        App.serviceEntities.SaveChanges();
                        MessageBox.Show("Данные успешно изменены!");
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.checkActiveAdim = 1;
                        mainWindow.AddService.Visibility = Visibility.Visible;
                        mainWindow.FormClientsService.Visibility = Visibility.Visible;
                        mainWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Услуга не может превышать 4 часов!");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Одна из стролк имела неверный формат!");
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

        private void tbCost_KeyDown(object sender, KeyEventArgs e)//Запрет на ввод букв
        {
            if (e.Key < Key.NumPad0 || e.Key > Key.NumPad9)
            {
                e.Handled = true;
            }
        }

        private void tbCost_PreviewKeyDown(object sender, KeyEventArgs e)//запрет на ввод минуса и пробела
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

        string fileName;
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

        private void btEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.ShowDialog();//Открытие проводника
                fileName = openFileDialog.FileName;//передаём в нашу глобавльную переменную путь + название
                Images.Source = new BitmapImage(new Uri("" + fileName));
                ForImages();
            }
            catch
            {
                MessageBox.Show("Данные введены некорректно!");
            }
        }
    }
}
