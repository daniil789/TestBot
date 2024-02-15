using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBot.BLL.Dto
{
    public class OrderDto
    {
        public string UserId { get; set; }
        public int GameId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
