using APP_STUDENTS.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace APP_STUDENTS.classes
{
    internal class DB
    {
        public static StudentsDBEntities Context { get; set; } = new StudentsDBEntities();
    }
}
