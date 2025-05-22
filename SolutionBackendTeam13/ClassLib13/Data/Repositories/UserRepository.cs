using ClassLib13.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib13.Data.Repositories
{
    internal class UserRepository
    {
        static UserRepository()
        {
            UserList = new List<User>();
            
        }
        public static List<User> UserList { get; set; }
     
    }
}