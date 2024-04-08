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
    /// Логика взаимодействия для ForActiveAdmin.xaml
    /// </summary>
    public partial class ForActiveAdmin : Window
    {
        public ForActiveAdmin()
        {
            InitializeComponent();
        }
        public int key;//переменная для передачи результатов проверки

        private void Search_Click(object sender, RoutedEventArgs e)//Проверка пароля
        {
            try
            {
                if (tbPasswordAdmin.Text == "0000")
                {
                    key = 1;
                    MessageBox.Show("Режим администратора активирован!");
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Неверный пароль!");
                    key = 0;
                }
            }
            catch { }
        }

        private void tbPasswordAdmin_PreviewKeyDown(object sender, KeyEventArgs e)//Зарпет на ввод минуса и пробела
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
