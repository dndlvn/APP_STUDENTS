using APP_STUDENTS.classes;
using APP_STUDENTS.models;
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

    public partial class EditGradesWindow : Window
    {
        private int _studentId;

        public EditGradesWindow(int studentId, string studentName)
        {
            InitializeComponent();
            _studentId = studentId;
            TbTitle.Text = studentName;
            Load();
        }

        private void Load()
        {
            var subjects = DB.Context.Subject.ToList();
            var terms = DB.Context.Term.ToList();
            var list = new List<GradeRow>();

            foreach (var subj in subjects)
            {
                foreach (var term in terms)
                {
                    var g = DB.Context.Grade.FirstOrDefault(x => x.StudentId == _studentId && x.SubjectId == subj.SubjectId && x.TermId == term.TermId);
                    list.Add(new GradeRow
                    {
                        GradeId = g?.GradeId ?? 0,
                        SubjectId = subj.SubjectId,
                        SubjectName = subj.SubjectName,
                        TermId = term.TermId,
                        TermName = term.TermName,
                        GradeValue = g?.GradeValue ?? 0
                    });
                }
            }

            GridGrades.ItemsSource = list;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var rows = GridGrades.ItemsSource as List<GradeRow>;
            foreach (var r in rows)
            {
                if (r.GradeValue != 0 && (r.GradeValue < 2 || r.GradeValue > 5))
                {
                    MessageBox.Show("Оценка должна быть 2..5 или пусто (0 = нет оценки).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            foreach (var r in rows)
            {
                var g = DB.Context.Grade.FirstOrDefault(x => x.StudentId == _studentId && x.SubjectId == r.SubjectId && x.TermId == r.TermId);
                if (g == null)
                {
                    if (r.GradeValue != 0)
                    {
                        g = new Grade
                        {
                            StudentId = _studentId,
                            SubjectId = r.SubjectId,
                            TermId = r.TermId,
                            GradeValue = (byte)r.GradeValue
                        };
                        DB.Context.Grade.Add(g);
                    }
                }
                else
                {
                    if (r.GradeValue != 0)
                        g.GradeValue = (byte)r.GradeValue;
                    else
                        DB.Context.Grade.Remove(g);
                }
            }

            DB.Context.SaveChanges();
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
