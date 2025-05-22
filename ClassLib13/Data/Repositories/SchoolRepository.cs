using ClassLib13.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib13.Data.Repositories
{
    internal class SchoolRepository
    {
        static SchoolRepository()
        {
            SchoolList = new List<School>();
        }

        public static List<School> SchoolList { get; set; }

        public static void Add(string name, string sub, int userid)
        {
            School school = new School
            {
                SchoolId = GetId(),
                Subscription = sub,
                Name = name,
                UserId = userid
            };

            Add(school);
        }

        public static int GetId()
        {
            return SchoolList.Count > 0 ? SchoolList.Max(x => x.SchoolId) + 1 : 1;
        }

        private static void Add(School school)
        {
            SchoolList.Add(school);
        }
    }
}