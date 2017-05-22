using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientsMSSql.View
{
    public class ClientCodeRealtion
    {
        public DateTime AddedClientCodeRelationDate { get; set;  }
        public string Description { get; set; }
        public string Code { get; set; }
        public decimal? Amount { get; set; }
        public bool ToDelete { get; set; }
        public int? ClientID { get; set; }
        public int? CodeID { get; set; }


    }
}
