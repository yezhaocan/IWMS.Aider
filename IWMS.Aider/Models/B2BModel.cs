using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IWMS.Aider.Models
{
    [Serializable]
    public class B2BModel
    {
        public string BillId { get; set; }
        public string UniCode { get; set; }
        public string Sku { get; set; }
    }
}
