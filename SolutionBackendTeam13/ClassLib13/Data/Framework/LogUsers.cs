using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLib13.Data;

namespace ClassLib13.Data.Framework
{
    public class LogUsers
    {
        public void UpdateLogs(string email)
        {
            UserData userdata = new UserData();
            _ = userdata.LogUserMethod(email);
        }
    }
}
