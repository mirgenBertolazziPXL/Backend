using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib13.Business.Entities
{
    public class Progression
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LearnPathId { get; set; }
        public int Percentage { get; set; }
    }
}
