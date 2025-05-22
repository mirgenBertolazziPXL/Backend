using ClassLib13.Business.Entities;
using ClassLib13.Data.Framework;
using ClassLib13.Data.Repositories;
using ClassLib13.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ClassLib13.Business
{
    public static class Users
    {
        public static IEnumerable<User> List()
        {
            return UserRepository.UserList;
        }

        public static SelectResult GetUsers()
        {
            UserData userData = new UserData();
            SelectResult result = userData.Select();
            return result;
        }

        public static InsertResult Add(string email, string password,int roleId, string firstName, string lastName, int schoolId, int parentId, UserData userData)
        {
            User user = new User();
            { 
            user.Email = email;
            user.Password = password;
            user.RoleId = roleId;
            user.Firstname = firstName;
            user.LastName = lastName;
            user.SchoolId = schoolId;
            user.ParentId = parentId;
        };
            InsertResult result = userData.Insert(user);
            return result;
        }
        public static InsertResult AddAdmin(string email, string password, string firstName, string lastName, UserData userData)
        {
            User user = new User();
            {
                user.Email = email;
                user.Password = password;
                user.RoleId = 5;
                user.Firstname = firstName;
                user.LastName = lastName;
                user.SchoolId = null;
                user.ParentId = null;
            };
            InsertResult result = userData.Insert(user);
            return result;
        }
    }
}
