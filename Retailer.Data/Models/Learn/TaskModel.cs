using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.Data.Models.Learn
{
    public class TaskModel : BaseModel
    {
        public string Task { get; set; }

        public bool IsComplete { get; set; }
    }
}
