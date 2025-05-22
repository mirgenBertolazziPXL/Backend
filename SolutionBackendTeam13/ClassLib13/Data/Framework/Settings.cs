using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib13.Data.Framework
{
    static class Settings
    {
        public static string GetConnectionString()
        {
            //string connectionString = "Trusted_Connection=True;";
            string connectionString = "user id = pxluser;";
            connectionString += "Password = pxluser;";
            connectionString += $@"Server=5CG215052M\PXLDIGITAL;";
            connectionString += $"Database=db_kristof_palmaers";
            //Eigen server Mirgen
            //var conn1 = "Server=PC5G;Database=Test;User id=sa;Password=pxl";
             var conn1 = "Server=10.128.4.7;Database=Db2025Team_13;User id=PxlUser_13;Password=SamApp13";
            return conn1;
        }
    }
}