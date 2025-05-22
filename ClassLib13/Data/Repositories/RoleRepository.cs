using ClassLib13.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib13.Data.Repositories
{
    internal class RoleRepository
    {
        static RoleRepository()
        {
            RoleList = new List<Role>();
        }

        public static List<Role> RoleList { get; set; }

        public static void Add(string name)
        {
            Role role = new Role
            {
                RoleId = GetId(),
                Name = name    
            };

            Add(role);
        }

        public static int GetId()
        {
            return RoleList.Count > 0 ? RoleList.Max(x => x.RoleId) + 1 : 1;
        }

        private static void Add(Role role)
        {
            RoleList.Add(role);
        }
    }
}