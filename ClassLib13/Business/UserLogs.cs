using ClassLib13.Business.Entities.ProductController;
using ClassLib13.Data;
using ClassLib13.Data.Framework;
using ClassLib13.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLib13.Business.Entities;
using System.Data.SqlClient;
using System.Net;


namespace ClassLib13.Business
{
    public class UserLogs
    {
        public static SelectResult GetUserLogs(List<int> filterIds, UserLogData userLogData, string groupBy)
        {
            return userLogData.SelectLogs(filterIds, groupBy);
        }
        public static SelectResult GetLastUserLogs(UserLogData userLogData)
        {
            return userLogData.SelectLastLogs();
        }
    }
}
