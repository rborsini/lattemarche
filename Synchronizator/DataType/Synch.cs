using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LatteMarche.Synch.DataType
{
    public class Synch
    {
        public int Id { get; set; }
        public DateTime LastDate { get; set; }
        public string Note { get; set; }

        public OperationTypeEnum OperationType { get; set; }

    }
}
