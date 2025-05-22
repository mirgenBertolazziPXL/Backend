﻿using ClassLib13.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib13.Data.Repositories
{
    static class StudentRepository
    {
        static StudentRepository()
        {
            StudentList = new List<Student>();
        }
        public static List<Student> StudentList { get; set; }
        public static void Add(string firstName, string lastName)
        {
            Student student = new Student();
            student.StudentId = GetId();
            student.FirstName = firstName;
            student.LastName = lastName;
            Add(student);
        }
        private static int GetId()
        {
            int id = 1;
            if (StudentList.Count > 0)
                id = StudentList.Max(x => x.StudentId) + 1;
            return id;
        }
        private static void Add(Student student)
        {
            StudentList.Add(student);
        }
       
    }
}
