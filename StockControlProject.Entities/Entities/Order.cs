using StockControlProject.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Entities.Entities
{
    public class Order : BaseEntity
    {
        public virtual Status Status { get; set; }
        public virtual User User { get; set; }
        
        //public string OrderDescription { get; set; }

    }
}
