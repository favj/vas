using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyMetrix360.Core.Models
{
    public class ServiceResponse
    {
        public bool Status { get; set; }
        public string Errors { get; set; }
        public string Output { get; set; }
    }
}
