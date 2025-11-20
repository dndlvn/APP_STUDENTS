using APP_STUDENTS.classes;
using APP_STUDENTS.models;
using APP_STUDENTS.windows;
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

namespace APP_STUDENTS
{
    public partial class MainWindow : Window
    {
        List<StudentGradesRow> rows = new List<StudentGradesRow>();

        public MainWindow()
        {
            InitializeComponent();
            RefreshGrid();
            loadSortFields();
        }


        private void RefreshGrid()
        {
            rows = DB.Context.Student
                .Select(s => new StudentGradesRow
                {
                    StudentId = s.StudentId,
                    StudentName = s.LastName + " " + s.FirstName,
                    Inf1 = DB.Context.Grade.Where(g => g.StudentId == s.StudentId && g.SubjectId == 1 && g.TermId == 1).Select(g => (int?)g.GradeValue).FirstOrDefault(),
                    Inf2 = DB.Context.Grade.Where(g => g.StudentId == s.StudentId && g.SubjectId == 1 && g.TermId == 2).Select(g => (int?)g.GradeValue).FirstOrDefault(),
                    Math1 = DB.Context.Grade.Where(g => g.StudentId == s.StudentId && g.SubjectId == 2 && g.TermId == 1).Select(g => (int?)g.GradeValue).FirstOrDefault(),
                    Math2 = DB.Context.Grade.Where(g => g.StudentId == s.StudentId && g.SubjectId == 2 && g.TermId == 2).Select(g => (int?)g.GradeValue).FirstOrDefault()
                })
                .ToList();


            GridStudents.ItemsSource = rows;
        }

        private void Row_Edit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var row = button?.DataContext as StudentGradesRow;
            if (row == null) return;

            var st = DB.Context.Student.Find(row.StudentId);
            if (st == null) return;

            var dlg = new StudentEditWindow(st.LastName, st.FirstName, st.MiddleName);
            if (dlg.ShowDialog() == true)
            {
                st.LastName = dlg.LastName;
                st.FirstName = dlg.FirstName;
                st.MiddleName = dlg.MiddleName;
                DB.Context.SaveChanges();
                RefreshGrid();
            }
        }

        private void Row_Delete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var row = button?.DataContext as StudentGradesRow;
            if (row == null) return;

            if (MessageBox.Show($"Удалить студента {row.StudentName}?", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var st = DB.Context.Student.Find(row.StudentId);
                if (st != null)
                {
                    DB.Context.Student.Remove(st);
                    DB.Context.SaveChanges();
                    RefreshGrid();
                }
            }
        }

        private void Row_EditGrades_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var row = button?.DataContext as StudentGradesRow;
            if (row == null) return;

            var dlg = new EditGradesWindow(row.StudentId, row.StudentName);
            if (dlg.ShowDialog() == true)
            {
                RefreshGrid();
            }
        }

        private void BtnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new StudentEditWindow();
            if (dlg.ShowDialog() == true)
            {
                var s = new Student
                {
                    LastName = dlg.LastName,
                    FirstName = dlg.FirstName,
                    MiddleName = dlg.MiddleName
                };
                DB.Context.Student.Add(s);
                DB.Context.SaveChanges();
                RefreshGrid();
            }
        }
        private void loadSortFields()
        {
            cmbFields.Items.Add("Ученик");
            cmbFields.Items.Add("Инф 1ч");
            cmbFields.Items.Add("Инф 2ч");
            cmbFields.Items.Add("Мат 1ч");
            cmbFields.Items.Add("Мат 2ч");
            cmbFields.SelectedIndex = 0;
        }
        private void btnSort_Click(object sender, RoutedEventArgs e)
        {
            bool up = rbUp.IsChecked == true;

            if (cmbFields.SelectedIndex == 0) 
                rows = up ? rows.OrderBy(u => u.StudentName).ToList()
                                 : rows.OrderByDescending(u => u.StudentName).ToList();
            else if (cmbFields.SelectedIndex == 1) 
                rows = up ? rows.OrderBy(u => u.Inf1 ?? 0).ToList()
                                 : rows.OrderByDescending(u => u.Inf1 ?? 0).ToList();
            else if (cmbFields.SelectedIndex == 2) 
                rows = up ? rows.OrderBy(u => u.Inf2 ?? 0).ToList()
                                 : rows.OrderByDescending(u => u.Inf2 ?? 0).ToList();
            else if (cmbFields.SelectedIndex == 3) 
                rows = up ? rows.OrderBy(u => u.Math1 ?? 0).ToList()
                                 : rows.OrderByDescending(u => u.Math1 ?? 0).ToList();
            else if (cmbFields.SelectedIndex == 4)
                rows = up ? rows.OrderBy(u => u.Math2 ?? 0).ToList()
                                 : rows.OrderByDescending(u => u.Math2 ?? 0).ToList();

            GridStudents.ItemsSource = rows;
        }

        private void cbFilter_Checked(object sender, RoutedEventArgs e)
        {
            if (cbFilter.IsChecked == true)
            {
                txtFilterWord.Visibility = Visibility.Visible;
                tbfilter.Visibility = Visibility.Visible;
            }
            else
            {
                txtFilterWord.Visibility = Visibility.Collapsed;
                tbfilter.Visibility = Visibility.Collapsed;

            }

        }
        private void txtFilterWord_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (cbFilter.IsChecked == true)
            {
                GridStudents.ItemsSource = DB.Context.Student
                    .Select(s => new StudentGradesRow
                    {
                        StudentId = s.StudentId,
                        StudentName = s.LastName + " " + s.FirstName
                    })
                    .Where(s => string.IsNullOrEmpty(txtFilterWord.Text) || s.StudentName.StartsWith(txtFilterWord.Text))
                    .ToList();


            }
        }

    }
    
}
