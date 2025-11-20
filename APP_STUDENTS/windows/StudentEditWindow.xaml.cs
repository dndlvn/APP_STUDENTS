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

namespace APP_STUDENTS.windows
{
    public partial class StudentEditWindow : Window
    {
        public string LastName => TbLast.Text.Trim();
        public string FirstName => TbFirst.Text.Trim();
        public string MiddleName => TbMiddle.Text.Trim();

        public StudentEditWindow(string last = "", string first = "", string middle = "")
        {
            InitializeComponent();
            TbLast.Text = last;
            TbFirst.Text = first;
            TbMiddle.Text = middle;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (LastName.Length > 100 || FirstName.Length > 100 || MiddleName.Length > 100)
            {
                MessageBox.Show("Длина фамилии, имени или отчества не должна превышать 50 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(FirstName))
            {
                MessageBox.Show("Заполните фамилию и имя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
