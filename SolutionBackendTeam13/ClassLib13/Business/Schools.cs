using ClassLib13.Business.Entities;
using ClassLib13.Data.Framework;
using ClassLib13.Data.Repositories;
using ClassLib13.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib13.Business
{
    public static class Schools
    {
        public static IEnumerable<School> List()
        {
            return SchoolRepository.SchoolList;
        }

        public static SelectResult GetSchools()
        {
            SchoolData schoolData = new SchoolData();
            return schoolData.Select();
        }

        public static InsertResult Add(string name, string subscription, int userid)
        {
            School school = new School
            {
                SchoolId = SchoolRepository.GetId(),
                Name = name,
                Subscription = subscription,
                UserId = userid
            };

            SchoolData schoolData = new SchoolData();
            return schoolData.Insert(school);
        }
    }
}
