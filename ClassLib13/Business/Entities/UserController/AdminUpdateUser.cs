using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib13.Business.Entities.UserController
{
    public class AdminUpdateUser
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Role { get; set; }
        public int ParentId { get; set; }
        public int UserId { get; set; }
    }
}
