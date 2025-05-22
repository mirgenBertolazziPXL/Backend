using ClassLib13.Business.Entities;
using ClassLib13.Data.Framework;
using ClassLib13.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib13.Data
{
    public class UserLogData : SqlServer
    {
        private readonly Dictionary<string, string[]> _groups = new Dictionary<string, string[]>() {
            { "year", new string[]{", YEAR(logdate) 'Year' FROM UserLogs", " GROUP BY YEAR(logdate)" } }
            , { "month", new string[] {"SELECT COUNT(LogDate) 'UserLogs'", ", CONCAT(YEAR(logdate),'-',MONTH(LogDate)) 'Month' FROM UserLogs", " GROUP BY CONCAT(YEAR(logdate),'-',MONTH(LogDate))" } }
            , { "week", new string[] {", DATEPART(WEEK, LogDate) 'Week', DATEADD(DAY, 2 - DATEPART(WEEKDAY, LogDate), CAST(LogDate AS DATE)) 'StartOfWeek', DATEADD(DAY, 8 - DATEPART(WEEKDAY, LogDate), CAST(LogDate AS DATE)) 'EndOfWeek' FROM UserLogs", " GROUP BY DATEPART(WEEK, LogDate), DATEADD(DAY, 2 - DATEPART(WEEKDAY, LogDate), CAST(LogDate as Date)), DATEADD(DAY, 8 - DATEPART(WEEKDAY, LogDate), CAST(LogDate AS DATE));" } }
            , { "day", new string[] {" ,LogDate FROM UserLogs"," GROUP BY LogDate" } }
        };
        
        public SelectResult SelectLogs(List<int> filterIds, string groupBy)
        {
            string[] group;
            group = _groups["day"];
            if (!string.IsNullOrWhiteSpace(groupBy))
            {
                if (_groups.ContainsKey(groupBy?.ToLower()))
                {
                    group = _groups[groupBy.ToLower()];
                }
            }

            SqlCommand selectCommand = new SqlCommand();
            string sqlQuery = $"SELECT COUNT(LogDate) 'UserLogs'{group[0]}";

            filterIds.RemoveAll(f => f == 0 || f == null);
            if (filterIds.Count > 0)
            {
                List<string> conditionsList = new List<string>();

                foreach (int id in filterIds)
                {
                    conditionsList.Add($"id = {id}");
                }

                if (conditionsList.Any())
                {
                    sqlQuery += " WHERE " + string.Join(" OR ", conditionsList);
                }

            }
            sqlQuery += group[1];
            selectCommand.CommandText = sqlQuery;
            return base.Select(selectCommand);
        }
        public SelectResult SelectLastLogs()
        {
            SqlCommand selectCommand = new SqlCommand();
            string sqlQuery = $"SELECT UserId, MAX(LogDate) 'LogDate' FROM UserLogs GROUP BY UserId";
            selectCommand.CommandText = sqlQuery;
            return base.Select(selectCommand);
        }
    }
}
