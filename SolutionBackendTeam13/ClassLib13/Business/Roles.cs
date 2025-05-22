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
    public static class Roles
    {
        public static IEnumerable<Role> List()
        {
            return RoleRepository.RoleList;
        }

        public static SelectResult GetRoles()
        {
            RoleData roleData = new RoleData();
            SelectResult result = roleData.Select();
            return result;
        }
        public static InsertResult Add(string name)
        {
            Role role = new Role
            {
                RoleId = RoleRepository.GetId(),
                Name = name,
               
            };

            RoleData roleData = new RoleData();
            return roleData.Insert(role);
        }
    }
}

