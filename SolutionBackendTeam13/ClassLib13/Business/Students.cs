using ClassLib13.Business.Entities;
using ClassLib13.Data.Framework;
using ClassLib13.Data;
using ClassLib13.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib13.Business
{
    public static class Students
    {
        public static IEnumerable<Student> List()
        {
            return StudentRepository.StudentList;
        }

        public static SelectResult GetStudents()
        {
            StudentData studentData = new StudentData();
            SelectResult result = studentData.Select();
            return result;
        }
        public static InsertResult Add(string firstName, string lastName)
        {
            Student student = new Student();
            student.FirstName = firstName;
            student.LastName = lastName;
            StudentData studentData = new StudentData();
            InsertResult result = studentData.Insert(student);
            return result;
        }
    }
}
