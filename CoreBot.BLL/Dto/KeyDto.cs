using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBot.BLL.Dto
{
    public class KeyDto
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string KeyValue { get; set; }
        public bool IsBought { get; set; }
    }

}
