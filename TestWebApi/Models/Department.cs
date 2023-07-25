using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApi
{
    public class Department
    {
        public string FiscalYear { get; set; }

        public int Id { get; set; }

        public String Name { get; set; }


        public double FundsAvailable { get; set; }

        public double FundsUsed { get; set; }

        public String  Remarks { get; set; }
    }
}
