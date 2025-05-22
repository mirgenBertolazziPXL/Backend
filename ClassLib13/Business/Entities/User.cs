using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib13.Business.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public int? SchoolId { get; set; }
        public int? ParentId { get; set; }

    }
}
